namespace Advent2022.Solutions;
using InputHandler;

internal static class Day01_2_R1
{
	public static void Execute()
	{
		var result = 0;

		var results = Program.GetAllInput()
			.Replace("\r", string.Empty)
			.Split("\n\n")
			.Select(s => s
				.Split("\n")
				.Sum(s => int.Parse(s)))
			.ToList();

		results.Sort();

		Console.WriteLine(results[^1] + results[^2] + results[^3]);
		Clipboard.SetText(result.ToString());
	}
}

