namespace Advent2022.Solutions;

internal static class Day09_1
{
	public static string Execute()
	{
		var input = Program.GetAllInput()
			.Replace("\r", string.Empty)
			.Split("\n")
			.Select(s => s)
			.ToList();
		dynamic result = 0;

		var head = (0, 0);
		var tail = (0, 0);

		var set = new HashSet<(int, int)>() { (0, 0) };

		int Distance(int x1, int y1, int x2, int y2)
		{
			return Math.Max(Math.Abs(y2 - y1), Math.Abs(x2 - x1));
		}

		foreach (var line in input)
		{
			var split = line.Split();
			var d = int.Parse(split[1]);
			var dx = 0;
			var dy = 0;
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

			if (Distance(head.Item1, head.Item2, tail.Item1, tail.Item2) > 1)
			{
				dx = -tail.Item1 + head.Item1;
				dy = -tail.Item2 + head.Item2;

				while (dx != 0 && dy != 0 && Distance(head.Item1, head.Item2, tail.Item1, tail.Item2) > 1)
				{
					tail = (tail.Item1 + Math.Sign(dx), tail.Item2 + Math.Sign(dy));
					dx = -tail.Item1 + head.Item1;
					dy = -tail.Item2 + head.Item2;
					set.Add(tail);
				}

				if (Math.Abs(dx) > 1)
				{
					for (int i = 1; i < Math.Abs(dx); i++)
					{
						tail = (tail.Item1 + Math.Sign(dx), tail.Item2);
						set.Add(tail);
					}
				}

				dx = -tail.Item1 + head.Item1;
				dy = -tail.Item2 + head.Item2;

				if (Math.Abs(dy) > 1)
				{
					for (int i = 1; i < Math.Abs(dy); i++)
					{
						tail = (tail.Item1, tail.Item2 + Math.Sign(dy));
						set.Add(tail);
					}
				}
			}
		}

		result = set.Count();

		return result.ToString();
	}
}
