// © Traxion Development Services

namespace CodeBase
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using AdventBase.Helpers;
	using AdventBase.Interfaces;

	public class Day17 : IAdventDay
	{
		private (long Min, long Max) _targetX;
		private (long Min, long Max) _targetY;
		private long _totalCount;

		public void LoadInput(string fileName)
		{
			List<string> input = InputLoader<string>.LoadStringList(fileName);

			//target area: x=236..262, y=-78..-58

			string[] targets = input.First().Replace("target area: ", "").Split(", ");

			string[] xRange = targets[0].Substring(2, targets[0].Length - 2).Split("..");
			string[] yRange = targets[1].Substring(2, targets[1].Length - 2).Split("..");

			_targetX = new(long.Parse(xRange[0]), long.Parse(xRange[1]));
			_targetY = new(long.Parse(yRange[0]), long.Parse(yRange[1]));
		}

		public long PartOne()
		{
			long maxYFound = 0;

			long largestValue = Math.Abs(_targetX.Max) >= Math.Abs(_targetY.Max)
									? Math.Abs(_targetX.Max)
									: Math.Abs(_targetY.Max);

			for (var x = 0; x <= _targetX.Max; x++)
			{
				for (long y = _targetY.Min; y <= Math.Abs(_targetY.Min); y++)
				{
					var targetHit = false;
					long maxY = 0;

					for (var n = 0; n <= largestValue; n++)
					{
						// Calculate X after n periods.
						long currentX = CalculateX(x, n);

						// Calculate Y after n periods.
						long currentY = CalculateY(y, n);

						if (currentY > maxY)
						{
							maxY = currentY;
						}

						// Break if target area is hit
						if (currentX >= _targetX.Min
							&& currentX <= _targetX.Max
							&& currentY >= _targetY.Min
							&& currentY <= _targetY.Max)
						{
							targetHit = true;
							_totalCount++;

							break;
						}

						if (currentX > _targetX.Max
							|| currentY < _targetY.Min)
						{
							break;
						}
					}

					if (targetHit && maxY > maxYFound)
					{
						maxYFound = maxY;
					}
				}
			}

			return maxYFound;
		}

		private long CalculateY(long start, long steps)
		{
			long result = start;

			while (steps > 0)
			{
				result += start - 1 * steps;

				steps--;
			}

			return result;
		}

		private long CalculateX(long start, long steps)
		{
			long result = start;

			while (steps > 0)
			{
				result += start - steps < 0
							  ? 0
							  : start - steps;

				steps--;
			}

			return result;
		}

		public long PartTwo()
		{
			return _totalCount;
		}
	}
}
