namespace Advent2022.Solutions;
using InputHandler;

internal static class Day10_1
{
	public static string Execute()
	{
		//var input = Input.ListUntilWhiteSpace(s => int.Parse(s), Program.GetLineOfInput);
		var input = Program.GetAllInput()
			.Replace("\r", string.Empty)
			.Split("\n")
			.Select(s => s)
			.ToList();
		dynamic result = 0;

		var strengths = new Dictionary<int, int>();
		void AddStrength(int i, int x)
		{
			if (i % 20 == 0)
			{
				//Console.WriteLine($"{i}: {i * x}");
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

		result = strengths[20] + strengths[60] + strengths[100] + strengths[140] + strengths[180] + strengths[220];

		return result.ToString();
	}
}
