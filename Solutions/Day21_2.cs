namespace Advent2022.Solutions;
using System.Numerics;
using System.Text.RegularExpressions;

internal static class Day21_2
{
	record Monkey(string Name)
	{
		public Monkey MonkeyA { get; set; } = null;
		public Monkey MonkeyB { get; set; } = null;
		public string Operation { get; set; } = null;
		public Func<BigInteger> Function { get; set; }

		public bool IsHumanSide
		{
			get
			{
				if (MonkeyA is null || MonkeyB is null)
				{
					return IsHuman;
				}
				else
				{
					return MonkeyA.IsHumanSide || MonkeyB.IsHumanSide;
				}
			}
		}

		public bool HasHuman
		{
			get
			{
				if (MonkeyA is null || MonkeyB is null)
				{
					return false;
				}
				else
				{
					return MonkeyA.IsHuman
						|| MonkeyB.IsHuman;
				}
			}
		}

		public bool IsHuman => Name == "humn";
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
				monkeys[match.Groups[1].Value].Operation = "num";
			}
			else
			{
				match = Regex.Match(line, @"([a-z]{4}): ([a-z]{4}) ([+/*-]) ([a-z]{4})");
				var monkeyName = match.Groups[1].Value;
				var monkeyA = match.Groups[2].Value;
				var operation = match.Groups[3].Value;
				var monkeyB = match.Groups[4].Value;

				if (monkeyName == "root")
				{
					monkeys[monkeyName].Function = () => 
						monkeys[monkeyA].Function() == monkeys[monkeyB].Function() 
						? 1 
						: 0;
				}
				else
				{
					monkeys[monkeyName].Function = () => operation switch
					{
						"+" => monkeys[monkeyA].Function() + monkeys[monkeyB].Function(),
						"-" => monkeys[monkeyA].Function() - monkeys[monkeyB].Function(),
						"*" => monkeys[monkeyA].Function() * monkeys[monkeyB].Function(),
						"/" => monkeys[monkeyA].Function() / monkeys[monkeyB].Function()
					};
				}

				monkeys[monkeyName].MonkeyA = monkeys[monkeyA];
				monkeys[monkeyName].MonkeyB = monkeys[monkeyB];
				monkeys[monkeyName].Operation = operation;
			}
		}

		Monkey humanSideMonkey, otherSideMonkey;
		if (monkeys["root"].MonkeyA.IsHumanSide)
		{
			humanSideMonkey = monkeys["root"].MonkeyA;
			otherSideMonkey = monkeys["root"].MonkeyB;
		}
		else
		{
			humanSideMonkey = monkeys["root"].MonkeyB;
			otherSideMonkey = monkeys["root"].MonkeyA;
		}

		var target = otherSideMonkey.Function();
		Console.WriteLine($"target: {target}");

		var monkeyWithHuman = monkeys.Values.First(x => x.HasHuman);
		BigInteger termA =
			monkeyWithHuman.MonkeyA.IsHuman
			? monkeyWithHuman.MonkeyB.Function()
			: monkeyWithHuman.MonkeyA.Function();

		var currentTarget = target;
		var currentMonkey = monkeys["root"].MonkeyA.IsHumanSide
			? monkeys["root"].MonkeyA
			: monkeys["root"].MonkeyB;
		while (!currentMonkey.IsHuman)
		{
			var otherMonkey = 
				currentMonkey.MonkeyA.IsHumanSide
				? currentMonkey.MonkeyB
				: currentMonkey.MonkeyA;

			var otherMonkeyValue = otherMonkey.Function();
			Console.WriteLine($"{currentMonkey.Operation} {otherMonkeyValue}");
			switch (currentMonkey.Operation)
			{
				case "+":
					currentTarget -= otherMonkeyValue;
					break;
				case "*":
					currentTarget /= otherMonkeyValue;
					break;
				case "-":
					if (otherMonkey == currentMonkey.MonkeyB)
					{
						currentTarget += otherMonkeyValue;
					}
					else
					{
						currentTarget *= -1;
						currentTarget += otherMonkeyValue;
					}
					break;
				case "/":
					if (otherMonkey == currentMonkey.MonkeyB)
					{
						currentTarget *= otherMonkeyValue;
					}
					else
					{
						currentTarget = otherMonkeyValue / currentTarget;
					}
					break;
			}

			currentMonkey =
				currentMonkey.MonkeyA.IsHumanSide
				? currentMonkey.MonkeyA
				: currentMonkey.MonkeyB;
		}

		result = currentTarget;
		return result.ToString();
	}
}
