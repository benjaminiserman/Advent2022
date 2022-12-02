namespace Advent2022.Solutions;
using InputHandler;

internal static class Day03_1
{
	public static string Execute()
	{
		//var input = Input.ListUntilWhiteSpace(s => int.Parse(s), Program.GetLineOfInput);
		var input = Program.GetAllInput()
			.Replace("\r", string.Empty)
			.Split("\n")
			.Select(s => s)
			.ToList();

		var result = 0;

		return result.ToString();
	}
}
