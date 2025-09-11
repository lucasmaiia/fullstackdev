namespace LeadsApi.Models;

public enum LeadStatus { New = 0, Accepted = 1, Declined = 2 }

public class Lead
{
    public int Id { get; set; }

    // Invited
    public string FirstName { get; set; } = "";
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public string Suburb { get; set; } = "";
    public string Category { get; set; } = "";
    public string Description { get; set; } = "";
    public decimal Price { get; set; }

    // Accepted 
    public string LastName { get; set; } = "";
    public string Phone { get; set; } = "";
    public string Email { get; set; } = "";

    public LeadStatus Status { get; set; } = LeadStatus.New;
}
