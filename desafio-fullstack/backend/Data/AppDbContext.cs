using LeadsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LeadsApi.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Lead> Leads => Set<Lead>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Lead>(e =>
        {
            e.Property(x => x.Price).HasColumnType("decimal(10,2)");
        });

        // Seed com datas fixas
        modelBuilder.Entity<Lead>().HasData(
            new Lead {
                Id = 1, FirstName = "Ana", LastName = "Silva", Phone="(31) 99999-1111",
                Email="ana@example.com", DateCreated = new DateTime(2025, 9, 1, 12, 0, 0, DateTimeKind.Utc),
                Suburb="Centro", Category="Plumbing", Description="Kitchen leak",
                Price=650.00m, Status=LeadStatus.New
            },
            new Lead {
                Id = 2, FirstName = "Bruno", LastName = "Souza", Phone="(31) 98888-2222",
                Email="bruno@example.com", DateCreated = new DateTime(2025, 9, 2, 15, 30, 0, DateTimeKind.Utc),
                Suburb="Savassi", Category="Electrical", Description="Outlet repair",
                Price=300.00m, Status=LeadStatus.New
            },
            new Lead {
                Id = 3, FirstName = "Carla", LastName = "Lima", Phone="(31) 97777-3333",
                Email="carla@example.com", DateCreated = new DateTime(2025, 9, 3, 9, 45, 0, DateTimeKind.Utc),
                Suburb="Funcionários", Category="Cleaning", Description="Post-renovation",
                Price=800.00m, Status=LeadStatus.Accepted
            }
        );
    }
}
