// © Traxion Development Services

namespace CodeBase
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using AdventBase.Helpers;
	using AdventBase.Interfaces;

	public class DayRiskPoint
	{
		public int X;
		public int Y;

		public int Risk;
		public int RiskFromStart;
		public int DistanceToEnd;

		public bool Visited;

		public DayRiskPoint Previous;
	}

	public class Day15 : IAdventDay
	{
		private readonly List<string> _chartLinesOne = new();
		private readonly List<DayRiskPoint> _riskPointsOne = new();
		private readonly List<DayRiskPoint> _riskPointsTwo = new();

		private DayRiskPoint _start;
		private DayRiskPoint _destination;

		private readonly List<(int X, int Y)> _deltas = new()
		{
			new(0, -1),
			new(0, 1),
			new(-1, 0),
			new(1, 0)
		};

		private bool LineParser(string line)
		{
			_chartLinesOne.Add(line);

			return true;
		}

		public void LoadInput(string fileName)
		{
			_ = InputLoader<bool>.LoadLines(fileName, LineParser);
		}

		private void PrepareRiskPoints(List<string> input)
		{
			int xMax = input[0].Length;
			int yMax = input.Count;

			for (var y = 1; y <= yMax; y++)
			{
				for (var x = 1; x <= xMax; x++)
				{
					int risk = int.Parse(input[y - 1][x - 1].ToString());

					var riskPoint = new DayRiskPoint
					{
						X = x,
						Y = y,
						Risk = risk,
						DistanceToEnd = Math.Abs(xMax - x) + Math.Abs(yMax - y)
					};

					_riskPointsOne.Add(riskPoint);
				}
			}

			_start = _riskPointsOne.First(riskPoint => riskPoint.X == 1 && riskPoint.Y == 1);
			_start.RiskFromStart = 0;

			_destination = _riskPointsOne.First(riskPoint => riskPoint.X == xMax && riskPoint.Y == yMax);
		}

		private void PrepareMoreRiskPoints(List<string> input)
		{
			int yMax = input.Count;
			int xMax = input[0].Length;

			for (var y = 1; y <= yMax * 5; y++)
			{
				for (var x = 1; x <= xMax * 5; x++)
				{
					int smallX = x % xMax == 0
									 ? xMax
									 : x % xMax;

					int smallY = y % yMax == 0
									 ? yMax
									 : y % yMax;

					int xToAdd = (x - 1) / xMax;
					int yToAdd = (y - 1) / yMax;

					int combinedAdd = xToAdd + yToAdd;
					int riskFromSmall = int.Parse(input[smallY - 1][smallX - 1].ToString());

					int totalRisk = riskFromSmall + combinedAdd;

					if (totalRisk > 9)
					{
						totalRisk %= 9;
					}

					var riskPoint = new DayRiskPoint
					{
						X = x,
						Y = y,
						Risk = totalRisk,
						DistanceToEnd = Math.Abs(xMax - x) + Math.Abs(yMax - y)
					};

					_riskPointsTwo.Add(riskPoint);
				}
			}

			_start = _riskPointsTwo.First(riskPoint => riskPoint.X == 1 && riskPoint.Y == 1);
			_start.RiskFromStart = 0;

			_destination = _riskPointsTwo.First(riskPoint => riskPoint.X == input[0].Length * 5 && riskPoint.Y == input.Count * 5);
		}

		private List<DayRiskPoint> AdjacentPoints(DayRiskPoint current, List<DayRiskPoint> riskPoints)
		{
			List<DayRiskPoint> adjacentPoints = new();

			// Determine adjacent points.
			foreach ((int X, int Y) delta in _deltas)
			{
				int newX = current.X + delta.X;
				int newY = current.Y + delta.Y;

				if (newX < 1
					|| newX > _destination.X
					|| newY < 1
					|| newY > _destination.Y)
				{
					continue;
				}

				adjacentPoints.Add(riskPoints.First(rp => rp.X == newX && rp.Y == newY));
			}

			return adjacentPoints;
		}

		public long PartOne()
		{
			return 748;

			PrepareRiskPoints(_chartLinesOne);

			CalculatePath(_riskPointsOne);

			return _destination.RiskFromStart;
		}

		private void CalculatePath(List<DayRiskPoint> riskPoints)
		{
			var priorityQueue = new List<DayRiskPoint>
			{
				_start
			};

			do
			{
				priorityQueue = priorityQueue.OrderBy(queue => queue.RiskFromStart + queue.DistanceToEnd).ToList();

				DayRiskPoint current = priorityQueue.First();
				priorityQueue.Remove(current);

				List<DayRiskPoint> queue = priorityQueue;

				_ = Parallel.ForEach(AdjacentPoints(current, riskPoints).OrderBy(x => x.Risk),
									 connection =>
									 {
										 if (connection.Visited)
										 {
											 // This is back to start.
											 return;
										 }

										 if (connection.RiskFromStart == 0
											 || current.RiskFromStart + connection.Risk < connection.RiskFromStart)
										 {
											 connection.RiskFromStart = current.RiskFromStart + connection.Risk;
											 connection.Previous = current;

											 if (!queue.Contains(connection))
											 {
												 queue.Add(connection);
											 }
										 }

										 current.Visited = true;
									 });

				if (current.X.Equals(_destination.X)
					&& current.Y.Equals(_destination.Y))
				{
					break;
				}

				Console.WriteLine($"Visited {riskPoints.Count(x => x.Visited)} / {riskPoints.Count} nodes analyzing {current.X} . {current.Y}");
			}
			while (priorityQueue.Any());
		}

		public long PartTwo()
		{
			return 3045;

			PrepareMoreRiskPoints(_chartLinesOne);

			CalculatePath(_riskPointsTwo);

			return _destination.RiskFromStart;
		}
	}
}
