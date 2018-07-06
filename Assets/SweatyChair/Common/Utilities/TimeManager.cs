using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using SweatyChair;

public class TimeManager : MonoBehaviour
{
	private static TimeManager _instance;

    private static List<Timer> _timers = new List<Timer>();

	void Awake()
	{
		_instance = this;
	}

    private void Update()
    {
        for (int i = 0, imax = _timers.Count; i < imax; i++)
        {
            //if (_timers[i] == null)
            //{
            //    RemoveManagedTimer(_timers[i]);
            //    continue;
            //}

            _timers[i].Update();
        }
    }

    public static void Invoke(UnityAction callAction, float seconds)
	{
		if (_instance != null)
			_instance.StartCoroutine(_instance.InvokeCoroutine(callAction, seconds));
	}

	private IEnumerator InvokeCoroutine(UnityAction callAction, float seconds)
	{
		yield return new WaitForSecondsRealtime(seconds);
		if (callAction != null)
			callAction();
	}

	public static void Start(IEnumerator routine)
	{
		if (_instance != null)
			_instance.StartCoroutine(routine);
	}

	public static void Stop(IEnumerator routine)
	{
		if (_instance != null && routine != null)
			_instance.StopCoroutine(routine);
	}

    #region Timers

    public static void AddManagedTimer(Timer timer)
    {
        if (!_timers.Contains(timer))
            _timers.Add(timer);
    }

    public static void RemoveManagedTimer(Timer timer)
    {
        _timers.Remove(timer);
    }

    #endregion

	public static IEnumerator WaitForFrames(int frames, UnityAction onCompleteAction)
	{
		IEnumerator waitRoutine = WaitForFramesRoutine(frames, onCompleteAction);
		Start(waitRoutine);
		return waitRoutine;
	}

	private static IEnumerator WaitForFramesRoutine(int frames, UnityAction onCompleteAction)
	{
		for (int i = 0; i < frames; i++) {
			yield return null;
		}

		if (onCompleteAction != null) {
			onCompleteAction();
		}
	}
}