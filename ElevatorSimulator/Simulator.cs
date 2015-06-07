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
		public bool[][] FindFastestPossibleConfiguration(int[] elevatorCapacities, int[] floorPopulations, int trials)
		{
			bool[][][] allPosibilities = Generator.GenerateElevatorConfigurations(floorPopulations.Length, elevatorCapacities.Length);
			return FindFastestConfiguration(allPosibilities, elevatorCapacities, floorPopulations, trials);
		}

		public bool[][] FindFastestConfiguration(bool[][][] elevatorConfigurations, int[] elevatorCapacities, int[] floorPopulations, int trials)
		{
			double fastestTime = -1; // Overwritten later
			bool[][] fastestConfiguration = null; // Overwritten later

			for (int i = 0; i < elevatorConfigurations.Length; i++)
			{
				bool[][] currentConfiguration = elevatorConfigurations[i];
				double currentAverageTime = SimulateAverage(currentConfiguration, elevatorCapacities, floorPopulations, trials);

				if (currentAverageTime < fastestTime || fastestTime == -1)
				{
					fastestTime = currentAverageTime;
					fastestConfiguration = currentConfiguration;
				}
			}

			return fastestConfiguration;
		}

		public double SimulateAverage(bool[][] elevatorConfiguration, int[] elevatorCapacities, int[] floorPopulations, int trials)
		{
			int total = 0;
			for (int i = 0; i < trials; i++)
			{
				total += Simulate(elevatorConfiguration, elevatorCapacities, floorPopulations);
			}

			return (double)total / (double)trials;
		}

		public int Simulate(bool[][] elevatorConfiguration, int[] elevatorCapacities, int[] floorPopulations)
		{
			Building building = new Building(elevatorConfiguration, elevatorCapacities, floorPopulations);
			return building.Simulate();
		}
	}
}
