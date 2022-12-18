namespace Advent2022.Solutions;

internal static class Day18_2
{
	public static string Execute()
	{
		var input = Program.GetAllInput()
			.Replace("\r", string.Empty)
			.Split("\n")
			.Select(s => s)
			.ToList();
		dynamic result = 0;

		int xMin = int.MaxValue, xMax = int.MinValue, yMin = int.MaxValue, yMax = int.MinValue, zMin = int.MaxValue, zMax = int.MinValue;

		HashSet<(int x, int y, int z)> outside = new();
		HashSet<(int x, int y, int z)> positions = new();
		HashSet<(int x, int y, int z)> pending = new();
		foreach (var line in input)
		{
			var split = line.Split(',');
			(int x, int y, int z) pos =
			(
				int.Parse(split[0]),
				int.Parse(split[1]),
				int.Parse(split[2])
			);

			xMin = Math.Min(xMin, pos.x);
			xMax = Math.Max(xMax, pos.x);

			yMin = Math.Min(yMin, pos.y);
			yMax = Math.Max(yMax, pos.y);

			zMin = Math.Min(zMin, pos.z);
			zMax = Math.Max(zMax, pos.z);

			positions.Add(pos);
		}

		Stack<(int x, int y, int z)> floodQueue = new();
		for (int i = xMin - 1; i <= xMax + 1; i++)
		{
			for (int j = yMin - 1; j <= yMax + 1; j++)
			{
				for (int k = zMin - 1; k <= zMax + 1; k++)
				{
					if (i == xMin - 1 || i == xMax + 1 || j == yMin - 1 || j == yMax + 1 || k == zMin - 1 || k == zMax + 1)
					{
						floodQueue.Push((i, j, k));
					}
				}
			}
		}

		while (floodQueue.TryPop(out var pos))
		{
			if (positions.Contains(pos) || outside.Contains(pos))
			{
				continue;
			}

			outside.Add(pos);
			var (x, y, z) = pos;

			if (x < xMin - 1
				|| x > xMax + 1
				|| y < yMin - 1
				|| y > yMax + 1
				|| z < zMin - 1
				|| z > zMax + 1)
			{
				continue;
			}

			floodQueue.Push((x + 1, y, z));
			floodQueue.Push((x - 1, y, z));
			floodQueue.Push((x, y + 1, z));
			floodQueue.Push((x, y - 1, z));
			floodQueue.Push((x, y, z + 1));
			floodQueue.Push((x, y, z - 1));
		}

		int surface = 0;
		foreach (var pos in positions)
		{
			if (outside.Contains((pos.x + 1, pos.y, pos.z)))
			{
				surface++;
			}
			if (outside.Contains((pos.x - 1, pos.y, pos.z)))
			{
				surface++;
			}
			if (outside.Contains((pos.x, pos.y + 1, pos.z)))
			{
				surface++;
			}
			if (outside.Contains((pos.x, pos.y - 1, pos.z)))
			{
				surface++;
			}
			if (outside.Contains((pos.x, pos.y, pos.z + 1)))
			{
				surface++;
			}
			if (outside.Contains((pos.x, pos.y, pos.z - 1)))
			{
				surface++;
			}
		}

		result = surface;
		return result.ToString();
	}
}
