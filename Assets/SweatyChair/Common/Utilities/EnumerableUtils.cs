using System;
using System.Linq;
using System.Collections.Generic;

public static class EnumerableUtils
{

	// Array Resize and Fill all nulls
	public static T[] Resize<T>(this T[] array, int sz) where T : new()
	{
		int prevLength = array == null ? 0 : array.Length;
		Array.Resize<T>(ref array, sz);
		if (prevLength < sz) {
			for (int i = prevLength; i < sz; i++)
				array[i] = new T();
		}
		return array;
	}

	public static T[] Resize<T>(this T[] array, int sz, T c)
	{
		int prevLength = array == null ? 0 : array.Length;
		Array.Resize<T>(ref array, sz);
		if (prevLength < sz) {
			for (int i = prevLength; i < sz; i++)
				array[i] = c;
		}
		return array;
	}

	// List Resize
	public static List<T> Resize<T>(this List<T> list, int sz) where T : new()
	{
		return Resize(list, sz, new T());
	}

	public static List<T> Resize<T>(this List<T> list, int sz, T c)
	{
		if (list == null)
			list = new List<T>();
		int cur = list.Count;
		if (sz < cur)
			list.RemoveRange(sz, cur - sz);
		else if (sz > cur)
			list.AddRange(Enumerable.Repeat(c, sz - cur));
		return list;
	}

	// Array Shuffle
	public static T[] Shuffle<T>(this T[] array)
	{
		var r = new Random((int)DateTime.Now.Ticks);
		for (int i = array.Length - 1; i > 0; i--) {
			int j = r.Next(0, i - 1);
			var e = array[i];
			array[i] = array[j];
			array[j] = e;
		}
		return array;
	}

}