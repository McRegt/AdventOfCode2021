// © Traxion Development Services

namespace CodeBase
{
	using System.Collections.Generic;
	using System.Linq;
	using AdventBase.Helpers;
	using AdventBase.Interfaces;

	public class Day21 : IAdventDay
	{
		private Dictionary<int, (int Score, int Start)> _players = new();
		private List<int> _possibleRolls = new();

		public void LoadInput(string fileName)
		{
			List<string> input = InputLoader<string>.LoadStringList(fileName);

			foreach (string line in input)
			{
				int playerId = int.Parse(line.Substring(7, 1));
				int start = int.Parse(line.Last().ToString());

				_players.Add(playerId, (0, start));
			}
		}

		public long PartOne()
		{
			var diceRolls = 0;
			var endGame = false;
			var originalPlayers = new Dictionary<int, (int Score, int Start)>(_players);

			while (!endGame)
			{
				foreach (int playerId in _players.Keys)
				{
					(int Score, int Start) value = _players[playerId];

					int newPosition = RollDicePartOne(diceRolls,
													  value.Start,
													  3,
													  100,
													  10);

					diceRolls += 3;

					value.Score += newPosition;
					value.Start = newPosition;

					_players[playerId] = value;

					if (value.Score >= 1000)
					{
						endGame = true;

						break;
					}
				}
			}

			(int Score, int Start) losingPair = _players.First(x => x.Value.Score < 1000).Value;

			_players = originalPlayers;

			return diceRolls * losingPair.Score;
		}

		private int RollDicePartOne(int diceRolls,
									int start,
									int rolls,
									int maxDiceValue,
									int maxGameBoard)
		{
			var totalDiceValue = 0;

			for (var roll = 0; roll < rolls; roll++)
			{
				diceRolls++;
				totalDiceValue += diceRolls % maxDiceValue;
			}

			int returnValue = (start + totalDiceValue) % maxGameBoard;

			if (returnValue == 0)
			{
				returnValue = maxGameBoard;
			}

			return returnValue;
		}

		public long PartTwo()
		{
			// Sort by highest as that makes debugging easier. :-)
			_possibleRolls = PossibleRolls().OrderByDescending(x => x).ToList();

			long resultOne = 0;
			long resultTwo = 0;

			// Trigger the recursive rolling by rolling all combinations at least once.
			foreach (int roll in _possibleRolls)
			{
				(long oneWins, long twoWins) result = PlayGame(0,
															   _players[1].Start,
															   0,
															   _players[2].Start,
															   true,
															   roll);

				resultOne += result.oneWins;
				resultTwo += result.twoWins;
			}

			long answer = resultOne > resultTwo
							  ? resultOne
							  : resultTwo;

			return answer;
		}

		private readonly Dictionary<(int playerOneScore, int playerOnePosition, int playerTwoScore, int playerTwoPosition, bool playerOne, int roll),
				(long oneWins, long twoWins)>
			_cache = new();

		private (long oneWins, long twoWins) PlayGame(int playerOneScore,
													  int playerOnePosition,
													  int playerTwoScore,
													  int playerTwoPosition,
													  bool playerOne,
													  int roll)
		{
			(int playerOneScore, int playerOnePosition, int playerTwoScore, int playerTwoPosition, bool playerOne, int roll) state =
				(playerOneScore, playerOnePosition, playerTwoScore, playerTwoPosition, playerOne, roll);

			if (_cache.ContainsKey(state))
			{
				return _cache[state];
			}

			int score;
			int position;

			if (playerOne)
			{
				score = playerOneScore;
				position = playerOnePosition;
			}
			else
			{
				score = playerTwoScore;
				position = playerTwoPosition;
			}

			position += roll;

			if (position % 10 == 0)
			{
				position = 10;
			}
			else
			{
				position %= 10;
			}

			score += position;

			// Score the winner.
			if (score >= 21)
			{
				(long, long) winner = playerOne
										  ? (1, 0)
										  : (0, 1);

				// Add the initial win for a state.
				_cache.Add(state, winner);

				return winner;
			}

			// Continue with recursive rolls for all possible roles.
			(long, long) totalWinner = (0, 0);

			foreach (int nextRoll in _possibleRolls)
			{
				// Role for the next player.
				(long, long) subwinner = playerOne
											 ? PlayGame(score,
														position,
														playerTwoScore,
														playerTwoPosition,
														false,
														nextRoll)
											 : PlayGame(playerOneScore,
														playerOnePosition,
														score,
														position,
														true,
														nextRoll);

				totalWinner.Item1 += subwinner.Item1;
				totalWinner.Item2 += subwinner.Item2;
			}

			// Add the win count for all recursive states.
			_cache.Add(state, totalWinner);

			return totalWinner;
		}

		private List<int> PossibleRolls()
		{
			List<int> results = new();

			List<int> diceValues = new()
			{
				1,
				2,
				3
			};

			foreach (int dice1 in diceValues)
			{
				foreach (int dice2 in diceValues)
				{
					foreach (int dice3 in diceValues)
					{
						int result = dice1 + dice2 + dice3;
						results.Add(result);
					}
				}
			}

			return results;
		}
	}
}
