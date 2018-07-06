using System.Linq;
using UnityEngine;

public abstract class ScriptableObjectSingleton<T> : ScriptableObject where T : ScriptableObject, new()
{
	
	private static T _instance;

	public static T current {
		get {
			if (!_instance) {
				// Try load with exact class name first
				_instance = Resources.Load<T>(typeof(T).ToString().Replace("SweatyChair.", ""));
				if (_instance == null) {
					// Try find with object type
					_instance = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();
					if (_instance == null) {
						// Create a new one if can't find any
						Debug.LogErrorFormat("{0}:current - Not found in Resources folder.", typeof(T));
						_instance = new T(); // Create a new one with default settings
					}
				}
			}
			return _instance;
		}
	}

}