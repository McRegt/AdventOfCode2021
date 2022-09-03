// © Traxion Development Services

namespace CodeBase
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using AdventBase.Helpers;
	using AdventBase.Interfaces;

	public class Day22 : IAdventDay
	{
		private readonly List<Cuboid> _inputCuboids = new();

		private struct Vector3
		{
			public long X;
			public long Y;
			public long Z;
		}

		private struct Cuboid
		{
			public Vector3 Min;
			public Vector3 Max;
			public bool TurnOn;
		}

		public void LoadInput(string fileName)
		{
			List<string> input = InputLoader<string>.LoadStringList(fileName);

			foreach (string line in input)
			{
				string[] onOff = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

				bool turnOn = onOff[0] == "on";

				string[] coordinates = onOff[1].Split(',', StringSplitOptions.RemoveEmptyEntries);
				string[] xValues = coordinates[0].Replace("x=", "").Split("..", StringSplitOptions.RemoveEmptyEntries);
				string[] yValues = coordinates[1].Replace("y=", "").Split("..", StringSplitOptions.RemoveEmptyEntries);
				string[] zValues = coordinates[2].Replace("z=", "").Split("..", StringSplitOptions.RemoveEmptyEntries);

				var from = new Vector3
				{
					X = long.Parse(xValues.First()),
					Y = long.Parse(yValues.First()),
					Z = long.Parse(zValues.First())
				};

				var to = new Vector3
				{
					X = long.Parse(xValues.Last()),
					Y = long.Parse(yValues.Last()),
					Z = long.Parse(zValues.Last())
				};

				Cuboid cuboid = new()
				{
					Min = from,
					Max = to,
					TurnOn = turnOn
				};

				_inputCuboids.Add(cuboid);
			}
		}

		public long PartOne()
		{
			return CalculateResultingVolumeWithMaxRange(50);
		}

		public long PartTwo()
		{
			return CalculateResultingVolumeWithMaxRange(long.MaxValue);
		}

		private long CalculateResultingVolumeWithMaxRange(long range)
		{
			List<Cuboid> resultingCubes = new();

			foreach (Cuboid cube in _inputCuboids.Where(x => Math.Abs(x.Min.X) <= range))
			{
				List<Cuboid> cubeForCubes = new();

				if (cube.TurnOn)
				{
					// No need to add cubes that turn off cubes, the default is already turned off.
					cubeForCubes.Add(cube);
				}

				foreach (Cuboid otherCube in resultingCubes)
				{
					// Calculate the intersect.
					Cuboid ? intersectionCube = CalculateIntersect(cube, otherCube);

					if (intersectionCube != null)
					{
						// Add inverted cube to cubes
						cubeForCubes.Add(intersectionCube.Value);
					}
				}

				resultingCubes.AddRange(cubeForCubes);
			}

			long result = CalculateResultingVolumes(resultingCubes);

			return result;
		}

		private long CalculateResultingVolumes(List<Cuboid> resultingCubes)
		{
			long result = 0;

			foreach (Cuboid cube in resultingCubes)
			{
				// Calculate the volume for the cube.
				var volume = Convert.ToInt64((cube.Max.X - cube.Min.X + 1) * (cube.Max.Y - cube.Min.Y + 1) * (cube.Max.Z - cube.Min.Z + 1));

				// Depending on the action add or subtract from the total result.
				if (cube.TurnOn)
				{
					result += volume;
				}
				else
				{
					result -= volume;
				}
			}

			return result;
		}

		private Cuboid ? CalculateIntersect(Cuboid cube, Cuboid otherCube)
		{
			// There is no intersect between the cubes.
			if (cube.Min.X > otherCube.Max.X
				|| cube.Max.X < otherCube.Min.X
				|| cube.Min.Y > otherCube.Max.Y
				|| cube.Max.Y < otherCube.Min.Y
				|| cube.Min.Z > otherCube.Max.Z
				|| cube.Max.Z < otherCube.Min.Z)
			{
				return null;
			}

			// Calculate the dimensions of the intersect.
			var xMin = Convert.ToInt64(Math.Max(cube.Min.X, otherCube.Min.X));
			var xMax = Convert.ToInt64(Math.Min(cube.Max.X, otherCube.Max.X));

			var yMin = Convert.ToInt64(Math.Max(cube.Min.Y, otherCube.Min.Y));
			var yMax = Convert.ToInt64(Math.Min(cube.Max.Y, otherCube.Max.Y));

			var zMin = Convert.ToInt64(Math.Max(cube.Min.Z, otherCube.Min.Z));
			var zMax = Convert.ToInt64(Math.Min(cube.Max.Z, otherCube.Max.Z));

			// The intersect for the given cube results in a list of intersection cubes.
			// Where the action on the intersect for the cube is the inverse of the compared cube.
			// If the cube being processed turns cubes on, the intersection will need to account for previously processed cubes that turned in off or on.
			// As such return the inverse of the cube being compared with.
			// This will result in turning cubes on and off in sequence, ending up with the correct result.
			return new Cuboid
			{
				Min = new Vector3
				{
					X = xMin,
					Y = yMin,
					Z = zMin
				},
				Max = new Vector3
				{
					X = xMax,
					Y = yMax,
					Z = zMax
				},

				TurnOn = !otherCube.TurnOn
			};
		}
	}
}
