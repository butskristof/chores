namespace Chores.Application.UnitTests.Common;

internal static class TestData
{
    internal class EmptyStrings : TheoryData<string>
    {
        public EmptyStrings()
        {
            Add(string.Empty);
            Add("");
            Add("    ");
        }
    }
}