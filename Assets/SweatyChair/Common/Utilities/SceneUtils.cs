using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SweatyChair
{
	public static class SceneUtils
	{

		public static Scene[] GetAllActiveScenes()
		{
			Scene[] returnArray = new Scene[SceneManager.sceneCount];
			for (int i = 0; i < SceneManager.sceneCount; i++) {
				returnArray[i] = SceneManager.GetSceneAt(i);
			}
			return returnArray;
		}

		public static bool IsSceneLoaded(string sceneName)
		{
			for (int i = 0; i < SceneManager.sceneCount; i++) {
				if (SceneManager.GetSceneAt(i).name == sceneName) { return true; }
			}
			return false;
		}

		public static bool IsSceneLoaded(int buildIndex)
		{
			for (int i = 0; i < SceneManager.sceneCount; i++) {
				if (SceneManager.GetSceneAt(i).buildIndex == buildIndex) { return true; }
			}
			return false;
		}

	}
}
