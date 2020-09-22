using FluentValidation;

namespace Application.Validators
{
    public static class ValidatorExtensions
    {
        public static IRuleBuilder<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            var options = ruleBuilder
            .NotEmpty()
            .MinimumLength(6).WithMessage("Password must be atleast six characters")
            .Matches("[A-Z]").WithMessage("Password must contain 1 uppercase letter")
            .Matches("[a-z]").WithMessage("Password must contain 1 lowercase character")
            .Matches("[0-9]").WithMessage("Password must contain 1 number")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain atleast 1 special character");

            return options;
        }
    }
}