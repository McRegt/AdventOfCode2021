// © Traxion Development Services

namespace CodeBase
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using AdventBase.Helpers;
	using AdventBase.Interfaces;

	public class Day08 : IAdventDay
	{
		private readonly List<Tuple<string, string>> _digitEntries = new();

		private bool LineParser(string line)
		{
			List<string> result = line.Split('|', StringSplitOptions.RemoveEmptyEntries).ToList();
			_digitEntries.Add(new Tuple<string, string>(result[0], result[1]));

			return true;
		}

		public void LoadInput(string fileName)
		{
			_ = InputLoader<bool>.LoadLines(fileName, LineParser);
		}

		public long PartOne()
		{
			long easyDigitCount = 0;

			var easyDigitLengths = new List<long>
			{
				2, // Representing 1
				3, // Representing 7
				4, // Representing 4
				7 // Representing 8
			};

			_digitEntries.ForEach(x =>
			{
				easyDigitCount += x.Item2.Split(' ', StringSplitOptions.RemoveEmptyEntries).Count(s => easyDigitLengths.Contains(s.Length));
			});

			return easyDigitCount;
		}

		private readonly List<(long Number, long Count)> _expectedList = new()
		{
			(0, 42),
			(1, 17),
			(2, 34),
			(3, 39),
			(4, 30),
			(5, 37),
			(6, 41),
			(7, 25),
			(8, 49),
			(9, 45)
		};

		public long PartTwo()
		{
			long totalValue = 0;

			foreach (Tuple<string, string> input in _digitEntries)
			{
				List<char> characters = input.Item1.Replace(" ", "").ToCharArray().ToList();

				Dictionary<char, int> result = characters.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());

				List<string> outputValues = input.Item2.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

				StringBuilder builder = new();

				foreach (string outputValue in outputValues)
				{
					var individualValue = 0;
					char[] array = outputValue.ToCharArray();

					foreach (char character in array)
					{
						individualValue += result[character];
					}

					builder.Append(_expectedList.First(x => x.Count.Equals(individualValue)).Number);
				}

				totalValue += long.Parse(builder.ToString());
			}

			return totalValue;
		}
	}
}
