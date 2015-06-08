using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulator
{
	class Elevator
	{
		/* CONSTANTS */
		private const int GroundFloor = -1;

		/* FIELDS */
		private Building _building;
		private bool[] _floorConfiguration;
		private int _capacity;

		private int _waitingTime;
		private int _movementTime;

		/* SIMULATION FIELDS */
		private int _currentFloor;
		private int[] _passengers;

		private int _waitingTimer;
		private int _movementTimer;

		private bool _wasWaiting;
		private bool _wasMoving;

		/* PROPERTIES */
		public int[] Passengers
		{
			get
			{
				return _passengers;
			}
			set
			{
				_passengers = value;
			}
		}

		public int[] FloorConfigurationAsInts
		{
			get
			{
				int[] floors = new int[_floorConfiguration.Length];
				int floorIndex = 0;
				for (int i = 0; i < _floorConfiguration.Length; i++)
				{
					if (_floorConfiguration[i])
					{
						floors[floorIndex] = i;
						floorIndex++;
					}
				}

				// Shorten array
				int[] output = new int[floorIndex];
				Array.Copy(floors, output, output.Length);

				return output;
			}
		}

		public int TotalPassengers
		{
			get
			{
				int total = 0;
				for (int i = 0; i < _passengers.Length; i++)
				{
					total += _passengers[i];
				}

				return total;
			}
		}

		/* INITIALIZATION */
		public Elevator(Building building, bool[] floorConfiguration, int capacity, int waitingTime, int movementTime)
		{
			this._building = building;
			this._floorConfiguration = floorConfiguration;
			this._capacity = capacity;
			this._waitingTime = waitingTime;
			this._movementTime = movementTime;

			int floorCount = floorConfiguration.Length;
			this._passengers = new int[floorCount];

			// Setup simulation fields
			this._currentFloor = GroundFloor;
			this._waitingTimer = 0;
			this._movementTimer = 0;

			this._wasWaiting = false;
			this._wasMoving = false;
		}

		/* SIMULATION */
		public void SimulateStep()
		{
			/* Waiting */
			if (IsWaiting())
			{
				SimulateWait();
				return;
			}
			else if (_wasWaiting)
			{
				_wasWaiting = false;

				// Ground Floor
				if (_currentFloor == GroundFloor)
				{
					LoadPassengers();
				}
				else
				{ // Populated Floor
					UnloadPassengers();
				}
			}

			/* Moving */
			if (IsMoving())
			{
				SimulateMove();
				return;
			}
			else if (_wasMoving)
			{
				_wasMoving = false;
				ChangeFloor();
			}

			/* On Ground Floor */
			if (_currentFloor == GroundFloor)
			{
				if (TotalPassengers > 0)
				{
					// Begin moving
					_movementTimer = _movementTime - 1; // -1 - consuming unit of time
					return;
				}
				else
				{
					// Begin waiting
					_waitingTimer = _waitingTime - 1; // -1 - consuming unit of time
					return;
				}
			}

			/* On Populated Floor */
			if (_currentFloor > GroundFloor)
			{
				if (_passengers[_currentFloor] > 0)
				{
					// Begin waiting
					_waitingTimer = _waitingTime - 1; // -1 - consuming unit of time
					return;
				}
				else
				{
					// Begin moving
					_movementTimer = _movementTime - 1; // -1 - consuming unit of time
					return;
				}
			}
		}

		/* SIMULATION ACTIONS */
		private bool IsWaiting()
		{
			return _waitingTimer > 0;
		}

		private bool IsMoving()
		{
			return _movementTimer > 0;
		}

		private void SimulateWait()
		{
			_wasWaiting = true;
			_waitingTimer -= 1;
		}

		private void SimulateMove()
		{
			_wasMoving = true;
			_movementTimer -= 1;
		}

		private void LoadPassengers()
		{
			if (IsDoneLoading())
			{
				return;
			}

			// Pull one person from each floor
			for (int i = 0; i < FloorConfigurationAsInts.Length; i++)
			{
				int currentFloor = FloorConfigurationAsInts[i];

				if (_building.WaitingPeople[currentFloor] > 0)
				{
					// 'Move' one person from building at currentFloor to elevator
					_building.WaitingPeople[currentFloor] -= 1;
					_passengers[currentFloor] += 1;
				}
			}

			// Randomly pull from listed floors until full
			int toDistribute = _capacity - TotalPassengers;
			while (toDistribute > 0)
			{
				// Make sure we aren't already done
				if (IsDoneLoading())
				{
					return;
				}

				// Determine which floor to pull from
				int floorCount = FloorConfigurationAsInts.Length;
				Random random = new Random();
				int floorRandom = random.Next(0, floorCount);
				int floor = FloorConfigurationAsInts[floorRandom];

				if (_building.WaitingPeople[floor] == 0)
				{
					// No one here to pull on to elevator
					continue;
				}
				else
				{
					// Move someone into the elevator
					_building.WaitingPeople[floor] -= 1;
					_passengers[floor] += 1;
					toDistribute -= 1;
				}
			}
		}

		private void UnloadPassengers()
		{
			_passengers[_currentFloor] = 0;
		}

		private void ChangeFloor()
		{
			// Move up if we have passengers left to drop off
			if(TotalPassengers > 0)
			{
				_currentFloor += 1;
			}
			else // Move down if we're empty
			{
				_currentFloor -= 1;
			}
		}

		/* MISCELLANEOUS */
		private bool IsDoneLoading()
		{
			for(int i = 0; i < FloorConfigurationAsInts.Length; i++)
			{
				if (_building.WaitingPeople[FloorConfigurationAsInts[i]] != 0)
				{
					return false;
				}
			}

			return true;
		}
	}
}
