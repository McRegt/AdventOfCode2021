// © Traxion Development Services

namespace CodeBase
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using AdventBase.Helpers;
	using AdventBase.Interfaces;

	public class Day07 : IAdventDay
	{
		private List<long> _crabs = new();

		private bool LineParser(string line)
		{
			_crabs = line.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();

			return true;
		}

		public void LoadInput(string fileName)
		{
			_ = InputLoader<bool>.LoadLines(fileName, LineParser);
		}

		public long PartOne()
		{
			long median = _crabs.OrderBy(x => x).Skip(_crabs.Count / 2).First();

			long fuelCount = _crabs.Select(x => Math.Abs(x - median)).Sum();

			return fuelCount;
		}

		public long PartTwo()
		{
			var fuelCount = long.MaxValue;

			for (long check = 1; check <= _crabs.Max(); check++)
			{
				long result = _crabs.Select(x => Math.Abs(x - check) * (Math.Abs(x - check) + 1) / 2).Sum();

				if (result < fuelCount)
				{
					fuelCount = result;
				}

				if (result > fuelCount)
				{
					// Break as soon as the result increases again.
					break;
				}
			}

			return fuelCount;
		}
	}
}
