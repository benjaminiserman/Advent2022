namespace Advent2022.Solutions;
using InputHandler;

internal static class Day12_1
{
	public static string Execute()
	{
		var input = Program.GetAllInput()
			.Replace("\r", string.Empty)
			.Split("\n")
			.Select(s => s)
			.ToList();
		dynamic result = 0;

		(int x, int y) start = default;
		(int x, int y) end = default;
		Dictionary<(int, int), int> heights = new();
		Dictionary<(int, int), int> scores = new();
		Dictionary<(int, int), bool> marks = new();
		for (int i = 0; i < input.Count; i++)
		{
			for (int j = 0; j < input[0].Length; j++)
			{
				if (input[i][j] == 'S')
				{
					start = (i, j);
					heights.Add((i, j), 'a' - 'a');
				}
				else if (input[i][j] == 'E')
				{
					end = (i, j);
					heights.Add((i, j), 'z' - 'a');
				}
				else
				{
					heights.Add((i, j), input[i][j] - 'a');
				}

				marks.Add((i, j), false);
				scores.Add((i, j), int.MaxValue);
			}
		}

		bool Valid(int x, int y) => heights.ContainsKey((x, y));

		var queue = new PriorityQueue<(int, int), int>();

		queue.Enqueue(start, 0);

		while (queue.TryDequeue(out (int x, int y) t, out int p))
		{
			int x = t.x;
			int y = t.y;
			marks[(x, y)] = true;
			if (x == end.x && y == end.y)
			{
				result = p;
				break;
			}

			if (Valid(x + 1, y) 
				&& heights[(x, y)] >= heights[(x + 1, y)] - 1
				&& !marks[(x + 1, y)]
				&& p + 1 < scores[(x + 1, y)])
			{
				queue.Enqueue((x + 1, y), p + 1);
				scores[(x + 1, y)] = p + 1;
			}

			if (Valid(x - 1, y)
				&& heights[(x, y)] >= heights[(x - 1, y)] - 1
				&& !marks[(x - 1, y)]
				&& p + 1 < scores[(x - 1, y)])
			{
				queue.Enqueue((x - 1, y), p + 1);
				scores[(x - 1, y)] = p + 1;
			}

			if (Valid(x, y + 1)
				&& heights[(x, y)] >= heights[(x, y + 1)] - 1
				&& !marks[(x, y + 1)]
				&& p + 1 < scores[(x, y + 1)])
			{
				queue.Enqueue((x, y + 1), p + 1);
				scores[(x, y + 1)] = p + 1;
			}

			if (Valid(x, y - 1)
				&& heights[(x, y)] >= heights[(x, y - 1)] - 1
				&& !marks[(x, y - 1)]
				&& p + 1 < scores[(x, y - 1)])
			{
				queue.Enqueue((x, y - 1), p + 1);
				scores[(x, y - 1)] = p + 1;
			}
		}

		return result.ToString();
	}
}
