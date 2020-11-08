using UnityEngine;
using System.Collections.Generic;

namespace SweatyChair
{

	/// <summary>
	/// Tutorial module settings.
	/// </summary>
	[CreateAssetMenu(fileName = "TutorialSettings", menuName = "Sweaty Chair/Settings/Tutorial")]
	public class TutorialSettings : ScriptableObjectSingleton<TutorialSettings>
	{

		public List<Tutorial> tutorials = new List<Tutorial>();

		public int skipCheckingStateMask = 0;
		public string canvasPath = "";
		public bool debugMode = false;

#if UNITY_EDITOR

		[UnityEditor.MenuItem("Sweaty Chair/Settings/Tutorial %#t")]
		public static void OpenSettings()
		{
			current.OpenAssetsFile();
		}

#endif

	}

}