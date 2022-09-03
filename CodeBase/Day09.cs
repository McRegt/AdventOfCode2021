// © Traxion Development Services

namespace CodeBase
{
	using System.Collections.Generic;
	using System.Linq;
	using AdventBase.Helpers;
	using AdventBase.Interfaces;

	public class Day09 : IAdventDay
	{
		private readonly List<string> _vents = new();
		private readonly Dictionary<(int X, int Y), int> _grid = new();
		private readonly List<(int X, int Y)> _lowPoints = new();
		private List<(int X, int Y)> _basinPoints = new();

		private readonly List<(int X, int Y)> _deltas = new()
		{
			new(0, -1),
			new(0, 1),
			new(-1, 0),
			new(1, 0)
		};

		private bool LineParser(string line)
		{
			_vents.Add(line);

			return true;
		}

		public void LoadInput(string fileName)
		{
			_ = InputLoader<bool>.LoadLines(fileName, LineParser);

			for (var y = 0; y < _vents.Count; y++)
			{
				char[] charArray = _vents[y].ToCharArray();

				for (var x = 0; x < charArray.Length; x++)
				{
					_grid.Add(new(x, y), int.Parse(charArray[x].ToString()));
				}
			}
		}

		public long PartOne()
		{
			long result = 0;

			int xLimit = _vents[0].Length - 1;
			int yLimit = _vents.Count - 1;

			foreach ((int X, int Y) position in _grid.Keys)
			{
				var lowest = true;

				foreach ((int X, int Y) delta in _deltas)
				{
					int checkX = position.X + delta.X;
					int checkY = position.Y + delta.Y;

					if (checkX < 0
						|| checkX > xLimit
						|| checkY < 0
						|| checkY > yLimit)
					{
						continue;
					}

					if (_grid[(checkX, checkY)] <= _grid[position])
					{
						lowest = false;
					}
				}

				if (!lowest)
				{
					continue;
				}

				_lowPoints.Add(position);
				result += _grid[position] + 1;
			}

			return result;
		}

		public long PartTwo()
		{
			List<int> basinSizes = new();

			foreach ((int X, int Y) lowPoint in _lowPoints)
			{
				// Reset the list.
				_basinPoints = new List<(int X, int Y)>();

				// Recursively generate the list.
				ExploreBasin(lowPoint);

				basinSizes.Add(_basinPoints.Distinct().Count());
			}

			return basinSizes.OrderBy(x => x).TakeLast(3).Aggregate(1, (x, y) => x * y);
		}

		public void ExploreBasin((int X, int Y) point)
		{
			int xLimit = _vents[0].Length - 1;
			int yLimit = _vents.Count - 1;

			// Add grid value itself.
			_basinPoints.Add(point);

			foreach ((int X, int Y) delta in _deltas)
			{
				int checkX = point.X + delta.X;
				int checkY = point.Y + delta.Y;

				if (checkX < 0
					|| checkX > xLimit
					|| checkY < 0
					|| checkY > yLimit)
				{
					continue;
				}

				// Check surrounding values for higher height, that is not 9.
				if (_grid[(checkX, checkY)] > _grid[point]
					&& _grid[(checkX, checkY)] != 9)
				{
					ExploreBasin((checkX, checkY));
				}
			}
		}
	}
}
