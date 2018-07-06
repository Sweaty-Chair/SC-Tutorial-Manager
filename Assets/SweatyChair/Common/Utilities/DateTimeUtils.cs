using UnityEngine;
using System;
using System.Collections;
using SweatyChair;

public static class DateTimeUtils
{

	private const int DAY_TO_SECONDS = 86400;

	public static DateTime nextDay {
		get { return Now().AddSeconds(DAY_TO_SECONDS - Now().TimeOfDay.TotalSeconds); }
	}

	public static DateTime GetDataTimeFromBinaryString(string s)
	{
		return DateTime.FromBinary(Convert.ToInt64(s));
	}

	public static string ToBinaryString(this DateTime dt)
	{
		return dt.ToBinary().ToString();
	}

	public static DateTime GetDateTimeFromBinary(long binary)
	{
		return DateTime.FromBinary(binary);
	}

	// dd: two digit day; MM: two digit month; HH: two digit hour, 24 hour clock; mm: tow digit minuts; ss: two digit second.
	// hh: two digit hour, 12 hour clock.
	public static string GetFormatStringByDateTime(System.DateTime dt)
	{
		return dt.ToString("ddMMyyyyHHmmss");
	}

	public static DateTime GetDateTimesFromFormatStrings(string s)
	{
		return DateTime.ParseExact(s, "ddMMyyyyHHmmss", null);
	}


	public static DateTime[] GetFutureHoursDateTimesFromFormatStrings(string[] strArray, int hours)
	{
		if (strArray == null)
			return null;
		DateTime[] results = new DateTime[strArray.Length];
		for (int i = 0, imax = strArray.Length; i < imax; i++) {
			DateTime dt = GetDataTimeFromBinaryString(strArray[i]);
			results[i] = ValidateFutureHours(dt, hours);
		}
		return results;
	}

	public static DateTime[] GetFutureMinutesDateTimesFromFormatStrings(string[] strArray, int minutes)
	{
		if (strArray == null)
			return null;
		DateTime[] results = new DateTime[strArray.Length];
		for (int i = 0, imax = strArray.Length; i < imax; i++) {
			DateTime dt = GetDataTimeFromBinaryString(strArray[i]);
			results[i] = ValidateFutureMinutes(dt, minutes);
		}
		return results;
	}

	public static DateTime[] GetFutureSecondsDateTimesFromFormatStrings(string[] strArray, int seconds)
	{
		if (strArray == null)
			return null;
		DateTime[] results = new DateTime[strArray.Length];
		for (int i = 0, imax = strArray.Length; i < imax; i++) {
			DateTime dt = GetDataTimeFromBinaryString(strArray[i]);
			results[i] = ValidateFutureSeconds(dt, seconds);
		}
		return results;
	}

	public static DateTime[] GetDateTimesFromFormatStrings(string[] strArray)
	{
		if (strArray == null)
			return null;
		DateTime[] results = new DateTime[strArray.Length];
		for (int i = 0, imax = strArray.Length; i < imax; i++)
			results[i] = GetDataTimeFromBinaryString(strArray[i]);
		return results;
	}

	// Get PlayerPrefs DateTime and make sure it is a past time
	public static DateTime GetPastPlayerPrefs(string key, DateTime defaultValue = default(DateTime))
	{
		DateTime dt = GetPlayerPrefs(key, defaultValue);
		return ValidateIsPast(dt);
	}

	// Get PlayerPrefs DateTime and make sure it is a future time for at least ? hours
	public static DateTime GetFutureHoursPlayerPrefs(string key, int hours, DateTime defaultValue = default(DateTime))
	{
		DateTime dt = GetPlayerPrefs(key, defaultValue);
		return ValidateFutureHours(dt, hours);
	}

	// Get PlayerPrefs DateTime and make sure it is a future time for at least ? minutes
	public static DateTime GetFutureMinutesPlayerPrefs(string key, int minutes, DateTime defaultValue = default(DateTime))
	{
		DateTime dt = GetPlayerPrefs(key, defaultValue);
		return ValidateFutureMinutes(dt, minutes);
	}

	// Get PlayerPrefs DateTime and make sure it is a future time for at least ? seconds
	public static DateTime GetFutureSecondsPlayerPrefs(string key, int seconds, DateTime defaultValue = default(DateTime))
	{
		DateTime dt = GetPlayerPrefs(key, defaultValue);
		return ValidateFutureSeconds(dt, seconds);
	}

	public static DateTime GetPlayerPrefs(string key, DateTime defaultValue = default(DateTime))
	{
		if (PlayerPrefs.HasKey(key))
			return GetDataTimeFromBinaryString(PlayerPrefs.GetString(key));
		if (defaultValue == default(DateTime))
			return Now();
		return defaultValue;
	}

