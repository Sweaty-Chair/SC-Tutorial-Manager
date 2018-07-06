using System;
using System.Linq;
using System.Collections.Generic;

public static class EnumUtils
{

	public static bool IsDefined<T>(int i) where T : struct, IConvertible
	{
		return Enum.IsDefined(typeof(T), i);
	}

	public static bool IsDefined<T>(string enumStr) where T : struct, IConvertible
	{
		return Enum.IsDefined(typeof(T), enumStr);
	}

	public static IEnumerable<T> GetValues<T>() where T : struct, IConvertible
	{
		if (!typeof(T).IsEnum)
			throw new ArgumentException("T must be an enumerated type");
		return Enum.GetValues(typeof(T)).Cast<T>();
	}

	public static int GetCount<T>() where T : struct, IConvertible
	{
		if (!typeof(T).IsEnum)
			throw new ArgumentException("T must be an enumerated type");
		return Enum.GetNames(typeof(T)).Length;
	}

    public static T GetRandom<T>() where T : struct, IConvertible
    {
        List<T> list = new List<T>(GetValues<T>());
        return list[UnityEngine.Random.Range(0, list.Count)];
    }

	public static T Parse<T>(string value, bool ignoreCase = true) where T : struct, IConvertible
	{
		return (T)Enum.Parse(typeof(T), value, ignoreCase);
	}

}