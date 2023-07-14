using System.Net;
using ChoresPoc.Api.Modules.Chores.Dto;
using ChoresPoc.Api.Modules.Chores.Endpoints;
using FastEndpoints;
using FluentAssertions;

namespace ChoresPoc.Api.Tests.Modules.Chores.Endpoints;

public class CreateChoreTests : IClassFixture<ApiFactory>
{
	private readonly ApiFactory _apiFactory;
	private readonly HttpClient _client;
	
	public CreateChoreTests(ApiFactory apiFactory)
	{
		_apiFactory = apiFactory;
		_client = apiFactory.CreateClient();
	}

	[Fact]
	public async Task CreateChore_ReturnsBadRequest()
	{
		var request = new CreateChore.Request(string.Empty, -1);
		var (response, result) = await _client
			.POSTAsync<CreateChore.Endpoint, CreateChore.Request, ErrorResponse>(request);

		response.Should().NotBeNull();
		response!.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		result.Should().NotBeNull();
		result!.Errors.Keys.Should().Contain("title");
		result!.Errors.Keys.Should().Contain("interval");
	}

	[Fact]
	public async Task CreateChore_ReturnsCreatedAtRoute()
	{
		const string title = "test chore";
		const int interval = 7;
		var request = new CreateChore.Request(title, interval);
		var (response, result) = await _client
			.POSTAsync<CreateChore.Endpoint, CreateChore.Request, ChoreDto>(request);

		response.Should().NotBeNull();
		response!.StatusCode.Should().Be(HttpStatusCode.Created);
		response!.Headers.Should().Contain(h => h.Key == "Location");
		result!.Should().NotBeNull();
		result!.Id.Should().NotBeEmpty();
		result!.Title.Should().Be(title);
		result!.Interval.Should().Be(interval);
	}
}