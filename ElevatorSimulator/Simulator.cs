using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulator
{
	class Simulator
	{
		/* SIMULATION */
		public static bool[][] FindFastestPossibleConfiguration(int[] elevatorCapacities, int[] floorPopulations, int elevatorWaitingTime, int elevatorMovementTime, int trials, int printoutInterval)
		{
			bool[][][] allPosibilities = Generator.GenerateElevatorConfigurations(floorPopulations.Length, elevatorCapacities.Length);
			return FindFastestConfiguration(allPosibilities, elevatorCapacities, floorPopulations, elevatorWaitingTime, elevatorMovementTime, trials, printoutInterval);
		}

		public static bool[][] FindFastestConfiguration(bool[][][] elevatorConfigurations, int[] elevatorCapacities, int[] floorPopulations, int elevatorWaitingTime, int elevatorMovementTime, int trials, int printoutInterval)
		{
			double fastestTime = -1; // Overwritten later
			bool[][] fastestConfiguration = null; // Overwritten later

			for (int i = 0; i < elevatorConfigurations.Length; i++)
			{
				// Figure out if we need to make a printout
				if(printoutInterval > 0)
				{
					if(i % printoutInterval == 0)
					{
						float percent = ((float)i / (float)elevatorConfigurations.Length) * 100f;
						Console.WriteLine("{0} of {1} simulations ran. - {2}%", i, elevatorConfigurations.Length, percent.ToString("#0.00"));
					}
				}

				bool[][] currentConfiguration = elevatorConfigurations[i];

				// Make sure this is a valid configuration
				if (!IsValidConfiguration(currentConfiguration))
				{
					// Skip this simulation
					continue;
				}

				double currentAverageTime = SimulateAverage(currentConfiguration, elevatorCapacities, floorPopulations, elevatorWaitingTime, elevatorMovementTime, trials);

				if (currentAverageTime < fastestTime || fastestTime == -1)
				{
					fastestTime = currentAverageTime;
					fastestConfiguration = currentConfiguration;
				}
			}

			return fastestConfiguration;
		}

		public static double SimulateAverage(bool[][] elevatorConfiguration, int[] elevatorCapacities, int[] floorPopulations, int elevatorWaitingTime, int elevatorMovementTime, int trials)
		{
			int total = 0;
			for (int i = 0; i < trials; i++)
			{
				total += Simulate(elevatorConfiguration, elevatorCapacities, floorPopulations, elevatorWaitingTime, elevatorMovementTime);
			}

			return (double)total / (double)trials;
		}

		public static int Simulate(bool[][] elevatorConfiguration, int[] elevatorCapacities, int[] floorPopulations, int elevatorWaitingTime, int elevatorMovementTime)
		{
			Building building = new Building(elevatorConfiguration, elevatorCapacities, floorPopulations, elevatorWaitingTime, elevatorMovementTime);
			return building.Simulate();
		}

		/* MISCELLANEOUS */
		private static bool IsValidConfiguration(bool[][] elevatorConfiguration)
		{
			bool[] floorHasElevator = new bool[elevatorConfiguration[0].Length];

			for(int elevator = 0; elevator < elevatorConfiguration.Length; elevator++)
			{
				for (int floor = 0; floor < elevatorConfiguration[elevator].Length; floor++)
				{
					if(elevatorConfiguration[elevator][floor])
					{
						floorHasElevator[floor] = true;
					}
				}
			}

			// Make sure each floor has an elevator going to it
			for(int i = 0; i < floorHasElevator.Length; i++)
			{
				if(!floorHasElevator[i])
				{
					return false;
				}
			}

			return true;
		}
	}
}
