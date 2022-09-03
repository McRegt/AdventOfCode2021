// © Traxion Development Services

namespace CodeBase
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Linq;
	using System.Text;
	using AdventBase.Helpers;
	using AdventBase.Interfaces;

	public class DaySnailPair
	{
		public long Depth;
		public long Left;
		public long Right;
	}

	public class Day18 : IAdventDay
	{
		private List<DaySnailPair> _snailPairs = new();
		private List<string> _input;

		public void LoadInput(string fileName)
		{
			_input = InputLoader<string>.LoadStringList(fileName);
		}

		public long PartOne()
		{
			string first = _input.First();

			for (var index = 1; index < _input.Count; index++)
			{
				var concat = $"[{first},{_input[index]}]";

				ConstructPairs(concat);
				ProcessPairs(_snailPairs);

				string result = ReverseToString(_snailPairs);
				first = result;
			}

			string formula = first.Replace("[", "(3*").Replace(",", "+").Replace("]", "*2)");

			var magnitude = Convert.ToDouble(new DataTable().Compute(formula, null));

			return Convert.ToInt64(magnitude);
		}

		private void ProcessPairs(List<DaySnailPair> pairs)
		{
			if (pairs.Any(pair => pair.Depth > 4))
			{
				DaySnailPair pairToExplode = pairs.First(pair => pair.Depth > 4);
				int indexOfExplosion = pairs.IndexOf(pairToExplode);

				// Process the left number.
				if (indexOfExplosion != 0)
				{
					var valueSet = false;
					var step = 1;

					do
					{
						if (!pairs[indexOfExplosion - step].Right.Equals(-1))
						{
							pairs[indexOfExplosion - step].Right += pairToExplode.Left;
							valueSet = true;
						}
						else if (!pairs[indexOfExplosion - step].Left.Equals(-1))
						{
							pairs[indexOfExplosion - step].Left += pairToExplode.Left;
							valueSet = true;
						}
						else
						{
							step++;
						}
					}
					while (!valueSet);
				}

				// Process the right number.
				if (indexOfExplosion != pairs.Count - 1)
				{
					var valueSet = false;
					var step = 1;

					do
					{
						if (!pairs[indexOfExplosion + step].Left.Equals(-1))
						{
							pairs[indexOfExplosion + step].Left += pairToExplode.Right;
							valueSet = true;
						}
						else if (!pairs[indexOfExplosion + step].Right.Equals(-1))
						{
							pairs[indexOfExplosion + step].Right += pairToExplode.Right;
							valueSet = true;
						}
						else
						{
							step++;
						}
					}
					while (!valueSet);
				}

				// Destroy the pair and replace parent value with a zero.
				if (indexOfExplosion != 0
					&& pairs[indexOfExplosion - 1].Right == -1
					&& pairs[indexOfExplosion - 1].Depth + 1 == pairToExplode.Depth)
				{
					pairs[indexOfExplosion - 1].Right = 0;
				}
				else if (indexOfExplosion != pairs.Count - 1
						 && pairs[indexOfExplosion + 1].Left == -1
						 && pairs[indexOfExplosion + 1].Depth + 1 == pairToExplode.Depth)
				{
					pairs[indexOfExplosion + 1].Left = 0;
				}

				pairs.Remove(pairToExplode);
			}
			else if (pairs.Any(pair => pair.Left >= 10 || pair.Right >= 10))
			{
				DaySnailPair pairToSplit = pairs.First(pair => pair.Left >= 10 || pair.Right >= 10);
				int indexOfSplit = pairs.IndexOf(pairToSplit);

				if (pairToSplit.Left >= 10)
				{
					var left = Convert.ToInt64(Math.Floor(pairToSplit.Left / 2.0));
					var right = Convert.ToInt64(Math.Ceiling(pairToSplit.Left / 2.0));

					var newPair = new DaySnailPair
					{
						Depth = pairToSplit.Depth + 1,
						Left = left,
						Right = right
					};

					pairs[indexOfSplit].Left = -1;
					pairs.Insert(indexOfSplit, newPair);
				}
				else if (pairToSplit.Right >= 10)
				{
					var left = Convert.ToInt64(Math.Floor(pairToSplit.Right / 2.0));
					var right = Convert.ToInt64(Math.Ceiling(pairToSplit.Right / 2.0));

					var newPair = new DaySnailPair
					{
						Depth = pairToSplit.Depth + 1,
						Left = left,
						Right = right
					};

					pairs[indexOfSplit].Right = -1;
					pairs.Insert(indexOfSplit + 1, newPair);
				}
			}
			else
			{
				return;
			}

			// Then recursively process the pairs.
			ProcessPairs(pairs);
		}

		private string ReverseToString(List<DaySnailPair> pairs)
		{
			long depth = pairs.First().Depth;

			var builder = new StringBuilder();

			long startDepth = 0;

			while (startDepth != depth)
			{
				builder.Append("[");
				startDepth++;
			}

			foreach (DaySnailPair pair in pairs)
			{
				long tempDepth = pair.Depth;

				while (tempDepth < depth)
				{
					builder.Append("]");
					depth--;
				}

				while (tempDepth > depth)
				{
					builder.Append("[");
					depth++;
				}

				if (pair.Left != -1)
				{
					builder.Append($"{pair.Left}");
				}

				builder.Append(",");

				if (pair.Right != -1)
				{
					builder.Append($"{pair.Right}]");
					depth--;
				}
			}

			while (depth > 0)
			{
				builder.Append("]");
				depth--;
			}

			return builder.ToString();
		}

		private void ConstructPairs(string input)
		{
			_snailPairs = new();

			long depth = 0;
			long left = -1;
			long right = -1;

			for (var index = 0; index < input.Length; index++)
			{
				char character = input[index];

				if (character.Equals('['))
				{
					depth++;

					continue;
				}

				if (char.IsNumber(character))
				{
					if (input[index + 1].Equals(','))
					{
						left = long.Parse(character.ToString());
					}
					else
					{
						right = long.Parse(character.ToString());
					}
				}

				if (character.Equals(',')
					&& input[index + 1].Equals('['))
				{
					var snailPair = new DaySnailPair
					{
						Depth = depth,
						Left = left,
						Right = right
					};

					_snailPairs.Add(snailPair);

					// Reset values
					left = -1;
					right = -1;
				}

				if (character.Equals(']'))
				{
					if (left != -1
						|| right != -1)
					{
						var snailPair = new DaySnailPair
						{
							Depth = depth,
							Left = left,
							Right = right
						};

						_snailPairs.Add(snailPair);

						left = -1;
						right = -1;
					}

					// Reset values
					depth--;
				}
			}
		}

		public long PartTwo()
		{
			long maxMagnitude = 0;

			for (var index = 0; index < _input.Count; index++)
			{
				string firstString = _input[index];

				for (var secondIndex = 0; secondIndex < _input.Count; secondIndex++)
				{
					// Skip itself.
					if (index == secondIndex)
					{
						continue;
					}

					string secondString = _input[secondIndex];

					var concat = $"[{firstString},{secondString}]";

					ConstructPairs(concat);
					ProcessPairs(_snailPairs);

					string result = ReverseToString(_snailPairs);

					string formula = result.Replace("[", "(3*").Replace(",", "+").Replace("]", "*2)");
					var magnitude = Convert.ToInt64(Convert.ToDouble(new DataTable().Compute(formula, null)));

					if (magnitude > maxMagnitude)
					{
						maxMagnitude = magnitude;
					}
				}
			}

			return maxMagnitude;
		}
	}
}