	public static DateTime[] GetFutureSecondsPlayerPrefsX(string key, int seconds, DateTime defaultValue = default(DateTime), int defaultSize = 0)
	{
		if (defaultValue == default(DateTime))
			defaultValue = Now();
		return GetFutureSecondsDateTimesFromFormatStrings(PlayerPrefsX.GetStringArray(key, defaultValue.ToBinaryString(), defaultSize), seconds);
	}

	public static DateTime[] GetPlayerPrefsX(string key, DateTime defaultValue = default(DateTime), int defaultSize = 0)
	{
		return GetDateTimesFromFormatStrings(PlayerPrefsX.GetStringArray(key, defaultValue.ToBinaryString(), defaultSize));
	}

	public static DateTime SetPlayerPrefsToNow(string key)
	{
		DateTime dt = Now();
		SetPlayerPrefs(key, dt);
		return dt;
	}

	public static DateTime SetPlayerPrefsHoursFromNow(string key, int hours)
	{
		DateTime dt = Now().AddHours(hours);
		SetPlayerPrefs(key, dt);
		return dt;
	}

	public static DateTime SetPlayerPrefsMinutesFromNow(string key, int minutes)
	{
		DateTime dt = Now().AddMinutes(minutes);
		SetPlayerPrefs(key, dt);
		return dt;
	}

	public static DateTime SetPlayerPrefsSecondsFromNow(string key, int seconds)
	{
		DateTime dt = Now().AddSeconds(seconds);
		SetPlayerPrefs(key, dt);
		return dt;
	}

	public static void SetPlayerPrefs(string key, DateTime value)
	{
		PlayerPrefs.SetString(key, value.ToBinaryString());
	}

	public static void SetPlayerPrefsX(string key, DateTime[] value)
	{
		if (value == null || value.Length == 0)
			PlayerPrefs.DeleteKey(key);
		string[] strArray = new string[value.Length];
		for (int i = 0, imax = value.Length; i < imax; i++)
			strArray[i] = value[i].ToBinaryString();
		PlayerPrefsX.SetStringArray(key, strArray);
	}

	public static TimeSpan GetInterval(DateTime startDT, DateTime endDT)
	{
		return endDT.Subtract(startDT);
	}

