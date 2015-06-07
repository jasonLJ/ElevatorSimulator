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
		private int[] _waitingPeople;
		private int[] _elevatorCapacities;
		private bool[][] _elevatorConfiguration;

		/* INITIALIZATION */
		public Building(bool[][] elevatorConfiguration, int[] elevatorCapacities, int[] floorPopulations)
		{
			Array.Copy(floorPopulations, _waitingPeople, floorPopulations.Length);
			Array.Copy(elevatorCapacities, _elevatorCapacities, elevatorCapacities.Length);
			Array.Copy(elevatorConfiguration, _elevatorConfiguration, elevatorConfiguration.Length);
		}

		/* SIMULATION */
		public int Simulate()
		{
			// Making local copies of everything we need
			int[] waitingPeople = new int[this._waitingPeople.Length];
			Array.Copy(this._waitingPeople, waitingPeople, this._waitingPeople.Length);

			Elevator[] elevators = CreateElevators(_elevatorConfiguration, _elevatorCapacities);

			// Main simulation loop
			bool simulating = true;
			int seconds = 0;
			while(simulating)
			{
				seconds += 1;
				for(int i = 0; i < elevators.Length; i++)
				{
					elevators[i].SimulateStep();
				}

				if(IsDoneSimulating(waitingPeople, elevators))
				{
					simulating = false;
				}
			}

			return seconds;
        }

		/* MISCELLANEOUS */
		private Elevator[] CreateElevators(bool[][] elevatorConfiguration, int[] elevatorCapacities)
		{
			Elevator[] elevators = new Elevator[elevatorConfiguration.Length];

			for(int i = 0; i < elevators.Length; i++)
			{
				Elevator newElevator = new Elevator(elevatorConfiguration[i], elevatorCapacities[i]);
				elevators[i] = newElevator;
			}

			return elevators;
		}

		private bool IsDoneSimulating(int[] waitingPeople, Elevator[] elevators)
		{
			// Verify that we have no one waiting
			for(int i = 0; i < waitingPeople.Length; i++)
			{
				if(waitingPeople[i] != 0)
				{
					return false;
				}
			}

			// Verify that every elevator is empty
			for(int i = 0; i < elevators.Length; i++)
			{
				int[] passengers = elevators[i].Passengers;

				for(int j = 0; j < passengers.Length; j++)
				{
					if(passengers[j] != 0)
					{
						return false;
					}
				}
			}

			return true;
		}
	}
}
