namespace Advent2022.Solutions;
using InputHandler;

internal static class Template
{
	public static void Execute()
	{
		var input = Input.ListUntilWhiteSpace(s => s, Program.GetLineOfInput);
		var result = 0;

		

		Console.WriteLine(result);
		Clipboard.SetText(result.ToString());
	}
}
