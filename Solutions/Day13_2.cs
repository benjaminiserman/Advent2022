namespace Advent2022.Solutions;
using InputHandler;
using System.Text;
using System.Text.RegularExpressions;

internal static class Day13_2
{
	public interface Entry : IComparable<Entry>
	{
		EntryList ToList();
	}
	public record struct Int(int Value) : Entry
	{
		public override string ToString() => $"{Value}";
		public EntryList ToList() => new(new() { this }, string.Empty);
		int IComparable<Entry>.CompareTo(Entry x) => Compare(this, x) switch
		{
			false => -1,
			null => 0,
			true => 1
		};
	}
	public record class EntryList(List<Entry> List, string Name) : Entry
	{
		public override string ToString() => $"[{string.Join(",", List)}]";

		public static EntryList Parse(string line, out int offset)
		{
			List<Entry> list = new();
			offset = 0;

			for (int i = 1; i < line.Length - 1;)
			{
				if (line[i] == '[')
				{
					list.Add(Parse(line[i..], out offset));
					i += offset + 1;
				}
				else if (line[i] == ']')
				{
					offset = i;
					return new(list, string.Empty);
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

			return new(list, line);
		}

		public EntryList ToList() => this;

		int IComparable<Entry>.CompareTo(Entry x) => Compare(this, x) switch
		{
			false => -1,
			null => 0,
			true => 1
		};
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

		List<EntryList> entryLists = input.Where(line => !string.IsNullOrWhiteSpace(line)).Select(x => EntryList.Parse(x, out _)).ToList();
		entryLists.Add(EntryList.Parse("[[2]]", out _));
		entryLists.Add(EntryList.Parse("[[6]]", out _));

		entryLists.Sort();
		entryLists.Reverse();

		result = (entryLists.IndexOf(entryLists.First(x => x.Name == "[[2]]")) + 1)
			* (entryLists.IndexOf(entryLists.First(x => x.Name == "[[6]]")) + 1);
		return result.ToString();
	}
}
