using ChoresPoc.Api.Modules.Chores.Dto;
using ChoresPoc.Api.Modules.Chores.Models;
using FluentAssertions;

namespace ChoresPoc.Api.Tests.Modules.Chores.Dto;

public class MappingExtensionsTests
{
	[Fact]
	public void ToDto_MapsProperties()
	{
		var id = Guid.Parse("68BA1823-241F-4C99-8346-81504854FB3F");
		const string title = "chore title";
		const int interval = 7;

		var entity = new Chore {Id = id, Title = title, Interval = interval};
		var dto = entity.ToDto();
		
		dto.Id.Should().Be(id);
		dto.Title.Should().Be(title);
		dto.Interval.Should().Be(7);
	}
}