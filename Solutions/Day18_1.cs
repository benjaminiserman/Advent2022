namespace Advent2022.Solutions;

internal static class Day18_1
{
	public static string Execute()
	{
		var input = Program.GetAllInput()
			.Replace("\r", string.Empty)
			.Split("\n")
			.Select(s => s)
			.ToList();
		dynamic result = 0;

		HashSet<(int x, int y, int z)> positions = new();
		foreach (var line in input)
		{
			var split = line.Split(',');
			(int, int, int) pos = 
			(
				int.Parse(split[0]),
				int.Parse(split[1]),
				int.Parse(split[2])
			);

			positions.Add(pos);
		}

		int surface = 0;
		foreach (var pos in positions)
		{
			if (!positions.Contains((pos.x + 1, pos.y, pos.z)))
			{
				surface++;
			}
			if (!positions.Contains((pos.x - 1, pos.y, pos.z)))
			{
				surface++;
			}
			if (!positions.Contains((pos.x, pos.y + 1, pos.z)))
			{
				surface++;
			}
			if (!positions.Contains((pos.x, pos.y - 1, pos.z)))
			{
				surface++;
			}
			if (!positions.Contains((pos.x, pos.y, pos.z + 1)))
			{
				surface++;
			}
			if (!positions.Contains((pos.x, pos.y, pos.z - 1)))
			{
				surface++;
			}
		}

		result = surface;
		return result.ToString();
	}
}
