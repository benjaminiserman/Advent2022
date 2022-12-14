namespace Advent2022.Solutions;
using InputHandler;

internal static class Day14_2
{
	public static string Execute()
	{
		//var input = Input.ListUntilWhiteSpace(s => int.Parse(s), Program.GetLineOfInput);
		var input = Program.GetAllInput()
			.Replace("\r", string.Empty)
			.Split("\n")
			.Select(s => s)
			.ToList();

		Dictionary<(int x, int y), char> dict = new();
		int bottom;

		static void Set<T1, T2>(Dictionary<T1, T2> dict, T1 key, T2 value)
		{
			if (dict.ContainsKey(key))
			{
				dict[key] = value;
			}
			else
			{
				dict.Add(key, value);
			}
		}

		bool IsOccupied((int x, int y) t)
		{
			if (dict.ContainsKey(t))
			{
				return dict[t] != ' ';
			}
			else if (t.y >= bottom + 2)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		(int x, int y) ParseTuple(string s)
		{
			var split = s.Split(",");
			var x = int.Parse(split[0]);
			var y = int.Parse(split[1]);
			return (x, y);
		}

		foreach (var line in input)
		{
			var split = line.Split(" -> ");
			var start = ParseTuple(split[0]);
			foreach (var following in split[1..])
			{
				var tuple = ParseTuple(following);
				for (var i = Math.Min(start.x, tuple.x); i <= Math.Max(start.x, tuple.x); i++)
				{
					for (var j = Math.Min(start.y, tuple.y); j <= Math.Max(start.y, tuple.y); j++)
					{
						dict.TryAdd((i, j), '#');
					}
				}

				start = tuple;
			}
		}

		bottom = dict.Keys.Max(t => t.y);
		bool cont = true;
		int count = 0;
		while (cont)
		{
			count++;
			Set(dict, (500, 0), 'O');
			var sandPos = (x: 500, y: 0);

			bool Try(int x, int y)
			{
				if (!IsOccupied((sandPos.x + x, sandPos.y + y)))
				{
					Set(dict, sandPos, ' ');
					Set(dict, (sandPos.x + x, sandPos.y + y), 'O');
					sandPos = (sandPos.x + x, sandPos.y + y);
					return true;
				}

				return false;
			}

			while (true)
			{
				var test = Try(0, 1);
				if (!test)
				{
					test = Try(-1, 1);
				}

				if (!test)
				{
					test = Try(1, 1);
				}

				if (sandPos == (500, 0))
				{
					cont = false;
					break;
				}

				if (!test)
				{
					break;
				}
			}
		}

		dynamic result = count;

		return result.ToString();
	}
}
