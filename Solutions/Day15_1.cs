namespace Advent2022.Solutions;
using InputHandler;
using System.Text.RegularExpressions;

internal static class Day15_1
{
	record struct Sensor(int SensorX, int SensorY, int BeaconX, int BeaconY)
	{
		public int Range => ManhattanDistance(BeaconX, BeaconY, SensorX, SensorY);
	}

	public static int ManhattanDistance(int ax, int ay, int bx, int by) => Math.Abs(ay - by) + Math.Abs(ax - bx);

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

		foreach (var line in input)
		{
			var match = Regex.Match(line, @"Sensor at x=(-?\d+), y=(-?\d+): closest beacon is at x=(-?\d+), y=(-?\d+)");

			var sx = int.Parse(match.Groups[1].Value);
			var sy = int.Parse(match.Groups[2].Value);
			var bx = int.Parse(match.Groups[3].Value);
			var by = int.Parse(match.Groups[4].Value);

			minX = Math.Min(minX, Math.Min(sx, bx));
			maxX = Math.Max(maxX, Math.Min(sx, bx));

			sensors.Add(new(sx, sy, bx, by));
			Console.WriteLine(sensors[^1]);
		}

		var maxRange = sensors.Max(x => x.Range);
		var count = 0;
		for (var i = minX - maxRange; i <= maxX + maxRange; i++)
		{
			if (sensors.Any(s => ManhattanDistance(s.SensorX, s.SensorY, i, 2000000) <= s.Range))
			{
				count++;
			}
		}

		result = count - 1;
		return result.ToString();
	}
}
