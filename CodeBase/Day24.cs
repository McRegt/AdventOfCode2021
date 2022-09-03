// © Traxion Development Services

namespace CodeBase
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using AdventBase.Helpers;
	using AdventBase.Interfaces;

	public class Day24 : IAdventDay
	{
		private long _highestModelNumber = 0;

		public enum Operator
		{
			Input,
			Add,
			Multiply,
			Divide,
			Modulo,
			Equal
		}

		public class Processor
		{
			public long X;
			public long Y;
			public long Z;
			public long W;
		}

		public class Operation
		{
			public Operator Operator;
			public string Input1;
			public string Input2;
		}

		private readonly List<List<Operation>> _operations = new();

		public void LoadInput(string fileName)
		{
			List<string> input = InputLoader<string>.LoadStringList(fileName);

			foreach (string line in input)
			{
				string[] values = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

				Operator op = GetOperator(values[0]);

				if (op == Operator.Input)
				{
					_operations.Add(new List<Operation>());

					_operations.Last()
							   .Add(new Operation
							   {
								   Operator = op,
								   Input1 = values[1]
							   });
				}
				else
				{
					_operations.Last()
							   .Add(new Operation
							   {
								   Operator = op,
								   Input1 = values[1],
								   Input2 = values[2]
							   });
				}
			}
		}

		private Operator GetOperator(string value)
		{
			switch (value)
			{
				case "inp":
					return Operator.Input;
				case "add":
					return Operator.Add;
				case "mul":
					return Operator.Multiply;
				case "div":
					return Operator.Divide;
				case "mod":
					return Operator.Modulo;
				case "eql":
					return Operator.Equal;
			}

			throw new ArgumentException($"Unsupported operator {value}.");
		}

		private Processor ProcessMoves(Processor processor, long inputNumber, List<Operation> operations)
		{
			var inputIndex = 0;

			foreach (Operation operation in operations)
			{
				switch (operation.Operator)
				{
					case Operator.Input:
						string variable = operation.Input1;
						var number = Convert.ToInt64(char.GetNumericValue(inputNumber.ToString()[inputIndex]));

						//Console.WriteLine($"Set {variable} to {number}");

						switch (variable)
						{
							case "x":
								processor.X = number;

								break;
							case "y":
								processor.Y = number;

								break;
							case "z":
								processor.Z = number;

								break;
							case "w":
								processor.W = number;

								break;
						}

						inputIndex++;

						break;
					default:
						processor = ResolveFormula(processor, operation);

						break;
				}
			}

			return processor;
		}

		public Processor ResolveFormula(Processor processor, Operation operation)
		{
			long input1 = GetInput(processor, operation.Input1);
			long input2 = GetInput(processor, operation.Input2);

			switch (operation.Operator)
			{
				case Operator.Add:
					input1 += input2;

					break;
				case Operator.Multiply:
					input1 *= input2;

					break;
				case Operator.Divide:
					input1 = Convert.ToInt64(Math.Floor(decimal.Divide(input1, input2)));

					break;
				case Operator.Modulo:
					input1 %= input2;

					break;
				case Operator.Equal:
					input1 = input1 == input2
								 ? 1
								 : 0;

					break;
			}

			processor = SetOutput(processor, operation.Input1, input1);

			return processor;
		}

		private Processor SetOutput(Processor processor, string input, long output)
		{
			switch (input)
			{
				case "x":
					processor.X = output;

					break;
				case "y":
					processor.Y = output;

					break;
				case "z":
					processor.Z = output;

					break;
				case "w":
					processor.W = output;

					break;
			}

			return processor;
		}

		private long GetInput(Processor processor, string input)
		{
			switch (input)
			{
				case "x":
					return processor.X;
				case "y":
					return processor.Y;
				case "z":
					return processor.Z;
				case "w":
					return processor.W;
				default:
					return Convert.ToInt64(input);
			}
		}

		public long PartOne()
		{
			List<long> validInputs = new();

			List<Processor> processorStates = new()
			{
				new Processor(),
				new Processor(),
				new Processor(),
				new Processor(),
				new Processor(),
				new Processor(),
				new Processor(),
				new Processor(),
				new Processor(),
			};

			for (var round = 0; round < 14; round++)
			{
				List<Operation> operations = _operations[round];

				List<Processor> newProcessorStates = new();

				for (long input = 9; input > 0; input--)
				{
					var state = 0;

					foreach (Processor previousState in processorStates)
					{
						state++;
						Processor newState = ProcessMoves(previousState, input, operations);
						newProcessorStates.Add(newState);

						Console.WriteLine($"{round}-{input}-{state} => {previousState.X}, {previousState.Y}, {previousState.Z}, {previousState.W} => {newState.X}, {newState.Y}, {newState.Z}, {newState.W}");
					}
				}

				processorStates = newProcessorStates;

				Console.WriteLine($"{processorStates.Count} states after round {round}.");
			}

			return validInputs.Max();
		}

		public long PartTwo()
		{
			return 0;
		}
	}
}
