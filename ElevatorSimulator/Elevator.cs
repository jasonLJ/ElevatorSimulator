using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulator
{
	class Elevator
	{
		/* FIELDS */
		private Building _building;
		private bool[] _floorConfiguration;

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

		public int[] FloorConfigurationInts
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
		public Elevator(Building building, bool[] floorConfiguration, int capacity)
		{
			this._building = building;
			this._floorConfiguration = floorConfiguration;

			int floorCount = floorConfiguration.Length;
			_passengers = new int[floorCount];

			// Setup simulation fields
			_currentFloor = 0;
			_waitingTimer = 0;
			_movementTimer = 0;

			_wasWaiting = false;
			_wasMoving = false;
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
				if (_currentFloor == 0)
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
			if (_currentFloor == 0)
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
			if (_currentFloor > 0)
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

			// We were somehow on a negative floor
			throw new ArgumentException("Elevator was on negative floor!");
		}

		/* SIMULATION ACTIONS */
		private bool IsWaiting()
		{
			return _waitingTimer != 0;
		}

		private bool IsMoving()
		{
			return _movementTimer != 0;
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
			// TODO: This is what you were working on Jason
		}

		/* MISCELLANEOUS */
	}
}
