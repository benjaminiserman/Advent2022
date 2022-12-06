namespace Advent2022.Solutions;

internal static class Day06_1
{
	public static string Execute()
	{
		var result = 0;
		var input = Program.GetAllInput();
		for (int i = 3; i < input.Length; i++)
		{
			var set = new HashSet<char>();
			for (int j = 0; j < 4; j++)
			{
				set.Add(input[i - j]);
			}
			
			if (set.Count == 4)
			{
				result = i + 1;
				break;
			}
		}

		return result.ToString();
	}
}
