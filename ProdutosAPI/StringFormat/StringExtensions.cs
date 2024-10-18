using System.Globalization;
using System.Text;

namespace ProdutosAPI.StringFormat;

public static class StringExtensions
{
    public static string RemoveAccents(this string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return input;

        string normalizedString = input.Normalize(NormalizationForm.FormD);
        StringBuilder stringBuilder = new();

        foreach (char c in normalizedString)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC).ToLower();
    }
}
