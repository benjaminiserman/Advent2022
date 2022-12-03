namespace Advent2022.Solutions;
using InputHandler;

internal static class Day03_2
{
	public static string Execute()
	{
		var input = Program.GetAllInput()
			.Replace("\r", string.Empty)
			.Split("\n")
			.Select(s => s)
			.ToList();

		var result = 0;

		foreach (var line in input
			.Select((x, i) => new { x, i })
			.GroupBy(x => x.i / 3)
			.Select(g => g.Select(x => x.x)))
		{
			var group = line.ToList();
			var c = group[0]
				.First(x => group[1].Contains(x) && group[2].Contains(x));

			if (c is >= 'A' and <= 'Z')
			{
				result += c - 'A' + 27;
			}
			else
			{
				result += c - 'a' + 1;
			}
		}

		return result.ToString();
	}
}
