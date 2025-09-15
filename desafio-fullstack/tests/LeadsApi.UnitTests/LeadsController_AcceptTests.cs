using System.Threading.Tasks;
using FluentAssertions;
using LeadsApi.Controllers;
using LeadsApi.Data;
using LeadsApi.Models;
using LeadsApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace LeadsApi.UnitTests;

public class LeadsController_AcceptTests
{
    private static AppDbContext NewDb()
    {
        var opt = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        return new AppDbContext(opt);
    }

    [Fact]
    public async Task Accept_should_set_status_and_apply_discount_and_send_email()
    {
        // arrange
        await using var db = NewDb();
        var email = new Mock<IEmailFake>();
        var lead = new Lead
        {
            FirstName = "Ana",
            LastName = "Silva",
            Price = 600m,
            Status = LeadStatus.New
        };
        db.Leads.Add(lead);
        await db.SaveChangesAsync();

        var sut = new LeadsController(db, email.Object);

        // act
        var result = await sut.Accept(lead.Id);

        // assert
        result.Should().BeOfType<NoContentResult>();

        var saved = await db.Leads.SingleAsync(l => l.Id == lead.Id);
        saved.Status.Should().Be(LeadStatus.Accepted);
        saved.Price.Should().Be(540m); // 10% off

        email.Verify(e => e.SendAsync(
            "vendas@test.com",
            It.Is<string>(s => s.Contains("aceito", StringComparison.OrdinalIgnoreCase)),
            It.IsAny<string>()),
            Times.Once);
    }

    [Fact]
    public async Task Accept_should_not_discount_when_price_is_500_or_less()
    {
        await using var db = NewDb();
        var email = new Mock<IEmailFake>();
        var lead = new Lead { FirstName = "Bob", Price = 500m, Status = LeadStatus.New };
        db.Leads.Add(lead); await db.SaveChangesAsync();

        var sut = new LeadsController(db, email.Object);

        await sut.Accept(lead.Id);

        var saved = await db.Leads.FindAsync(lead.Id);
        saved!.Price.Should().Be(500m);
    }
}
