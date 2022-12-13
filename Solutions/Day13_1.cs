namespace Advent2022.Solutions;
using InputHandler;
using System.Text;
using System.Text.RegularExpressions;

internal static class Day13_1
{
	public interface Entry
	{
		EntryList ToList();
	}
	public record struct Int(int Value) : Entry 
	{
		public override string ToString() => $"{Value}";
		public EntryList ToList() => new(new() { this });
	}
	public record class EntryList(List<Entry> List) : Entry 
	{
		public override string ToString() => $"[{string.Join(",", List)}]";

		public static EntryList Parse(string line, out int offset)
		{
			List<Entry> list = new();
			offset = 0;

			for (int i = 1; i < line.Length - 1; )
			{
				if (line[i] == '[')
				{
					list.Add(Parse(line[i..], out offset));
					i += offset + 1;
				}
				else if (line[i] == ']')
				{
					offset = i;
					return new(list);
				}
				else if (line[i] == ',')
				{
					i++;
				}
				else
				{
					int x = int.Parse(Regex.Match(line[i..], @"\d+").Value);
					list.Add(new Int(x));
					i += Math.Max((int)Math.Ceiling(Math.Log10(x)), 1);
				}
			}

			return new(list);
		}

		public EntryList ToList() => this;
	}

	public static bool? Compare(Entry left, Entry right)
	{
		if (left is Int li && right is Int ri)
		{
			if (li.Value == ri.Value)
			{
				return null;
			}
			else
			{
				return li.Value < ri.Value;
			}
		}
		else
		{
			var leftList = left.ToList();
			var rightList = right.ToList();

			for (int i = 0; i < leftList.List.Count && i < rightList.List.Count; i++)
			{
				var comp = Compare(leftList.List[i], rightList.List[i]);
				if (comp != null)
				{
					return comp;
				}
			}

			if (leftList.List.Count == rightList.List.Count)
			{
				return null;
			}
			else
			{
				return leftList.List.Count < rightList.List.Count;
			}
		}

		return null;
	}

	public static string Execute()
	{
		//var input = Input.ListUntilWhiteSpace(s => int.Parse(s), Program.GetLineOfInput);
		var input = Program.GetAllInput()
			.Replace("\r", string.Empty)
			.Split("\n")
			.Select(s => s)
			.ToList();
		dynamic result = 0;

		List<int> stuff = new();

		for (int i = 0; i < input.Count; i += 3)
		{
			var lineA = EntryList.Parse(input[i], out _);
			var lineB = EntryList.Parse(input[i + 1], out _);

			var comp = Compare(lineA, lineB);
			if (comp == true)
			{
				stuff.Add(i / 3 + 1);
			}
			else if (comp == null)
			{
				Console.WriteLine(lineA);
				Console.WriteLine(lineB);
				Console.WriteLine("Null!!!!");
			}
		}

		result = stuff.Sum();
		return result.ToString();
	}
}
