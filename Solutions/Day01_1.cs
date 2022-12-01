namespace Advent2022.Solutions;
using InputHandler;

internal static class Day01_1
{
    public static void Execute()
    {
        var result = 0;

        while (true)
        {
			var input = Input.ListUntilWhiteSpace(s => int.Parse(s), Program.GetLineOfInput);
            if (input.Count == 0)
            {
                break;
            }

            var test = input.Sum();
            if (test > result)
            {
                result = test;
            }
		}        

        Console.WriteLine(result);
        Clipboard.SetText(result.ToString());
    }
}
