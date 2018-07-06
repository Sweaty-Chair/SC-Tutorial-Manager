using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SweatyChair
{

	public static class StringUtils
	{

		// Contains letter and number
		private static Regex REGEX_NON_DIGIT = new Regex(@"[^\d]");
		private static Regex REGEX_NON_NUMBER = new Regex(@"[^\d\.]");
		private static Regex REGEX_NON_ALPHANUMERIC = new Regex(@"[^a-zA-Z0-9]");
		private static Regex REGEX_ALPHANUMERIC = new Regex(@"[a-zA-Z0-9]");

		// Contains chinese characters
		private const string PATTERN_CHS_CHT = @"[\u4e00-\u9fa5]";
		private static Regex REGEX_CHS_CHT = new Regex(PATTERN_CHS_CHT);
		private const string PATTERN_EN_NUM = @"[a-zA-Z0-9\(\)\_\-\.\!\@]";
		private static Regex REGEX_EN_NUM = new Regex(PATTERN_EN_NUM);

		public static int IndexOfNth(this string str, char c, int n)
		{
			int s = -1;
			for (int i = 0; i < n; i++) {
				s = str.IndexOf(c, s + 1);
				if (s == -1)
					break;
			}
			return s;
		}

		public static string StripName(this string name, int maxLength = 16)
		{
			if (name.Length > maxLength) {
				string[] names = name.Split(' ');
				if (names.Length >= 2) { // 2+ words name
					if (names[0].Length > maxLength) {
						return names[0].Substring(0, maxLength - 1) + ".";
					} else if (names[0].Length == maxLength) {
						return names[0];
					} else {
						if (names.Length == 2) {
							return names[0] + " " + names[1].Substring(0, Mathf.Clamp(maxLength - names[0].Length, 0, names[1].Length)) + ".";
						} else {
							if (names[0].Length + names[1].Length > maxLength)
								return names[0] + " " + names[1].Substring(0, Mathf.Clamp(maxLength - names[0].Length, 0, names[1].Length)) + ".";
							else
								return names[0] + " " + names[1].Substring(0, Mathf.Clamp(maxLength - names[0].Length, 0, names[1].Length)) + " " + names[2].Substring(0, Mathf.Clamp(maxLength - names[0].Length - names[1].Length, 0, names[2].Length)) + ".";
						}
					}
				} else { // 1 word name
					return name.Substring(0, maxLength - 1) + ".";
				}
			}
			return name;
		}

		public static string StripAfterLast(this string s, string delimiter = ",")
		{
			int delimiterInd = s.LastIndexOf(delimiter);
			if (delimiterInd >= 0)
				return s.Substring(0, delimiterInd + 1);
			return s;
		}

		public static string StripBeforeLast(this string s, string delimiter = ",")
		{
			int delimiterInd = s.LastIndexOf(delimiter);
			if (delimiterInd >= 0)
				return s.Substring(0, delimiterInd);
			return s;
		}

		public static string StripParenthesis(this string s)
		{
			int parenthesisPos = s.IndexOf("("[0]);
			if (parenthesisPos > 0)
				return s.Substring(0, parenthesisPos);
			return s;
		}

		public static string SubstringAfterLast(this string s, string delimiter = ",")
		{
			int delimiterInd = s.LastIndexOf(delimiter);
			if (delimiterInd >= 0)
				return s.Substring(delimiterInd + delimiter.Length);
			return s;
		}

		public static string ToUCFirst(this string s)
		{
			if (string.IsNullOrEmpty(s))
				return "";
			return char.ToUpper(s[0]) + s.Substring(1);
		}

		public static string ToSingular(this string s)
		{
			if (s.LastIndexOf("ies") == s.Length - 3)
				return s.Substring(0, s.Length - 3);
			if (s.LastIndexOf("es") == s.Length - 2)
				return s.Substring(0, s.Length - 2);
			if (s.LastIndexOf("s") == s.Length - 1)
				return s.Substring(0, s.Length - 1);
			return s;
		}

		public static string ToPlural(this string s, int num = 2)
		{
			if (num <= 1)
				return s; // No need pural

			if (s.LastIndexOf("y") == s.Length - 1)
				return s.Substring(0, s.Length - 1) + "ies";
			if (s.LastIndexOf("th") == s.Length - 2)
				return s + "es";
			if (s.LastIndexOf("s") == s.Length - 1 || s.LastIndexOf("es") == s.Length - 2) // Skip if string already ended with -s or -es
			return s;
			return s + "s";
		}

		public static string SecondsToMinutesStr(float seconds)
		{
			return "" + Mathf.FloorToInt(seconds / 60).ToString("D2") + ":" + Mathf.RoundToInt(seconds % 60).ToString("D2");
		}

		public static string SecondsToHoursStr(float seconds)
		{
			return "" + Mathf.FloorToInt(seconds / 3600).ToString("D2") + ":" + Mathf.FloorToInt((seconds % 3600) / 60).ToString("D2") + ":" + Mathf.RoundToInt(seconds % 60).ToString("D2");
		}

		public static string ToRomanString(this int i)
		{
			switch (i) {
				case 0:
					return "I";
				case 1:
					return "II";
				case 2:
					return "III";
				default:
					return string.Empty;
			}
		}

		public static string MultiplerString(this int i)
		{
			switch (i) {
				case 2:
					return "Double";
				case 3:
					return "Triple";
				case 4:
					return "Quadruple";
				case 5:
					return "Quintuple";
			}
			return "";
		}

		public static string MarkMystery(this string s)
		{
			if (string.IsNullOrEmpty(s))
				return string.Empty;
			return REGEX_ALPHANUMERIC.Replace(s, "?");
		}

		// Strip all characters except digits (0~9)
		public static string StripNonDigit(this string s)
		{
			if (string.IsNullOrEmpty(s))
				return string.Empty;
			return REGEX_NON_DIGIT.Replace(s, "");
		}

		// Try prase to the first appeared integer
		public static int ParseToInt(this string s, int defaultValue = 0)
		{
			int result = defaultValue;
			int.TryParse(StripNonDigit(s), out result);
			return result;
		}
	
		// Try prase to the first appeared decimal
		public static decimal ParseToDecimal(this string s, decimal defaultValue = 0)
		{
			decimal result = defaultValue;
			decimal.TryParse(StripNonNumber(s), out result);
			return result;
		}
	
		// Strip all characters except digits (0~9 AND .)
		public static string StripNonNumber(this string s)
		{
			if (string.IsNullOrEmpty(s))
				return string.Empty;
			return REGEX_NON_NUMBER.Replace(s, "");
		}

		public static string StripNonAlphanumeric(this string s)
		{
			if (string.IsNullOrEmpty(s))
				return string.Empty;
			return REGEX_NON_ALPHANUMERIC.Replace(s, "");
		}

		public static bool IsChinese(this string s)
		{
			return Regex.IsMatch(s, PATTERN_CHS_CHT);
		}

		public static bool ContainChinese(this string s)
		{
			return REGEX_CHS_CHT.IsMatch(s);
		}

		public static bool IsEngOrNum(this string s)
		{
			return Regex.IsMatch(s, PATTERN_EN_NUM);
		}

		public static bool IsValidIP(this string s)
		{
			if (string.IsNullOrEmpty(s))
				return false;
			System.Net.IPAddress address;
			if (System.Net.IPAddress.TryParse(s, out address)) {
				switch (address.AddressFamily) {
					case System.Net.Sockets.AddressFamily.InterNetwork:
					case System.Net.Sockets.AddressFamily.InterNetworkV6:
						return true;
					default:
						return false;
				}
			}
			return false;
		}

		public static bool ContainEngOrNum(this string s)
		{
			return REGEX_EN_NUM.IsMatch(s);
		}

		public static string CamelCaseToSpace(this string s)
		{
			return Regex.Replace(s, "(\\B[A-Z])", " $1");
		}

		public static string TrimSpaces(this string s)
		{
			return s.Replace(" ", "");
		}

		// Use string builder to build string by string array is good for performance, for 10+ strings.
		public static string BuildString(string[] strings)
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < strings.Length; i++)
				sb.Append(strings[i]);
			return sb.ToString();
		}

		public static string ArrayToString<T>(T[] array, string seperator = ",")
		{
			if (array.Length <= 0)
				return "[]";
			string tmp = "[";
			foreach (T t in array)
				tmp += (t == null ? "null" : t.ToString()) + seperator;
			return tmp.Substring(0, tmp.Length - 1) + "]";
		}

		public static string ArrayToString(object[] array, string seperator = ",")
		{
			if (array.Length <= 0)
				return "[]";
			string tmp = "[";
			foreach (object o in array)
				tmp += (o == null ? "null" : o.ToString()) + seperator;
			return tmp.Substring(0, tmp.Length - 1) + "]";
		}


		// Convert a List to a string, for debug purpose
		public static string ListToString(IList list, string seperator = ",")
		{
			if (list == null)
				return "null";
			if (list.Count <= 0)
				return "[]";
			string tmp = "[";
			foreach (object o in list)
				tmp += (o == null ? "null" : o.ToString()) + seperator;
			return tmp.Substring(0, tmp.Length - 1) + "]";
		}

		// Convert a Dictionary to a string, for debug purpose
		public static string DictionaryToString(IDictionary dict)
		{
			if (dict == null)
				return "null";
			if (dict.Count <= 0)
				return "[]";
			string tmp = "[";
			foreach (DictionaryEntry de in dict) {
				if (de.Value is IList)
					tmp += string.Format("{{{0}:{1}}},", de.Key, ListToString(de.Value as IList));
				else if (de.Value is IDictionary)
					tmp += string.Format("{{{0}:{1}}},", de.Key, DictionaryToString(de.Value as IDictionary));
				else
					tmp += string.Format("{{{0}:{1}}},", de.Key, de.Value);
			}
			return tmp.Substring(0, tmp.Length - 1) + "]";
		}

		// Convert a Collection to a string, for debug purpose
		public static string CollectionToString(ICollection coll, string seperator = ",")
		{
			if (coll == null)
				return "null";
			if (coll.Count <= 0)
				return "[]";
			string tmp = "[";
			foreach (object o in coll)
				tmp += (o == null ? "null" : o.ToString()) + seperator;
			return tmp.Substring(0, tmp.Length - 1) + "]";
		}

		public static string ToRed(this string txt)
		{
			return ToColor(txt, "ff0000");
		}

		public static string ToGreen(this string txt)
		{
			return ToColor(txt, "00ff00");
		}

		public static string ToBlue(this string txt)
		{
			return ToColor(txt, "0000ff");
		}

		public static string ToColor(this string txt, string color)
		{
			return string.Format("<color=#{0}>{1}</color>", color, txt);
		}

	}

}