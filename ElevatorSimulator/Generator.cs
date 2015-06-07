using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulator
{
	public class Generator
	{
		/* GENERATION */

		// Generates and returns every possible floor configuration
		public static bool[][] GenerateFloorConfigurations(int floorCount)
		{
			// Mathematical determination for the number of possible configurations
			int configurationCount = (int)Math.Pow(2, floorCount);

			bool[][] configurations = new bool[configurationCount][];

			// Main generation loop
			for (int i = 0; i < configurationCount; i++)
			{
				bool[] currentGeneration = new bool[floorCount];
				string binary = Convert.ToString(i, 2);

				// Force binary.Length == floorCount
				int sizeDifference = floorCount - binary.Length;
				binary = AddPrecedingChars(binary, '0', sizeDifference);

				// Convert binary string to Boolean arary
				currentGeneration = BinaryToBooleanArray(binary);
				configurations[i] = currentGeneration;
			}

			return configurations;
		}

		// Generates and returns every possible elevator configuration
		public static bool[][][] GenerateElevatorConfigurations(int floorCount, int elevatorCount)
		{
			bool[][] floorConfigurations = GenerateFloorConfigurations(floorCount);

			int floorConfigurationCount = floorConfigurations.Length;
			int elevatorConfigurationCount = (int)(Math.Pow(floorConfigurationCount, elevatorCount)); // x floor configs across y elevators = x^y elevator configs

			bool[][][] elevatorConfigurations = new bool[elevatorConfigurationCount][][];

			// Index = # of Elevator in Configuration - Value[Index] = Index in floorConfigurations of that elevator's configuration
			int[] initialConfiguration = CreateBlankArray(0, elevatorCount);
			elevatorConfigurations[0] = IndexesToArrayValues(initialConfiguration, floorConfigurations);

			// Main generation loop
			int[] lastConfiguration = initialConfiguration;
			for (int i = 1; i < elevatorConfigurationCount; i++) // i = 1, as we already added the first option
			{
				int[] currentConfiguration = new int[lastConfiguration.Length];
				Array.Copy(lastConfiguration, currentConfiguration, currentConfiguration.Length);

				// Search backwards for the first value we can increment
				int lastFloorIndex = floorConfigurationCount - 1;
				int incrementedIndex = -1; // Overwritten
				for (int j = currentConfiguration.Length - 1; j >= 0; j--)
				{
					if (currentConfiguration[j] < lastFloorIndex)
					{
						currentConfiguration[j]++;
						incrementedIndex = j;
						break; // Done searching
					}
				}

				// Set every value in front of the incremented value to 0
				for (int j = incrementedIndex + 1; j < currentConfiguration.Length; j++)
				{
					currentConfiguration[j] = 0;
				}

				elevatorConfigurations[i] = IndexesToArrayValues(currentConfiguration, floorConfigurations);
				lastConfiguration = currentConfiguration; // Preparing for next iteration
			}

			return elevatorConfigurations;
		}

		/* MISCELLANEOUS */

		// Adds multiple ('count' many) char 'precedings' to string original, and returns result
		private static string AddPrecedingChars(string original, char preceding, int count)
		{
			string output = original;

			for (int i = 0; i < count; i++)
			{
				output = preceding + output;
			}

			return output;
		}

		// Converts a binary number to a boolean array
		private static bool[] BinaryToBooleanArray(string binary)
		{
			bool[] output = new bool[binary.Length];

			char[] binaryChars = binary.ToCharArray();
			for (int j = 0; j < binaryChars.Length; j++)
			{
				output[j] = BinaryCharToBoolean(binaryChars[j]);
			}

			return output;
		}

		// Converts a binary char, either 0 or 1, to its binary counterpart
		private static bool BinaryCharToBoolean(char binaryChar)
		{
			switch (binaryChar)
			{
				case '0':
					return false;
				case '1':
					return true;
			}

			// Something other than a 0 or 1 was passed
			throw new ArgumentOutOfRangeException("binaryChar", "Only values of '0' and '1' are recognized as binary.");
		}

		// Creates an array containing only one unique value at all indexes
		private static int[] CreateBlankArray(int value, int length)
		{
			int[] output = new int[length];

			// Fill array with indicated value
			for (int i = 0; i < output.Length; i++)
			{
				output[i] = value;
			}

			return output;
		}

		// Creates an array of Boolean arrays whose values correspond to the values at the indexes in the int array 'indexes'
		private static bool[][] IndexesToArrayValues(int[] indexes, bool[][] values)
		{
			bool[][] output = new bool[indexes.Length][];

			for (int i = 0; i < indexes.Length; i++)
			{
				// Using the index listed in indexes to access the value in values
				output[i] = values[indexes[i]];
			}

			return output;
		}
	}
}
