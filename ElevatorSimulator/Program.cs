using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulator
{
	class Program
	{
		static void Main(string[] args)
		{
			// Setup simulation details
			int[] elevatorCapacities = { 0, 0, 0, 0 };
			int[] floorPopulations = { 0, 80, 80, 80, 80 };
			/*
			int trials = 10;

			bool[][] fastestConfiguration = Simulator.FindFastestPossibleConfiguration(elevatorCapacities, floorPopulations, trials, 1000);
			*/

			Console.WriteLine("Beginning...");

			bool[][][] elevatorConfigs = Generator.GenerateElevatorConfigurations(5, 4);
			bool[][] config = elevatorConfigs[elevatorConfigs.Length - 1];

			int time = Simulator.Simulate(config, elevatorCapacities, floorPopulations, 22, 3);

			Console.WriteLine(time);

			// Prevent auto-closing
			Console.WriteLine("Press any key to terminate...");
			Console.ReadKey();
        }
	}
}
