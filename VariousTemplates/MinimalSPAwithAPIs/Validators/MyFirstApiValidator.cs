namespace MinimalSPAwithAPIs.Validators;

public class CreateMyFirstApiValidator : AbstractValidator<CreateMyFirstApiCommand>
{
    private readonly MyDbContext _db;

    public CreateMyFirstApiValidator(MyDbContext db)
    {
        _db = db;

        RuleFor(x => x)
            .Must(x => x.model.StartingDate <= x.model.EndingDate);

        RuleFor(x => x.model.StartingDate)
            .NotEmpty().WithMessage("StartingDate is required.")
            .GreaterThanOrEqualTo(DateTime.Today).WithMessage("Pay attention to the date interval.");

        RuleFor(x => x.model.EndingDate)
            .NotEmpty().WithMessage("EndingDate is required.")
            .GreaterThan(x => x.model.StartingDate).WithMessage("Pay attention to the date interval.");
    }
}
