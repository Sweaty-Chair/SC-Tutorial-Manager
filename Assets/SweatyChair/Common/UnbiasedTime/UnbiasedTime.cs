using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Threading;
using System.Net;
using System.Net.Sockets;

public class UnbiasedTime : MonoBehaviour {

	private static UnbiasedTime instance;
	public static UnbiasedTime Instance {
		get {
			if (instance == null) {
				GameObject g = new GameObject ("UnbiasedTimeSingleton");
				instance = g.AddComponent<UnbiasedTime> ();
				DontDestroyOnLoad(g);
			}
			return instance;
		}
	}

	// Estimated difference in seconds between device time and real world time
	// timeOffset = deviceTime - worldTime;
	[HideInInspector]
	public long timeOffset = 0;

	[HideInInspector]
	public long? ntpTimeOffset = 0;	

	private Thread socketThread = null;
	private UdpClient client;

//	void Awake() {
//		SessionStart();
//	}

	void OnApplicationPause (bool pause) {
		if (pause) {
			SessionEnd();
		}
		else {
			SessionStart();
		}
	}

	void OnApplicationQuit () {
		SessionEnd();
		if (socketThread != null) { 
			socketThread.Abort(); 
		}
	}

	// Returns estimated DateTime value taking into account possible device time changes
	public DateTime UtcNow() {
		if (ntpTimeOffset != null) {
			return DateTime.UtcNow.AddSeconds ( -1.0f * (double)ntpTimeOffset );
		}
		else {
			return DateTime.UtcNow.AddSeconds ( -1.0f * timeOffset );
		}
	}

	// Returns estimated DateTime value taking into account possible device time changes
	public DateTime Now() {
		if (ntpTimeOffset != null) {
			return DateTime.Now.AddSeconds ( -1.0f * (double)ntpTimeOffset );
		}
		else {
			return DateTime.Now.AddSeconds ( -1.0f * timeOffset );
		}
	}

	// This method updates network time offset asynchronously. 
	// It is called automatically on each session start event. 
	// The Now() method will use network offset if it is available, otherwise 
	// it will fallback to estimated offline real time from native plugin. 
	public void UpdateNetworkTimeOffset() {
		//if (SweatyChair.ServerSettings.current.getTimeFromServer) {
		//	GetServerUtc();
		//} else {
			socketThread = new Thread(new ThreadStart(NtpImpl));
			socketThread.IsBackground = true;
			socketThread.Start();
		//}
	}

	// timeOffset value is cached for performance reasons (calls to native plugins can be expensive). 
	// This method is used to update offset value in cases if you think device time was changed by user. 
	// 
	// However, time offset is updated automatically when app gets backgrounded or foregrounded. 
	// 
	public void UpdateTimeOffset() {
		#if UNITY_ANDROID
		UpdateTimeOffsetAndroid();
		#elif UNITY_IPHONE
		UpdateTimeOffsetIOS();
		#endif
	}

	private void SessionStart () {
		#if UNITY_ANDROID
		StartAndroid();
		#elif UNITY_IPHONE
		StartIOS();
		#endif
		UpdateNetworkTimeOffset();
	}

	private void SessionEnd () {
		#if UNITY_ANDROID
		EndAndroid();
		#elif UNITY_IPHONE
		EndIOS();
		#endif
	}

