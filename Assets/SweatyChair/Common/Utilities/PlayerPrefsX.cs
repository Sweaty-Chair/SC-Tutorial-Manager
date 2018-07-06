using UnityEngine;
using System;
using System.Collections.Generic;

namespace SweatyChair
{
	public static class PlayerPrefsX
	{
		#region Vector 3

		/// <summary>
		/// Stores a Vector3 value into a Key
		/// </summary>
		public static bool SetVector3(string key, Vector3 vector)
		{
			return SetFloatArray(key, new float[3] { vector.x, vector.y, vector.z });
		}

		/// <summary>
		/// Finds a Vector3 value from a Key
		/// </summary>
		public static Vector3 GetVector3(string key)
		{
			float[] floatArray = GetFloatArray(key);
			if (floatArray.Length < 3)
				return Vector3.zero;
			return new Vector3(floatArray[0], floatArray[1], floatArray[2]);
		}

		#endregion

		#region Bool Array

		/// <summary>
		/// Stores a Bool Array or Multiple Parameters into a Key
		/// </summary>
		public static bool SetBoolArray(string key, params bool[] boolArray)
		{
			if (boolArray == null || boolArray.Length == 0) {
				PlayerPrefs.DeleteKey(key);
				return false;
			}
		
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			for (int i = 0, imax = boolArray.Length - 1; i < imax; i++)
				sb.Append(boolArray[i]).Append("|");
			sb.Append(boolArray[boolArray.Length - 1]);
		
			try {
				PlayerPrefs.SetString(key, sb.ToString());
			} catch (Exception e) {
				Debug.Log(e);
				return false;
			}
			return true;
		}

		/// <summary>
		/// Returns a Bool Array from a Key
		/// </summary>
		public static bool[] GetBoolArray(string key)
		{
			if (PlayerPrefs.HasKey(key)) {
				string[] stringArray = PlayerPrefs.GetString(key).Split('|');
				bool[] boolArray = new bool[stringArray.Length];
				for (int i = 0, imax = stringArray.Length; i < imax; i++)
					boolArray[i] = Convert.ToBoolean(stringArray[i]);
				return boolArray;
			}
			return new bool[0];
		}

		/// <summary>
		/// Returns a Bool Array from a Key
		/// Note: Uses default values to initialize if no key was found
		/// </summary>
		public static bool[] GetBoolArray(string key, bool defaultValue, int defaultSize)
		{
			if (PlayerPrefs.HasKey(key))
				return GetBoolArray(key);
			bool[] boolArray = new bool[defaultSize];
			for (int i = 0; i < defaultSize; i++)
				boolArray[i] = defaultValue;
			return boolArray;
		}

		#endregion

		#region Int Array

		/// <summary>
		/// Stores a Int Array or Multiple Parameters into a Key
		/// </summary>
		public static bool SetIntArray(string key, params int[] intArray)
		{
			if (intArray == null || intArray.Length == 0) {
				PlayerPrefs.DeleteKey(key);
				return false;
			}
		
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			for (int i = 0, imax = intArray.Length - 1; i < imax; i++)
				sb.Append(intArray[i]).Append("|");
			sb.Append(intArray[intArray.Length - 1]);
		
			try {
				PlayerPrefs.SetString(key, sb.ToString());
			} catch (Exception e) {
				Debug.Log(e);
				return false;
			}
			return true;
		}

		/// <summary>
		/// Returns a Int Array from a Key
		/// </summary>
		public static int[] GetIntArray(string key)
		{
			if (PlayerPrefs.HasKey(key)) {
				string[] stringArray = PlayerPrefs.GetString(key).Split('|');
				int[] intArray = new int[stringArray.Length];
				for (int i = 0, imax = stringArray.Length; i < imax; i++)
					intArray[i] = Convert.ToInt32(stringArray[i]);
				return intArray;
			}
			return new int[0];
		}

		/// <summary>
		/// Returns a Int Array from a Key
		/// Note: Uses default values to initialize if no key was found
		/// </summary>
		public static int[] GetIntArray(string key, int defaultValue, int defaultSize)
		{
			if (PlayerPrefs.HasKey(key))
				return GetIntArray(key);
			int[] intArray = new int[defaultSize];
			for (int i = 0; i < defaultSize; i++)
				intArray[i] = defaultValue;
			return intArray;
		}

		#endregion

		#region Float Array

