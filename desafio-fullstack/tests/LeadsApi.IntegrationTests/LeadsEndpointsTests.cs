using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using LeadsApi.Models;

namespace LeadsApi.IntegrationTests;

public class LeadsEndpointsTests : IClassFixture<TestAppFactory>
{
    private readonly HttpClient _client;
    public LeadsEndpointsTests(TestAppFactory factory) => _client = factory.CreateClient();

    [Fact]
    public async Task GET_api_leads_should_return_new_leads()
    {
        var resp = await _client.GetAsync("/api/leads?status=New");
        resp.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await resp.Content.ReadFromJsonAsync<List<Lead>>();
        items.Should().NotBeNull();
        items!.Should().OnlyContain(l => l.Status == LeadStatus.New);
        items.Count.Should().BeGreaterThan(0);
    }
}