	private void NtpImpl() {
		const string ntpServer = "pool.ntp.org";
		var ntpData = new byte[48];
		ntpData[0] = 0x1B; //LeapIndicator = 0 (no warning), VersionNum = 3 (IPv4 only), Mode = 3 (Client Mode)

		try {
			var addresses = Dns.GetHostEntry(ntpServer).AddressList;
			var ipEndPoint = new IPEndPoint(addresses[0], 123);
			var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

			socket.ReceiveTimeout = 5000; // milliseconds
			socket.SendTimeout = 5000;
			socket.Connect(ipEndPoint);
			socket.Send(ntpData);
			socket.Receive(ntpData);
			socket.Close();

			ulong intPart = (ulong)ntpData[40] << 24 | (ulong)ntpData[41] << 16 | (ulong)ntpData[42] << 8 | (ulong)ntpData[43];
			ulong fractPart = (ulong)ntpData[44] << 24 | (ulong)ntpData[45] << 16 | (ulong)ntpData[46] << 8 | (ulong)ntpData[47];

			var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);

			if (milliseconds < 1000000)
				return;

			var networkDateTime = (new DateTime(1900, 1, 1)).AddMilliseconds((long)milliseconds);
			ntpTimeOffset = (long?) (DateTime.UtcNow - networkDateTime).TotalSeconds;
		} catch {
			ntpTimeOffset = null;
		}
	}

	//private void GetServerUtc() {
	//	SweatyChair.ServerManager.Get("utc", SetNetworkDateTime);
	//}

	public void SetNetworkDateTime(Hashtable ht) {
		if (ht.ContainsKey("utc")) {
			SetNetworkDateTime(DataUtils.GetDateTime(ht["utc"] as string));
		}
	}

	public void SetNetworkDateTime(DateTime networkDateTime) {
		ntpTimeOffset = (long?) (DateTime.UtcNow - networkDateTime).TotalSeconds;
	}

	// Platform specific code
	// 

	#if UNITY_IPHONE
	[DllImport ("__Internal")]
	private static extern void _vtcOnSessionStart();

	[DllImport ("__Internal")]
	private static extern void _vtcOnSessionEnd();

	[DllImport ("__Internal")]
	private static extern int _vtcTimestampOffset();


	private void UpdateTimeOffsetIOS() {
	if (Application.platform != RuntimePlatform.IPhonePlayer) {
	return;
	}

	timeOffset = _vtcTimestampOffset();
	}

	private void StartIOS() {
	if (Application.platform != RuntimePlatform.IPhonePlayer) {
	return;
	}

	_vtcOnSessionStart();
	timeOffset = _vtcTimestampOffset();
	}

	private void EndIOS() {
	if (Application.platform != RuntimePlatform.IPhonePlayer) {
	return;
	}

	_vtcOnSessionEnd();
	}
	#endif


	#if UNITY_ANDROID
	private void UpdateTimeOffsetAndroid() {
		if (Application.platform != RuntimePlatform.Android) {
			return;
		}

		using (var activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) 
		using (var unbiasedTimeClass = new AndroidJavaClass("com.vasilij.unbiasedtime.UnbiasedTime")) {
			var playerActivityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
			if (playerActivityContext != null && unbiasedTimeClass != null) {
				timeOffset = unbiasedTimeClass.CallStatic <long> ("vtcTimestampOffset", playerActivityContext);
			}
		}		
	}

	private void StartAndroid() {
		if (Application.platform != RuntimePlatform.Android) {
			return;
		}

		using (var activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		using (var unbiasedTimeClass = new AndroidJavaClass("com.vasilij.unbiasedtime.UnbiasedTime")) {
			var playerActivityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
			if (playerActivityContext != null && unbiasedTimeClass != null) {
				unbiasedTimeClass.CallStatic ("vtcOnSessionStart", playerActivityContext);
				timeOffset = unbiasedTimeClass.CallStatic <long> ("vtcTimestampOffset");
			}
		}
	}

	private void EndAndroid() {
		if (Application.platform != RuntimePlatform.Android) {
			return;
		}

		using (var activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) 
		using (var unbiasedTimeClass = new AndroidJavaClass("com.vasilij.unbiasedtime.UnbiasedTime")) {
			var playerActivityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
			if (playerActivityContext != null && unbiasedTimeClass != null) {
				unbiasedTimeClass.CallStatic ("vtcOnSessionEnd", playerActivityContext);
			}
		}
	}
	#endif

}