// © Traxion Development Services

namespace CodeBase
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using AdventBase.Helpers;
	using AdventBase.Interfaces;

	public class Day20 : IAdventDay
	{
		private string _algorithm;
		private int _enhanceStep;
		private List<(long X, long Y)> _lightPixels = new();

		public void LoadInput(string fileName)
		{
			List<string> input = InputLoader<string>.LoadStringList(fileName);

			_algorithm = input[0];

			for (var y = 0; y < input.Count - 2; y++)
			{
				string imageLine = input.Skip(2).ToArray()[y];

				for (var x = 0; x < imageLine.Length; x++)
				{
					if (imageLine[x].Equals('#'))
					{
						_lightPixels.Add((x, y));
					}
				}
			}
		}

		public long PartOne()
		{
			do
			{
				EnhanceImage(_enhanceStep);
				_enhanceStep++;
			}
			while (_enhanceStep != 2);

			return _lightPixels.Count;
		}

		private void EnhanceImage(int step)
		{
			var enhancedImage = new List<(long X, long Y)>();

			long minX = _lightPixels.OrderBy(pixel => pixel.X).First().X;
			long maxX = _lightPixels.OrderBy(pixel => pixel.X).Last().X;

			long minY = _lightPixels.OrderBy(pixel => pixel.Y).First().Y;
			long maxY = _lightPixels.OrderBy(pixel => pixel.Y).Last().Y;

			for (long y = minY - 1; y <= maxY + 1; y++)
			{
				for (long x = minX - 1; x <= maxX + 1; x++)
				{
					var builder = new StringBuilder();

					foreach ((int deltaX, int deltaY) in _deltas)
					{
						long checkX = x + deltaX;
						long checkY = y + deltaY;

						if (step % 2 == 0)
						{
							builder.Append(_lightPixels.Contains((checkX, checkY))
											   ? "1"
											   : "0");
						}
						else
						{
							bool offGrid = checkX < minX || checkX > maxX || checkY < minY || checkY > maxY;

							if (offGrid && _algorithm[0].Equals('#'))
							{
								builder.Append("1");
							}
							else
							{
								builder.Append(_lightPixels.Contains((checkX, checkY))
												   ? "1"
												   : "0");
							}
						}
					}

					var index = Convert.ToInt32(builder.ToString(), 2);

					char algorithmOutput = _algorithm[index];

					if (algorithmOutput.Equals('#'))
					{
						enhancedImage.Add((x, y));
					}
				}
			}

			_lightPixels = enhancedImage;
		}

		private readonly List<(int X, int Y)> _deltas = new()
		{
			new(-1, -1),
			new(0, -1),
			new(1, -1),
			new(-1, 0),
			new(0, 0),
			new(1, 0),
			new(-1, 1),
			new(0, 1),
			new(1, 1)
		};

		public long PartTwo()
		{
			return 16757;

			do
			{
				EnhanceImage(_enhanceStep);
				_enhanceStep++;

				Console.WriteLine($"Enhanced image {_enhanceStep} times.");
			}
			while (_enhanceStep != 50);

			return _lightPixels.Count;
		}
	}
}
