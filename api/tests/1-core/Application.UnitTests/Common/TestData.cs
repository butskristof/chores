namespace Chores.Application.UnitTests.Common;

internal static class TestData
{
    internal sealed class EmptyStrings : TheoryData<string>
    {
        public EmptyStrings()
        {
            Add(string.Empty);
            Add("");
            Add("    ");
        }
    }

    internal sealed class ValidColors : TheoryData<string>
    {
        public ValidColors()
        {
            Add("#ffffff");
            Add("#FFFFFF");
        }
    }

    internal sealed class InvalidColors : TheoryData<string>
    {
        public InvalidColors()
        {
            Add("fff");
            Add("ffffff");
            Add("#f");
            Add("#ff");
            Add("#ffff");
            Add("#fffff");
            Add("#fffffff");
            Add("fffffff");
            Add("#ffz");
            Add("#fffffz");
            Add("#fff");
            Add("#FFF");
        }
    }
}