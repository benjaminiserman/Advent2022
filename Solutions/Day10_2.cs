namespace Advent2022.Solutions;
using InputHandler;

internal static class Day10_2
{
	public static string Execute()
	{
		var input = Program.GetAllInput()
			.Replace("\r", string.Empty)
			.Split("\n")
			.Select(s => s)
			.ToList();
		dynamic result = 0;

		var strengths = new Dictionary<int, int>();
		void AddStrength(int i, int x)
		{
			if (Math.Abs(x - ((i - 1) % 40)) <= 1)
			{
				Console.Write("#");
			}
			else
			{
				Console.Write(" ");
			}

			if (i % 40 == 0)
			{
				Console.WriteLine();
				strengths.Add(i, i * x);
			}
		}

		int cycle = 0;
		int x = 1;
		foreach (var line in input)
		{
			var split = line.Split();
			if (split[0] == "noop")
			{
				cycle++;
				AddStrength(cycle, x);
			}
			else if (split[0] == "addx")
			{
				cycle++;
				AddStrength(cycle, x);
				cycle++;
				AddStrength(cycle, x);
				x += int.Parse(split[1]);
			}
		}

		return result.ToString();
	}
}
