namespace Advent2022.Solutions;
using InputHandler;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;

internal static class Day22_2
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

		var length = 50;

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
					else if (!openTiles.Contains(newPosition)) // yes I hard coded the wrapping
					{
						(int x, int y) newerPosition;
						if (0 <= currentPosition.y && currentPosition.y < length) // AB
						{
							if (facing == Direction.Down) // -> C
							{
								newerPosition = openTiles
									.Where(t => length <= t.y && t.y < length * 2
										&& t.y == currentPosition.x - length)
									.MaxBy(t => t.x);
								facing = Direction.Left;
							}
							else if (facing == Direction.Up) // -> F
							{
								if (length <= currentPosition.x && currentPosition.x < length * 2) // A -> F
								{
									newerPosition = openTiles
										.Where(t => length * 3 <= t.y && t.y < length * 4
											&& t.y == currentPosition.x + length * 2)
										.MinBy(t => t.x);
									facing = Direction.Right;
								}
								else if (length * 2 <= currentPosition.x && currentPosition.x < length * 3) // B -> F
								{
									newerPosition = openTiles
										.Where(t => 0 <= t.x && t.x < length
											&& t.x == currentPosition.x - length * 2)
										.MaxBy(t => t.y);
									facing = Direction.Up;
								}
								else
								{
									throw new();
								}
							}
							else if (facing == Direction.Left) // -> E
							{
								newerPosition = openTiles
									.Where(t => length * 2 <= t.y && t.y < length * 3
										&& t.y == (length - currentPosition.y - 1) + length * 2)
									.MinBy(t => t.x);
								facing = Direction.Right;
							}
							else if (facing == Direction.Right) // -> D
							{
								newerPosition = openTiles
									.Where(t => length <= t.x && t.x < length * 2
										&& t.y == (length - currentPosition.x - 1) + length * 2)
									.MaxBy(t => t.x);
								facing = Direction.Left;
							}
							else
							{
								throw new();
							}
						}
						else if (length <= currentPosition.y && currentPosition.y < length * 2) // C
						{
							if (facing == Direction.Left) // -> E
							{
								newerPosition = openTiles
									.Where(t => 0 <= t.x && t.x < length
										&& t.x == currentPosition.y - length)
									.MinBy(t => t.y);
								facing = Direction.Down;
							}
							else if (facing == Direction.Right) // -> B
							{
								newerPosition = openTiles
									.Where(t => length * 2 <= t.x && t.x < length * 3
										&& t.x == currentPosition.y + length)
									.MaxBy(t => t.y);
								facing = Direction.Up;
							}
							else
							{
								throw new();
							}
						}
						else if (length * 2 <= currentPosition.y && currentPosition.y < length * 3) // ED
						{
							if (facing == Direction.Down) // -> F
							{
								newerPosition = openTiles
									.Where(t => length * 3 <= t.y && t.y < length * 4
										&& t.y == currentPosition.x + length * 2)
									.MaxBy(t => t.x);
								facing = Direction.Left;
							}
							else if (facing == Direction.Up) // -> C
							{
								newerPosition = openTiles
									.Where(t => length <= t.y && t.y < length * 2
										&& t.y == currentPosition.x + length)
									.MinBy(t => t.x);
								facing = Direction.Right;
							}
							else if (facing == Direction.Left) // -> A
							{
								newerPosition = openTiles
									.Where(t => 0 <= t.y && t.y < length
										&& t.y == length * 3 - currentPosition.y - 1)
									.MinBy(t => t.x);
								facing = Direction.Right;
							}
							else if (facing == Direction.Right) // -> B
							{
								newerPosition = openTiles
									.Where(t => 0 <= t.y && t.y < length
										&& t.y == length * 3 - currentPosition.y - 1)
									.MaxBy(t => t.x);
								facing = Direction.Left;
							}
							else
							{
								throw new();
							}
						}
						else if (length * 3 <= currentPosition.y && currentPosition.y < length * 4) // F
						{
							if (facing == Direction.Left) // F -> A
							{
								newerPosition = openTiles
									.Where(t => length <= t.x && t.x < length * 2
										&& t.x == currentPosition.y - length * 2)
									.MinBy(t => t.y);
								facing = Direction.Down;
							}
							else if (facing == Direction.Down) // F -> B
							{
								newerPosition = openTiles
									.Where(t => length * 2 <= t.x && t.x < length * 3
										&& t.x == currentPosition.x + length * 2)
									.MinBy(t => t.y);
								facing = Direction.Down;
							}
							else if (facing == Direction.Right) // F -> D
							{
								newerPosition = openTiles
									.Where(t => length <= t.x && t.x < length * 2
										&& t.x == currentPosition.y - length * 2)
									.MaxBy(t => t.y);
								facing = Direction.Up;
							}
							else
							{
								throw new();
							}
						}
						else
						{
							throw new();
						}

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
