namespace Advent2022.Solutions;
using InputHandler;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;

internal static class Day22_1
{
	enum Direction
	{
		Up, Down, Left, Right
	}

	public static string Execute()
	{
		var input = Program.GetAllInput()
			.Replace("\r", string.Empty)
			.Split("\n")
			.Select(s => s)
			.ToArray();
		dynamic result = 0;

		var openTiles = new HashSet<(int x, int y)>();
		var solidWalls = new HashSet<(int x, int y)>();

		for (int i = 0; i < input.Length - 2; i++)
		{
			for (int j = 0; j < input[i].Length; j++)
			{
				switch (input[i][j])
				{
					case ' ':
						break;
					case '.':
						openTiles.Add((j, i));
						break;
					case '#':
						openTiles.Add((j, i));
						solidWalls.Add((j, i));
						break;
				}
			}
		}

		var startPosition = openTiles.Where(t => t.y == 0).MinBy(t => t.x);
		var currentPosition = startPosition;

		var instructionsLine = input[^1];
		Console.WriteLine($"open tiles: {openTiles.Count}");
		Console.WriteLine($"solid walls: {solidWalls.Count}");
		Console.WriteLine($"instructions: {instructionsLine}");
		Console.WriteLine($"start: {startPosition.x}, {startPosition.y}");

		//var match = Regex.Match(instructionsLine, @"(?:(\d+)([RL]))+(\d+)");
		var match = Regex.Split(instructionsLine, "(?<=\\d)(?=\\D)|(?=\\d)(?<=\\D)");
		var facing = Direction.Right;
		foreach (var group in match)
		{
			var instruction = group;
			//Console.WriteLine($"inst: {instruction}");

			if (int.TryParse(instruction, out var steps))
			{
				var newPosition = currentPosition;
				for (int i = 0; i < steps; i++)
				{
					newPosition = facing switch
					{
						Direction.Left => (currentPosition.x - 1, currentPosition.y),
						Direction.Right => (currentPosition.x + 1, currentPosition.y),
						Direction.Up => (currentPosition.x, currentPosition.y - 1),
						Direction.Down => (currentPosition.x, currentPosition.y + 1),
					};

					if (solidWalls.Contains(newPosition))
					{
						newPosition = currentPosition;
					}
					else if (!openTiles.Contains(newPosition))
					{
						var newerPosition = facing switch
						{
							Direction.Right => openTiles
								.Where(t => t.y == newPosition.y)
								.MinBy(t => t.x),
							Direction.Left => openTiles
								.Where(t => t.y == newPosition.y)
								.MaxBy(t => t.x),
							Direction.Down => openTiles
								.Where(t => t.x == newPosition.x)
								.MinBy(t => t.y),
							Direction.Up => openTiles
								.Where(t => t.x == newPosition.x)
								.MaxBy(t => t.y)
						};

						if (!solidWalls.Contains(newerPosition))
						{
							newPosition = newerPosition;
						}
						else
						{
							newPosition = currentPosition;
						}
					}

					currentPosition = newPosition;
					//Console.WriteLine($"pos: {newPosition.x}, {newPosition.y}");
				}
			}
			else
			{
				if (instruction == "L")
				{
					facing = facing switch
					{
						Direction.Left => Direction.Down,
						Direction.Down => Direction.Right,
						Direction.Right => Direction.Up,
						Direction.Up => Direction.Left
					};
				}
				else
				{
					facing = facing switch
					{
						Direction.Left => Direction.Up,
						Direction.Up => Direction.Right,
						Direction.Right => Direction.Down,
						Direction.Down => Direction.Left
					};
				}
			}
		}

		result = 1000 * (currentPosition.y + 1) + 4 * (currentPosition.x + 1) + facing switch
		{
			Direction.Right => 0,
			Direction.Down => 1,
			Direction.Left => 2,
			Direction.Up => 3,
		};

		return result.ToString();
	}
}
