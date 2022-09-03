// © Traxion Development Services

namespace CodeBase
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using AdventBase.Helpers;
	using AdventBase.Interfaces;

	public class Day06 : IAdventDay
	{
		private const long MaxAge = 8;
		private Dictionary<long, long> _emptyFishes;
		private Dictionary<long, long> _newFishes = new();
		private Dictionary<long, long> _totalFishes = new();

		private bool LineParser(string line)
		{
			List<long> entries = line.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();

			_emptyFishes = CreateEmptyFishList(MaxAge);

			_newFishes = new Dictionary<long, long>(_emptyFishes);

			foreach (long entry in entries)
			{
				_newFishes[entry]++;
			}

			return true;
		}

		public void LoadInput(string fileName)
		{
			_ = InputLoader<bool>.LoadLines(fileName, LineParser);
		}

		private Dictionary<long, long> CreateEmptyFishList(long count)
		{
			Dictionary<long, long> fishes = new();

			for (var i = 0; i <= count; i++)
			{
				fishes.Add(i, 0);
			}

			return fishes;
		}

		public long PartOne()
		{
			_totalFishes = new Dictionary<long, long>(_newFishes);

			return CountFish(80);
		}

		public long PartTwo()
		{
			_totalFishes = new Dictionary<long, long>(_newFishes);

			return CountFish(256);
		}

		public long CountFish(long numberOfDays)
		{
			for (long day = 1; day <= numberOfDays; day++)
			{
				var newFishes = new Dictionary<long, long>(_emptyFishes);

				for (long i = MaxAge; i >= 0; i--)
				{
					if (i == 0)
					{
						newFishes[8] = _totalFishes[0];
						newFishes[6] = newFishes[6] + _totalFishes[0];

						continue;
					}

					newFishes[i - 1] = _totalFishes[i];
				}

				_totalFishes = newFishes;
			}

			return _totalFishes.Values.Sum();
		}
	}
}
