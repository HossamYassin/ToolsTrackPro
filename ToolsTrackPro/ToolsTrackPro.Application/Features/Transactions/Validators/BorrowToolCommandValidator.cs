namespace ToolsTrackPro.Application.Features.Transactions.Validators
{
    using FluentValidation;
    using ToolsTrackPro.Application.Features.Transactions.Commands;

    public class BorrowToolCommandValidator : AbstractValidator<BorrowToolCommand>
    {
        public BorrowToolCommandValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("UserId must be greater than 0.");

            RuleFor(x => x.ToolId)
                .GreaterThan(0).WithMessage("ToolId must be greater than 0.");

            RuleFor(x => x.BorrowDate)
                .NotEmpty().WithMessage("BorrowDate is required.");

            RuleFor(x => x.DueDate)
                .NotEmpty().WithMessage("DueDate is required.")
                .GreaterThan(x => x.BorrowDate).WithMessage("DueDate must be later than BorrowDate.");
        }
    }

}
