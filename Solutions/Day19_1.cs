namespace Advent2022.Solutions;

using System.Diagnostics;
using System.Text.RegularExpressions;

internal static class Day19_1
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
	}

	record Node(Dictionary<Resource, int> Robots, Dictionary<Resource, int> Resources, Resource? NextRobot, int Time);

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

			blueprints.Add(blueprint);
		}

		var solves = new Dictionary<Blueprint, Dictionary<Node, int>>();
		foreach (var blueprint in blueprints)
		{
			Console.WriteLine($"blueprint {blueprint.Id}");
			var queue = new PriorityQueue<Node, int>();
			var watch = new Stopwatch();
			watch.Start();
			int max = 0;
			int Heuristic(Node node) =>
				- node.Robots.SafeGet(Resource.Obsidian) * 3
				- node.Robots.SafeGet(Resource.Geode) * 10 * node.Time;
			Dictionary<Resource, int> IncreaseResources(Node node)
			{
				var resources = new Dictionary<Resource, int>(node.Resources);
				foreach (var kvp in node.Robots)
				{
					resources.SafeSet(kvp.Key, resources.SafeGet(kvp.Key) + kvp.Value);
				}

				return resources;
			}

			void QueuePurchases(Node node, Blueprint blueprint)
			{
				if (node.Time == 0)
				{
					throw new("bad");
				}

				if (node.Robots.SafeGet(Resource.Geode) * (node.Time - 1) + (node.Time - 1) * (node.Time - 1) / 2 + node.Resources.SafeGet(Resource.Geode) < max)
				{
					//Console.WriteLine("not happening");
					return;
				}

				var resources = IncreaseResources(node);
				queue.Enqueue(node with
				{
					NextRobot = null,
					Resources = resources,
					Time = node.Time - 1
				}, Heuristic(node));

				if (node.Time > 1)
				{
					foreach (var robot in blueprint.Robots)
					{
						if (robot.Value.All(r => node.Resources.SafeGet(r.Key) >= r.Value))
						{
							var resourcesAfterPurchase = new Dictionary<Resource, int>(resources);
							foreach (var kvp in blueprint.Robots[robot.Key])
							{
								resourcesAfterPurchase.SafeSet(kvp.Key, resourcesAfterPurchase.SafeGet(kvp.Key) - kvp.Value);
							}

							queue.Enqueue(node with
							{
								NextRobot = robot.Key,
								Resources = resourcesAfterPurchase,
								Time = node.Time - 1,
							}, Heuristic(node));
						}
					}
				}
			}

			QueuePurchases(new Node(Robots: new()
			{
				{ Resource.Ore, 1 }
			}, Resources: new() { }, NextRobot: null, Time: 24), blueprint);
			
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

				if (node.Time == 0)
				{
					if (!solves.ContainsKey(blueprint))
					{
						solves.Add(blueprint, new());
					}

					var amt = node.Resources.SafeGet(Resource.Geode);
					if (amt > max)
					{
						solves[blueprint].SafeSet(node, amt);
						max = amt;
						Console.WriteLine($"new max: {max} @ {watch.Elapsed}");
					}
				}
				else
				{
					var robots = new Dictionary<Resource, int>(node.Robots);
					if (node.NextRobot is Resource nextRobot)
					{
						robots.SafeSet(nextRobot, robots.SafeGet(nextRobot) + 1);
						QueuePurchases(node with
						{
							Robots = robots
						}, blueprint);
					}
					else
					{
						QueuePurchases(node, blueprint);
					}
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

		result = solves.Sum(x => x.Key.Id * x.Value.Max(y => y.Value));
		return result.ToString();
	}
}
