namespace Advent2022.Solutions;
using InputHandler;

internal static class Day01_2_R1
{
	public static string Execute()
	{
		var results = Program.GetAllInput()
			.Replace("\r", string.Empty)
			.Split("\n\n")
			.Select(s => s
				.Split("\n")
				.Sum(s => int.Parse(s)))
			.ToList();

		results.Sort();

		var result = results[^3] + results[^2] + results[^1];
		return result.ToString();
	}
}

