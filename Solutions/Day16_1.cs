namespace Advent2022.Solutions;
using InputHandler;
using System.Data.Common;
using System.Text.RegularExpressions;

internal static class Day16_1
{
	record Valve(string Name, int FlowRate, HashSet<string> LeadsTo)
	{
		public Dictionary<Valve, int> Distances = new();

		public void FillDistances(int valveCount, Dictionary<string, Valve> valves)
		{
			var queue = new Queue<(Valve, int)>();
			queue.Enqueue((this, 0));
			Distances.Add(this, 0);
			while (Distances.Count != valveCount)
			{
				var (v, d) = queue.Dequeue();
				foreach (var neighbor in v.LeadsTo)
				{
					if (!Distances.ContainsKey(valves[neighbor]))
					{
						Distances.Add(valves[neighbor], d + 1);
						queue.Enqueue((valves[neighbor], d + 1));
					}
				}
			}
		}
	}

	record struct PassAround(Valve v, int amt, int t, Dictionary<Valve, int> opened);

	public static string Execute()
	{
		var input = Program.GetAllInput()
			.Replace("\r", string.Empty)
			.Split("\n")
			.Select(s => s)
			.ToList();
		dynamic result = 0;

		var valves = new Dictionary<string, Valve>();
		var opened = new List<string>();

		Dictionary<string, (int, int)> Traverse(Valve start, int time, Dictionary<string, (int, int)> visited = null)
		{
			visited ??= new();
			var queue = new List<string>();

			foreach (var tunnel in start.LeadsTo)
			{
				if (!visited.ContainsKey(tunnel) && !opened.Contains(tunnel))
				{
					visited.Add(tunnel, (valves[tunnel].FlowRate * (time - 1), time - 1)); // - 1 for open
					queue.Add(tunnel);
				}
			}

			foreach (var tunnel in queue)
			{
				Traverse(valves[tunnel], time - 1, visited);
			}

			return visited;
		}

		KeyValuePair<string, (int amt, int time)> FindBest(Valve start, int time)
		{
			return Traverse(start, time).MaxBy(x => x.Value.Item1);
		}

		foreach (var line in input)
		{
			if (string.IsNullOrWhiteSpace(line))
				continue;
			var match = Regex.Match(line, @"Valve (..) has flow rate=(\d+); tunnels? leads? to valves? ((.., )*(..))");
			var name = match.Groups[1].Value;
			var flowRate = int.Parse(match.Groups[2].Value);
			var leadsTo = match.Groups[3].Value.Split(", ").ToHashSet();

			valves.Add(name, new(name, flowRate, leadsTo));
		}

		foreach (var valve in valves)
		{
			valve.Value.FillDistances(valves.Count, valves);
		}

		var start = valves["AA"];
		var current = start;

		//var queue = new PriorityQueue<(Valve current, int amt, int t, Dictionary<Valve, int> opened), int>();

		//queue.Enqueue((start, 0, 30, new()), 0);

		//while (queue.TryDequeue(out var v, out var p))
		//{
		//	if (v.t == 0)
		//	{
		//		foreach (var kvp in v.opened)
		//		{
		//			Console.WriteLine($"{kvp.Key.Name} @ {kvp.Value}");
		//		}

		//		result = v.amt;
		//		break;
		//	}

		//	if (!v.opened.ContainsKey(v.current) && v.current.FlowRate != 0)
		//	{
		//		var newOpened = new Dictionary<Valve, int>(v.opened)
		//		{
		//			{ v.current, v.t - 1 }
		//		};

		//		var newP = v.amt + v.current.FlowRate * (v.t - 1);

		//		queue.Enqueue((v.current, newP, v.t - 1, newOpened), -newP);
		//	}

		//	foreach (var neighbor in v.current.LeadsTo)
		//	{
		//		queue.Enqueue((valves[neighbor], v.amt, v.t - 1, v.opened), p);
		//	}
		//}

		List<Valve> sortedValves = valves.Values
			.Where(v => v.FlowRate != 0)
			.OrderByDescending(v => v.FlowRate)
			.ToList();

		Queue<PassAround> queue = new();
		queue.Enqueue(new(start, 0, 30, new()));

		var currentBest = 0;
		
		int HeuristicFunction(PassAround t)
		{
			var time = t.t;
			var h = 0;
			foreach (var valve in sortedValves)
			{
				if (!t.opened.ContainsKey(valve))
				{
					time -= 2;

					if (time <= 0)
					{
						break;
					}

					h += time * valve.FlowRate;
				}
			}

			return h;
		}

		List<PassAround> solves = new();

		Console.WriteLine($"heur: {HeuristicFunction(queue.Peek())}");

		while (queue.TryDequeue(out var v))
		{
			if (v.amt + HeuristicFunction(v) + 200 < currentBest)
			{
				continue;
			}

			if (!v.opened.ContainsKey(v.v) && v.v.FlowRate != 0 && v.t > 1)
			{
				var newOpened = new Dictionary<Valve, int>(v.opened)
				{
					{ v.v, v.t - 1 }
				};

				var newP = v.amt + v.v.FlowRate * (v.t - 1);

				if (newP > currentBest)
				{
					Console.WriteLine($"new best: {newP}");
					currentBest = newP;
				}

				queue.Enqueue(new(v.v, newP, v.t - 1, newOpened));
			}

			if (v.t > 1)
			{
				foreach (var neighbor in v.v.LeadsTo)
				{
					queue.Enqueue(new(valves[neighbor], v.amt, v.t - 1, v.opened));
				}
			}
			else
			{
				solves.Add(v);
			}
		}

		//foreach (var s in solves)
		//{
		//	Console.WriteLine(s);
		//	foreach (var q in s.opened)
		//	{
		//		Console.WriteLine($"opened {q.Key.Name} @ {q.Value}");
		//	}
		//}

		result = solves.Max(x => x.amt);

		//for (var i = 30; i > 0;)
		//{
		//	try
		//	{
		//		var best = FindBest(current, i);
		//		current = valves[best.Key];
		//		i = best.Value.time;
		//		result += best.Value.amt;
		//		opened.Add(best.Key);
		//		Console.WriteLine($"Opened: {best.Key} for {best.Value.amt} @ {best.Value.time}");
		//	}
		//	catch
		//	{
		//		return result.ToString();
		//	}
		//}

		return result.ToString();
	}
}
