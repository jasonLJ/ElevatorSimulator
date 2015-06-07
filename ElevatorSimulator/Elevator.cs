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
		private bool[] floorConfiguration;
		private int[] passengers;

		/* PROPERTIES */
		public int[] Passengers
		{
			get
			{
				return passengers;
			}
			set
			{
				passengers = value;
			}
		}

		/* INITIALIZATION */
		public Elevator(bool[] floorConfiguration, int capacity)
		{
			int floorCount = floorConfiguration.Length;
			this.floorConfiguration = floorConfiguration;

			passengers = new int[floorCount];
		}

		/* SIMULATION */
		public void SimulateStep()
		{

		}

		/* SIMULATION ACTIONS */

		/* MISCELLANEOUS */
	}
}
