namespace MinimalSPAwithAPIs.Models.Filters;

public partial class MyUsersFilter : Filter
{
    public int? PrimaryKey { get; set; }

    public string? FiscalCode { get; set; } = null!;

    public string? EmployeeID { get; set; } = null!;

    public string? Surname { get; set; } = null!;

    public string? Name { get; set; } = null!;

    public string? Email { get; set; } = null!;

    public string? PhoneNumber { get; set; } = null!;

    public string? Profile { get; set; } = null!;

    public DateTime? StartingDate { get; set; }

    public DateTime? EndingDate { get; set; }

    public string? State { get; set; } = null!;

    public string? Role { get; set; } = null!;

    public string? LastUpdateUser { get; set; } = null!;

    public DateTime? LastUpdateDate { get; set; }

    public string? LastUpdateApp { get; set; } = null!;
}