// © Traxion Development Services

namespace CodeBase
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using AdventBase.Helpers;
	using AdventBase.Interfaces;

	public class Day14 : IAdventDay
	{
		private readonly Dictionary<string, string> _pairs = new();
		private Dictionary<string, long> _polymerPairs = new();

		private bool LineParser(string line)
		{
			if (line.Contains("->"))
			{
				string[] pair = line.Split(" -> ", StringSplitOptions.RemoveEmptyEntries);
				_pairs.Add(pair[0], pair[1]);
			}
			else if (string.IsNullOrWhiteSpace(line))
			{
				return true;
			}
			else
			{
				for (var startIndex = 0; startIndex <= line.Length - 2; startIndex++)
				{
					string subString = line.Substring(startIndex, 2);

					if (_polymerPairs.ContainsKey(subString))
					{
						_polymerPairs[subString]++;
					}
					else
					{
						_polymerPairs.Add(subString, 1);
					}
				}
			}

			return true;
		}

		public void LoadInput(string fileName)
		{
			_ = InputLoader<bool>.LoadLines(fileName, LineParser);
		}

		public long PartOne()
		{
			ProcessPairs(10);

			return CalculateAnswer();
		}

		private void ProcessPairs(int days)
		{
			for (var day = 1; day <= days; day++)
			{
				Dictionary<string, long> newPolymerPairs = new();

				foreach (KeyValuePair<string, long> pair in _polymerPairs)
				{
					if (_pairs.ContainsKey(pair.Key))
					{
						string combined = pair.Key.Insert(1, _pairs[pair.Key]);

						IncreasePairValue(combined.Substring(0, 2), pair.Value, newPolymerPairs);
						IncreasePairValue(combined.Substring(1, 2), pair.Value, newPolymerPairs);
					}
					else
					{
						throw new ArgumentException("No pair?");
					}
				}

				_polymerPairs = newPolymerPairs;
			}
		}

		private void IncreasePairValue(string pairKey, long pairValue, Dictionary<string, long> polymerPairs)
		{
			if (polymerPairs.ContainsKey(pairKey))
			{
				polymerPairs[pairKey] += pairValue;
			}
			else
			{
				polymerPairs.Add(pairKey, pairValue);
			}
		}

		private long CalculateAnswer()
		{
			Dictionary<char, long> countable = new();

			foreach (KeyValuePair<string, long> polymerPair in _polymerPairs)
			{
				char newKey = polymerPair.Key.Last();

				if (countable.ContainsKey(newKey))
				{
					countable[newKey] += polymerPair.Value;
				}
				else
				{
					countable.Add(newKey, polymerPair.Value);
				}
			}

			long maxCount = countable.Values.Max();
			long minCount = countable.Values.Min();

			return maxCount - minCount;
		}

		public long PartTwo()
		{
			ProcessPairs(30);

			return CalculateAnswer();
		}
	}
}
