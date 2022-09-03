// © Traxion Development Services

namespace CodeBase
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using AdventBase.Helpers;
	using AdventBase.Interfaces;

	public class Day13 : IAdventDay
	{
		private readonly HashSet<(int X, int Y)> _dots = new();
		private readonly Dictionary<int, string> _folds = new();

		private bool LineParser(string line)
		{
			if (line.Contains(','))
			{
				string[] coordinates = line.Split(',', StringSplitOptions.RemoveEmptyEntries);
				_dots.Add((int.Parse(coordinates[0]), int.Parse(coordinates[1])));
			}
			else if (string.IsNullOrWhiteSpace(line))
			{
				return true;
			}
			else
			{
				string[] fold = line.Replace("fold along ", "").Split('=', StringSplitOptions.RemoveEmptyEntries);
				_folds.Add(int.Parse(fold[1]), fold[0]);
			}

			return true;
		}

		public void LoadInput(string fileName)
		{
			_ = InputLoader<bool>.LoadLines(fileName, LineParser);
		}

		public long PartOne()
		{
			KeyValuePair<int, string> firstFold = _folds.First();

			Fold(firstFold);

			return _dots.Count;
		}

		private void Fold(KeyValuePair<int, string> fold)
		{
			if (fold.Value.Equals("x"))
			{
				FoldX(fold.Key);
			}
			else
			{
				FoldY(fold.Key);
			}
		}

		private void FoldX(int foldLine)
		{
			List<(int X, int Y)> foldingDots = _dots.Where(dot => dot.X > foldLine).ToList();

			foreach ((int X, int Y) foldingDot in foldingDots)
			{
				int x = foldingDot.X - (foldingDot.X - foldLine) * 2;

				_dots.Add((x, foldingDot.Y));
			}

			_dots.RemoveWhere(dot => dot.X > foldLine);
		}

		private void FoldY(int foldLine)
		{
			List<(int X, int Y)> foldingDots = _dots.Where(dot => dot.Y > foldLine).ToList();

			foreach ((int X, int Y) foldingDot in foldingDots)
			{
				int y = foldingDot.Y - (foldingDot.Y - foldLine) * 2;

				_dots.Add((foldingDot.X, y));
			}

			_dots.RemoveWhere(dot => dot.Y > foldLine);
		}

		public long PartTwo()
		{
			foreach (KeyValuePair<int, string> fold in _folds.Skip(1))
			{
				Fold(fold);
			}

			// Print the answer.
			int maxX = _dots.Max(dot => dot.X);
			int maxY = _dots.Max(dot => dot.Y);

			Console.WriteLine("");

			for (var y = 0; y <= maxY; y++)
			{
				for (var x = 0; x <= maxX; x++)
				{
					if (_dots.Contains((x, y)))
					{
						Console.Write('#');
					}
					else
					{
						Console.Write(' ');
					}
				}

				Console.Write(Environment.NewLine);
			}

			Console.WriteLine("");

			return 0;
		}
	}
}
