using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulator
{
	class Building
	{
		/* FIELDS */
		private bool[][] _elevatorConfiguration;
		private int[] _elevatorCapacities;
		private int[] _waitingPeople;
		private int _elevatorWaitingTime;
		private int _elevatorMovementTime;

		/* PROPERTIES */
		public int[] WaitingPeople
		{
			get
			{
				return _waitingPeople;
			}
			set
			{
				_waitingPeople = value;
			}
		}

		/* INITIALIZATION */
		public Building(bool[][] elevatorConfiguration, int[] elevatorCapacities, int[] floorPopulations, int elevatorWaitingTime, int elevatorMovementTime)
		{
			this._waitingPeople = new int[floorPopulations.Length];
			Array.Copy(floorPopulations, _waitingPeople, floorPopulations.Length);

			this._elevatorCapacities = new int[elevatorCapacities.Length];
			Array.Copy(elevatorCapacities, _elevatorCapacities, elevatorCapacities.Length);

			this._elevatorConfiguration = new bool[elevatorConfiguration.Length][];
			Array.Copy(elevatorConfiguration, _elevatorConfiguration, elevatorConfiguration.Length);

			this._elevatorWaitingTime = elevatorWaitingTime;
			this._elevatorMovementTime = elevatorMovementTime;
		}

		/* SIMULATION */
		public int Simulate()
		{
			// Making local copies of everything we need
			int[] waitingPeople = new int[this._waitingPeople.Length];
			Array.Copy(this._waitingPeople, waitingPeople, this._waitingPeople.Length);

			Elevator[] elevators = CreateElevators(_elevatorConfiguration, _elevatorCapacities, _elevatorWaitingTime, _elevatorMovementTime);

			// Main simulation loop
			bool simulating = true;
			int seconds = 0;
			while (simulating)
			{
				seconds += 1;
				for (int i = 0; i < elevators.Length; i++)
				{
					elevators[i].SimulateStep();
				}

				if (IsDoneSimulating(waitingPeople, elevators))
				{
					simulating = false;
				}
			}

			return seconds;
		}

		/* MISCELLANEOUS */
		private Elevator[] CreateElevators(bool[][] elevatorConfiguration, int[] elevatorCapacities, int waitingTime, int movementTime)
		{
			Elevator[] elevators = new Elevator[elevatorConfiguration.Length];

			for (int i = 0; i < elevators.Length; i++)
			{
				Elevator newElevator = new Elevator(this, elevatorConfiguration[i], elevatorCapacities[i], waitingTime, movementTime);
				elevators[i] = newElevator;
			}

			return elevators;
		}

		private bool IsDoneSimulating(int[] waitingPeople, Elevator[] elevators)
		{
			// Verify that we have no one waiting
			for (int i = 0; i < waitingPeople.Length; i++)
			{
				if (waitingPeople[i] != 0)
				{
					return false;
				}
			}

			// Verify that every elevator is empty
			for (int i = 0; i < elevators.Length; i++)
			{
				if (elevators[i].TotalPassengers != 0)
				{
					return false;
				}
			}

			return true;
		}
	}
}
