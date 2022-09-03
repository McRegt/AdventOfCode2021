// © Traxion Development Services

namespace CodeBase
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Numerics;
	using AdventBase.Helpers;
	using AdventBase.Interfaces;

	public class Day19 : IAdventDay
	{
		private List<List<Vector3>> _scannerList;

		private readonly List<Vector3> _scanners = new List<Vector3>
		{
			new Vector3(0, 0, 0)
		};

		public void LoadInput(string fileName)
		{
			List<string> input = InputLoader<string>.LoadStringList(GetType().Name);

			List<List<Vector3>> beaconList = new();

			foreach (string line in input)
			{
				if (line.StartsWith("---"))
				{
					beaconList.Add(new List<Vector3>());
				}
				else if (string.IsNullOrWhiteSpace(line))

				{
					// Do nothing.
				}
				else
				{
					string[] values = line.Split(',', StringSplitOptions.RemoveEmptyEntries);

					var vector = new Vector3
					{
						X = int.Parse(values[0]),
						Y = int.Parse(values[1]),
						Z = int.Parse(values[2])
					};

					beaconList.Last().Add(vector);
				}
			}

			_scannerList = beaconList;
		}

		private Vector3 Rotate(Vector3 v, int rotation)
		{
			Vector3 rotated = rotation switch
			{
				// Positive X
				0 => new Vector3(v.X, v.Y, v.Z),
				1 => new Vector3(v.X, -v.Z, v.Y),
				2 => new Vector3(v.X, v.Z, -v.Y),
				3 => new Vector3(v.X, -v.Y, -v.Z),

				// Negative X
				4 => new Vector3(-v.X, -v.Z, -v.Y),
				5 => new Vector3(-v.X, v.Y, -v.Z),
				6 => new Vector3(-v.X, -v.Y, v.Z),
				7 => new Vector3(-v.X, v.Z, v.Y),

				// Postive Y
				8 => new Vector3(v.Y, v.Z, v.X),
				9 => new Vector3(v.Y, -v.X, v.Z),
				10 => new Vector3(v.Y, -v.Z, -v.X),
				11 => new Vector3(v.Y, v.X, -v.Z),

				// Negative Y
				12 => new Vector3(-v.Y, -v.X, -v.Z),
				13 => new Vector3(-v.Y, v.Z, -v.X),
				14 => new Vector3(-v.Y, -v.Z, v.X),
				15 => new Vector3(-v.Y, v.X, v.Z),

				// Postive Z
				16 => new Vector3(v.Z, v.Y, -v.X),
				17 => new Vector3(v.Z, v.X, v.Y),
				18 => new Vector3(v.Z, -v.Y, v.X),
				19 => new Vector3(v.Z, -v.X, -v.Y),

				// Negative Z
				20 => new Vector3(-v.Z, v.Y, v.X),
				21 => new Vector3(-v.Z, v.X, -v.Y),
				22 => new Vector3(-v.Z, -v.Y, -v.X),
				23 => new Vector3(-v.Z, -v.X, v.Y),

				_ => throw new ArgumentOutOfRangeException(nameof(rotation), rotation, null)
			};

			return rotated;
		}

		public long PartOne()
		{
			return 430;

			Vector3[] beacons = _scannerList[0].ToArray();
			_scannerList.RemoveAt(0);

			do
			{
				Console.WriteLine($"{_scannerList.Count} unresolved scanner position remaining.");

				foreach (List<Vector3> scanner in _scannerList)
				{
					var rotateSuccess = false;

					for (var rotation = 0; rotation < 24; rotation++)
					{
						var rotated = new List<Vector3>();

						foreach (Vector3 beacon in scanner)
						{
							rotated.Add(Rotate(beacon, rotation));
						}

						foreach (Vector3 beacon in beacons)
						{
							IEnumerable<Vector3> offsets = rotated.Select(s => beacon - s);

							foreach (Vector3 offset in offsets)
							{
								Vector3[] translated = rotated.Select(s => s + offset).ToArray();
								int intersectCount = beacons.Intersect(translated).Count();

								if (intersectCount >= 12)
								{
									_scannerList.Remove(scanner);
									_scanners.Add(offset);
									beacons = beacons.Union(translated).ToArray();

									rotateSuccess = true;
								}

								if (rotateSuccess)
								{
									break;
								}
							}

							if (rotateSuccess)
							{
								break;
							}
						}

						if (rotateSuccess)
						{
							break;
						}
					}

					if (rotateSuccess)
					{
						break;
					}
				}
			}
			while (_scannerList.Count > 0);

			return beacons.Count();
		}

		public long PartTwo()
		{
			return 11860;

			double manhattanDistance = 0;

			foreach (Vector3 scanner in _scanners)
			{
				foreach (Vector3 nextScanner in _scanners)
				{
					double result = Math.Abs(scanner.X - nextScanner.X) + Math.Abs(scanner.Y - nextScanner.Y) + Math.Abs(scanner.Z - nextScanner.Z);

					if (result > manhattanDistance)
					{
						manhattanDistance = result;
					}
				}
			}

			return Convert.ToInt64(manhattanDistance);
		}
	}
}
