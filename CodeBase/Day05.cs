// © Traxion Development Services

namespace CodeBase
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using AdventBase.Helpers;
	using AdventBase.Interfaces;

	public class Day05 : IAdventDay
	{
		private readonly List<ThermalVent> _thermalVents = new();

		private readonly HashSet<string> _uniqueVents = new();
		private readonly List<string> _duplicateVents = new();

		private struct ThermalVent
		{
			public Tuple<int, int> X;
			public Tuple<int, int> Y;
		}

		private ThermalVent LineParser(string line)
		{
			ThermalVent thermalVent = new();

			int x1;
			int x2;

			int y1;
			int y2;

			List<string> coordinates = line.Replace("->", "")
										   .Split(' ', StringSplitOptions.RemoveEmptyEntries)
										   .ToList();

			List<string> coordinate1 = coordinates.First()
												  .Split(',', StringSplitOptions.RemoveEmptyEntries)
												  .ToList();

			x1 = int.Parse(coordinate1.First());
			y1 = int.Parse(coordinate1.Last());

			List<string> coordinate2 = coordinates.Last()
												  .Split(',', StringSplitOptions.RemoveEmptyEntries)
												  .ToList();

			x2 = int.Parse(coordinate2.First());
			y2 = int.Parse(coordinate2.Last());

			thermalVent.X = new Tuple<int, int>(x1, x2);
			thermalVent.Y = new Tuple<int, int>(y1, y2);

			_thermalVents.Add(thermalVent);

			return thermalVent;
		}

		public void LoadInput(string fileName)
		{
			_ = InputLoader<ThermalVent>.LoadLines(fileName, LineParser);
		}

		public long PartOne()
		{
			List<ThermalVent> vents = _thermalVents.Where(vent => vent.X.Item1.Equals(vent.X.Item2) || vent.Y.Item1.Equals(vent.Y.Item2)).ToList();

			foreach (ThermalVent vent in vents)
			{
				int x1 = vent.X.Item1;
				int x2 = vent.X.Item2;

				int y1 = vent.Y.Item1;
				int y2 = vent.Y.Item2;

				if (x2 < x1)
				{
					(x2, x1) = (x1, x2);
				}

				if (y2 < y1)
				{
					(y2, y1) = (y1, y2);
				}

				for (int y = y1; y <= y2; y++)
				{
					for (int x = x1; x <= x2; x++)
					{
						var ventSpot = $"{x}-{y}";

						if (!_uniqueVents.Add(ventSpot))
						{
							_duplicateVents.Add(ventSpot);
						}
					}
				}
			}

			return _duplicateVents.Distinct().Count();
		}

		public long PartTwo()
		{
			List<ThermalVent> vents = _thermalVents.Where(vent => !vent.X.Item1.Equals(vent.X.Item2) && !vent.Y.Item1.Equals(vent.Y.Item2)).ToList();

			foreach (ThermalVent vent in vents)
			{
				int slope = (vent.Y.Item1 - vent.Y.Item2) / (vent.X.Item1 - vent.X.Item2);

				int offset = vent.Y.Item1 - vent.X.Item1 * slope;

				for (int x = vent.X.Item1; x <= vent.X.Item2; x++)
				{
					int y = slope * x + offset;

					var ventSpot = $"{x}-{y}";

					if (!_uniqueVents.Add(ventSpot))
					{
						_duplicateVents.Add(ventSpot);
					}
				}

				for (int x = vent.X.Item1; x >= vent.X.Item2; x--)
				{
					int y = slope * x + offset;

					var ventSpot = $"{x}-{y}";

					if (!_uniqueVents.Add(ventSpot))
					{
						_duplicateVents.Add(ventSpot);
					}
				}
			}

			return _duplicateVents.Distinct().Count();
		}
	}
}
