// © Traxion Development Services

namespace CodeBase
{
	using System.Collections.Generic;
	using AdventBase.Helpers;
	using AdventBase.Interfaces;

	public class Day01 : IAdventDay
	{
		private List<int> _integerList = new();

		public void LoadInput(string fileName)
		{
			_integerList = InputLoader<int>.LoadIntList(fileName);
		}

		public long PartOne()
		{
			var depthIncreases = 0;

			for (var i = 1; i < _integerList.Count; i++)
			{
				if (_integerList[i - 1] < _integerList[i])
				{
					depthIncreases++;
				}
			}

			return depthIncreases;
		}

		public long PartTwo()
		{
			var depthIncreases = 0;

			for (var i = 3; i < _integerList.Count; i++)
			{
				int previousRange = _integerList[i - 3] + _integerList[i - 2] + _integerList[i - 1];
				int currentRange = _integerList[i - 2] + _integerList[i - 1] + _integerList[i];

				if (previousRange < currentRange)
				{
					depthIncreases++;
				}
			}

			return depthIncreases;
		}
	}
}
