namespace Advent2022.Solutions;
using InputHandler;

internal static class Day08_1
{
	public static string Execute()
	{
		//var input = Input.ListUntilWhiteSpace(s => int.Parse(s), Program.GetLineOfInput);
		var input = Program.GetAllInput()
			.Replace("\r", string.Empty)
			.Split("\n")
			.Select(s => s)
			.ToList();

		var trees = new Dictionary<(int, int), (bool vis, int t)>();

		for (int i = 0; i < input.Count; i++)
		{
			for (int j = 0; j < input[0].Length; j++)
			{
				trees[(i, j)] = (i == 0 || i == input.Count - 1 || j == 0 || j == input[0].Length - 1, input[i][j] - '0');
			}
		}

		var result = 0;
		
		for (int i = 1; i < input.Count - 1; i++)
		{
			for (int j = 1; j < input[0].Length - 1; j++)
			{
				bool upFlag = false, rightFlag = false, leftFlag = false , downFlag = false;

				for (int k = 1; k < Math.Max(input.Count, input[0].Length); k++)
				{
					if (i - k >= 0 && trees[(i - k, j)].t >= trees[(i, j)].t)
					{
						leftFlag = true;
					}

					if (i + k < input.Count && trees[(i + k, j)].t >= trees[(i, j)].t)
					{
						rightFlag = true;
					}

					if (j - k >= 0 && trees[(i, j - k)].t >= trees[(i, j)].t)
					{
						downFlag = true;
					}

					if (j + k < input[0].Length && trees[(i, j + k)].t >= trees[(i, j)].t)
					{
						upFlag = true;
					}
				}

				if (!leftFlag || !rightFlag || !upFlag || !downFlag)
				{
					trees[(i, j)] = (true, trees[(i, j)].t);
				}
			}
		}

		result = trees.Count(x => x.Value.vis);
		

		return result.ToString();
	}
}
