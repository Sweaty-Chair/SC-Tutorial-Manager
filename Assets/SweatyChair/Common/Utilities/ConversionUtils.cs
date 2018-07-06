using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SweatyChair
{

	public static class ConversionUtils
	{

		#region Set Get Int[]

		/// <summary>
		/// Converts a Int Array into a string
		/// </summary>
		public static string SetIntArray(params int[] intArray)
		{
			if (intArray == null || intArray.Length == 0) {
				return "";
			}

			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			for (int i = 0, imax = intArray.Length - 1; i < imax; i++)
				sb.Append(intArray[i]).Append("|");
			sb.Append(intArray[intArray.Length - 1]);

			return sb.ToString();
		}

		/// <summary>
		/// Returns a Int Array from a string
		/// </summary>
		public static int[] GetIntArray(string array)
		{
			if (array == null || array.Length == 0) {
				return new int[0];
			}
			string[] stringArray = array.Split('|');
			int[] intArray = new int[stringArray.Length];
			for (int i = 0, imax = stringArray.Length; i < imax; i++)
				intArray[i] = Convert.ToInt32(stringArray[i]);
			return intArray;
		}

		/// <summary>
		/// Returns a Int Array from a string
		/// Note: Uses default values to initialize if string is not valid
		/// </summary>
		public static int[] GetIntArray(string array, int defaultValue, int defaultSize)
		{
			int[] returnArray = GetIntArray(array);
			if (returnArray != new int[0])
				return returnArray;

			int[] intArray = new int[defaultSize];
			for (int i = 0; i < defaultSize; i++)
				intArray[i] = defaultValue;
			return intArray;
		}

		#endregion

	}
}
