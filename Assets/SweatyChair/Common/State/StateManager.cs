using UnityEngine;
using UnityEngine.Events;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public static class StateManager
{

	public static event UnityAction<State> stateChangedEvent;

	private static State _currentState = State.None;

	public static void Set(State newState)
	{
		if (_currentState == newState)
			return;

		_currentState = newState;

		if (stateChangedEvent != null)
			stateChangedEvent(_currentState);
	}

	public static State Get()
	{
		return _currentState;
	}

	// Returns true if the state matches the _currentState
	public static bool Compare(State checkState)
	{
		return checkState == _currentState;
	}

	// Returns true if the state mask included the _currentState
	public static bool Compare(int checkStateMask)
	{
		return (checkStateMask & (1 << (int)_currentState)) != 0;
	}

	public static void AddChangeListener(UnityAction<State> callback)
	{
		stateChangedEvent += callback;
	}

	#if UNITY_EDITOR

	[MenuItem("Debug/Print State", false, 1)]
	public static void PrintState()
	{
		Debug.Log(Get());
	}

	#endif

}