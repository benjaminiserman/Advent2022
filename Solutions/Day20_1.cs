﻿namespace Advent2022.Solutions;
using InputHandler;
using System.Data.Common;
using System.Text.RegularExpressions;

internal static class Day20_1
{
	class Integer
	{
		public int Value { get; set; }
	}

	public static string Execute()
	{
		var input = Program.GetAllInput()
			.Replace("\r", string.Empty)
			.Split("\n")
			.Select(s => s)
			.ToList();
		dynamic result = 0;

		var initialList = input.Select(x => new Integer() { Value = int.Parse(x) }).ToList();

		var list = new List<Integer>(initialList);

		//Console.WriteLine(string.Join(',', list.Select(x => x.Value)));
		foreach (var number in initialList)
		{
			if (number.Value == 0) continue;
			var index = list.IndexOf(number);
			//Console.WriteLine($"moving {number.Value}");
			list.Remove(number);

			index += number.Value;

			index = (index % list.Count + list.Count) % list.Count;

			list.Insert(index, number);

			//Console.WriteLine(string.Join(',', list.Select(x => x.Value)));
		}

		var zeroIndex = list.IndexOf(list.First(x => x.Value == 0));
		result = list[(zeroIndex + 1000) % list.Count].Value 
			+ list[(zeroIndex + 2000) % list.Count].Value 
			+ list[(zeroIndex + 3000) % list.Count].Value;
		return result.ToString();
	}
}