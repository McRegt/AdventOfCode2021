// © Traxion Development Services

namespace CodeBase
{
	using System.Collections.Generic;
	using AdventBase.Helpers;
	using AdventBase.Interfaces;

	public class Day02 : IAdventDay
	{
		private List<Command> _commands = new();

		public struct Command
		{
			public string Direction;
			public int Value;
		}

		public Command LineParser(string line)
		{
			string[] array = line.Split(' ');

			var command = new Command
			{
				Direction = array[0],
				Value = int.Parse(array[1])
			};

			return command;
		}

		public void LoadInput(string fileName)
		{
			_commands = InputLoader<Command>.LoadLines(fileName, LineParser);
		}

		public long PartOne()
		{
			var forward = 0;
			var depth = 0;

			_commands.ForEach(x =>
			{
				switch (x.Direction)
				{
					case "up":
						depth = depth - x.Value;

						break;
					case "down":
						depth = depth + x.Value;

						break;
					default:
						forward = forward + x.Value;

						break;
				}
			});

			return forward * depth;
		}

		public long PartTwo()
		{
			var forward = 0;
			var aim = 0;
			var depth = 0;

			_commands.ForEach(x =>
			{
				switch (x.Direction)
				{
					case "up":
						aim = aim - x.Value;

						break;
					case "down":
						aim = aim + x.Value;

						break;
					default:
						forward = forward + x.Value;
						depth = depth + aim * x.Value;

						break;
				}
			});

			return forward * depth;
		}
	}
}
