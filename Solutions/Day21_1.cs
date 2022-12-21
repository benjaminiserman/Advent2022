namespace Advent2022.Solutions;
using System.Numerics;
using System.Text.RegularExpressions;

internal static class Day21_1
{
	record Monkey(string Name)
	{
		public Func<BigInteger> Function { get; set; }
	}

	public static string Execute()
	{
		var input = Program.GetAllInput()
			.Replace("\r", string.Empty)
			.Split("\n")
			.Select(s => s)
			.ToList();
		dynamic result = 0;

		var monkeys = input
			.Select(line => new Monkey(line.Split(':')[0]))
			.ToDictionary(m => m.Name, m => m);

		foreach (var line in input)
		{
			var match = Regex.Match(line, @"([a-z]{4}): (\d+)");
			if (match.Success)
			{
				var num = int.Parse(match.Groups[2].Value);
				monkeys[match.Groups[1].Value].Function = () => num;
			}
			else
			{
				match = Regex.Match(line, @"([a-z]{4}): ([a-z]{4}) ([+/*-]) ([a-z]{4})");
				var monkeyName = match.Groups[1].Value;
				var monkeyA = match.Groups[2].Value;
				var operation = match.Groups[3].Value;
				var monkeyB = match.Groups[4].Value;

				monkeys[monkeyName].Function = () => operation switch
				{
					"+" => monkeys[monkeyA].Function() + monkeys[monkeyB].Function(),
					"-" => monkeys[monkeyA].Function() - monkeys[monkeyB].Function(),
					"*" => monkeys[monkeyA].Function() * monkeys[monkeyB].Function(),
					"/" => monkeys[monkeyA].Function() / monkeys[monkeyB].Function()
				};
			}
		}

		result = monkeys["root"].Function();
		return result.ToString();
	}
}
