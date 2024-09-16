using FluentValidation;
using CodeNetEmployeeProjectTask.Models;
using CodeNetEmployeeProjectTask.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace CodeNetEmployeeProjectTask.Validators
{
    public class EmployeeValidator : AbstractValidator<Employee>
    {
        private readonly AppDbContext _dbContext;

        public EmployeeValidator(AppDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(e => e.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Length(1, 100).WithMessage("Name length must be between 1 and 100 characters.")
                .Matches(@"^[a-zA-Z\s]*$").WithMessage("Name cannot contain numbers or special characters.")
                .Must(BeUniqueName).WithMessage("Employee name is already used.");

            RuleFor(e => e.Email)
                .EmailAddress().WithMessage("Invalid email format.")
                .Matches(@"^\S+$").WithMessage("Email cannot contain spaces.")
                .When(e => !string.IsNullOrEmpty(e.Email))
                .Must(BeUniqueEmail).WithMessage("this Email is already used.")
                .When(e => !string.IsNullOrEmpty(e.Email));

            RuleFor(e => e.Email)
                .NotEmpty().WithMessage("Email is required.")
                .When(e => e.Email != null);  
        }

        private bool BeUniqueName(Employee employee, string name)
        {
            var normalizedName = NormalizeName(name);
            return !_dbContext.Employees
                .Any(e => NormalizeName(e.Name) == normalizedName && e.EmployeeId != employee.EmployeeId);
        }

        private bool BeUniqueEmail(Employee employee, string email)
        {
            var normalizedEmail = NormalizeEmail(email);
            return !_dbContext.Employees
                .Any(e => NormalizeEmail(e.Email) == normalizedEmail && e.EmployeeId != employee.EmployeeId);
        }

        private string NormalizeName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return string.Empty;

            var trimmedName = name.Trim();
            var normalizedName = Regex.Replace(trimmedName, @"\s+", " ");
            return normalizedName.ToLower();
        }

        private string NormalizeEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return string.Empty;

            var trimmedEmail = email.Trim();
            return trimmedEmail.ToLower();
        }
    }
}
