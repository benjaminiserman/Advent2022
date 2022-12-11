namespace Advent2022.Solutions;
using InputHandler;
using System.Text.RegularExpressions;

internal static class Day11_2
{
	class Monkey
	{
		public int id = 0;
		public Queue<Item> items = new();
		public bool add, mult, square;
		public int factor;
		public int throwIfTrue = 0, throwIfFalse = 0;
		public long interactCount = 0;
		public int testFactor;
	}

	record Item(int startValue)
	{
		public Dictionary<int, int> modulos = new();

		public void Init(List<int> testFactors)
		{
			foreach (var testFactor in testFactors)
			{
				modulos.Add(testFactor, startValue % testFactor);
			}
		}
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
		var testFactors = new List<int>();

		foreach (var monkeyText in input)
		{
			var monkey = new Monkey();

			monkey.id = int.Parse(Regex.Match(monkeyText[0], @"Monkey (\d+)").Groups[1].Value);

			var items = Regex.Match(monkeyText[1], @"Starting items: ((\d+, )*\d+)").Groups[1].Value.Split(",").Select(x => int.Parse(x));
			foreach (var item in items)
			{
				monkey.items.Enqueue(new(item));
			}

			var operationMatch = Regex.Match(monkeyText[2], @"Operation: new = old ([*+]) (old|\d+)");
			var op = operationMatch.Groups[1].Value;
			var operand = operationMatch.Groups[2].Value;
			if (op == "*")
			{
				if (operand == "old")
				{
					monkey.square = true;
				}
				else
				{
					monkey.factor = int.Parse(operand);
					monkey.mult = true;
				}
			}
			else if (op == "+")
			{
				monkey.add = true;
				monkey.factor = int.Parse(operand);
			}

			var testDivBy = int.Parse(Regex.Match(monkeyText[3], @"Test: divisible by (\d+)").Groups[1].Value);
			testFactors.Add(testDivBy);
			monkey.testFactor = testDivBy;

			monkey.throwIfTrue = int.Parse(Regex.Match(monkeyText[4], @"If true: throw to monkey (\d+)").Groups[1].Value);
			monkey.throwIfFalse = int.Parse(Regex.Match(monkeyText[5], @"If false: throw to monkey (\d+)").Groups[1].Value);

			monkeys.Add(monkey);
		}

		foreach (var monkey in monkeys)
		{
			foreach (var item in monkey.items)
			{
				item.Init(testFactors);
			}
		}

		for (int i = 0; i < 10_000; i++)
		{
			foreach (var monkey in monkeys)
			{
				while (monkey.items.Any())
				{
					var item = monkey.items.Dequeue();

					if (monkey.square)
					{
						foreach (var modulo in testFactors)
						{
							var curr = item.modulos[modulo];
							curr *= curr;
							curr %= modulo;
							item.modulos[modulo] = curr;
						}
					}
					else if (monkey.mult)
					{
						foreach (var modulo in testFactors)
						{
							var curr = item.modulos[modulo];
							curr *= monkey.factor;
							curr %= modulo;
							item.modulos[modulo] = curr;
						}
					}
					else if (monkey.add)
					{
						foreach (var modulo in testFactors)
						{
							var curr = item.modulos[modulo];
							curr += monkey.factor;
							curr %= modulo;
							item.modulos[modulo] = curr;
						}
					}

					var y = item.modulos[monkey.testFactor] == 0 ? monkey.throwIfTrue : monkey.throwIfFalse;

					if (item.modulos[monkey.testFactor] == 0)
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
