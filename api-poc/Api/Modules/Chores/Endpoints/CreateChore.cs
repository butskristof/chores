using ChoresPoc.Api.Data;
using ChoresPoc.Api.Modules.Chores.Dto;
using ChoresPoc.Api.Modules.Chores.Models;
using FastEndpoints;
using FluentValidation;

namespace ChoresPoc.Api.Modules.Chores.Endpoints;

public static class CreateChore
{
	public record Request(string Title, int Interval);

	public record Response(Guid Id, string Title, int Interval) : ChoreDto(Id, Title, Interval);

	public class Validator : Validator<Request>
	{
		public Validator()
		{
			RuleFor(x => x.Title).NotEmpty();
			RuleFor(x => x.Interval).GreaterThan(0);
		}
	}

	public class Mapper : Mapper<Request, Response, Chore>
	{
		public override Chore ToEntity(Request r) => new()
		{
			Title = r.Title,
			Interval = r.Interval,
		};

		public override Response FromEntity(Chore e) => new(e.Id, e.Title, e.Interval);
	}

	public class Endpoint : Endpoint<Request, Response, Mapper>
	{
		private readonly ChoreDbContext _context;

		public Endpoint(ChoreDbContext context)
		{
			_context = context;
		}

		public override void Configure()
		{
			Post("/chores");
			AllowAnonymous();
		}

		public override async Task HandleAsync(Request req, CancellationToken ct)
		{
			var entity = Map.ToEntity(req);
			_context.Chores.Add(entity);
			await _context.SaveChangesAsync(ct);
			var response = Map.FromEntity(entity);
			await SendCreatedAtAsync<GetChore.Endpoint>(new {response.Id}, response, cancellation: ct);
		}
	}
}