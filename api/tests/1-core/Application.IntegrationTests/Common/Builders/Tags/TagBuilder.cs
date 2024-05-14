using Chores.Domain.Models.Tags;

namespace Chores.Application.IntegrationTests.Common.Builders.Tags;

internal sealed class TagBuilder
{
    private Guid _id = Guid.NewGuid();
    private string _name = "some tag";
    private string? _color = null;
    private string? _icon = null;

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

    internal TagBuilder WithColor(string? color)
    {
        _color = color;
        return this;
    }

    internal TagBuilder WithIcon(string? icon)
    {
        _icon = icon;
        return this;
    }

    internal Tag Build() => new()
    {
        Id = _id,
        Name = _name,
        Color = _color,
        Icon = _icon,
    };

    public static implicit operator Tag(TagBuilder builder) => builder.Build();
}