using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using UsuariosAPI.DTOs;
using UsuariosAPI.Models;

using UsuariosAPI.Services;
using UsuariosAPI.Tokens;

namespace UsuariosAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthController> _logger;

    public AuthController(ITokenService tokenService, 
        UserManager<ApplicationUser> userManager, 
        RoleManager<IdentityRole> roleManager, 
        IConfiguration configuration, 
        ILogger<AuthController> logger)
    {
        _tokenService = tokenService;
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _logger = logger;
    }


    [HttpGet]
    public ActionResult<string> Get()
    {
        return "AutorizaController  :: Acessado em : "
            + DateTime.Now.ToLongDateString();
    }


    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] CadastroUsuarioDTO model)
    {
        if (model.Password != model.PasswordConfirm)
        {
            return BadRequest("A senha e a confirmação de senha não correspondem.");
        }

        var userExists = await _userManager.FindByNameAsync(model.UserName!);
        if (userExists != null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });
        }


        var emailExists = await _userManager.FindByEmailAsync(model.EmailRegister!);
        if (emailExists != null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Email already exists!" });
        }


        ApplicationUser user = new ApplicationUser
        {
            Email = model.EmailRegister,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = model.UserName
        };

        var result = await _userManager.CreateAsync(user, model.Password!);
        if (!result.Succeeded)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed." });
        }

        return Ok(new Response { Status = "Success", Message = "Usuario criado com sucesso!" });
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UsuarioDTO model)
    {
        var user = await _userManager.FindByNameAsync(model.UserName!);
        if (user is not null && await _userManager.CheckPasswordAsync(user, model.Password!))
        {

            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
                 new Claim(ClaimTypes.NameIdentifier, user.Id),
                 new Claim (ClaimTypes.Name, user.UserName!),
                 new Claim (ClaimTypes.Email, user.Email!),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                 new Claim("IsAuthenticated", "true")

            };


            var token = _tokenService.GenerateAccessToken(authClaims,
                _configuration);

            var refreshToken = _tokenService.GenerateRefreshToken();

            _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInMinutes"],
                out int refreshTokenValidityInMinutes);

            user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(refreshTokenValidityInMinutes);

            user.RefreshToken = refreshToken;

            await _userManager.UpdateAsync(user);

            return Ok(new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                Expiration = token.ValidTo
            });
        }

        return Unauthorized("Incorret Credentials!");
    }

    [HttpPost("refreshToken")]
    public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
    {
        if (tokenModel is null)
        {
            return BadRequest("Invalid client request");
        }

        string accessToken = tokenModel.AccessToken
            ?? throw new ArgumentNullException(nameof(tokenModel));
        string refreshToken = tokenModel.RefreshToken
            ?? throw new ArgumentException(nameof(tokenModel));

        var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken!, _configuration);
        if (principal == null)
        {
            return BadRequest("Invalid access token/refresh token");
        }

        string username = principal.Identity.Name;
        var user = await _userManager.FindByNameAsync(username!);
        if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            return BadRequest("Invalid access token/refresh token");
        }

        var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims.ToList(), _configuration);
        var newRefreshToken = _tokenService.GenerateRefreshToken();
        user.RefreshToken = newRefreshToken;
        await _userManager.UpdateAsync(user);

        return new ObjectResult(new
        {
            accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
            refreshToken = newRefreshToken
        });

    }


    [HttpPost("MudarSenha")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> ChangePassword(AlterarSenhaModel model)
    {
        // Obtém o nome de usuário do usuário atualmente logado
        var username = User.Identity.Name;
        if (username == null)
        {
            return BadRequest("Nome de usuário não encontrado na solicitação.");
        }

        // Obtém o objeto de usuário correspondente ao nome de usuário
        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
        {
            return BadRequest("Usuário não encontrado.");
        }


        if (model.PasswordNow == model.PasswordNew)
        {
            return BadRequest("A nova senha não pode ser igual a senha atual.");
        }

        // Verifica se a senha atual está correta
        var passwordCheck = await _userManager.CheckPasswordAsync(user, model.PasswordNow);
        if (!passwordCheck)
        {
            return BadRequest("A senha atual está incorreta.");
        }

        var result = await _userManager.ChangePasswordAsync(user, model.PasswordNow, model.PasswordNew);
        if (result.Succeeded)
        {
            return Ok("Senha alterada com sucesso!");
        }
        else
        {
            return BadRequest(result.Errors);
        }
    }
}
