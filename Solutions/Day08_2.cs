namespace Advent2022.Solutions;
using InputHandler;

internal static class Day08_2
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

		List<int> scores = new();

		for (int i = 1; i < input.Count - 1; i++)
		{
			for (int j = 1; j < input[0].Length - 1; j++)
			{
				bool upFlag = false, rightFlag = false, leftFlag = false, downFlag = false;
				int a = 0, b = 0, c = 0, d = 0;

				for (int k = 1; k < Math.Max(input.Count, input[0].Length); k++)
				{
					if (j - k < 0) leftFlag = true;
					if (j + k >= input[0].Length) rightFlag = true;
					if (i - k < 0) downFlag = true;
					if (i + k >= input.Count) upFlag = true;

					if (!leftFlag)
					{
						a++;
						if (trees[(i, j - k)].t >= trees[(i, j)].t)
						{
							leftFlag = true;
						}
					}

					if (!rightFlag)
					{
						b++;
						if (trees[(i, j + k)].t >= trees[(i, j)].t)
						{
							rightFlag = true;
						}
					}

					if (!downFlag)
					{
						c++;
						if (trees[(i - k, j)].t >= trees[(i, j)].t)
						{
							downFlag = true;
						}
					}

					if (!upFlag)
					{
						d++;
						if (trees[(i + k, j)].t >= trees[(i, j)].t)
						{
							upFlag = true;
						}
					}
				}

				scores.Add(a * b * c * d);
			}
		}

		//result = trees.Count(x => x.Value.vis);
		result = scores.Max();

		return result.ToString();
	}
}
