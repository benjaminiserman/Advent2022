namespace Advent2022;

using Advent2022.Solutions;
#pragma warning disable IDE0022 // Use expression body for methods

public static class Program
{
	[STAThread]
	public static void Main()
	{
		//var result = Day24_1.Execute();
		var result = Day24_2.Execute();

		Console.WriteLine(result);
		Clipboard.SetText(result.ToString());
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

	public static string GetAllInput() => File.ReadAllText(InputFilePath);

	public static void ReloadInput() => _lines = File.ReadAllLines(InputFilePath);
}