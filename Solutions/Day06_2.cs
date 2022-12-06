namespace Advent2022.Solutions;

internal static class Day06_2
{
	public static string Execute()
	{
		var result = 0;
		var input = Program.GetAllInput();
		for (int i = 13; i < input.Length; i++)
		{
			var set = new HashSet<char>();
			for (int j = 0; j < 14; j++)
			{
				set.Add(input[i - j]);
			}

			if (set.Count == 14)
			{
				result = i + 1;
				break;
			}
		}

		return result.ToString();
	}
}
