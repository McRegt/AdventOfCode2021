// © Traxion Development Services

namespace CodeBase
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using AdventBase.Helpers;
	using AdventBase.Interfaces;

	public class Day23 : IAdventDay
	{
		public class GameState
		{
			public List<char> Hall;
			public List<List<char>> Rooms;
			public int RoomSize;
			public int Depth;
			public long Result;
			public bool EndReached;
		}

		private readonly List<char> _startHallWay = new();

		private readonly List<List<char>> _startRooms = new()
		{
			new List<char>(),
			new List<char>(),
			new List<char>(),
			new List<char>()
		};

		private static char GetColumnCharacter(int index)
		{
			switch (index)
			{
				case 0:
					return 'A';
				case 1:
					return 'B';
				case 2:
					return 'C';
				case 3:
					return 'D';
			}

			throw new ArgumentException($"Invalid index {index} for target column.");
		}

		private static int GetWeight(char type)
		{
			switch (type)
			{
				case 'A':
					return 1;
				case 'B':
					return 10;
				case 'C':
					return 100;
				case 'D':
					return 1000;
			}

			throw new ArgumentException($"Unexpected amphipod type: {type}.");
		}

		private static int GetTargetColumn(char character)
		{
			switch (character)
			{
				case 'A':
					return 2;
				case 'B':
					return 4;
				case 'C':
					return 6;
				case 'D':
					return 8;
			}

			throw new ArgumentException($"Unexpected column character: {character}.");
		}

		private readonly List<int> _roomIndexes = new()
		{
			2,
			4,
			6,
			8
		};

		private readonly List<int> _hallIndexes = new()
		{
			0,
			1,
			3,
			5,
			7,
			9,
			10
		};

		public void LoadInput(string fileName)
		{
			List<string> input = InputLoader<string>.LoadStringList(fileName);

			foreach (string line in input)
			{
				string subLine = line.Replace(" ", "");

				if (subLine.All(character => character.Equals('#')))
				{
					continue;
				}

				if (subLine.Any(character => character.Equals('.')))
				{
					int count = subLine.Count(character => character.Equals('.'));

					while (count != 0)
					{
						_startHallWay.Add('.');
						count--;
					}

					continue;
				}

				string[] order = subLine.Split('#', StringSplitOptions.RemoveEmptyEntries);

				_startRooms[0].Add(char.Parse(order[0]));
				_startRooms[1].Add(char.Parse(order[1]));
				_startRooms[2].Add(char.Parse(order[2]));
				_startRooms[3].Add(char.Parse(order[3]));
			}
		}

		private long _minimumEnergy = long.MaxValue;

		public long PartOne()
		{
			GameState startState = new()
			{
				Hall = _startHallWay,
				Rooms = _startRooms,
				RoomSize = 2,
				Depth = 0,
				Result = -1
			};

			MakeMove(startState);

			return _minimumEnergy;
		}

		private void MakeMove(GameState state)
		{
			// Select move candidates.
			List<int> movePositions = SelectMovePods(state);

			foreach (int moveFrom in movePositions)
			{
				char moveCharacter = SelectCharacterFromState(state, moveFrom);
				int moveTo = GetTargetColumn(moveCharacter);

				if (IsDirectPathFree(state, moveFrom, moveTo))
				{
					// Update the game state for the next moves.
					GameState newState = UpdateGameState(state,
														 moveCharacter,
														 moveFrom,
														 moveTo);

					// A direct move or to/from hallway move has been made.
					// Now check if the end state has been reached.
					if (IsEndStateReached(newState))
					{
						if (_minimumEnergy > newState.Result)
						{
							_minimumEnergy = newState.Result;
						}

						return;
					}

					MakeMove(newState);
				}
				else
				{
					// Otherwise, all possible moves to free (and reachable) hallway columns must be recursively visited.
					List<int> freeAndReachableHallwayIndexes = GetFreeAndReachableHallwayIndexes(state, moveFrom);

					// Based on the previous moves, there can be a number of situations.
					// 1. No moves are available.
					if (freeAndReachableHallwayIndexes.Count == 0)
					{
						// Continue with the next possible move.
						// No game state update is required as no move was made.
						continue;
					}

					foreach (int reachableIndex in freeAndReachableHallwayIndexes)
					{
						// Update the game state for the next moves.
						GameState newState = UpdateGameState(state,
															 moveCharacter,
															 moveFrom,
															 reachableIndex);

						MakeMove(newState);

						if (IsEndStateReached(newState))
						{
							if (_minimumEnergy > newState.Result)
							{
								_minimumEnergy = newState.Result;
							}

							return;
						}
					}
				}
			}
		}

		private bool IsEndStateReached(GameState state)
		{
			var allRoomsComplete = true;

			// For the end state to be reached:
			for (var roomIndex = 0; roomIndex < state.Rooms.Count; roomIndex++)
			{
				List<char> roomPods = state.Rooms[roomIndex];

				// All rooms must contain the maximum number of pods.
				if (roomPods.Count != state.RoomSize)
				{
					allRoomsComplete = false;

					break;
				}

				// And all pods must match the desired character.
				char desiredCharacterForRoom = GetColumnCharacter(roomIndex);

				if (!roomPods.All(pod => pod.Equals(desiredCharacterForRoom)))
				{
					allRoomsComplete = false;

					break;
				}
			}

			return allRoomsComplete;
		}

		private List<int> GetFreeAndReachableHallwayIndexes(GameState state, int from)
		{
			List<int> freeAndReachableHallWayIndexes = new();

			if (_hallIndexes.Contains(from))
			{
				// No move necessary, you cannot move across the hall.
				return freeAndReachableHallWayIndexes;
			}

			int hallWayMin = _hallIndexes.Min();
			int hallWayMax = _hallIndexes.Max();

			// Pods cannot go over each other, as such there are two comparisons needed.
			// One to look left of the A below, and one to look right of the A.
			// . B . . A . . B .
			int lookLeftIndex = from - 1;

			while (lookLeftIndex >= hallWayMin)
			{
				if (_roomIndexes.Contains(lookLeftIndex))
				{
					// If the target is a room column, continue with the next one.
					lookLeftIndex--;
				}
				else if (state.Hall[lookLeftIndex].Equals('.'))
				{
					// If the target is an empty spot, add it as possibility.
					freeAndReachableHallWayIndexes.Add(lookLeftIndex);
					lookLeftIndex--;
				}
				else
				{
					// Otherwise break the loop for the left.
					break;
				}
			}

			int lookRightIndex = from + 1;

			while (lookRightIndex <= hallWayMax)
			{
				if (_roomIndexes.Contains(lookRightIndex))
				{
					// If the target is a room column, continue with the next one.
					lookRightIndex++;
				}
				else if (state.Hall[lookRightIndex].Equals('.'))
				{
					// If the target is an empty spot, add it as possibility.
					freeAndReachableHallWayIndexes.Add(lookRightIndex);
					lookRightIndex++;
				}
				else
				{
					// Otherwise break the loop for the right.
					break;
				}
			}

			return freeAndReachableHallWayIndexes.OrderBy(x => x).ToList();
		}

		private GameState UpdateGameState(GameState oldState,
										  char character,
										  int from,
										  int to)
		{
			var state = new GameState
			{
				Depth = oldState.Depth,
				Hall = new(oldState.Hall),
				Rooms = new List<List<char>>(),
				RoomSize = oldState.RoomSize,
				Result = oldState.Result
			};

			// Deep cloning hell.
			oldState.Rooms.ForEach(x => state.Rooms.Add(new(x)));

			// Calculate and increase moved cost.
			// Important: Do before updating the lists below.
			long moveCost = GetMoveCost(state,
										character,
										from,
										to);

			if (state.Result == -1)
			{
				// Initial move;
				state.Result = moveCost;
			}
			else
			{
				state.Result += moveCost;
			}

			// The game state can be updated in a number of ways:
			// 1. A direct move between source and target column.
			if (_roomIndexes.Contains(from)
				&& _roomIndexes.Contains(to))
			{
				// Remove it from the source column.
				state.Rooms[GetRoomIndex(from)].RemoveAt(0);

				// Insert at first position in the target column.
				state.Rooms[GetRoomIndex(to)].Insert(0, character);
			}

			// 2. A move between source and hallway column.
			else if (_roomIndexes.Contains(from)
					 && _hallIndexes.Contains(to))
			{
				// Remove it from the source column.
				state.Rooms[GetRoomIndex(from)].RemoveAt(0);

				// Set at position in the hallway.
				state.Hall[to] = character;
			}

			// 3. A move from hallway column into a target column.
			else if (_hallIndexes.Contains(from)
					 && _roomIndexes.Contains(to))
			{
				// Remove it from the hallway.
				state.Hall[from] = '.';

				// Insert at first position in the target column.
				state.Rooms[GetRoomIndex(to)].Insert(0, character);
			}
			else
			{
				throw new ArgumentException($"Invalid move for {character} from column {from} to column {to}.");
			}

			// Increase depth for debugging
			state.Depth++;

			// Sanity check
			if (state.Hall.Count(x => !x.Equals('.')) + state.Rooms.Sum(x => x.Count) != 8)
			{
				throw new ArgumentException("Game state entered a state with lost pods.");
			}

			return state;
		}

		private long GetMoveCost(GameState state,
								 char character,
								 int from,
								 int to)
		{
			long moves = 0;

			// A move consist of three sub-steps.

			// 1. Moving out of a room (either 1 or 2 moves).
			if (_roomIndexes.Contains(from))
			{
				// The number of moves necessary to get out of a room, is the total of:
				// A. The moves to the front of the room.
				moves += Math.Abs(state.Rooms[GetRoomIndex(from)].Count - state.RoomSize);

				// B. Always one to move into the actual hallway.
				moves++;
			}

			// 2. Moving between columns (at least 1 move to either side).
			moves += Math.Abs(to - from);

			// 3. Moving into a room (either 1 or 2 moves).
			if (_roomIndexes.Contains(to))
			{
				// The number of moves necessary to get into a room, is the total of:
				// A. The moves needed to get as far back as possible.
				moves += state.RoomSize - state.Rooms[GetRoomIndex(to)].Count;
			}

			long moveCost = CalculateMoveCost(moves, character);

			return moveCost;
		}

		private long CalculateMoveCost(long moves, char character)
		{
			long moveCost = moves * GetWeight(character);

			if (moveCost == 0)
			{
				throw new DivideByZeroException($"An invalid move has occured when moving {character}.");
			}

			return moveCost;
		}

		private int GetRoomIndex(int column)
		{
			return column / 2 - 1;
		}

		private bool IsDirectPathFree(GameState state, int from, int to)
		{
			int roomIndex = GetRoomIndex(to);

			// The direct path is not free if the target column (room) is full.
			if (state.Rooms[roomIndex].Count == state.RoomSize)
			{
				return false;
			}

			int max = from > to
						  ? from
						  : to;

			int min = from < to
						  ? from
						  : to;

			// Otherwise the target column is free, only other pods in the hallway can obstruct.
			for (int hallIndex = min + 1; hallIndex < max; hallIndex++)
			{
				if (!state.Hall[hallIndex].Equals('.'))
				{
					return false;
				}
			}

			return true;
		}

		private char SelectCharacterFromState(GameState state, int movePosition)
		{
			if (_hallIndexes.Contains(movePosition))
			{
				// The hall position can be returned immediately.
				return state.Hall[movePosition];
			}

			if (!_roomIndexes.Contains(movePosition))
			{
				throw new ArgumentException($"Unexpected movePosition {movePosition} cannot be selected from the game state.");
			}

			int roomPosition = GetRoomIndex(movePosition);

			return state.Rooms[roomPosition].First();
		}

		private List<int> SelectMovePods(GameState state)
		{
			List<int> indexesToMove = new();

			// First select the pods from the rooms, as these can potentially be moved straight into their targets rooms.
			for (var roomIndex = 0; roomIndex < state.Rooms.Count; roomIndex++)
			{
				switch (roomIndex)
				{
					case 0:
						// Check for any character in a room, but only select the first.
						// This ensure that a higher level is moved out, to make way for a lower incorrect value.
						if (state.Rooms[roomIndex].Any(x => !x.Equals('A')))
						{
							indexesToMove.Add(2);
						}

						break;
					case 1:
						if (state.Rooms[roomIndex].Any(x => !x.Equals('B')))
						{
							indexesToMove.Add(4);
						}

						break;
					case 2:
						if (state.Rooms[roomIndex].Any(x => !x.Equals('C')))
						{
							indexesToMove.Add(6);
						}

						break;
					case 3:
						if (state.Rooms[roomIndex].Any(x => !x.Equals('D')))
						{
							indexesToMove.Add(8);
						}

						break;
					default:
						throw new ArgumentException($"Unexpected room index {roomIndex}.");
				}
			}

			// Select the indexes from the hallway.
			for (var hallIndex = 0; hallIndex < state.Hall.Count; hallIndex++)
			{
				// If a hall index coincides with a room, skip it, it is reserved for room moves.
				if (_roomIndexes.Contains(hallIndex))
				{
					continue;
				}

				// If the spot is free, skip it.
				if (state.Hall[hallIndex].Equals('.'))
				{
					continue;
				}

				// Otherwise there is an pod here, add the index for upcoming move.
				indexesToMove.Add(hallIndex);
			}

			return indexesToMove;
		}

		public long PartTwo()
		{
			return 0;
		}
	}
}
