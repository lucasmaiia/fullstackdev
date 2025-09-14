using LeadsApi.Data;
using LeadsApi.Models;
using LeadsApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeadsApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeadsController(AppDbContext db, IEmailFake email) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string? status = "New")
    {
        if (!Enum.TryParse<LeadStatus>(status, true, out var st))
            return BadRequest("Invalid status. Use New, Accepted or Declined.");

        var leads = await db.Leads
            .Where(l => l.Status == st)
            .OrderByDescending(l => l.DateCreated)
            .ToListAsync();

        return Ok(leads);
    }

    [HttpPost("seed")]
    public async Task<IActionResult> Seed([FromQuery] int count = 20)
    {
        count = Math.Clamp(count, 1, 200);

        var rnd = new Random();
        string[] firsts = ["Ana", "Bruno", "Carla", "Diego", "Eva", "Fábio", "Giulia", "Heitor", "Isabela", "João", "Kauã", "Lívia", "Marcos", "Nina", "Otávio", "Paula", "Rafa", "Sofia", "Tiago", "Vitória"];
        string[] lasts = ["Silva", "Souza", "Lima", "Almeida", "Gomes", "Barbosa", "Mendes", "Pereira", "Cardoso", "Cunha", "Vieira", "Rocha", "Araujo", "Ferreira", "Castro"];
        string[] suburbs = ["Centro", "Savassi", "Funcionários", "Serra", "Santo Agostinho", "Cidade Nova", "Barro Preto", "Pampulha"];
        string[] categories = ["Plumbing", "Electrical", "Cleaning", "Painting", "Moving", "Landscaping", "Carpentry"];

        var newLeads = new List<Lead>(capacity: count);

        for (int i = 0; i < count; i++)
        {
            var fn = firsts[rnd.Next(firsts.Length)];
            var ln = lasts[rnd.Next(lasts.Length)];
            var full = $"{fn}.{ln}".ToLower();

            var price = Math.Round((decimal)rnd.Next(150, 1200) + rnd.Next(0, 99) / 100m, 2);

            newLeads.Add(new Lead
            {
                FirstName = fn,
                LastName = ln,
                Phone = $"(31) 9{rnd.Next(6000, 9999)}-{rnd.Next(1000, 9999)}",
                Email = $"{full}@example.com",
                DateCreated = DateTime.UtcNow.AddMinutes(-rnd.Next(0, 60 * 24 * 7)),
                Suburb = suburbs[rnd.Next(suburbs.Length)],
                Category = categories[rnd.Next(categories.Length)],
                Description = "Auto-generated lead for testing.",
                Price = price,
                Status = LeadStatus.New
            });
        }

        db.Leads.AddRange(newLeads);
        var added = await db.SaveChangesAsync();

        return Ok(new { added });
    }

    [HttpPost("{id}/accept")]
    public async Task<IActionResult> Accept(int id)
    {
        var lead = await db.Leads.FindAsync(id);
        if (lead is null) return NotFound();

        if (lead.Status == LeadStatus.Accepted) return NoContent();

        if (lead.Price > 500) lead.Price = Math.Round(lead.Price * 0.9m, 2);
        lead.Status = LeadStatus.Accepted;
        await db.SaveChangesAsync();

        await email.SendAsync("vendas@test.com",
            $"Lead {lead.Id} aceito",
            $"Lead {lead.Id} ({lead.FirstName} {lead.LastName}) aceito. Preço final: {lead.Price}");

        return NoContent();
    }



    [HttpPost("{id}/decline")]
    public async Task<IActionResult> Decline(int id)
    {
        var lead = await db.Leads.FindAsync(id);
        if (lead is null) return NotFound();

        if (lead.Status == LeadStatus.Declined) return NoContent();

        lead.Status = LeadStatus.Declined;
        await db.SaveChangesAsync();
        return NoContent();
    }
    
    [HttpPost("seed/preset")]
public async Task<IActionResult> SeedPreset([FromQuery] bool clear = false)
{
    if (clear)
    {
        var news = await db.Leads.Where(l => l.Status == LeadStatus.New).ToListAsync();
        if (news.Count > 0)
        {
            db.Leads.RemoveRange(news);
            await db.SaveChangesAsync();
        }
    }

    var now = DateTime.UtcNow;

    var items = new List<Lead>
    {
        new Lead {
            FirstName = "Bill", LastName = "Turner",
            Email = "bill.turner@example.com", Phone = "(31) 98888-1111",
            Suburb = "Yanderra 2574", Category = "Painters",
            Description = "Need to paint 2 aluminum windows and a sliding glass door",
            DateCreated = now.AddDays(-7).AddHours(2).AddMinutes(37),
            Price = 62.00m, Status = LeadStatus.New
        },
        new Lead {
            FirstName = "Craig", LastName = "Foster",
            Email = "craig.foster@example.com", Phone = "(31) 97777-2222",
            Suburb = "Woolooware 2230", Category = "Interior Painters",
            Description = "internal walls 3 colours",
            DateCreated = now.AddDays(-7).AddHours(1).AddMinutes(15),
            Price = 49.00m, Status = LeadStatus.New
        },
        new Lead {
            FirstName = "Isabela", LastName = "Silva",
            Email = "isabela.silva@example.com", Phone = "(31) 99999-2222",
            Suburb = "Cidade Nova", Category = "Landscaping",
            Description = "Poda de árvores pequenas e limpeza do jardim.",
            DateCreated = now.AddDays(-1).AddHours(-1),
            Price = 638.17m, Status = LeadStatus.New
        },
        new Lead {
            FirstName = "Ana", LastName = "Souza",
            Email = "ana.souza@example.com", Phone = "(31) 98888-3333",
            Suburb = "Centro", Category = "Plumbing",
            Description = "Vazamento na pia da cozinha; precisa troca de sifão.",
            DateCreated = now.AddDays(-2).AddHours(3),
            Price = 650.00m, Status = LeadStatus.New
        },
        new Lead {
            FirstName = "Bruno", LastName = "Almeida",
            Email = "bruno.almeida@example.com", Phone = "(31) 97777-4444",
            Suburb = "Savassi", Category = "Electrical",
            Description = "Tomada da sala com mau contato; possível curto.",
            DateCreated = now.AddDays(-3).AddHours(5),
            Price = 300.00m, Status = LeadStatus.New
        },
        new Lead {
            FirstName = "Carla", LastName = "Mendes",
            Email = "carla.mendes@example.com", Phone = "(31) 96666-5555",
            Suburb = "Funcionários", Category = "Cleaning",
            Description = "Limpeza pós-obra em apartamento de 70m².",
            DateCreated = now.AddDays(-4).AddHours(4),
            Price = 220.00m, Status = LeadStatus.New
        }
    };

    db.Leads.AddRange(items);
    var added = await db.SaveChangesAsync();
    return Ok(new { added });
}

}
