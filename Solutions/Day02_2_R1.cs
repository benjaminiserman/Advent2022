namespace Advent2022.Solutions;

internal static class Day02_2_R1
{
	public static string Execute()
	{
		var input = Program.GetAllInput()
			.Replace("\r", string.Empty)
			.Split("\n")
			.Select(s => s)
			.ToList();

		var result = input.Sum(line => line switch
		{
			"A X" => 0 + 3,
			"A Y" => 3 + 1,
			"A Z" => 6 + 2,

			"B X" => 0 + 1,
			"B Y" => 3 + 2,
			"B Z" => 6 + 3,

			"C X" => 0 + 2,
			"C Y" => 3 + 3,
			"C Z" => 6 + 1,

			_ => throw new("Invalid input.")
		});

		return result.ToString();
	}
}
