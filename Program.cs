namespace Advent2022;

using Advent2022.Solutions;
#pragma warning disable IDE0022 // Use expression body for methods

public static class Program
{
	[STAThread]
	public static void Main()
	{
		Day02_1.Execute();
		//Day02_2.Execute();
	}

	public static string InputFilePath => "input.txt";

	private static string[] _lines = null;
	private static int _nextLine = 0;
	public static string GetLineOfInput()
	{
		_lines ??= File.ReadAllLines(InputFilePath);
		if (_nextLine < _lines.Length)
		{
			return _lines[_nextLine++];
		}
		else
		{
			return string.Empty;
		}
	}

	public static void ReloadInput() => _lines = File.ReadAllLines(InputFilePath);
}