using LeadsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LeadsApi.Data;

public static class SeedHelper
{
    public static async Task EnsureSeedAsync(AppDbContext db, int targetNew = 30)
    {
        var currentNew = await db.Leads.CountAsync(l => l.Status == LeadStatus.New);
        var toAdd = targetNew - currentNew;
        if (toAdd <= 0) return;

        var rnd = new Random();

        string[] firsts = ["Ana","Bruno","Carla","Diego","Eva","Fábio","Giulia","Heitor","Isabela","João","Kauã","Lívia","Marcos","Nina","Otávio","Paula","Rafa","Sofia","Tiago","Vitória"];
        string[] lasts  = ["Silva","Souza","Lima","Almeida","Gomes","Barbosa","Mendes","Pereira","Cardoso","Cunha","Vieira","Rocha","Araujo","Ferreira","Castro"];
        string[] suburbs = ["Centro","Savassi","Funcionários","Serra","Santo Agostinho","Cidade Nova","Barro Preto","Pampulha"];
        string[] categories = ["Plumbing","Electrical","Cleaning","Painting","Moving","Landscaping","Carpentry"];

        string[] problems =
        [
            "Kitchen leak", "Bathroom faucet dripping", "Outlet repair", "Short circuit near panel",
            "Deep cleaning for 2 bedrooms + kitchen", "Internal walls 3 colours", "Move 1BR apartment",
            "Backyard cleanup and pruning", "Install custom shelves", "Fix broken door frame"
        ];

        var leads = new List<Lead>(capacity: toAdd);

        for (int i = 0; i < toAdd; i++)
        {
            var fn = firsts[rnd.Next(firsts.Length)];
            var ln = lasts[rnd.Next(lasts.Length)];
            var full = $"{fn}.{ln}".ToLower();

            var price = Math.Round((decimal)rnd.Next(150, 1200) + rnd.Next(0, 99) / 100m, 2);

            var cat = categories[rnd.Next(categories.Length)];
            var desc = problems[rnd.Next(problems.Length)];

            leads.Add(new Lead
            {
                FirstName = fn,
                LastName  = ln,
                Phone     = $"(31) 9{rnd.Next(6000, 9999)}-{rnd.Next(1000, 9999)}",
                Email     = $"{full}@example.com",
                DateCreated = DateTime.UtcNow.AddMinutes(-rnd.Next(0, 60 * 24 * 7)),
                Suburb    = suburbs[rnd.Next(suburbs.Length)],
                Category  = cat,
                Description = desc,
                Price     = price,
                Status    = LeadStatus.New
            });
        }

        db.Leads.AddRange(leads);
        await db.SaveChangesAsync();
    }
}
