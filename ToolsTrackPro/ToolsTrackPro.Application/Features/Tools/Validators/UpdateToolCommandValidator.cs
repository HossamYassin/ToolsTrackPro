using FluentValidation;
using ToolsTrackPro.Application.Features.Tools.Commands;

namespace ToolsTrackPro.Application.Features.Tools.Validators
{
    public class UpdateToolCommandValidator : AbstractValidator<UpdateToolCommand>
    {
        public UpdateToolCommandValidator()
        {
            RuleFor(x => x.Tool.Name)
                .NotEmpty().WithMessage("Tool name is required.")
                .MaximumLength(255).WithMessage("Tool name cannot exceed 255 characters.");

            RuleFor(x => x.Tool.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
        }
    }

}
