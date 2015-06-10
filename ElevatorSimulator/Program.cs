/*
	This is a C# re-write of a program I originally wrote in Java
	This version isn't much better, as both programs were written in
	a marathon session of caffeine-fueled, late-night programming.
	
	Look on in disgust, as it is written poorly.
*/

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
			int[] elevatorCapacities = { 10, 10, 10, 10 };
			int[] floorPopulations = { 80, 80, 80, 80, 80 };

			int waitTime = 22;
			int moveTime = 3;
	
			int trials = 10;
			int printInterval = 1000;

			bool[][] fastestConfiguration = Simulator.FindFastestPossibleConfiguration(elevatorCapacities, floorPopulations, waitTime, moveTime, trials, printInterval);



			// Prevent auto-closing
			Console.WriteLine("Press any key to terminate...");
			Console.ReadKey();
        }

		/* STATIC METHODS */
		public static string ArrayToString(int[] array)
		{
			return String.Join(",", array.Select(p => p.ToString()).ToArray());
		}
	}
}
