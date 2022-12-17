namespace Advent2022.Solutions;
using InputHandler;
using System.Text.RegularExpressions;

internal static class Day17_2
{
	record struct HeightMap(byte a, byte b, byte c, byte d, byte e, byte f, byte g, int air);

	public static string Execute()
	{
		var input = Program.GetAllInput()
			.Replace("\r", string.Empty)
			.Split("\n")
			.Select(s => s)
			.ToList();
		dynamic result = 0;

		var jets = input[0];

		var shapes = new List<List<(long x, long y)>>()
		{
			new() {(0, 0), (1, 0), (2, 0), (3, 0)},
			new() {(0, 1), (1, 1), (2, 1), (1, 0), (1, 2)},
			new() { (0, 0), (1, 0), (2, 0), (2, 1), (2, 2)},
			new() {(0, 0), (0, 1), (0, 2), (0, 3)},
			new() {(0, 0), (1, 0), (1, 1), (0, 1)},
		};

		var positions = new HashSet<(long x, long y)>();
		Dictionary<HeightMap, (long cycle, long height)> heightMaps = new();
		long heightMapMin = 0;
		T GetFrom<T>(IList<T> list, long i) => list[(int)(i % list.Count())];
		long top = -1;
		(long x, long y) spawnPos = (x: 2, y: top + 4);
		long chamberWidth = 7;
		byte[] currentHeightMap = new byte[chamberWidth];

		bool Valid(long x, long y)
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

		var target = 1_000_000_000_000;
		long air = 0;
		long targetCycle = -1;
		long targetTop = -1;
		(long, HeightMap) recorded = default;
		(long, HeightMap) Simulate(long start, long target, bool check)
		{
			for (long shapeID = start; shapeID < target; shapeID++)
			{
				var pos = spawnPos;
				var shape = GetFrom(shapes, shapeID);

				while (true)
				{
					var dir = jets[(int)(air++ % jets.Length)];
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
							if (pos.y + shapePos.y - heightMapMin > currentHeightMap[pos.x])
							{
								currentHeightMap[pos.x] = (byte)(pos.y + shapePos.y - heightMapMin);
							}

							if (pos.y + shapePos.y > top)
							{
								top = pos.y + shapePos.y;
							}
						}

						break;
					}
				}

				if (check)
				{
					var min = currentHeightMap.Min();
					if (min != 0)
					{
						heightMapMin += min;
						for (int i = 0; i < currentHeightMap.Length; i++)
						{
							currentHeightMap[i] -= min;
						}

						min = 0;
					}

					var record = new HeightMap
					(
						currentHeightMap[0],
						currentHeightMap[1],
						currentHeightMap[2],
						currentHeightMap[3],
						currentHeightMap[4],
						currentHeightMap[5],
						currentHeightMap[6],
						(int)(air % jets.Length)
					);

					if (heightMaps.ContainsKey(record))
					{
						if (targetCycle == -1)
						{
							Console.WriteLine($"loop found! {shapeID}");
							Console.WriteLine($"repeat with {heightMaps[record].cycle}");
							Console.WriteLine($"height: {top} - {heightMaps[record].height}");
							targetTop = top;
							targetCycle = shapeID - heightMaps[record].cycle;
							recorded = (shapeID, record);
							spawnPos = (2, top + 4);
							return recorded;
						}
						else
						{
							targetCycle--;
							if (targetCycle == 0)
							{
								return recorded;
							}
						}
					}
					else
					{
						if (targetCycle != -1)
						{
							Console.WriteLine("broken!");
							throw new();
						}
						heightMaps.Add(record, (shapeID, top));
					}
				}

				spawnPos = (2, top + 4);
			}

			return (-1, default);
		}

		var (loop, map) = Simulate(0, target, true);
		if (loop != -1)
		{
			var loopLength = loop - heightMaps[map].cycle;
			var loopHeight = top - heightMaps[map].height;
			var currentTop = top;
			var cycles = target / loopLength;
			var height = top + loopHeight * (cycles - 1);

			Console.WriteLine($"ll: {loopLength}, lh: {loopHeight}");

			var mod = target % loopLength;

			Simulate(heightMaps[map].cycle + loopLength * cycles + 1, target, false);
			height += top - currentTop;
			result = height + 1;
		}
		else
		{
			result = top + 1;
		}

		for (var j = top; j >= top - 10; j--)
		{
			for (int i = 0; i < chamberWidth; i++)
			{
				if (positions.Contains((i, j)))
				{
					Console.Write("#");
				}
				else
				{
					Console.Write(" ");
				}
			}

			Console.WriteLine();
		}

		return result.ToString();
	}
}
