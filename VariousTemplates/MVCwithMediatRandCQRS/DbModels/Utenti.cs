namespace MVCwithMediatRandCQRS.DbModels;

public partial class Utenti
{
    public int Id { get; set; }
    
    public string Nome { get; set; } = null!;

    public string Cognome { get; set; } = null!;

    public string Email { get; set; } = null!;
}
