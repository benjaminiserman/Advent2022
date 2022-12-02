namespace Advent2022.Solutions;

internal static class Day02_1
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

			result += b switch
			{
				'X' => 1,
				'Y' => 2,
				'Z' => 3
			};

			if (a == 'A' && b == 'Y')
			{
				result += 6;
			}
			else if (a == 'A' && b == 'X')
			{
				result += 3;
			}

			if (a == 'B' && b == 'Z')
			{
				result += 6;
			}
			else if (a == 'B' && b == 'Y')
			{
				result += 3;
			}

			if (a == 'C' && b == 'X')
			{
				result += 6;
			}
			else if (a == 'C' && b == 'Z')
			{
				result += 3;
			}
		}

		return result.ToString();
	}
}
