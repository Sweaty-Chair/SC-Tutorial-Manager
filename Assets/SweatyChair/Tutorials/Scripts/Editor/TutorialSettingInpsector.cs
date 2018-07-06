using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class TutorialSettingInpsector
{

	[MenuItem("Sweaty Chair/Tutorial Settings")]
	public static void ServerSettings()
	{
		TutorialSettings settings = Resources.Load<TutorialSettings>("TutorialSettings");

		if (settings == null) {
			settings = ScriptableObject.CreateInstance<TutorialSettings>();
			var assetPathAndName = AssetDatabase.GenerateUniqueAssetPath("Assets/SweatyChair/Server/Resources/TutorialSettings.asset");
			Debug.Log("Creating TutorialSettings asset at " + assetPathAndName);
			AssetDatabase.CreateAsset(settings, assetPathAndName);
		}

		Selection.activeObject = settings;
	}
}