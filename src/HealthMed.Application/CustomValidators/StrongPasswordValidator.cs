using System.Text.RegularExpressions;
using FluentValidation;
using FluentValidation.Validators;

namespace HealthMed.Application.CustomValidators;

public partial class StrongPasswordValidator<T> : PropertyValidator<T, string>
{
    public override string Name => "StrongPasswordValidator";

    public override bool IsValid(ValidationContext<T> context, string password)
    {
        if (string.IsNullOrWhiteSpace(password)) return false;

        // Check length
        if (password.Length < 8) return false;

        var hasUpper = UpperCase().IsMatch(password);
        var hasLower = LowerCase().IsMatch(password);
        var hasDigit = Number().IsMatch(password);
        var hasSpecial = SpecialChars().IsMatch(password);

        return hasUpper && hasLower && hasDigit && hasSpecial;
    }

    protected override string GetDefaultMessageTemplate(string errorCode)
        => "A senha deve ter pelo menos 8 caracteres, conter uma letra maiúscula, uma letra minúscula, um número e um caractere especial.";
    
    
    [GeneratedRegex("[A-Z]")]
    private static partial Regex UpperCase();
    
    [GeneratedRegex("[a-z]")]
    private static partial Regex LowerCase();
    
    [GeneratedRegex("[0-9]")]
    private static partial Regex Number();
    
    [GeneratedRegex(@"[\@\$\!\%\*\?\&]")]
    private static partial Regex SpecialChars();
}