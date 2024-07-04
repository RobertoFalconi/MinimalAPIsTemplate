namespace MinimalSPAwithAPIs.Validators;

public class AggiornaMyFirstApiValidator : AbstractValidator<AggiornaMyFirstApiCommand>
{
    private readonly MyDbContext _db;

    public AggiornaMyFirstApiValidator(MyDbContext db)
    {
        _db = db;

        RuleFor(x => x)
            .Must(x => x.model.StartingDate <= x.model.EndingDate)
            .WithMessage("Attenzione! L’intervallo temporale indicato non è corretto.");

        RuleFor(x => x.model.StartingDate)
            .NotEmpty().WithMessage("La data di inizio validità è obbligatoria.")
            .GreaterThanOrEqualTo(DateTime.Today).WithMessage("La data di inizio validità non può essere antecedente alla data di sistema.");

        RuleFor(x => x.model.EndingDate)
            .NotEmpty().WithMessage("La data di fine validità è obbligatoria.")
            .GreaterThan(x => x.model.StartingDate).WithMessage("La data di fine validità deve essere successiva alla data di inizio validità.");

        RuleFor(x => x)
            .MustAsync(DateDoNotOverlap)
            .WithMessage("Attenzione! L'intervallo temporale indicato non è congruente.");
    }

    private async Task<bool> DateDoNotOverlap(AggiornaMyFirstApiCommand dto, CancellationToken cancellation)
    {
        var overlaps = await _db.MyFirstApiDb
             .Where(x => x.State != "C")
            .AnyAsync(pt => pt.StartingDate <= dto.model.EndingDate && pt.EndingDate >= dto.model.StartingDate && pt.PrimaryKey != dto.model.PrimaryKey, cancellation);
        return !overlaps;
    }
}

public class InserisciMyFirstApiValidator : AbstractValidator<InserisciMyFirstApiCommand>
{
    private readonly MyDbContext _db;

    public InserisciMyFirstApiValidator(MyDbContext db)
    {
        _db = db;

        RuleFor(x => x)
            .Must(x => x.model.StartingDate <= x.model.EndingDate)
            .WithMessage("Attenzione! L’intervallo temporale indicato non è corretto.");

        RuleFor(x => x.model.StartingDate)
            .NotEmpty().WithMessage("La data di inizio validità è obbligatoria.")
            .GreaterThanOrEqualTo(DateTime.Today).WithMessage("La data di inizio validità non può essere antecedente alla data di sistema.");

        RuleFor(x => x.model.EndingDate)
            .NotEmpty().WithMessage("La data di fine validità è obbligatoria.")
            .GreaterThan(x => x.model.StartingDate).WithMessage("La data di fine validità deve essere successiva alla data di inizio validità.");
    }
}