		/// <summary>
		/// Stores a Float Array or Multiple Parameters into a Key
		/// </summary>
		public static bool SetFloatArray(string key, params float[] floatArray)
		{
			if (floatArray == null || floatArray.Length == 0) {
				PlayerPrefs.DeleteKey(key);
				return false;
			}
		
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			for (int i = 0, imax = floatArray.Length - 1; i < imax; i++)
				sb.Append(floatArray[i]).Append("|");
			sb.Append(floatArray[floatArray.Length - 1]);
		
			try {
				PlayerPrefs.SetString(key, sb.ToString());
			} catch (Exception e) {
				Debug.Log(e);
				return false;
			}
			return true;
		}

		/// <summary>
		/// Returns a Float Array from a Key
		/// </summary>
		public static float[] GetFloatArray(string key)
		{
			if (PlayerPrefs.HasKey(key)) {
				string[] stringArray = PlayerPrefs.GetString(key).Split('|');
				float[] floatArray = new float[stringArray.Length];
				for (int i = 0, imax = stringArray.Length; i < imax; i++)
					floatArray[i] = Convert.ToSingle(stringArray[i]);
				return floatArray;
			}
			return new float[0];
		}

		/// <summary>
		/// Returns a String Array from a Key
		/// Note: Uses default values to initialize if no key was found
		/// </summary>
		public static float[] GetFloatArray(string key, float defaultValue, int defaultSize)
		{
			if (PlayerPrefs.HasKey(key))
				return GetFloatArray(key);
			float[] floatArray = new float[defaultSize];
			for (int i = 0; i < defaultSize; i++)
				floatArray[i] = defaultValue;
			return floatArray;
		}

		#endregion

		#region String Array

		/// <summary>
		/// Stores a String Array or Multiple Parameters into a Key w/ specific char seperator
		/// </summary>
		public static bool SetStringArray(string key, char separator, params string[] stringArray)
		{
			if (stringArray == null || stringArray.Length == 0) {
				PlayerPrefs.DeleteKey(key);
				return false;
			}
			try {
				PlayerPrefs.SetString(key, String.Join(separator.ToString(), stringArray));
			} catch (Exception e) {
				Debug.Log(e);
				return false;
			}
			return true;
		}

		/// <summary>
		/// Stores a Bool Array or Multiple Parameters into a Key
		/// </summary>
		public static bool SetStringArray(string key, params string[] stringArray)
		{
			if (!SetStringArray(key, "\n"[0], stringArray))
				return false;
			return true;
		}

		/// <summary>
		/// Returns a String Array from a key & char seperator
		/// </summary>
		public static string[] GetStringArray(string key, char separator)
		{
			if (PlayerPrefs.HasKey(key))
				return PlayerPrefs.GetString(key).Split(separator);
			return new string[0];
		}

		/// <summary>
		/// Returns a Bool Array from a key
		/// </summary>
		public static string[] GetStringArray(string key)
		{
			if (PlayerPrefs.HasKey(key))
				return PlayerPrefs.GetString(key).Split('\n');
			return new string[0];
		}

		/// <summary>
		/// Returns a String Array from a key & char seperator
		/// Note: Uses default values to initialize if no key was found
		/// </summary>
		public static string[] GetStringArray(string key, char separator, string defaultValue, int defaultSize)
		{
			if (PlayerPrefs.HasKey(key))
				return PlayerPrefs.GetString(key).Split(separator);
			string[] stringArray = new string[defaultSize];
			for (int i = 0; i < defaultSize; i++)
				stringArray[i] = defaultValue;
			return stringArray;
		}

		/// <summary>
		/// Returns a String Array from a key
		/// Note: Uses default values to initialize if no key was found
		/// </summary>
		public static String[] GetStringArray(string key, string defaultValue, int defaultSize)
		{
			return GetStringArray(key, "\n"[0], defaultValue, defaultSize);
		}

		#endregion

		#region Int List

		/// <summary>
		/// Stores a Int List
		/// </summary>
		public static bool SetIntList(string key, List<int> intList)
		{
			if (intList == null || intList.Count == 0) {
				PlayerPrefs.DeleteKey(key);
				return false;
			}

			return SetIntArray(key, intList.ToArray());
		}

		/// <summary>
		/// Returns a Int List from a Key
		/// </summary>
		public static List<int> GetIntList(string key)
		{
			return new List<int>(GetIntArray(key));
		}

