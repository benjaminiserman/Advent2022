namespace Advent2022.Solutions;

internal static class Day02_2
{
	public static string Execute()
	{
		var input = Program.GetAllInput()
			.Replace("\r", string.Empty)
			.Split("\n")
			.Select(s => s)
			.ToList();

		var result = 0;

		foreach (var line in input)
		{
			var split = line.Split();
			var a = split[0][0];
			var b = split[1][0];

			result += (a, b) switch
			{
				('A', 'Z') => 6 + 2,
				('A', 'Y') => 3 + 1,
				('A', 'X') => 0 + 3,

				('B', 'Z') => 6 + 3,
				('B', 'Y') => 3 + 2,
				('B', 'X') => 0 + 1,

				('C', 'Z') => 6 + 1,
				('C', 'Y') => 3 + 3,
				('C', 'X') => 0 + 2,
			};
		}

		return result.ToString();
	}
}
