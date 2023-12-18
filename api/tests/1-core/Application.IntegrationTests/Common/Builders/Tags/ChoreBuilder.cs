using Chores.Domain.Models.Chores;

namespace Chores.Application.IntegrationTests.Common.Builders.Tags;

internal sealed class ChoreBuilder
{
    private Guid _id = Guid.NewGuid();
    private string _name = "some chore";
    private int _interval = 10;

    internal ChoreBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    internal ChoreBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    internal ChoreBuilder WithInterval(int interval)
    {
        _interval = interval;
        return this;
    }

    internal Chore Build() => new() { Id = _id, Name = _name, Interval = _interval };
    public static implicit operator Chore(ChoreBuilder builder) => builder.Build();
}