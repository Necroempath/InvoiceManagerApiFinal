using FluentValidation;
using System.Text.RegularExpressions;

namespace InvoiceManagerApiFinal.Validators;

public static class ValidationRulesExtensions
{
    public static IRuleBuilderOptions<T, string> CustomPassword<T>(this IRuleBuilder<T, string> builder, 
        bool mustContainsLowerCase = true, bool mustContainsUpperCase = true, bool mustContainsDigit = true, int length = 6)
    {
        return builder.Must(password =>
        {
            if (password.Length < length)
                return false;

            if (mustContainsLowerCase && !Regex.IsMatch(password, @"[a-z]"))
                return false;

            if (mustContainsUpperCase && !Regex.IsMatch(password, @"[A-Z]"))
                return false;

            if (mustContainsDigit && !Regex.IsMatch(password, @"\d"))
                return false;

            return true;
        });
    } 
}
