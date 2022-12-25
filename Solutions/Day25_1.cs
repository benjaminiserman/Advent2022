namespace Advent2022.Solutions;
using InputHandler;
using System.Numerics;
using System.Text.RegularExpressions;

internal static class Day25_1
{
	public static string Execute()
	{
		var input = Program.GetAllInput()
			.Replace("\r", string.Empty)
			.Split("\n")
			.Select(s => s)
			.ToList();
		dynamic result = 0;
		long dec = 0;

		long GetDecimalFromSnafu(string snafu)
		{
			var digits = new List<int>();
			foreach (var c in snafu)
			{
				digits.Add(c switch
				{
					'=' => -2,
					'-' => -1,
					'0' => 0,
					'1' => 1,
					'2' => 2
				});
			}

			long sum = 0;
			foreach (var d in digits)
			{
				sum *= 5;
				sum += d;
			}

			return sum;
		}

		string ToBaseFive(long dec)
		{
			string baseFive = string.Empty;
			while (dec > 0)
			{
				baseFive += dec % 5;
				dec /= 5;
			}

			return baseFive.Reverse().Aggregate(string.Empty, (s, c) => s + c.ToString());
		}

		string ToSnafu(long dec)
		{
			List<int> snafuDigits = new();
			var carry = 0;
			while (dec > 0)
			{
				var mod = (int)(dec % 5);
				if (mod <= 2)
				{
					snafuDigits.Add(mod);
				}
				else
				{
					dec += 2;
					snafuDigits.Add(mod - 5);
				}

				dec /= 5;
			}

			snafuDigits.Reverse();
			var snafu = string.Empty;
			foreach (var d in snafuDigits)
			{
				snafu += d switch
				{
					-2 => '=',
					-1 => '-',
					0 => '0',
					1 => '1',
					2 => '2'
				};
			}

			return snafu;
		}

		foreach (var line in input)
		{
			dec += GetDecimalFromSnafu(line);
		}

		Console.WriteLine(dec);
		result = ToSnafu(dec);

		return result.ToString();
	}
}
