// © Traxion Development Services

namespace CodeBase
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using AdventBase.Helpers;
	using AdventBase.Interfaces;

	public class Day03 : IAdventDay
	{
		private readonly List<BitArray> _bitStrings = new();
		private int _length;

		private bool LineParser(string line)
		{
			_bitStrings.Add(CreateBitArray(line));
			_length = line.Length;

			return true;
		}

		public void LoadInput(string fileName)
		{
			InputLoader<bool>.LoadLines(fileName, LineParser);
		}

		private BitArray CreateBitArray(string line)
		{
			bool[] array = line.ToCharArray().Reverse().Select(x => Convert.ToBoolean(Convert.ToInt32(x.ToString()))).ToArray();

			return new BitArray(array);
		}

		private long ParseBitArray(BitArray input)
		{
			var array = new int[1];
			input.CopyTo(array, 0);

			return array[0];
		}

		public long PartOne()
		{
			var gammaArray = new BitArray(_length);
			var epsilonArray = new BitArray(_length);

			for (var i = 0; i < _length; i++)
			{
				int gammaCount = _bitStrings.Where(x => x[i].Equals(true)).ToList().Count;

				bool gammaResult = gammaCount > _bitStrings.Count / 2;
				gammaArray[i] = gammaResult;
				epsilonArray[i] = !gammaResult;
			}

			long gammaInt = ParseBitArray(gammaArray);
			long epsilonInt = ParseBitArray(epsilonArray);

			return gammaInt * epsilonInt;
		}

		public long PartTwo()
		{
			BitArray oxygenArray = DetermineCommonalityResult(true);
			BitArray scrubberArray = DetermineCommonalityResult(false);

			long oxygenInt = ParseBitArray(oxygenArray);
			long scrubberInt = ParseBitArray(scrubberArray);

			return oxygenInt * scrubberInt;
		}

		private BitArray DetermineCommonalityResult(bool mostCommon)
		{
			var commonalityStrings = new List<BitArray>(_bitStrings);

			for (var i = 0; i < _length; i++)
			{
				int position = _length - (1 + i);

				if (commonalityStrings.Count == 1)
				{
					break;
				}

				List<BitArray> oneStrings = commonalityStrings.Where(x => x[position].Equals(true)).ToList();

				double commonalityBorder = Math.Ceiling(commonalityStrings.Count / 2.0);

				if (mostCommon
						? oneStrings.Count >= commonalityBorder
						: commonalityBorder > oneStrings.Count)
				{
					commonalityStrings = oneStrings;
				}
				else
				{
					commonalityStrings.RemoveAll(x => oneStrings.Contains(x));
				}
			}

			return commonalityStrings.First();
		}
	}
}
