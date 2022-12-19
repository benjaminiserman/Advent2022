namespace Advent2022.Solutions;

using System.Diagnostics;
using System.Text.RegularExpressions;

internal static class Day19_2
{
	enum Resource
	{
		Ore,
		Clay,
		Obsidian,
		Geode
	}

	static T2 SafeGet<T1, T2>(this Dictionary<T1, T2> dict, T1 key)
	{
		if (dict.ContainsKey(key))
		{
			return dict[key];
		}
		else
		{
			dict.Add(key, default);
			return default;
		}
	}

	static void SafeSet<T1, T2>(this Dictionary<T1, T2> dict, T1 key, T2 value)
	{
		if (dict.ContainsKey(key))
		{
			dict[key] = value;
		}
		else
		{
			dict.Add(key, value);
		}
	}

	record Blueprint(int Id)
	{
		public Dictionary<Resource, Dictionary<Resource, int>> Robots { get; set; } = new();

		public Dictionary<Resource, int> Maxes { get; set; } = new();

		public void SetMaxes()
		{
			foreach (var robot in Robots)
			{
				Maxes.Add(robot.Key, Robots.Max(x => x.Value.SafeGet(robot.Key)));
			}

			Maxes[Resource.Geode] = int.MaxValue;
		}
	}

	record Node(Dictionary<Resource, int> Robots, Dictionary<Resource, int> Resources, int Time, List<(Resource, int)> Actions);

	public static string Execute()
	{
		var input = Program.GetAllInput()
			.Replace("\r", string.Empty)
			.Split("\n")
			.Select(s => s)
			.ToList();
		dynamic result = 0;

		var blueprints = new List<Blueprint>();
		foreach (var line in input)
		{
			var match = Regex.Match(line, @"Blueprint (\d+): Each ore robot costs (\d+) ore. Each clay robot costs (\d+) ore. Each obsidian robot costs (\d+) ore and (\d+) clay. Each geode robot costs (\d+) ore and (\d+) obsidian.");

			var blueprint = new Blueprint(int.Parse(match.Groups[1].Value));
			blueprint.Robots.Add(Resource.Ore, new()
			{
				{ Resource.Ore, int.Parse(match.Groups[2].Value) },
			});

			blueprint.Robots.Add(Resource.Clay, new()
			{
				{ Resource.Ore, int.Parse(match.Groups[3].Value) },
			});

			blueprint.Robots.Add(Resource.Obsidian, new()
			{
				{ Resource.Ore, int.Parse(match.Groups[4].Value) },
				{ Resource.Clay, int.Parse(match.Groups[5].Value) },
			});

			blueprint.Robots.Add(Resource.Geode, new()
			{
				{ Resource.Ore, int.Parse(match.Groups[6].Value) },
				{ Resource.Obsidian, int.Parse(match.Groups[7].Value) },
			});

			blueprint.SetMaxes();
			blueprints.Add(blueprint);
		}

		blueprints = blueprints.Take(3).ToList();

		//var solves = new Dictionary<Blueprint, Dictionary<Node, int>>();
		var maxes = new Dictionary<Blueprint, int>();
		foreach (var blueprint in blueprints)
		{
			Console.WriteLine($"blueprint {blueprint.Id}");
			var queue = new PriorityQueue<Node, int>();
			var watch = new Stopwatch();
			watch.Start();
			int max = 0;
			int Heuristic(Node node) =>
				-node.Robots.SafeGet(Resource.Obsidian) * 3
				- node.Robots.SafeGet(Resource.Geode) * 10 * node.Time;

			void QueuePurchases(Node node, Blueprint blueprint)
			{
				if (node.Time == 0)
				{
					throw new("bad");
				}

				if (node.Robots.SafeGet(Resource.Geode) * node.Time + node.Time * node.Time / 2 + node.Resources.SafeGet(Resource.Geode) < max)
				{
					//Console.WriteLine("not happening");
					return;
				}

				if (node.Time > 1)
				{
					foreach (var robotBlueprint in blueprint.Robots)
					{
						if (robotBlueprint.Value
								.All(r => r.Value == 0 || node.Robots.SafeGet(r.Key) >= 1)
							&& node.Robots.SafeGet(robotBlueprint.Key) < blueprint.Maxes[robotBlueprint.Key])
						{
							var timeElapsed = Math.Max(1, (int)Math.Ceiling(robotBlueprint.Value
								.Max(resource => (double)(resource.Value - node.Resources.SafeGet(resource.Key)) / node.Robots.SafeGet(resource.Key))) + 1);

							if (node.Time - timeElapsed < 0)
							{
								continue;
							}

							var resources = new Dictionary<Resource, int>(node.Resources);
							foreach (var kvp in node.Resources)
							{
								resources.SafeSet(kvp.Key, kvp.Value + node.Robots.SafeGet(kvp.Key) * timeElapsed - robotBlueprint.Value.SafeGet(kvp.Key));
							}

							var robots = new Dictionary<Resource, int>(node.Robots);
							robots.SafeSet(robotBlueprint.Key, robots.SafeGet(robotBlueprint.Key) + 1);

							queue.Enqueue(node with
							{
								Robots = robots,
								Resources = resources,
								Time = node.Time - timeElapsed,
								Actions = new List<(Resource, int)>(node.Actions) { (robotBlueprint.Key, node.Time - timeElapsed) }
							}, Heuristic(node));
						}
					}
				}
			}

			QueuePurchases(new Node(Robots: new()
			{
				{ Resource.Ore, 1 }
			}, Resources: new() { }, Time: 32, new()), blueprint);

			while (queue.TryDequeue(out var node, out var priority))
			{
				if (Console.KeyAvailable && Console.ReadKey().KeyChar == 'q')
				{
					Console.WriteLine("break input received.");
					break;
				}

				if (watch.Elapsed > TimeSpan.FromMinutes(10))
				{
					Console.WriteLine("Time limit reached.");
					break;
				}

				var amt = node.Resources.SafeGet(Resource.Geode) + node.Robots.SafeGet(Resource.Geode) * node.Time;
				if (amt > max)
				{
					maxes.SafeSet(blueprint, amt);
					max = amt;
					Console.WriteLine($"{node.Time} -> {node.Robots.SafeGet(Resource.Geode)} rbs and {node.Resources.SafeGet(Resource.Geode)} geodes");
					Console.WriteLine($"new max: {max} @ {watch.Elapsed}");
					foreach (var action in node.Actions)
					{
						//Console.WriteLine($" - {action.Item1} bought at {24 - action.Item2}");
					}
				}

				if (node.Time > 0)
				{
					QueuePurchases(node, blueprint);
				}
			}
		}

		//foreach (var blueprint in solves)
		//{
		//	Console.WriteLine($"Blueprint: {blueprint.Key.Id} ({blueprint.Value.Max(x => x.Value)}): ");
		//	foreach (var solve in blueprint.Value)
		//	{
		//		Console.WriteLine($" - {solve.Value}");
		//	}
		//}

		//result = solves.Sum(x => x.Key.Id * x.Value.Max(y => y.Value));
		result = maxes
			.Select(x => x.Value)
			.Aggregate((x, y) => x * y);
		return result.ToString();
	}
}
