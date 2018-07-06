using UnityEngine;
using System.Collections;

public static class AndroidUtils
{

	#if UNITY_ANDROID

	/// <summary>
	/// Checks the Android app is available in player's device.
	/// </summary>
	public static bool CheckAppIsAvailable(string package)
	{
		AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject ca = up.GetStatic<AndroidJavaObject>("currentActivity");
		AndroidJavaObject packageManager = ca.Call<AndroidJavaObject>("getPackageManager");

		//take the list of all packages on the device
		AndroidJavaObject appList = packageManager.Call<AndroidJavaObject>("getInstalledPackages", 0);
		int num = appList.Call<int>("size");
		for (int i = 0; i < num; i++) {
			AndroidJavaObject appInfo = appList.Call<AndroidJavaObject>("get", i);
			string packageNew = appInfo.Get<string>("packageName");
			if (packageNew.CompareTo(package) == 0)
				return true;
		}
		return false;
	}

	public static int signatureHash {
		get {
			#if !UNITY_EDITOR
			try {
				using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
					using (AndroidJavaObject curActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) {
						using (AndroidJavaObject packageManager = curActivity.Call<AndroidJavaObject>("getPackageManager")) {
							// Get Android application name
							string packageName = curActivity.Call<string>("getPackageName");
							// Get Signature
							int signatureInt = packageManager.GetStatic<int>("GET_SIGNATURES");  
							using (AndroidJavaObject packageInfo = packageManager.Call<AndroidJavaObject>("getPackageInfo", packageName, signatureInt)) {
								AndroidJavaObject[] signatures = packageInfo.Get<AndroidJavaObject[]>("signatures");  
								// return Signature hash
								if (signatures != null && signatures.Length > 0) {
									int hashCode = signatures[0].Call<int>("hashCode");  
									return hashCode;
								}
							}
						}
					}
				}
			} catch (System.Exception e) {
				Debug.Log(e);
				return 0;
			}
			#endif
			return 0;
		}
	}

	#endif

}