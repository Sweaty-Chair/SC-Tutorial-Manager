using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public static class DataUtils
{

	public static Vector2 GetVector2(string s, Vector2 defaultValue = default(Vector2), string error = "")
	{
		return GetVector2(s, ',', defaultValue, error);
	}

	public static Vector2 GetVector2(string s, char separator, Vector2 defaultValue = default(Vector2), string error = "")
	{
		if (!string.IsNullOrEmpty(s)) {
			string[] values = s.Split(separator);
			if (values.Length >= 2) {
				bool success = true;
				float f1, f2;
				success &= float.TryParse(values[0], out f1);
				success &= float.TryParse(values[1], out f2);
				if (success)
					return new Vector2(f1, f2);
			}
		}
		if (!string.IsNullOrEmpty(error))
			Debug.LogError(error);
		return defaultValue;
	}

	public static Vector3 GetVector3(string s, Vector3 defaultValue = default(Vector3), string error = "")
	{
		return GetVector3(s, ',', defaultValue, error);
	}

	public static Vector3 GetVector3(string s, char separator, Vector3 defaultValue = default(Vector3), string error = "")
	{
		if (!string.IsNullOrEmpty(s)) {
			string[] values = s.Split(separator);
			if (values.Length >= 2) {
				bool success = true;
				float f1, f2, f3;
				success &= float.TryParse(values[0], out f1);
				success &= float.TryParse(values[1], out f2);
				success &= float.TryParse(values[1], out f3);
				if (success)
					return new Vector3(f1, f2, f3);
			}
		}
		if (!string.IsNullOrEmpty(error))
			Debug.LogError(error);
		return defaultValue;
	}

	public static string Vector3ToString(Vector3 v, char split = ',')
	{
		return string.Empty + v.x + split + v.y + split + v.z;
	}

	public static Vector4 GetVector4(string s, Vector4 defaultValue = default(Vector4), string error = "")
	{
		return GetVector4(s, ',', defaultValue, error);
	}

	public static Vector4 GetVector4(string s, char separator, Vector4 defaultValue = default(Vector4), string error = "")
	{
		if (!string.IsNullOrEmpty(s)) {
			string[] values = s.Split(separator);
			if (values.Length >= 2) {
				bool success = true;
				float f1, f2, f3, f4;
				success &= float.TryParse(values[0], out f1);
				success &= float.TryParse(values[1], out f2);
				success &= float.TryParse(values[1], out f3);
				success &= float.TryParse(values[1], out f4);
				if (success)
					return new Vector4(f1, f2, f3, f4);
			}
		}
		if (!string.IsNullOrEmpty(error))
			Debug.LogError(error);
		return defaultValue;
	}

	public static int GetInt(object o, int defaultValue = 0, string error = "")
	{
		if (o is int)
			return (int)o;
		if (o is double)
			return (int)((double)o);
		return GetInt(o as string, defaultValue, error);
	}

	public static int GetInt(string s, int defaultValue = 0, string error = "")
	{
		int i;
		bool success = int.TryParse(s, out i);
		if (success)
			return i;
		if (!string.IsNullOrEmpty(error))
			Debug.LogError(error);
		return defaultValue;
	}

	public static float GetFloat(object o, float defaultValue = 0, string error = "")
	{
		if (o is float)
			return (float)o;
		if (o is double)
			return (float)((double)o);
		return GetFloat(o as string, defaultValue, error);
	}

	public static float GetFloat(string s, float defaultValue = 0, string error = "")
	{
		float f;
		bool success = float.TryParse(s, out f);
		if (success)
			return f;
		if (!string.IsNullOrEmpty(error))
			Debug.LogError(error);
		return defaultValue;
	}

	public static bool GetBool(string s)
	{
		return !string.IsNullOrEmpty(s) && s != "0";
	}

	public static DateTime GetDateTime(string s, DateTime defaultValue = default(DateTime), string error = "")
	{
		DateTime d = default(DateTime);
		bool success = DateTime.TryParse(s, out d);
		if (success)
			return d;
		if (!string.IsNullOrEmpty(error))
			Debug.LogError(error);
		return defaultValue;
	}

	public static int[] GetIntArray(string s, int defaultValue = 0, string error = "")
	{
		string[] stringArray = s.Split('|');
		int[] intArray = new int[stringArray.Length];
		for (int i = 0, imax = stringArray.Length; i < imax; i++) {
			intArray[i] = GetInt(stringArray[i], defaultValue, error);
		}
		return intArray;
	}

	public static string[] GetStringArray(string s)
	{
		string[] stringArray = s.Split('|');
		return stringArray;
	}

	public static bool IsAllDigital(string s)
	{
		if (!string.IsNullOrEmpty(s))
			return s.All(char.IsDigit);
		else
			return false;
	}

	public static int Parse(int? i, int defaultValue = 0)
	{
		return i == null ? defaultValue : (int)i;
	}

}