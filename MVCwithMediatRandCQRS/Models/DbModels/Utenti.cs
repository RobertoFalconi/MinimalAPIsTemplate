namespace MVCwithMediatRandCQRS.Web.Models.DbModels;

public partial class Utenti
{
    public int Id { get; set; }

    public string CodiceFiscale { get; set; } = null!;

    public string Nome { get; set; } = null!;

    public string Cognome { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public string Ruolo { get; set; } = null!;
}
