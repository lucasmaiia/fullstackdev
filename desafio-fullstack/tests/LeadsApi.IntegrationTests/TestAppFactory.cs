using LeadsApi.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LeadsApi.IntegrationTests;

public class TestAppFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
           
            var desc = services.Single(sd => sd.ServiceType == typeof(DbContextOptions<AppDbContext>));
            services.Remove(desc);

            
            services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("itdb"));

            
            using var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.EnsureCreated();

            if (!db.Leads.Any())
            {
                db.Leads.AddRange(
                    new LeadsApi.Models.Lead { FirstName="Ana", Status=LeadsApi.Models.LeadStatus.New, Price=300 },
                    new LeadsApi.Models.Lead { FirstName="Bruno", Status=LeadsApi.Models.LeadStatus.New, Price=600 }
                );
                db.SaveChanges();
            }
        });
    }
}
