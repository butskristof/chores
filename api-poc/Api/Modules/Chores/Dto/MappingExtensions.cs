using ChoresPoc.Api.Modules.Chores.Models;

namespace ChoresPoc.Api.Modules.Chores.Dto;

public static class MappingExtensions
{
	public static ChoreDto ToDto(this Chore chore) => new(chore.Id, chore.Title, chore.Interval);
}