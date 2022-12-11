namespace Advent2022.Solutions;
using InputHandler;
using System.Text.RegularExpressions;

internal static class Day11_1
{
	class Monkey
	{
		public int id = 0;
		public Queue<int> items = new();
		public Func<int, int> operation = x => x;
		public Predicate<int> test = x => true;
		public int throwIfTrue = 0, throwIfFalse = 0;
		public int interactCount = 0;
	}

	record Item(int startValue)
	{
		public List<int> factors = new();
		public int offset = 0;
	}

	public static string Execute()
	{
		var input = Program.GetAllInput()
			.Replace("\r", string.Empty)
			.Split("\n\n")
			.Select(s => s
				.Split("\n"))
				.ToList()
			.ToList();
		dynamic result = 0;

		var monkeys = new List<Monkey>();

		foreach (var monkeyText in input)
		{
			var monkey = new Monkey();

			monkey.id = int.Parse(Regex.Match(monkeyText[0], @"Monkey (\d+)").Groups[1].Value);

			var items = Regex.Match(monkeyText[1], @"Starting items: ((\d+, )*\d+)").Groups[1].Value.Split(",").Select(x => int.Parse(x));
			foreach (var item in items)
			{
				monkey.items.Enqueue(item);
			}

			var operationMatch = Regex.Match(monkeyText[2], @"Operation: new = old ([*+]) (old|\d+)");
			var op = operationMatch.Groups[1].Value;
			var operand = operationMatch.Groups[2].Value;
			Func<int, int> opFunction = null;
			if (op == "*")
			{
				if (operand == "old")
				{
					opFunction = x => x * x;
				}
				else
				{
					int y = int.Parse(operand);
					opFunction = x => x * y;
				}
			}
			else if (op == "+")
			{
				if (operand == "old")
				{
					opFunction = x => x + x;
				}
				else
				{
					int y = int.Parse(operand);
					opFunction = x => x + y;
				}
			}
			monkey.operation = opFunction ?? throw new("bad");

			var testDivBy = int.Parse(Regex.Match(monkeyText[3], @"Test: divisible by (\d+)").Groups[1].Value);
			monkey.test = x => x % testDivBy == 0;

			monkey.throwIfTrue = int.Parse(Regex.Match(monkeyText[4], @"If true: throw to monkey (\d+)").Groups[1].Value);
			monkey.throwIfFalse = int.Parse(Regex.Match(monkeyText[5], @"If false: throw to monkey (\d+)").Groups[1].Value);

			monkeys.Add(monkey);
		}

		for (int i = 0; i < 20; i++)
		{
			foreach (var monkey in monkeys)
			{
				while (monkey.items.Any())
				{
					var item = monkey.items.Dequeue();
					item = monkey.operation(item);
					item /= 3;
					if (monkey.test(item))
					{
						monkeys[monkey.throwIfTrue].items.Enqueue(item);
					}
					else
					{
						monkeys[monkey.throwIfFalse].items.Enqueue(item);
					}

					monkey.interactCount++;
				}
			}
		}

		var list = monkeys.OrderByDescending(monkey => monkey.interactCount).ToList();
		result = list[0].interactCount * list[1].interactCount;

		return result.ToString();
	}
}
