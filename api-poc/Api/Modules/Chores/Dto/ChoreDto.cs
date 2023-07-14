namespace ChoresPoc.Api.Modules.Chores.Dto;

public record ChoreDto(
	Guid Id,
	string Title,
	int Interval);