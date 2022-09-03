// © Traxion Development Services

namespace CodeBase
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using AdventBase.Helpers;
	using AdventBase.Interfaces;

	public class Day16 : IAdventDay
	{
		private string _hexInput;
		private long _versionNumberTotal;
		private long _expressionValue;

		public void LoadInput(string fileName)
		{
			List<string> input = InputLoader<string>.LoadStringList(fileName);

			_hexInput = input.First();
		}

		public long PartOne()
		{
			string binaryString = string.Join(string.Empty,
											  _hexInput.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));

			while (binaryString.Contains("1"))
			{
				(long Version, long TypeId, string Remainder, long Value) result = ParseBinaryString(binaryString);
				binaryString = result.Remainder;
				_expressionValue = result.Value;
			}

			return _versionNumberTotal;
		}

		private (long Version, long TypeId, string Remainder, long Value) ParseBinaryString(string input)
		{
			(long Version, long TypeId, string Remainder, long Value) result = ResolveHeaders(input);

			// Part one solution addition
			_versionNumberTotal += result.Version;

			if (result.TypeId == 4)
			{
				(string Remainder, long Value) literalResult = ResolveLiteralValue(result.Remainder);
				result.Remainder = literalResult.Remainder;
				result.Value = literalResult.Value;
			}
			else
			{
				(string Remainder, long Value) operatorResult = ResolveOperatorValue(result.Remainder, result.TypeId);
				result.Remainder = operatorResult.Remainder;
				result.Value = operatorResult.Value;
			}

			return result;
		}

		private (string Remainder, long Value) ResolveLiteralValue(string input, string partialInput = null)
		{
			// The value must be parsed in sections of 5 until a section that starts with 0 is found.
			bool next = input.Substring(0, 1).Equals("1");
			string partial = input.Substring(1, 4);

			input = input.Remove(0, 5);

			long resultValue = 0;

			if (next)
			{
				(string Remainder, long Value) literal = ResolveLiteralValue(input, string.Concat(partialInput, partial));
				input = literal.Remainder;
				resultValue = literal.Value;
			}
			else
			{
				var value = Convert.ToInt64(string.Concat(partialInput, partial), 2);

				return (input, value);
			}

			return (input, resultValue);
		}

		private (string Remainder, List<long> Values) ParseOperatorOnLength(string input, long operatorLength)
		{
			var processedLength = 0;

			List<long> values = new();

			while (processedLength < operatorLength)
			{
				(long Version, long TypeId, string Remainder, long Value) result = ParseBinaryString(input);

				processedLength += input.Length - result.Remainder.Length;
				input = result.Remainder;
				values.Add(result.Value);
			}

			return (input, values);
		}

		private (string Remainder, List<long> Values) ParseOperatorOnPacketNumber(string input, long subpackets)
		{
			var packagesProcessed = 0;

			List<long> values = new();

			while (packagesProcessed < subpackets)
			{
				(long Version, long TypeId, string Remainder, long Value) result = ParseBinaryString(input);

				packagesProcessed++;
				input = result.Remainder;
				values.Add(result.Value);
			}

			return (input, values);
		}

		private (string Remainder, long Value) ResolveOperatorValue(string input, long type)
		{
			// Take the first character to determine what the length parameter is.
			bool lengthMode = input.Substring(0, 1).Equals("0");

			input = input.Remove(0, 1);

			List<long> values;

			if (lengthMode)
			{
				var length = 15;
				var operatorLength = Convert.ToInt64(input.Substring(0, length), 2);
				input = input.Remove(0, length);

				// Process length mode.
				(string Remainder, List<long> Values) lengthResult = ParseOperatorOnLength(input, operatorLength);
				input = lengthResult.Remainder;
				values = lengthResult.Values;
			}
			else
			{
				var length = 11;
				var packets = Convert.ToInt32(input.Substring(0, length), 2);
				input = input.Remove(0, length);

				// Process number of packets
				(string Remainder, List<long> Values) numberResult = ParseOperatorOnPacketNumber(input, packets);
				input = numberResult.Remainder;
				values = numberResult.Values;
			}

			long calculatedValue = CalculateValue(values, type);

			return (input, calculatedValue);
		}

		private long CalculateValue(List<long> values, long type)
		{
			switch (type)
			{
				case 0:
					return values.Sum();
				case 1:
					return values.Aggregate((a, b) => a * b);
				case 2:
					return values.Min();
				case 3:
					return values.Max();
				case 5:
					return values[0] > values[1]
							   ? 1
							   : 0;
				case 6:
					return values[0] < values[1]
							   ? 1
							   : 0;
				case 7:
					return values[0] == values[1]
							   ? 1
							   : 0;
			}

			throw new ArgumentException($"Type {type} is not supported.");
		}

		private (long Version, long TypeId, string Remainder, long Value) ResolveHeaders(string input)
		{
			var version = Convert.ToInt64(input.Substring(0, 3), 2);
			var typeId = Convert.ToInt64(input.Substring(3, 3), 2);

			return (version, typeId, input.Remove(0, 6), 0);
		}

		public long PartTwo()
		{
			return _expressionValue;
		}
	}
}
