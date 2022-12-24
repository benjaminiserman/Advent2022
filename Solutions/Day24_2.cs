namespace Advent2022.Solutions;

internal static class Day24_2
{
	enum Direction
	{
		Up, Down, Left, Right
	}
	record Blizzard(Direction Direction)
	{
		public int X { get; set; }
		public int Y { get; set; }

		public Blizzard Move()
		{
			return Direction switch
			{
				Direction.Up => new(Direction)
				{
					X = X,
					Y = Y - 1
				},
				Direction.Left => new(Direction)
				{
					X = X - 1,
					Y = Y
				},
				Direction.Right => new(Direction)
				{
					X = X + 1,
					Y = Y
				},
				Direction.Down => new(Direction)
				{
					X = X,
					Y = Y + 1
				},
			};
		}
	}

	record BoardState(List<Blizzard> Blizzards, HashSet<(int x, int y)> BlizzardPositions, int Time)
	{
		public void Print(int xMin, int xMax, int yMin, int yMax)
		{
			Console.WriteLine($"Time: {Time}, blizzard: {Blizzards.Count}, occluded: {BlizzardPositions.Count}");

			for (int j = yMin; j <= yMax; j++)
			{
				for (int i = xMin; i <= xMax; i++)
				{
					if (i == xMin || i == xMax || j == yMin || j == yMax)
					{
						Console.Write("#");
					}
					else if (!BlizzardPositions.Contains((i, j)))
					{
						Console.Write(".");
					}
					else
					{
						Console.Write("$");
					}
				}

				Console.WriteLine();
			}
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

		var walls = new HashSet<(int x, int y)>();
		var openTiles = new HashSet<(int x, int y)>();
		var blizzards = new List<Blizzard>();

		for (var i = 0; i < input.Count; i++)
		{
			for (var j = 0; j < input[i].Length; j++)
			{
				switch (input[i][j])
				{
					case '#':
						walls.Add((j, i));
						break;
					case '.':
						openTiles.Add((j, i));
						break;
					case '^':
						openTiles.Add((j, i));
						blizzards.Add(new(Direction.Up)
						{
							X = j,
							Y = i,
						});
						break;
					case 'v':
						openTiles.Add((j, i));
						blizzards.Add(new(Direction.Down)
						{
							X = j,
							Y = i,
						});
						break;
					case '>':
						openTiles.Add((j, i));
						blizzards.Add(new(Direction.Right)
						{
							X = j,
							Y = i,
						});
						break;
					case '<':
						openTiles.Add((j, i));
						blizzards.Add(new(Direction.Left)
						{
							X = j,
							Y = i,
						});
						break;
				}
			}
		}

		int xMin, xMax, yMin, yMax;
		xMin = walls.Min(x => x.x);
		xMax = walls.Max(x => x.x);
		yMin = walls.Min(x => x.y);
		yMax = walls.Max(x => x.y);

		List<BoardState> boardStates = new()
		{
			new(blizzards, blizzards.Select(x => (x.X, x.Y)).ToHashSet(), 0)
		};

		boardStates.First().Print(xMin, xMax, yMin, yMax);

		BoardState GetBoardState(int i)
		{
			if (i < boardStates.Count)
			{
				return boardStates[i];
			}
			else
			{
				return SimulateBoardState(i);
			}
		}

		BoardState SimulateBoardState(int target)
		{
			var lastState = boardStates.Last();
			var blizzards = new List<Blizzard>();
			for (var i = boardStates.Count; i <= target; i++)
			{
				Console.WriteLine($"simulating {i}");
				foreach (var blizzard in lastState.Blizzards)
				{
					var newBlizzard = blizzard.Move();
					if (newBlizzard.X >= xMax)
					{
						newBlizzard = new(newBlizzard.Direction)
						{
							X = xMin + 1,
							Y = newBlizzard.Y
						};
					}
					else if (newBlizzard.Y >= yMax)
					{
						newBlizzard = new(newBlizzard.Direction)
						{
							X = newBlizzard.X,
							Y = yMin + 1
						};
					}
					else if (newBlizzard.X <= xMin)
					{
						newBlizzard = new(newBlizzard.Direction)
						{
							X = xMax - 1,
							Y = newBlizzard.Y
						};
					}
					else if (newBlizzard.Y <= yMin)
					{
						newBlizzard = new(newBlizzard.Direction)
						{
							X = newBlizzard.X,
							Y = yMax - 1
						};
					}

					blizzards.Add(newBlizzard);
				}

				lastState = new(blizzards, blizzards.Select(x => (x.X, x.Y)).ToHashSet(), i);
				boardStates.Add(lastState);
				blizzards = new();
			}

			return boardStates.Last();
		}

		var queue = new PriorityQueue<((int x, int y), bool startReached, bool goalReached, BoardState state, List<(int x, int y)> positions), int>();
		var closedSet = new HashSet<((int x, int y), bool startReached, bool goalReached, int t)>();

		(int x, int y) start = openTiles.Where(t => t.y == yMin).MinBy(t => t.x);
		(int x, int y) target = openTiles.Where(t => t.y == yMax).MaxBy(t => t.x);

		int Heuristic((int x, int y) pos, bool startReached, bool goalReached, BoardState state)
		{
			//return state.Time;
			if (!goalReached || (startReached && goalReached))
			{
				return state.Time + Math.Abs(target.x - pos.x) + Math.Abs(target.y - pos.y);
			}
			else
			{
				return state.Time + Math.Abs(start.x - pos.x) + Math.Abs(start.y - pos.y);
			}
		}

		result = int.MaxValue;
		queue.Enqueue((start, false, false, boardStates.First(), new() { start }), Heuristic(start, false, false, boardStates.First()));
		int maxTime = 0;
		long a = 0;

		while (queue.TryDequeue(out var t, out var p))
		{
			var ((x, y), startReached, goalReached, state, list) = t;

			var currentState = GetBoardState(state.Time + 1);

			if (x == start.x && y == start.y && goalReached)
			{
				startReached = true;
			}

			if (x == target.x && y == target.y)
			{
				goalReached = true;
				if (startReached)
				{
					if (currentState.Time - 1 < result)
					{
						result = currentState.Time - 1;
						Console.WriteLine(result);
					}

					GetBoardState(currentState.Time - 1).Print(xMin, xMax, yMin, yMax);
					break;
				}
			}

			bool Try(int x, int y)
			{
				if (openTiles.Contains((x, y))
					&& !currentState.BlizzardPositions.Contains((x, y))
					&& !closedSet.Contains(((x, y), startReached, goalReached, currentState.Time)))
				{
					closedSet.Add(((x, y), startReached, goalReached, currentState.Time));
					queue.Enqueue(((x, y), startReached, goalReached, currentState, null), Heuristic((x, y), startReached, goalReached, currentState));
					return true;
				}

				return false;
			}

			Try(x + 1, y);
			Try(x, y + 1);
			Try(x - 1, y);
			Try(x, y - 1);
			Try(x, y);
		}

		return result.ToString();
	}
}