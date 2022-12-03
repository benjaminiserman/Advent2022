namespace Advent2022.Solutions;

using System.Security.Cryptography.X509Certificates;
using InputHandler;

internal static class Day03_1
{
	public static string Execute()
	{
		var input = Program.GetAllInput()
			.Replace("\r", string.Empty)
			.Split("\n")
			.Select(s => s)
			.ToList();

		var result = 0;

		foreach (var line in input)
		{
			var filtered = line[..(line.Length / 2)]
				.Where(c => line[(line.Length / 2)..].Contains(c))
				.ToList();

			var c = filtered.First();
			if (c is >= 'A' and <= 'Z')
			{
				result += c - 'A' + 27;
			}
			else
			{
				result += c - 'a' + 1;
			}
		}

		return result.ToString();
	}
}
