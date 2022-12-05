namespace Advent2022.Solutions;
using InputHandler;
using System.Text.RegularExpressions;

internal static class Day05_1
{
	public static string Execute()
	{
		var crates = Input.ListUntilWhiteSpace(s => s, Program.GetLineOfInput);
		var instructions = Input.ListUntilWhiteSpace(s => s, Program.GetLineOfInput);

		crates.Reverse();
		var num = crates[0].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(s => int.Parse(s)).Max();
		crates.RemoveAt(0);

		var stacks = new List<Stack<char>>();
		for (int i = 0; i < num; i++)
		{
			stacks.Add(new Stack<char>());
		}

		foreach (var row in crates)
		{
			for (int i = 1, j = 0; i < row.Length; i += 4, j++)
			{
				if (row[i] != ' ')
				{
					stacks[j].Push(row[i]);
				}
			}
		}

		foreach (var instruction in instructions)
		{
			var match = Regex.Match(instruction, @"move (\d+) from (\d+) to (\d+)");
			var move = int.Parse(match.Groups[1].Value);
			var from = int.Parse(match.Groups[2].Value);
			var to = int.Parse(match.Groups[3].Value);


			foreach (var _ in Enumerable.Range(0, move))
			{
				var popped = stacks[from - 1].Pop();
				stacks[to - 1].Push(popped);
			}
		}

		var result = string.Join("", stacks.Select(s => s.Peek()));

		return result.ToString();
	}
}