	public static string ToDisplayString(this TimeSpan timeSpan, bool highDetail = false)
	{
		string textFormat = string.Empty;

		if (timeSpan.Days > 0) {

			if (highDetail) {
				textFormat = "{0}d {1}h {2}m {3}s";
				return LocalizeUtils.GetFormat(TermCategory.Default, textFormat, timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
			}

			textFormat = "{0}h {1}min";
			if (timeSpan.Minutes == 0)
				textFormat = "{0}h";
			return LocalizeUtils.GetFormat(TermCategory.Default, textFormat, (int)timeSpan.TotalHours, timeSpan.Minutes);

		} else if (timeSpan.Hours > 0) {

			if (highDetail) {
				textFormat = "{0}h {1}m {2}s";
				return LocalizeUtils.GetFormat(TermCategory.Default, textFormat, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
			}

			textFormat = "{0}h {1}min";
			if (timeSpan.Minutes == 0)
				textFormat = "{0}h";
			return LocalizeUtils.GetFormat(TermCategory.Default, textFormat, timeSpan.Hours, timeSpan.Minutes);

		} else if (timeSpan.Minutes > 0) {

			textFormat = "{0}m {1}s";
			if (timeSpan.Seconds == 0)
				textFormat = "{0}m";
			return LocalizeUtils.GetFormat(TermCategory.Default, textFormat, timeSpan.Minutes, timeSpan.Seconds);

		} else if (timeSpan.Seconds >= 0) {

			textFormat = "{0}s";
			return LocalizeUtils.GetFormat(TermCategory.Default, textFormat, timeSpan.Seconds);

		} else {

			return LocalizeUtils.Get(TermCategory.Default, "Completed!");

		}
	}

	public static DateTime UtcNow()
	{
		#if UNITY_EDITOR
		if (!Application.isPlaying)
			return DateTime.UtcNow; // Just use DateTime.UtcNow when debuging in Editor and not playing
		#endif
		DateTime utcNow = UnbiasedTime.Instance.UtcNow();
		if (utcNow.Year <= 2016) {
			#if UNITY_EDITOR || DEBUG
			Debug.LogErrorFormat("DateTimeUtils:UTCNow - Failed: UnbiasedTime.Instance.UtcNow={0}, DateTime.UtcNow={1}", utcNow, DateTime.UtcNow);
			#endif
			utcNow = DateTime.UtcNow;
		}
		return utcNow;
	}

	public static DateTime Now()
	{
		#if UNITY_EDITOR
		if (!Application.isPlaying)
			return DateTime.Now; // Just use DateTime.Now when debuging in Editor and not playing
		#endif
		DateTime dateNow = UnbiasedTime.Instance.Now();
		if (dateNow.Year <= 2016) {
			#if UNITY_EDITOR || DEBUG
			Debug.LogErrorFormat("TimeManager- UnbiasedTime failed:{0} using DefaultDateTime:{1}", dateNow, DateTime.Now);
			#endif
			dateNow = DateTime.Now;
		}
		return dateNow;
	}

	/// <summary>
	/// Gets the seconds from a future DateTime.
	/// </summary>
	public static double GetSecondsFromNow(DateTime futureDateTime)
	{
		double result = (futureDateTime - Now()).TotalSeconds;
		return result > 0 ? result : 0;
	}

	/// <summary>
	/// Gets the seconds from a pased DateTime.
	/// </summary>
	public static double GetSecondsToNow(DateTime pastDateTime)
	{
		double result = (Now() - pastDateTime).TotalSeconds;
		return result > 0 ? result : 0;
	}

	/// <summary>
	/// Gets the seconds from a future DateTime.
	/// </summary>
	public static double GetMinutesFromNow(DateTime futureDateTime)
	{
		double result = (futureDateTime - Now()).TotalMinutes;
		return result > 0 ? result : 0;
	}

	/// <summary>
	/// Gets the seconds from a pased DateTime.
	/// </summary>
	public static double GetMinutesToNow(DateTime pastDateTime)
	{
		double result = (Now() - pastDateTime).TotalMinutes;
		return result > 0 ? result : 0;
	}

	/// <summary>
	/// Gets the seconds from a future DateTime.
	/// </summary>
	public static double GetHoursFromNow(DateTime futureDateTime)
	{
		double result = (futureDateTime - Now()).TotalHours;
		return result > 0 ? result : 0;
	}

	/// <summary>
	/// Gets the seconds from a pased DateTime.
	/// </summary>
	public static double GetHoursToNow(DateTime pastDateTime)
	{
		double result = (Now() - pastDateTime).TotalHours; 
		return result > 0 ? result : 0;
	}

	/// <summary>
	/// Gets the TimeSpan from a future DateTime.
	/// </summary>
	public static TimeSpan GetTimeSpanFromNow(DateTime futureDateTime)
	{
		return futureDateTime - Now();
	}

	/// <summary>
	/// Gets the TimeSpan from a pased DateTime.
	/// </summary>
	public static TimeSpan GetTimeSpanToNow(DateTime pastDateTime)
	{
		return Now() - pastDateTime;
	}

	public static DateTime GetDateTimeAfterSeconds(double seconds)
	{
		return Now().AddSeconds(seconds);
	}

	public static bool IsPast(DateTime dt)
	{
		return Now() > dt;
	}

	public static bool IsFuture(DateTime dt)
	{
		return Now() < dt;
	}

	// PlayerPrefs saved DateTime has wired chance to be set to be a far future DateTime after a crash, validate here
	public static DateTime ValidateIsPast(DateTime dt)
	{
		if (IsFuture(dt))
			return Now();
		return dt;
	}

	public static DateTime ValidateFutureHours(DateTime dt, int hours)
	{
		if (GetHoursFromNow(dt) > hours)
			return Now().AddHours(hours);
		return dt;
	}

	public static DateTime ValidateFutureMinutes(DateTime dt, int minutes)
	{
		if (GetMinutesFromNow(dt) > minutes)
			return Now().AddMinutes(minutes);
		return dt;
	}

	public static DateTime ValidateFutureSeconds(DateTime dt, int seconds)
	{
		if (GetSecondsFromNow(dt) > seconds)
			return Now().AddSeconds(seconds);
		return dt;
	}

	public static DateTime Parse(DateTime? dateTime, DateTime defaultValue = default(DateTime))
	{
		return dateTime == null ? defaultValue : (DateTime)dateTime;
	}

	public static DateTime ParseEpoch(long? epochMilliseconds, DateTime defaultValue = default(DateTime))
	{
		if (epochMilliseconds == null)
			return defaultValue;
		return (new DateTime(1970, 1, 1)).AddMilliseconds((long)epochMilliseconds);
	}

	public static DateTime UnbiasedToLocalDateTime(this DateTime unbiasedDateTime)
	{
		return DateTime.Now.Add(unbiasedDateTime - Now());
	}

	public static DateTime LocalToUnbiasedDateTime(this DateTime localDateTime)
	{
		return Now().Add(localDateTime - DateTime.Now);
	}

	public static DateTime ServerUtcToLocalDateTime(this DateTime serverUtcDateTime)
	{
		return DateTime.Now.Add(serverUtcDateTime - UtcNow());
	}

	public static DateTime LocalToServerUtcDateTime(DateTime localDateTime)
	{
		return UtcNow().Add(localDateTime - DateTime.Now);
	}

}