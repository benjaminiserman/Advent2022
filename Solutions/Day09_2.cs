namespace Advent2022.Solutions;

internal static class Day09_2
{
	static int Distance(int x1, int y1, int x2, int y2)
	{
		return Math.Max(Math.Abs(y2 - y1), Math.Abs(x2 - x1));
	}

	static (int, int) Simulate((int, int) head, (int, int) tail, HashSet<(int, int)> set, bool isTail)
	{
		if (Distance(head.Item1, head.Item2, tail.Item1, tail.Item2) > 1)
		{
			int dx = -tail.Item1 + head.Item1;
			int dy = -tail.Item2 + head.Item2;

			if ((dx != 0 || dy != 0) && Distance(head.Item1, head.Item2, tail.Item1, tail.Item2) > 1)
			{
				tail = (tail.Item1 + Math.Sign(dx), tail.Item2 + Math.Sign(dy));
				dx = -tail.Item1 + head.Item1;
				dy = -tail.Item2 + head.Item2;
				if (isTail)
					set.Add(tail);
			}
		}

		return tail;
	}

	public static string Execute()
	{
		var input = Program.GetAllInput()
			.Replace("\r", string.Empty)
			.Split("\n")
			.Select(s => s)
			.ToList();
		dynamic result = 0;

		var rope = Enumerable.Repeat((0, 0), 10).ToList();

		var set = new HashSet<(int, int)>() { (0, 0) };

		foreach (var line in input)
		{
			var split = line.Split();
			var d = int.Parse(split[1]);
			var dx = 0;
			var dy = 0;
			var head = rope[0];
			switch (split[0])
			{
				case "U":
					dy = d;
					head = (head.Item1, head.Item2 + dy);
					break;
				case "D":
					dy = -d;
					head = (head.Item1, head.Item2 + dy);
					break;
				case "L":
					dx = -d;
					head = (head.Item1 + dx, head.Item2);
					break;
				case "R":
					dx = d;
					head = (head.Item1 + dx, head.Item2);
					break;
			}
			rope[0] = head;
			bool flag = true;
			while (flag)
			{
				flag = false;
				for (int i = 1; i < rope.Count; i++)
				{
					rope[i] = Simulate(rope[i - 1], rope[i], set, i == rope.Count - 1);
					if (Distance(rope[i].Item1, rope[i].Item2, rope[i - 1].Item1, rope[i - 1].Item2) > 1)
						flag = true;
				}
			}
		}

		Console.WriteLine("END");
		foreach (var (x, y) in set)
		{
			Console.WriteLine($" - ({x}, {y})");
		}

		result = set.Count();

		return result.ToString();
	}
}
