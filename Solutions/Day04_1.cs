﻿namespace Advent2022.Solutions;
using InputHandler;

internal static class Day04_1
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
			var split = line.Split(",");
			var left = split[0].Split("-");
			var right = split[1].Split("-");

			int a = int.Parse(left[0]);
			int b = int.Parse(left[1]);
			int x = int.Parse(right[0]);
			int y = int.Parse(right[1]);

			if (a <= x && b >= y)
			{
				result++;
			}
			else if (x <= a && y >= b)
			{
				result++;
			}
		}

		return result.ToString();
	}
}
