using UnityEngine;
using SweatyChair.UI;
using SweatyChair.StateManagement;

namespace SweatyChair
{

	/// <summary>
	/// An initializer to initialze the Tutorial module. Put this into the first scene of the game.
	/// </summary>
	[System.Serializable]
	public class TutorialInitializer : PersistentSingleton<TutorialInitializer>
	{

		private void OnEnable()
		{
#if GAMESPARKS
            GameSparksManager.accountDetailsSucceedEvent += (response) => {
                // Hack: dont sync when first tutorial not yet completed, so that the first tutorial can always be triggered even for returned players
                if (isFirstTutorialCompleted)
					UpdateTutorialIds(response.ScriptData.GetIntList("tutorialIds"));
            };
#endif
		}

		private void Start()
		{
			InvokeRepeating("CheckTutorialTriggers", 1, 1);
		}

#if UNITY_EDITOR

		// Debug only
		private void Update()
		{
			if (Input.GetKeyUp(KeyCode.V)) {
				TutorialPanel.DestroyInstance();
				TutorialManager.CompleteCurrentTutorial();
			}
		}

#endif

#if GAMESPARKS

        public static void UpdateTutorialIds(List<int> tutorialIds)
        {
            if (tutorialIds != null) {
                foreach (int id in tutorialIds)
                    Tutorial t = GetTutorial(id);
            }
        }

#endif

		// Check every 1 second
		private void CheckTutorialTriggers()
		{
			if (TutorialManager.currentTutorial != null)
				return;

			if (StateManager.Compare((State)TutorialManager.settings.skipCheckingStateMask)) // Do not check these states to save performance
				return;

			//if (StateManager.Compare(TutorialManager.settings.skipCheckingStateMask)) // Do not check these states to save performance
			//	return;

			if (TutorialManager.areAllTutorialsCompleted) { // No more tutorial, just destroy myself
				Destroy(gameObject);
				return;
			}

			foreach (Tutorial t in TutorialManager.tutorials) {
				if (t.isCompleted)
					continue;
				if (t.isValidated) {
					TutorialManager.currentTutorial = t;
					break;
				}
			}

			if (TutorialManager.isTutorialRunning)
				GameAnalytics.FirstInteraction(); // Log a first interaction event here before any tutorial start
			TutorialManager.StartCurrentTutorial();
		}

	}

}