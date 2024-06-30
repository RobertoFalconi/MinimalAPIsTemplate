namespace MinimalSPAwithAPIs.Validators;

public class MyUsersValidator : AbstractValidator<MyUsersDTO>
{
    public MyUsersValidator()
    {
        RuleFor(x => x.StartingDate)
            .NotEmpty().WithMessage("La data di inizio abilitazione è obbligatoria.");

        RuleFor(x => x.EndingDate)
            .NotEmpty().WithMessage("La data di fine abilitazione è obbligatoria.");

        RuleFor(x => x)
            .Must(x => x.EndingDate >= x.StartingDate)
            .WithMessage("La data di fine abilitazione non può essere antecedente alla data di inizio abilitazione.");
    }
}
