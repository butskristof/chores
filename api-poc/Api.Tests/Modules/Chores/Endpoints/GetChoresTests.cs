using System.Net;
using ChoresPoc.Api.Modules.Chores.Dto;
using ChoresPoc.Api.Tests.Data;
using FluentAssertions;

namespace ChoresPoc.Api.Tests.Modules.Chores.Endpoints;

public class GetChoresTests : IClassFixture<ApiFactory>
{
    private readonly ApiFactory _apiFactory;
    private readonly HttpClient _client;

    public GetChoresTests(ApiFactory apiFactory)
    {
        _apiFactory = apiFactory;
        _client = apiFactory.CreateClient();
    }

    [Fact]
	public async Task GetChore_ReturnsChoreDto()
	{
		var response = await _client.GetAsync($"/chores/{TestData.Chore1Id}");
		
		response.IsSuccessStatusCode.Should().BeTrue();
		response.StatusCode.Should().Be(HttpStatusCode.OK);
		var content = await response.Content.ReadFromJsonAsync<ChoreDto>();
		content.Should().NotBeNull();
		content!.Id.Should().Be(TestData.Chore1.Id);
		content!.Title.Should().Be(TestData.Chore1.Title);
		content!.Interval.Should().Be(TestData.Chore1.Interval);
	}

	[Fact]
	public async Task GetChore_ReturnsNotFound()
	{
		var id = Guid.Parse("7D249098-E2C7-4F3E-AEA9-77B2B4E1A995");

		var response = await _client.GetAsync($"/chores/{id}");

		response.IsSuccessStatusCode.Should().BeFalse();
		
		response.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}
}