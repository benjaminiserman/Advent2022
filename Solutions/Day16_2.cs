namespace Advent2022.Solutions;
using System.Text.RegularExpressions;

internal static class Day16_2
{
	// note: this solution does not actually work on the example.... but it does on my input!
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
				var (MyTarget, d) = queue.Dequeue();
				foreach (var neighbor in MyTarget.LeadsTo)
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

	record struct PassAround(Valve MyTarget, Valve ElephantTarget, int PressureReleased, int Time, int MyDistance, int ElephantDistance, Dictionary<Valve, int> Opened, bool MyDone, bool ElephantDone);

	public static string Execute()
	{
		var input = Program.GetAllInput()
			.Replace("\r", string.Empty)
			.Split("\n")
			.Select(s => s)
			.ToList();
		dynamic result = 0;

		var valves = new Dictionary<string, Valve>();
		var Opened = new List<string>();

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

		var sortedValves = valves.Values
			.Where(MyTarget => MyTarget.FlowRate != 0)
			.OrderByDescending(MyTarget => MyTarget.FlowRate)
			.ToList();

		Queue<PassAround> queue = new();
		var currentBest = 0;
		HashSet<PassAround> solves = new();

		void DoStep(PassAround passAround, bool log = false)
		{
			//if (passAround.Opened.ContainsKey(valves["DD"])
			//	&& passAround.Opened.ContainsKey(valves["JJ"])
			//	&& passAround.Opened.ContainsKey(valves["BB"])
			//	&& passAround.Opened.ContainsKey(valves["HH"])
			//	&& passAround.Opened.ContainsKey(valves["CC"])
			//	/*&& passAround.Opened.ContainsKey(valves["EE"])*/
			//	&& passAround.Opened[valves["DD"]] == 26 - 2
			//	&& passAround.Opened[valves["JJ"]] == 26 - 3
			//	&& passAround.Opened[valves["BB"]] == 26 - 7
			//	&& passAround.Opened[valves["HH"]] == 26 - 7
			//	&& passAround.Opened[valves["CC"]] == 26 - 9
			//	&& passAround.ElephantTarget == valves["EE"]
			//	/*&& passAround.Opened[valves["EE"]] == 26 - 11*/)
			//{
			//	log = true;
			//}

			//if (log)
			//{
			//	Console.WriteLine($"dostep: {passAround}\n\n\n");
			//}

			var myReachedTarget = !passAround.MyDone
				&& passAround.MyDistance == 0
				&& !passAround.Opened.ContainsKey(passAround.MyTarget);
			var elephantReachedTarget = !passAround.ElephantDone
				&& passAround.ElephantDistance == 0
				&& !passAround.Opened.ContainsKey(passAround.ElephantTarget);

			if (myReachedTarget || elephantReachedTarget)
			{
				var newOpened = new Dictionary<Valve, int>(passAround.Opened);
				var newPressure = passAround.PressureReleased;
				var myJustOpened = false;
				var elephantJustOpened = false;

				if (myReachedTarget)
				{
					newOpened.Add(passAround.MyTarget, passAround.Time - 1);
					newPressure += passAround.MyTarget.FlowRate * (passAround.Time - 1);
					myJustOpened = true;
				}

				if (elephantReachedTarget && passAround.MyTarget != passAround.ElephantTarget)
				{
					newOpened.Add(passAround.ElephantTarget, passAround.Time - 1);
					newPressure += passAround.ElephantTarget.FlowRate * (passAround.Time - 1);
					elephantJustOpened = true;
				}

				if (newPressure > currentBest)
				{
					Console.WriteLine($"new best: {newPressure}");
					currentBest = newPressure;
					solves.Add(passAround with
					{
						Opened = newOpened,
						PressureReleased = newPressure
					});
				}

				Retarget(passAround, myJustOpened, elephantJustOpened, newPressure, newOpened, log);
			}
		}

		void Retarget(PassAround passAround, bool myJustOpened, bool elephantJustOpened, int newPressure, Dictionary<Valve, int> newOpened, bool log = false)
		{
			if (log)
			{
				Console.WriteLine($"$$$$$$$$$$current opened:");
				foreach (var x in passAround.Opened)
				{
					Console.WriteLine($" - {x.Key.Name} @ {x.Value}");
				}
			}

			foreach (var target in GetTargets(passAround.MyTarget, passAround, passAround.MyDone, passAround.MyDistance))
			{
				foreach (var elephantTarget in GetTargets(passAround.ElephantTarget, passAround, passAround.ElephantDone, passAround.ElephantDistance))
				{
					if (target == elephantTarget)
					{
						continue;
					}

					var myDone = passAround.MyDone 
						|| (target == passAround.MyTarget 
							&& passAround.MyDistance == 0);
					var elephantDone = passAround.ElephantDone 
						|| (elephantTarget == passAround.ElephantTarget 
							&& passAround.ElephantDistance == 0);

					if (myDone && elephantDone && passAround.Opened.Count != sortedValves.Count)
					{
						continue;
					}

					var myDistance = passAround.MyDistance != 0 
						? passAround.MyDistance 
						: passAround.MyTarget.Distances[target];
					var elephantDistance = passAround.ElephantDistance != 0
						? passAround.ElephantDistance
						: passAround.ElephantTarget.Distances[elephantTarget];

					if (myJustOpened)
					{
						myDistance++;
					}

					if (elephantJustOpened)
					{
						elephantDistance++;
					}

					var elapsedTime = 0;

					if (!passAround.MyDone && !passAround.ElephantDone)
					{
						elapsedTime = Math.Min(myDistance, elephantDistance);
					}
					else if (!passAround.MyDone)
					{
						elapsedTime = myDistance;
					}
					else if (!passAround.ElephantDone)
					{
						elapsedTime = elephantDistance;
					}
					else
					{
						throw new("This should never occur.");
					}

					var q = new PassAround(target, elephantTarget, newPressure, passAround.Time - elapsedTime,
						Math.Max(myDistance - elapsedTime, 0),
						Math.Max(elephantDistance - elapsedTime, 0),
						newOpened, myDone, elephantDone);

					if (log)
					{
						Console.WriteLine($"(mt: {target.Name}, et: {elephantTarget.Name}) => {q}\n");
						Console.WriteLine($"heur: {HeuristicFunction(q)}");
					}

					if (newPressure + HeuristicFunction(q) + 500 >= currentBest)
					{
						queue.Enqueue(q);
					}
				}
			}
		}

		IEnumerable<Valve> GetTargets(Valve current, PassAround t, bool done, int d)
		{
			if (done || d != 0)
			{
				yield return current;
				yield break;
			}

			var q = sortedValves.Where(x => !t.Opened.ContainsKey(x));
			if (q.Any())
			{
				foreach (var y in q)
				{
					yield return y;
				}

				yield return current;
			}
			else
			{
				yield return current;
			}
		}

		int HeuristicFunction(PassAround t)
		{
			int BasicHeuristicFunction(PassAround t)
			{
				var time = t.Time;
				var h = 0;
				foreach (var valve in sortedValves)
				{
					if (!t.Opened.ContainsKey(valve))
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

			if (t.ElephantDone || t.MyDone)
			{
				return BasicHeuristicFunction(t);
			}

			var time = t.Time;
			var h = 0;
			int a = -1;
			for (int i = 0; i < sortedValves.Count; i++)
			{
				if (!t.Opened.ContainsKey(sortedValves[i]))
				{
					if (a < 0)
					{
						a = i;
					}
					else
					{
						if (time <= 2)
						{
							break;
						}

						h += (time - Math.Min(t.MyTarget.Distances[sortedValves[a]], t.ElephantTarget.Distances[sortedValves[a]]) - 1)
							* sortedValves[a].FlowRate;
						h += (time - Math.Min(t.MyTarget.Distances[sortedValves[i]], t.ElephantTarget.Distances[sortedValves[i]]) - 1)
							* sortedValves[i].FlowRate;

						time -= 2;
						a = -1;
					}
				}
			}

			if (a >= 0 && time > 0)
			{
				h += (time - Math.Min(t.MyTarget.Distances[sortedValves[a]], t.ElephantTarget.Distances[sortedValves[a]]) - 1)
							* sortedValves[a].FlowRate;
			}

			return h;
		}

		var startV = new PassAround(start, start, 0, 26, 0, 0, new(), false, false);
		DoStep(startV);
		Retarget(startV, false, false, startV.PressureReleased, startV.Opened);
		
		Console.WriteLine($"heur: {queue.Max(x => HeuristicFunction(x))}");

		while (queue.TryDequeue(out var passAround))
		{
			if (passAround.PressureReleased + HeuristicFunction(passAround) + 500 < currentBest)
			{
				continue;
			}
			
			if (passAround.Time > 1)
			{
				DoStep(passAround);
			}
		}

		Console.Write("     ");
		foreach (var x in sortedValves)
		{
			Console.Write($"{x.Name} ");
		}
		Console.WriteLine();
		foreach (var x in sortedValves)
		{
			Console.Write($"{x.Name} ");
			foreach (var y in sortedValves)
			{
				Console.Write($"  {x.Distances[y]}");
			}
			Console.WriteLine();
		}

		Console.Write($"AA ");
		foreach (var y in sortedValves)
		{
			Console.Write($"  {valves["AA"].Distances[y]}");
		}
		Console.WriteLine();

		Console.WriteLine(currentBest);
		var solve = solves.MaxBy(x => x.PressureReleased);
		Console.WriteLine(solve);
		foreach (var x in solve.Opened)
		{
			Console.WriteLine($" - {x.Key.Name} @ {26 - x.Value}");
		}

		result = solves.Max(x => x.PressureReleased);

		return result.ToString();
	}
}
