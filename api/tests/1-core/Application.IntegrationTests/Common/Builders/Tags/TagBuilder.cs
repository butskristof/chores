using Chores.Domain.Models;

namespace Chores.Application.IntegrationTests.Common.Builders.Tags;

internal sealed class TagBuilder
{
    private Guid _id = Guid.NewGuid();
    private string _name = "some tag";

    internal TagBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    internal TagBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    internal Tag Build() => new() { Id = _id, Name = _name };

    public static implicit operator Tag(TagBuilder builder) => builder.Build();
}