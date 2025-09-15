using System.Threading.Tasks;
using FluentAssertions;
using LeadsApi.Controllers;
using LeadsApi.Data;
using LeadsApi.Models;
using LeadsApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeadsApi.UnitTests;

public class LeadsController_GetTests
{
    private static AppDbContext NewDb()
    {
        var opt = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        return new AppDbContext(opt);
    }

    [Fact]
    public async Task Get_should_return_only_requested_status()
    {
        await using var db = NewDb();
        var email = new DummyEmail();
        db.Leads.AddRange(
            new Lead { FirstName="A", Status = LeadStatus.New },
            new Lead { FirstName="B", Status = LeadStatus.Accepted },
            new Lead { FirstName="C", Status = LeadStatus.New }
        );
        await db.SaveChangesAsync();

        var sut = new LeadsController(db, email);
        var result = await sut.Get("New") as OkObjectResult;

        result.Should().NotBeNull();
        var list = (result!.Value as IEnumerable<Lead>)!.ToList();
        list.Should().HaveCount(2);
        list.Should().OnlyContain(l => l.Status == LeadStatus.New);
    }

    private sealed class DummyEmail : IEmailFake
    {
        public Task SendAsync(string to, string subject, string body) => Task.CompletedTask;
    }
}
