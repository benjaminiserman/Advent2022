namespace Advent2022.Solutions;
using InputHandler;
using System.Text.RegularExpressions;

internal static class Day17_1
{
	public static string Execute()
	{
		var input = Program.GetAllInput()
			.Replace("\r", string.Empty)
			.Split("\n")
			.Select(s => s)
			.ToList();
		dynamic result = 0;

		var jets = input[0];

		var shapes = new List<List<(int x, int y)>>()
		{
			new() {(0, 0), (1, 0), (2, 0), (3, 0)},
			new() {(0, 1), (1, 1), (2, 1), (1, 0), (1, 2)},
			new() { (0, 0), (1, 0), (2, 0), (2, 1), (2, 2)},
			new() {(0, 0), (0, 1), (0, 2), (0, 3)},
			new() {(0, 0), (1, 0), (1, 1), (0, 1)},
		};

		var positions = new HashSet<(int x, int y)>();
		T GetFrom<T>(IList<T> list, int i) => list[i % list.Count()];
		int GetTop() => positions.Max(x => x.y);
		var spawnPos = (x: 2, y: 3);
		int chamberWidth = 7;

		bool Valid(int x, int y)
		{
			if (x < 0 || x >= chamberWidth)
			{
				return false;
			}

			if (y < 0)
			{
				return false;
			}

			if (positions.Contains((x, y)))
			{
				return false;
			}

			return true;
		}

		int air = 0;
		for (int shapeID = 0; shapeID < 2022; shapeID++)
		{
			var pos = spawnPos;
			var shape = GetFrom(shapes, shapeID);

			while (true)
			{
				var dir = jets[air++ % jets.Length];
				switch (dir)
				{
					case '<':
						if (shape.All(shapePos => Valid(pos.x + shapePos.x - 1, pos.y + shapePos.y)))
						{
							pos = (pos.x - 1, pos.y);
						}
						break;
					case '>':
						if (shape.All(shapePos => Valid(pos.x + shapePos.x + 1, pos.y + shapePos.y)))
						{
							pos = (pos.x + 1, pos.y);
						}
						break;
				}

				if (shape.All(shapePos => Valid(pos.x + shapePos.x, pos.y + shapePos.y - 1)))
				{
					pos = (pos.x, pos.y - 1);
				}
				else
				{
					foreach (var shapePos in shape)
					{
						positions.Add((pos.x + shapePos.x, pos.y + shapePos.y));
					}

					break;
				}
			}

			spawnPos = (2, GetTop() + 4);
		}

		result = GetTop() + 1;

		return result.ToString();
	}
}
