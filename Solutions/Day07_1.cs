namespace Advent2022.Solutions;

internal static class Day07_1
{
	interface Entry
	{
		public string Name { get; }
		public int Size { get; }
	}

	record MyFile(string Name, int Size) : Entry { }

	record MyDirectory(string Name) : Entry
	{
		public List<Entry> entries = new();
		public int Size
		{
			get
			{
				int size = 0;
				foreach (var entry in entries)
				{
					size += entry.Size;
				}
				return size;
			}
		}
		public IEnumerable<MyDirectory> Enumerate()
		{
			yield return this;
			foreach (var x in entries)
			{
				if (x is MyDirectory d)
				{
					foreach (var y in d.Enumerate())
					{
						yield return y;
					}
				}
			}
		}
	}

	public static string Execute()
	{
		Stack<MyDirectory> path = new();
		MyDirectory rootDirectory = new("/");

		void Execute(string command, List<string> outputs)
		{
			if (string.IsNullOrEmpty(command))
			{
				return;
			}

			var split = command.Split().Skip(1).ToList();
			switch (split[0])
			{
				case "cd":
					if (split[1] == "..")
					{
						path.Pop();
					}
					else if (split[1] == "/")
					{
						path.Clear();
						path.Push(rootDirectory);
					}
					else
					{
						MyDirectory dir;
						if (!path.Peek().entries.Any(x => x.Name == split[1]))
						{
							dir = new(split[1]);
							path.Peek().entries.Add(dir);
						}
						else
						{
							dir = (MyDirectory)path.Peek().entries.First(x => x.Name == split[1]);
						}

						path.Push(dir);
					}
					break;
				case "ls":
					foreach (var output in outputs)
					{
						var outputSplit = output.Split();
						if (outputSplit[0] == "dir")
						{
							if (!path.Peek().entries.Any(x => x.Name == outputSplit[1]))
							{
								path.Peek().entries.Add(new MyDirectory(outputSplit[1]));
							}
						}
						else
						{
							if (!path.Peek().entries.Any(x => x.Name == outputSplit[1]))
							{
								path.Peek().entries.Add(new MyFile(outputSplit[1], int.Parse(outputSplit[0])));
							}
						}
					}
					break;
			}
		}

		var input = Program.GetAllInput()
			.Replace("\r", string.Empty)
			.Split("\n")
			.Select(s => s)
			.ToList();

		string lastCommand = string.Empty;
		List<string> outputs = new();
		for (int i = 0; i < input.Count; i++)
		{
			var line = input[i];
			if (line[0] == '$')
			{
				Execute(lastCommand, outputs);
				outputs.Clear();
				lastCommand = line;
			}
			else
			{
				outputs.Add(line);
			}
		}

		Execute(lastCommand, outputs);

		var sizes = rootDirectory
			.Enumerate()
			.Select(x => x.Size)
			.Where(x => x <= 100000);
		var result = sizes.Sum();

		return result.ToString();
	}
}
