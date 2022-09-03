// © Traxion Development Services

namespace CodeBase
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using AdventBase.Helpers;
	using AdventBase.Interfaces;

	public class Day04 : IAdventDay
	{
		private readonly List<BingoCard> _bingoCards = new();

		private readonly List<int> _drawRange = new();

		private BingoCard _rawCard = new()
		{
			Bingos = new List<List<int>>()
		};

		private struct BingoCard
		{
			public List<List<int>> Bingos;
		}

		private void LineParser(string line)
		{
			if (_drawRange.Count == 0)
			{
				line.Split(',').ToList().ForEach(x => _drawRange.Add(int.Parse(x)));

				return;
			}

			if (string.IsNullOrWhiteSpace(line))
			{
				if (_rawCard.Bingos.Count > 0)
				{
					int count = _rawCard.Bingos.Count;

					for (var i = 0; i < _rawCard.Bingos[0].Count; i++)
					{
						List<int> column = _rawCard.Bingos.GetRange(0, count).Select(x => x[i]).ToList();
						_rawCard.Bingos.Add(column);
					}

					_bingoCards.Add(_rawCard);
				}

				_rawCard = new BingoCard
				{
					Bingos = new List<List<int>>()
				};

				return;
			}

			var row = new List<int>();
			line.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList().ForEach(x => row.Add(int.Parse(x)));
			_rawCard.Bingos.Add(row);
		}

		public void LoadInput(string fileName)
		{
			List<string> lines = InputLoader<int>.LoadStringList(GetType().Name);

			foreach (string line in lines)
			{
				LineParser(line);
			}

			LineParser(string.Empty);
		}

		public long PartOne()
		{
			return PlayBingo(true);
		}

		public long PartTwo()
		{
			return PlayBingo(false);
		}

		private int PlayBingo(bool firstBingo)
		{
			var bingo = false;

			for (var i = 5; i <= _drawRange.Count; i++)
			{
				List<int> drawnNumbers = _drawRange.GetRange(0, i);

				foreach (BingoCard bingoCard in new List<BingoCard>(_bingoCards))
				{
					if (bingoCard.Bingos.Any(possibleBingo => possibleBingo.TrueForAll(x => drawnNumbers.Contains(x))))
					{
						bingo = true;
					}

					if (bingo)
					{
						if (firstBingo || _bingoCards.Count == 1)
						{
							int bingoSum = bingoCard.Bingos.SelectMany(x => x)
													.Where(x => !drawnNumbers.Contains(x))
													.Distinct()
													.Sum();

							return bingoSum * drawnNumbers.Last();
						}

						_bingoCards.Remove(bingoCard);
						bingo = false;
					}
				}
			}

			throw new ArgumentException("There should have been a result by now.");
		}
	}
}