		/// <summary>
		/// Returns a Int List from a Key
		/// Note: Uses default values to initialize if no key was found
		/// </summary>
		public static List<int> GetIntList(string key, int defaultValue, int defaultSize)
		{
			return new List<int>(GetIntArray(key, defaultValue, defaultSize));
		}

		#endregion

		#region Bool List

		/// <summary>
		/// Stores a Bool List
		/// </summary>
		public static bool SetBoolList(string key, List<bool> boolList)
		{
			if (boolList == null || boolList.Count == 0) {
				PlayerPrefs.DeleteKey(key);
				return false;
			}

			return SetBoolArray(key, boolList.ToArray());
		}

		/// <summary>
		/// Returns a Bool List from a Key
		/// </summary>
		public static List<bool> GetBoolList(string key)
		{
			return new List<bool>(GetBoolArray(key));
		}

		/// <summary>
		/// Returns a Int List from a Key
		/// Note: Uses default values to initialize if no key was found
		/// </summary>
		public static List<bool> GetBoolList(string key, bool defaultValue, int defaultSize)
		{
			return new List<bool>(GetBoolArray(key, defaultValue, defaultSize));
		}

		#endregion

		#region String List

		public static bool SetStringList(string key, List<string> stringList)
		{
			if (stringList == null || stringList.Count == 0) {
				PlayerPrefs.DeleteKey(key);
				return false;
			}

			return SetStringArray(key, stringList.ToArray());
		}

		public static List<string> GetStringList(string key)
		{
			return new List<string>(GetStringArray(key));
		}

		public static List<string> GetStringList(string key, string defaultVaule, int defaultSize)
		{
			return new List<string>(GetStringArray(key, defaultVaule, defaultSize));
		}

		#endregion

		#region Int Dictionary

		/// <summary>
		/// Stores a Int Dictionary
		/// </summary>
		public static bool SetIntDictionary(string key, Dictionary<int, int> intDict)
		{
			if (intDict == null || intDict.Count == 0) {
				PlayerPrefs.DeleteKey(key);
				return false;
			}

			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			foreach (KeyValuePair<int, int> kvp in intDict)
				sb.Append(string.Format("{0},{1}|", kvp.Key, kvp.Value));
			sb.Remove(sb.Length - 1, 1);

			try {
				PlayerPrefs.SetString(key, sb.ToString());
			} catch (Exception e) {
				Debug.Log(e);
				return false;
			}
			return true;
		}

		/// <summary>
		/// Returns a Int Dictionary from a Key
		/// </summary>
		public static Dictionary<int, int> GetIntDictionary(string key)
		{
			Dictionary<int, int> result = new Dictionary<int, int>();
			if (PlayerPrefs.HasKey(key)) {
				string[] stringArray = PlayerPrefs.GetString(key).Split('|');
				for (int i = 0, imax = stringArray.Length; i < imax; i++) {
					string[] kvpArray = stringArray[i].Split(',');
					if (kvpArray.Length < 2)
						continue;
					int intKey = Convert.ToInt32(kvpArray[0]);
					int intValue = Convert.ToInt32(kvpArray[1]);
					result[intKey] = intValue;
				}
			}
			return result;
		}
		#endregion

		#region Dictionary<int, String>
		public static bool SetStringDictionary(string key, Dictionary<int, string> strDic)
		{
			if (strDic == null || strDic.Count == 0) {
				PlayerPrefs.DeleteKey(key);
				return false;
			}

			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			foreach (KeyValuePair<int, string> kvp in strDic)
				sb.Append(string.Format("{0},{1}|", kvp.Key, kvp.Value));
			sb.Remove(sb.Length - 1, 1);

			try {
				PlayerPrefs.SetString(key, sb.ToString());
			} catch (Exception e) {
				Debug.Log(e);
				return false;
			}
			return true;
		}

		public static Dictionary<int, string> GetStringDictionary(string key)
		{
			Dictionary<int, string> result = new Dictionary<int, string>();

			if (PlayerPrefs.HasKey(key)) {
				string[] stringArray = PlayerPrefs.GetString(key).Split('|');
				for (int i = 0, imax = stringArray.Length; i < imax; i++) {
					string[] kvpArray = stringArray[i].Split(',');
					if (kvpArray.Length < 2)
						continue;
					int intKey = Convert.ToInt32(kvpArray[0]);
					string stringValue = kvpArray[1];
					result[intKey] = stringValue;
				}
			}
			return result;
		}
		#endregion

	}
}