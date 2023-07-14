using ChoresPoc.Api.Data;
using ChoresPoc.Api.Modules.Chores.Dto;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ChoresPoc.Api.Modules.Chores.Endpoints;

public static class GetChore
{
	public record Request(Guid Id);
	
	public record Response(Guid Id, string Title, int Interval) : ChoreDto(Id, Title, Interval);

	[HttpGet("/chores/{Id}")]
	[AllowAnonymous]
	public class Endpoint : Endpoint<Request, Response>
	{
		private readonly ChoreDbContext _context;

		public Endpoint(ChoreDbContext context)
		{
			_context = context;
		}

		public override async Task HandleAsync(Request req, CancellationToken ct)
		{
			var chore = await _context.Chores
				.SingleOrDefaultAsync(c => c.Id == req.Id, ct);
			
			if (chore is null)
			{
				await SendNotFoundAsync(ct);
				return;
			}
			
			var response = new Response(chore.Id, chore.Title, chore.Interval);
			await SendOkAsync(response, ct);
		}
	}
}