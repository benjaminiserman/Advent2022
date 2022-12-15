namespace Advent2022.Solutions;
using InputHandler;
using System.Text.RegularExpressions;

internal static class Day15_2
{
	record struct Sensor(int SensorX, int SensorY, int BeaconX, int BeaconY)
	{
		public int Range => ManhattanDistance(BeaconX, BeaconY, SensorX, SensorY);
	}

	public static int ManhattanDistance(int ax, int ay, int bx, int by) => Math.Abs(ay - by) + Math.Abs(ax - bx);

	public static IEnumerable<(int, int)> BoundaryFunction(int ax, int ay, int range)
	{
		range += 1;
		yield return (ax - range, ay);
		yield return (ax + range, ay);

		for (int i = ax - range + 1; i < ax + range; i++)
		{
			yield return (i, ay + range -  Math.Abs(ax - i));
			yield return (i, ay - range - Math.Abs(ax - i));
		}
	}

	public static string Execute()
	{
		var input = Program.GetAllInput()
			.Replace("\r", string.Empty)
			.Split("\n")
			.Select(s => s)
			.ToList();
		dynamic result = 0;

		var sensors = new List<Sensor>();
		var minX = int.MaxValue;
		var maxX = int.MinValue;
		var minY = int.MaxValue;
		var maxY = int.MinValue;

		foreach (var line in input)
		{
			var match = Regex.Match(line, @"Sensor at x=(-?\d+), y=(-?\d+): closest beacon is at x=(-?\d+), y=(-?\d+)");

			var sx = int.Parse(match.Groups[1].Value);
			var sy = int.Parse(match.Groups[2].Value);
			var bx = int.Parse(match.Groups[3].Value);
			var by = int.Parse(match.Groups[4].Value);

			minX = Math.Min(minX, Math.Min(sx, bx));
			maxX = Math.Max(maxX, Math.Min(sx, bx));
			minY = Math.Min(minY, Math.Min(sy, by));
			maxY = Math.Max(maxY, Math.Min(sy, by));

			sensors.Add(new(sx, sy, bx, by));
		}

		var maxRange = sensors.Max(x => x.Range);
		sensors = sensors.OrderByDescending(x => x.Range).ToList();

		foreach (var t in BoundaryFunction(0, 0, 3))
		{
			Console.WriteLine($"({t.Item1}, {t.Item2})");
		}

		foreach (var sensor in sensors)
		{
			foreach ((int x, int y) t in BoundaryFunction(sensor.SensorX, sensor.SensorY, sensor.Range))
			{
				int i = t.x;
				int j = t.y;
				if (i < 0 || i > 4000000 || j < 0 || j > 4000000)
					continue;
				if (!sensors.Any(s => ManhattanDistance(s.SensorX, s.SensorY, i, j) <= s.Range))
				{
					result = (long)i * 4000000 + j;
					Console.WriteLine($"({i}, {j}) => {i * 4000000 + j}");
				}
			}
		}

		return result.ToString();
	}
}
