namespace Advent2022.Solutions;
using System.Linq;

internal static class Day23_2
{
	public static string Execute()
	{
		var input = Program.GetAllInput()
			.Replace("\r", string.Empty)
			.Split("\n")
			.Select(s => s)
			.ToList();
		dynamic result = 0;

		var elves = new HashSet<(int x, int y)>();
		var proposals = new Dictionary<(int x, int y), ((int x, int y) elf, int v)>();
		for (int i = 0; i < input.Count; i++)
		{
			for (int j = 0; j < input[i].Length; j++)
			{
				if (input[i][j] == '#')
				{
					elves.Add((j, i));
				}
			}
		}

		Print();

		for (int i = 0; true; i++)
		{
			foreach (var elf in elves)
			{
				void AddProposal((int x, int y) position, (int x, int y) elf)
				{
					if (proposals.ContainsKey(position))
					{
						proposals[position] = (elf, 100);
					}
					else
					{
						proposals.Add(position, (elf, 1));
					}
				}

				var (x, y) = elf;

				var west = () =>
				{
					if (!elves.Contains((x - 1, y - 1))
						&& !elves.Contains((x - 1, y))
						&& !elves.Contains((x - 1, y + 1)))
					{
						AddProposal((x - 1, y), elf);
						return true;
					}

					return false;
				};

				var east = () =>
				{
					if (!elves.Contains((x + 1, y - 1))
						&& !elves.Contains((x + 1, y))
						&& !elves.Contains((x + 1, y + 1)))
					{
						AddProposal((x + 1, y), elf);
						return true;
					}

					return false;
				};

				var north = () =>
				{
					if (!elves.Contains((x - 1, y - 1))
						&& !elves.Contains((x, y - 1))
						&& !elves.Contains((x + 1, y - 1)))
					{
						AddProposal((x, y - 1), elf);
						return true;
					}

					return false;
				};

				var south = () =>
				{
					if (!elves.Contains((x - 1, y + 1))
						&& !elves.Contains((x, y + 1))
						&& !elves.Contains((x + 1, y + 1)))
					{
						AddProposal((x, y + 1), elf);
						return true;
					}

					return false;
				};

				if (!elves.Contains((x - 1, y - 1))
					&& !elves.Contains((x - 1, y))
					&& !elves.Contains((x - 1, y + 1))
					&& !elves.Contains((x, y - 1))
					&& !elves.Contains((x, y + 1))
					&& !elves.Contains((x + 1, y - 1))
					&& !elves.Contains((x + 1, y))
					&& !elves.Contains((x + 1, y + 1)))
				{
					continue;
				}

				var array = new Func<bool>[] { north, south, west, east };

				for (int j = 0; j < 4; j++)
				{
					if (array[(i + j) % 4]())
					{
						break;
					}
				}
			}

			bool did = false;
			foreach (var proposal in proposals)
			{
				if (proposal.Value.v == 1)
				{
					if (!elves.Remove(proposal.Value.elf))
					{
						throw new();
					}

					elves.Add(proposal.Key);
					did = true;
				}
			}

			if (!did)
			{
				result = i + 1;
				break;
			}

			proposals.Clear();
			//Print();
		}

		void Print()
		{
			var xMin = elves.Min(t => t.x);
			var yMin = elves.Min(t => t.y);
			var xMax = elves.Max(t => t.x);
			var yMax = elves.Max(t => t.y);

			Console.WriteLine($"{elves.Count}");
			Console.WriteLine($"{xMin}-{xMax}, {yMin}-{yMax}");

			for (int j = yMin; j <= yMax; j++)
			{
				for (int i = xMin; i <= xMax; i++)
				{
					if (!elves.Contains((i, j)))
					{
						Console.Write(".");
					}
					else
					{
						Console.Write("#");
					}
				}

				Console.WriteLine();
			}
		}

		return result.ToString();
	}
}
