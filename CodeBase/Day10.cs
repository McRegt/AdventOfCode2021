// © Traxion Development Services

namespace CodeBase
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using AdventBase.Helpers;
	using AdventBase.Interfaces;

	public class Day10 : IAdventDay
	{
		private readonly List<string> _syntaxLines = new();
		private readonly List<string> _incompleteLines = new();

		private readonly Dictionary<char, char> _syntaxPairs = new()
		{
			{
				'{', '}'
			},
			{
				'[', ']'
			},
			{
				'(', ')'
			},
			{
				'<', '>'
			},
		};

		private bool LineParser(string line)
		{
			_syntaxLines.Add(line);

			return true;
		}

		public void LoadInput(string fileName)
		{
			_ = InputLoader<bool>.LoadLines(fileName, LineParser);
		}

		public long PartOne()
		{
			long result = 0;

			foreach (string syntaxLine in _syntaxLines)
			{
				List<char> syntaxProcessing = new();
				var invalid = false;

				foreach (char character in syntaxLine)
				{
					if (_syntaxPairs.Keys.Contains(character))
					{
						syntaxProcessing.Add(character);
					}
					else
					{
						char expectedClosingCharacter = _syntaxPairs[syntaxProcessing.Last()];

						if (character.Equals(expectedClosingCharacter))
						{
							syntaxProcessing.RemoveAt(syntaxProcessing.Count - 1);
						}
						else
						{
							invalid = true;

							// Invalid character.
							switch (character)
							{
								case ')':
									result += 3;

									break;
								case ']':
									result += 57;

									break;
								case '}':
									result += 1197;

									break;
								case '>':
									result += 25137;

									break;
								default:
									throw new ArgumentException($"Invalid character ({character}) encountered.");
							}

							// Stop after first syntax error.
							break;
						}
					}
				}

				if (!invalid)
				{
					_incompleteLines.Add(syntaxLine);
				}
			}

			return result;
		}

		public long PartTwo()
		{
			List<long> results = new();

			foreach (string incompleteLine in _incompleteLines)
			{
				List<char> characters = new();

				foreach (char character in incompleteLine)
				{
					if (_syntaxPairs.Keys.Contains(character))
					{
						characters.Add(character);
					}
					else
					{
						char expectedClosingCharacter = _syntaxPairs[characters.Last()];

						if (character.Equals(expectedClosingCharacter))
						{
							characters.RemoveAt(characters.Count - 1);
						}
					}
				}

				// Processing remaining characters.
				long lineResult = 0;

				for (int i = characters.Count - 1; i >= 0; i--)
				{
					// First multiply by 5.
					lineResult = lineResult * 5;

					char endCharacter = _syntaxPairs[characters[i]];

					switch (endCharacter)
					{
						case ')':
							lineResult += 1;

							break;
						case ']':
							lineResult += 2;

							break;
						case '}':
							lineResult += 3;

							break;
						case '>':
							lineResult += 4;

							break;
						default:
							throw new ArgumentException($"Invalid character ({endCharacter}) encountered.");
					}
				}

				results.Add(lineResult);
			}

			return results.OrderBy(x => x).Skip(results.Count / 2).First();
		}
	}
}
