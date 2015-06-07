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
			bool[][][] elevatorConfigs = Generator.GenerateElevatorConfigurations(5, 4);

			// Prevent auto-closure
			Console.WriteLine("Press any key to end...");
			Console.ReadKey();
		}
	}
}
