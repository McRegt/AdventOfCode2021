// © Traxion Development Services

namespace CodeBase
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using AdventBase.Helpers;
	using AdventBase.Interfaces;

	public class Day11 : IAdventDay
	{
		private readonly List<string> _inputList = new();
		private readonly Dictionary<(long X, long Y), int> _octopi = new();

		private readonly List<(int X, int Y)> _deltas = new()
		{
			new(-1, -1),
			new(0, -1),
			new(1, -1),
			new(-1, 0),
			new(1, 0),
			new(-1, 1),
			new(0, 1),
			new(1, 1)
		};

		private bool LineParser(string line)
		{
			_inputList.Add(line);

			return true;
		}

		public void LoadInput(string fileName)
		{
			_ = InputLoader<bool>.LoadLines(fileName, LineParser);

			for (var y = 0; y < _inputList.Count; y++)
			{
				char[] charArray = _inputList[y].ToCharArray();

				for (var x = 0; x < charArray.Length; x++)
				{
					_octopi.Add(new(x, y), int.Parse(charArray[x].ToString()));
				}
			}
		}

		public long PartOne()
		{
			Dictionary<(long X, long Y), int> octopiOne = new(_octopi);

			long flashes = 0;

			for (var day = 1; day <= 100; day++)
			{
				// First increase all levels.
				foreach (KeyValuePair<(long X, long Y), int> octopus in octopiOne)
				{
					octopiOne[octopus.Key]++;
				}

				// Processes propagation
				Dictionary<(long X, long Y), int> octopiTemp = new(octopiOne);

				foreach (KeyValuePair<(long X, long Y), int> octopus in octopiTemp)
				{
					if (octopus.Value > 9)
					{
						PropagateFlashes(octopus.Key, octopiOne);
					}
				}

				// Reset values.
				foreach (KeyValuePair<(long X, long Y), int> octopus in octopiOne)
				{
					if (octopus.Value > 9)
					{
						flashes++;
						octopiOne[octopus.Key] = 0;
					}
				}
			}

			return flashes;
		}

		private void PropagateFlashes((long X, long Y) octopus, Dictionary<(long X, long Y), int> octoPi)
		{
			long xLimit = octoPi.Keys.Max(x => x.X);
			long yLimit = octoPi.Keys.Max(x => x.Y);

			// It will flash, increase neighboring values.
			foreach ((int x, int y) in _deltas)
			{
				long checkX = octopus.X + x;
				long checkY = octopus.Y + y;

				if (checkX < 0
					|| checkX > xLimit
					|| checkY < 0
					|| checkY > yLimit)
				{
					continue;
				}

				octoPi[(checkX, checkY)]++;

				if (octoPi[(checkX, checkY)] == 10)
				{
					PropagateFlashes((checkX, checkY), octoPi);
				}
			}
		}

		public long PartTwo()
		{
			Dictionary<(long X, long Y), int> octopiTwo = new(_octopi);

			for (var day = 1; day < int.MaxValue; day++)
			{
				// First increase all levels.
				foreach (KeyValuePair<(long X, long Y), int> octopus in octopiTwo)
				{
					octopiTwo[octopus.Key]++;
				}

				// Processes propagation
				Dictionary<(long X, long Y), int> octopiTemp = new(octopiTwo);

				foreach (KeyValuePair<(long X, long Y), int> octopus in octopiTemp)
				{
					if (octopus.Value > 9)
					{
						PropagateFlashes(octopus.Key, octopiTwo);
					}
				}

				// Reset values.
				long flashes = 0;

				foreach (KeyValuePair<(long X, long Y), int> octopus in octopiTwo)
				{
					if (octopus.Value > 9)
					{
						flashes++;
						octopiTwo[octopus.Key] = 0;
					}
				}

				if (flashes == octopiTwo.Count)
				{
					return day;
				}
			}

			throw new ArgumentException("Unknown convergence.");
		}
	}
}
