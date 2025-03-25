namespace ToolsTrackPro.Application.Features.Transactions.Validators
{
    using FluentValidation;
    using ToolsTrackPro.Application.Features.Transactions.Commands;

    public class ReturnToolCommandValidator : AbstractValidator<ReturnToolCommand>
    {
        public ReturnToolCommandValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("UserId must be greater than 0.");

            RuleFor(x => x.ToolId)
                .GreaterThan(0).WithMessage("ToolId must be greater than 0.");

            RuleFor(x => x.ReturnDate)
                .NotEmpty().WithMessage("ReturnDate is required.");
        }
    }

}
