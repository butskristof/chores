using ChoresPoc.Api.Data;
using ChoresPoc.Api.Modules.Chores.Dto;
using ChoresPoc.Api.Modules.Chores.Models;
using FastEndpoints;
using FluentValidation;

namespace ChoresPoc.Api.Modules.Chores.Endpoints;

public static class CreateChore
{
	public record Request(string Title, int Interval);

	public class Validator : Validator<Request>
	{
		public Validator()
		{
			RuleFor(x => x.Title).NotEmpty();
			RuleFor(x => x.Interval).GreaterThan(0);
		}
	}

	public class Endpoint : Endpoint<Request, ChoreDto>
	{
		private readonly ChoreDbContext _context;

		public Endpoint(ChoreDbContext context)
		{
			_context = context;
		}

		public override void Configure()
		{
			Post("/chores");
		}

		public override async Task HandleAsync(Request req, CancellationToken ct)
		{
			var entity = new Chore
			{
				Title = req.Title,
				Interval = req.Interval,
			};
			_context.Chores.Add(entity);
			await _context.SaveChangesAsync(ct);
			var response = entity.ToDto();
			await SendCreatedAtAsync<GetChore.Endpoint>(new {response.Id}, response, cancellation: ct);
		}
	}
}