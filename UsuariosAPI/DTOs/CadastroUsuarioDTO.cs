using System.ComponentModel.DataAnnotations;
using UsuariosAPI.Validations;

namespace UsuariosAPI.DTOs;

public class CadastroUsuarioDTO
{
    [Required(ErrorMessage = "Informe o e-mail!")]
    [EmailAddress(ErrorMessage = "E-mail inválido!")]
    public string EmailRegister { get; set; }

    [Required(ErrorMessage = "Informe o nome de usuário")]
    [StringLength(50, ErrorMessage = "Limite de caracteres excedido!")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Informe a senha!")]
    [DataType(DataType.Password)]
    [SenhaValidation(ErrorMessage = "Senha requer entre 6 e 20 caracteres!")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Confirme sua senha")]
    [DataType(DataType.Password)]
    [SenhaValidation(ErrorMessage = "Senha requer entre 6 e 20 caracteres!")]
    [Compare(nameof(Password), ErrorMessage = "A senha e a confirmação não são iguais")]
    public string PasswordConfirm { get; set; }
}
