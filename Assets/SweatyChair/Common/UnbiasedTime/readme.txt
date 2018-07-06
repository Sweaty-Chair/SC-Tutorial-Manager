# Time-cheat prevention

iOS native plugin to prevent cheating by changing phone time in settings. 
Works in offline mode. 

Plugin have simple API which main method is Now(). It returns current timestamp 
that is initialized at first app launch and increment monotonically regardless of 
time changes in device settings. 

### Usage

 1. Plugins/iOS folder contains native static lib for iOS: `libUnbiasedTime.a`. 
    It should be included automatically in the generated Xcode project. 

 2. UnbiasedTime.cs is a wrapper script for native lib. It is a singleton class
    and creates empty GameObject on first access to `UnbiasedTime.Instance`. 

 3. Call `UnbiasedTime.Instance.Now()` to get estimated real timestamp. Use it for
    time span calculations, but be aware that it have 1 second precision. 

### Sample project

Simple demo included. It have two timers: one naive using `DateTime.Now` and the other 
is using `UnbiasedTime.Instance.Now()`. Timers are implemented by remembering timestamp of timespan end in PlayerPrefs 
so they keep counting even if app is backgrounded or closed. Launch it on your mobile device, tap +60 seconds several times, then go to
settings and increment current device time by few minutes. One timer will advance forward, while the other will keep
it's speed unchanged measuring real time. 


### Caution

It is advised to use timestamp server to get real world time and fallback to `UnbiasedTime.Instance.Now()` only 
when offline. 


### ntp server sync

Package contains implementation for network time protocol (ntp). It requires .NET sockets which is a pro-only feature on mobile. 
The script `UnbiasedTime/UnbiasedTime_UnityPro.txt` uses ntp when available and fallback to offline method in case of errors. 
To enable this feature, just rename the `UnbiasedTime_UnityPro.txt` to `UnbiasedTime.cs` - it requires Unity3d Pro on mobile platforms, 
but works for desktop and editor. 

