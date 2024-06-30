namespace MinimalSPAwithAPIs.Models.DB;

public partial class MyFirstApiDb
{
    public int PrimaryKey { get; set; }

    public string Description { get; set; } = null!;

    public DateTime StartingDate { get; set; }

    public DateTime EndingDate { get; set; }

    public string? Notes { get; set; }

    public string State { get; set; } = null!;

    public string LastUpdateUser { get; set; } = null!;

    public DateTime LastUpdateDate { get; set; }

    public string LastUpdateApplication { get; set; } = null!;
}
