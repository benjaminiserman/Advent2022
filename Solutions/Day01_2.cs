namespace Advent2022.Solutions;
using InputHandler;

internal static class Day01_2
{
	public static void Execute()
	{
		var result = 0;

		List<int> results = new();

		while (true)
		{
			var input = Input.ListUntilWhiteSpace(s => int.Parse(s), Program.GetLineOfInput);
			if (input.Count == 0)
			{
				break;
			}

			results.Add(input.Sum());
		}

		results.Sort();


		Console.WriteLine(results[^1] + results[^2] + results[^3]);
		Clipboard.SetText(result.ToString());
	}
}