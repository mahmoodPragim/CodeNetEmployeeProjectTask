using FluentValidation;
using CodeNetEmployeeProjectTask.Models;
using CodeNetEmployeeProjectTask.Data;
using System.Linq;

namespace CodeNetEmployeeProjectTask.Validators
{
    public class ProjectValidator : AbstractValidator<Project>
    {
        private readonly AppDbContext _dbContext;
        public ProjectValidator(AppDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Project name is required.")
                .Length(1, 100).WithMessage("Project name length must be between 1 and 100 characters.")
                .Matches(@"^[a-zA-Z\s]*$").WithMessage("Project name cannot contain numbers or special characters.")
                .Must(BeUniqueName).WithMessage("Project name must be unique.")
                .When(p => !string.IsNullOrEmpty(p.Name)); 

            RuleFor(p => p.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
        }
        private bool BeUniqueName(Project project, string name)
        {
            var normalizedName = NormalizeName(name);

            return !_dbContext.Projects
                .Any(p => NormalizeName(p.Name) == normalizedName && p.ProjectId != project.ProjectId);
        }
        private string NormalizeName(string name)
        {
            return string.IsNullOrWhiteSpace(name) ? string.Empty : name.Trim().ToLower();
        }
    }
}
