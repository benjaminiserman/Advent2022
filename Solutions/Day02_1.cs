namespace Advent2022.Solutions;
using InputHandler;

internal static class Day02_1
{
	public static void Execute()
	{
		var input = Program.GetAllInput()
			.Replace("\r", string.Empty)
			.Split("\n")
			.Select(s => s)
			.ToList();
		var result = 0;

		

		Console.WriteLine(result);
		Clipboard.SetText(result.ToString());
	}
}
