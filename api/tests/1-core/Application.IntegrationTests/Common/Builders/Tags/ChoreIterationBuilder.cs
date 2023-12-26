using Chores.Domain.Models.Chores;

namespace Chores.Application.IntegrationTests.Common.Builders.Tags;

internal sealed class ChoreIterationBuilder
{
    private Guid _id = Guid.NewGuid();

    // default value is before the default FakeTimeProvider UtcNow value
    private DateTimeOffset _date = new(1999, 12, 31, 0, 0, 0, TimeSpan.Zero);
    private string? _notes = null;

    internal ChoreIterationBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    internal ChoreIterationBuilder WithDate(DateTimeOffset date)
    {
        _date = date;
        return this;
    }

    internal ChoreIterationBuilder WithNotes(string? notes)
    {
        _notes = notes;
        return this;
    }

    internal ChoreIteration Build() => new()
    {
        Id = _id,
        Date = _date,
        Notes = _notes,
    };

    public static implicit operator ChoreIteration(ChoreIterationBuilder builder) => builder.Build();
}