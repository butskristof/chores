using FastEndpoints;
using FluentValidation;

namespace ChoresPoc.Api.Endpoints;

public static class Hello
{
	public record Request(string Name);
	public record Response(string Message);

	public class Validator : Validator<Request>
	{
		public Validator()
		{
			RuleFor(r => r.Name).NotEmpty();
		}
	}

	public class Handler : Endpoint<Request, Response>
	{
		public override void Configure()
		{
			Get("/hello");
			AllowAnonymous();
		}

		public override async Task HandleAsync(Request req, CancellationToken ct)
		{
			if (req.Name == "Archibald")
				AddError(r => r.Name, "That's a silly name.");
			
			ThrowIfAnyErrors();
			
			var response = new Response($"Hello, {req.Name}!");
			
			await SendAsync(response, cancellation: ct);
		}
	}
}