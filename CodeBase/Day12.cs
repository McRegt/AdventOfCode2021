// © Traxion Development Services

namespace CodeBase
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using AdventBase.Helpers;
	using AdventBase.Interfaces;

	public class Day12 : IAdventDay
	{
		private readonly List<string> _points = new();
		private readonly List<(string Key, string Value)> _inputs = new();

		private readonly Dictionary<string, List<string>> _navigationChart = new();

		private bool LineParser(string line)
		{
			string[] link = line.Split('-', StringSplitOptions.RemoveEmptyEntries);
			_inputs.Add((link[0], link[1]));

			_points.Add(link[0]);
			_points.Add(link[1]);

			return true;
		}

		public void LoadInput(string fileName)
		{
			_ = InputLoader<bool>.LoadLines(fileName, LineParser);

			foreach (string point in _points.Distinct())
			{
				List<string> toPoints = new();

				_inputs.Where(x => x.Key.Equals(point))
					   .ToList()
					   .ForEach(x =>
					   {
						   toPoints.Add(x.Value);
					   });

				_inputs.Where(x => x.Value.Equals(point))
					   .ToList()
					   .ForEach(x =>
					   {
						   toPoints.Add(x.Key);
					   });

				_navigationChart.Add(point, toPoints);
			}
		}

		public long PartOne()
		{
			var start = "start";
			var end = "end";

			int paths = TraversePath(start,
									 end,
									 new HashSet<string>
									 {
										 start
									 });

			return paths;
		}

		private int TraversePath(string source,
								 string destination,
								 HashSet<string> paths)
		{
			if (source.Equals(destination))
			{
				return 1;
			}

			var count = 0;

			foreach (string nextDestination in _navigationChart[source].Where(x => !paths.Contains(x)))
			{
				var newPaths = new HashSet<string>(paths);

				if (nextDestination.All(char.IsLower))
				{
					newPaths.Add(nextDestination);
				}

				count += TraversePath(nextDestination,
									  destination,
									  newPaths);
			}

			return count;
		}

		private int TraversePathSingleDuplicate(string source,
												string destination,
												Dictionary<string, int> paths)
		{
			if (source.Equals(destination))
			{
				return 1;
			}

			var count = 0;

			foreach (string nextDestination in _navigationChart[source])
			{
				if (paths.ContainsKey(nextDestination)
					&& (paths.Values.Contains(2)
						|| nextDestination.Equals("start")
						|| nextDestination.Equals("end")))
				{
					continue;
				}

				var newPaths = new Dictionary<string, int>(paths);

				if (nextDestination.All(char.IsLower))
				{
					if (newPaths.ContainsKey(nextDestination))
					{
						newPaths[nextDestination]++;
					}
					else
					{
						newPaths.Add(nextDestination, 1);
					}
				}

				count += TraversePathSingleDuplicate(nextDestination,
													 destination,
													 newPaths);
			}

			return count;
		}

		public long PartTwo()
		{
			var start = "start";
			var end = "end";

			int paths = TraversePathSingleDuplicate(start,
													end,
													new Dictionary<string, int>
													{
														{
															start, 1
														}
													});

			return paths;
		}
	}
}
