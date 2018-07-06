using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

namespace SweatyChair
{

    [System.Serializable]
    public class TutorialManager : PersistentSingleton<TutorialManager>
    {

        public static event UnityAction coreTutorialsChangedEvent;
        public static event UnityAction<Tutorial> tutorialCompletedEvent;

        public static Tutorial currentTutorial;

        public static bool isRunningATutorial
        {
            get { return currentTutorial != null; }
        }

        public List<Tutorial> tutorials = new List<Tutorial>();

        public int skipCheckingStateMask = 0;
        public bool debugMode = false;

        public static bool isFirstTutorialCompleted
        {
            get { return s_InstanceExists && s_Instance.tutorials[0].isCompleted; }
        }

        public static bool areAllTutorialsCompleted
        {
            get
            {
                if (s_Instance == null)
                    return true;
                foreach (Tutorial t in s_Instance.tutorials)
                {
                    if (!t.isCompleted)
                        return false;
                }
                return true;
            }
        }

        public static bool s_DebugMode
        {
            get { return s_InstanceExists && s_Instance.debugMode; }
        }

        #region Core Tutorials

        public static bool areCoreTutorialsCompleted
        {
            get
            {
                if (s_Instance == null)
                    return true;
                foreach (Tutorial t in s_Instance.tutorials)
                {
                    if (!t.isCore)
                        continue;
                    if (!t.isCompleted)
                        return false;
                }
                return true;
            }
        }

        public static int totalCoreTutorials
        {
            get
            {
                if (s_Instance == null)
                    return 0;
                int i = 0;
                foreach (Tutorial t in s_Instance.tutorials)
                {
                    if (!t.isCore)
                        continue;
                    i++;
                }
                return i;
            }
        }

        public static int totalCoreTutorialsCompleted
        {
            get
            {
                if (s_Instance == null)
                    return 0;
                int i = 0;
                foreach (Tutorial t in s_Instance.tutorials)
                {
                    if (!t.isCore)
                        continue;
                    if (!t.isCompleted)
                        continue;
                    i++;
                }
                return i;
            }
        }

        #endregion

        void OnEnable()
        {
#if GAMESPARKS
            GameSparksManager.accountDetailsSucceedEvent += (response) => {
                // Hack: dont sync when first tutorial not yet completed, so that the first tutorial can always be triggered even for returned players
                if (isFirstTutorialCompleted)
                UpdateTutorialIds(response.ScriptData.GetIntList("tutorialIds"));
            };
#elif GAMESAVE
            GameSave.gameSaveUpdatedEvent += CheckSkipTutorialsForOldPlayers;
            CheckSkipTutorialsForOldPlayers();
#endif
        }

        void Start()
        {
            InvokeRepeating("CheckTutorialTriggers", 1, 1);
        }

#if UNITY_EDITOR

        // Debug only
        void Update()
        {
            if (Input.GetKeyUp(KeyCode.V)) {
                TutorialPanel.DestroyInstance();
                CompleteCurrentTutorial();
            }
        }

#endif

#if GAMESPARKS

        public static void UpdateTutorialIds(List<int> tutorialIds)
        {
            if (tutorialIds != null) {
                foreach (int id in tutorialIds) {
                    Tutorial t = GetTutorial(id);
//                  if (t != null)
//                      t.SetCompletedLocal();
                }
//              OnCoreTutorialsChanged();
            }
        }

        // Backward compatiable
        public static string GetCompletedTutorialsJson()
        {
            List<int> completedTutorialIds = new List<int>();
            if (s_Instance != null) {
                foreach (Tutorial t in s_Instance.tutorials) {
                    if (t.isCompleted)
                        completedTutorialIds.Add(t.id);
                }
            }
            Hashtable ht = new Hashtable { { "completed", completedTutorialIds.ToArray() } };
            return MiniJSON.jsonEncode(ht);
        }

#endif

        private void CheckSkipTutorialsForOldPlayers()
        {
            if (areAllTutorialsCompleted)
                return;
            foreach (Tutorial t in tutorials)
                t.CheckCompleteForReturnPlayer();
            if (coreTutorialsChangedEvent != null)
                coreTutorialsChangedEvent();
        }

        // Check every 1 second
        private void CheckTutorialTriggers()
        {
            if (currentTutorial != null)
                return;

            if (StateManager.Compare(skipCheckingStateMask)) // Do not check these states to save performance
                return;

            if (areAllTutorialsCompleted)
            { // No more tutorial, just destroy myself
                Destroy(gameObject);
                return;
            }

            foreach (Tutorial t in tutorials)
            {
                if (t.isCompleted)
                {
                    if (t.validator != null && t.resetOnComplete) // Destroy the validator to avoid validator logics keep running
                        Destroy(t.validator);
                    continue;
                }
                if (t.isValidated)
                {
                    currentTutorial = t;
                    break;
                }
            }

            StartCurrentTutorial();
        }


        private IEnumerator LoadCurrentTutorialSceneCoroutine()
        {
            if (currentTutorial == null)
                yield break;

            int currentSceneBuildIndex = SceneManager.GetActiveScene().buildIndex;

            AsyncOperation async = SceneManager.LoadSceneAsync(currentTutorial.sceneName);

            while (!async.isDone)
                yield return null;

            currentTutorial.StartTutorial();

            Scene scene = SceneManager.GetSceneByName(currentTutorial.sceneName);
            SceneManager.SetActiveScene(scene);

            async = SceneManager.UnloadSceneAsync(currentSceneBuildIndex);
            while (!async.isDone)
                yield return null;
        }

        public static void StartTutorial(int id)
        {
            if (areAllTutorialsCompleted) // Either all tutorials has been completed or never instantiated
                return;
            Tutorial t = GetTutorial(id);
            if (t != null && !t.isCompleted)
                currentTutorial = t;
            StartCurrentTutorial();
        }

        private static void StartCurrentTutorial() // Public for debug
        {
            if (currentTutorial != null)
            {
                if (string.IsNullOrEmpty(currentTutorial.sceneName) || SceneManager.GetActiveScene().name == currentTutorial.sceneName) // No scene need to load
                    currentTutorial.StartTutorial();
                else
                    s_Instance.StartCoroutine(s_Instance.LoadCurrentTutorialSceneCoroutine());
            }
        }

        /// <summary>
        /// Starts the tutorial task by index on the current tutorial step of the current tutorial.
        /// </summary>
        /// <param name="taskIndex">index of tutorial instance task</param>
        public static void StartCurrentTutorialTask(int taskIndex)
        {
            if (currentTutorial != null)
                currentTutorial.StartTask(taskIndex);
            else
                Debug.LogErrorFormat("TutorialManager:StartCurrentTutorialStep - Could not start task {0}, currentTutorial=null", taskIndex);
        }

        /// <summary>
        /// Completes the tutorial task by index on the current tutorial step of the current tutorial.
        /// </summary>
        /// <param name="taskIndex">index of tutorial instance task</param>
        public static void CompleteCurrentTutorialTask(int taskIndex)
        {
            if (currentTutorial != null)
                currentTutorial.CompleteTask(taskIndex);
            else
                Debug.LogErrorFormat("TutorialManager::CompleteCurrentTutorialTask - Could not complete task {0}, currentTutorial=null", taskIndex);
        }

        /// <summary>
        /// Completes the current tutorial instance.
        /// </summary>
        public static void CompleteCurrentTutorialStep()
        {
            CompleteCurrentTutorialSteps(1);
        }

        /// <summary>
        /// Completes a number of current tutorial instances.
        /// </summary>
        /// <param name="stepIndex">count of tutorial instances</param>
        public static void CompleteCurrentTutorialSteps(int count)
        {
            if (currentTutorial != null)
            {
                currentTutorial.CompleteSteps(count);
            }
            else
            {
                // Tutorial instance may be dragged out for debug directly in Editor mode, we try to find it here
                TutorialStep ts = GameObject.FindObjectOfType<TutorialStep>();
                if (ts == null)
                    Debug.LogError("TutorialManager::CompleteCurrentTutorialSteps - Failed, currentTutorial=null");
                else
                    ts.Complete();
            }
        }

        public static Tutorial GetCoreTutorialByIndex(int index)
        {
            if (!s_InstanceExists)
            {
                Debug.Log("TutorialManager:GetTutorial - s_Instance=null");
                return null;
            }

            List<Tutorial> coreTutorials = new List<Tutorial>();
            foreach (Tutorial t in s_Instance.tutorials)
            {
                if (t.isCore)
                    coreTutorials.Add(t);
            }

            if (index < 0)
                index += coreTutorials.Count;

            if (index < 0 || index >= coreTutorials.Count)
            {
                Debug.LogFormat("TutorialManager:GetTutorial - Invalid index={0}", index);
                return null;
            }

            return coreTutorials[index];
        }

        public static Tutorial GetTutorial(int id)
        {
            if (!s_InstanceExists)
            {
                Debug.Log("TutorialManager:GetTutorial - s_Instance=null");
                return null;
            }
            foreach (Tutorial t in s_Instance.tutorials)
            {
                if (t.id == id)
                    return t;
            }
            Debug.LogFormat("TutorialManager:GetTutorial - Invalid id={0}", id);
            return null;
        }

        public static Tutorial GetTutorial(string prefabName)
        {
            if (!s_InstanceExists)
            {
                Debug.Log("TutorialManager:GetTutorial - s_Instance=null");
                return null;
            }
            foreach (Tutorial t in s_Instance.tutorials)
            {
                if (t.prefab.name == prefabName)
                    return t;
            }
            Debug.LogFormat("TutorialManager:GetTutorial - Invalid prefabName={0}", prefabName);
            return null;
        }

        public static Tutorial GetTutorial(TutorialValidator validator)
        {
            if (!s_InstanceExists)
            {
                Debug.Log("TutorialManager:GetTutorial - s_Instance=null");
                return null;
            }
            foreach (Tutorial t in s_Instance.tutorials)
            {
                if (t.validator == validator)
                    return t;
            }
            Debug.LogFormat("TutorialManager:GetTutorial - Invalid validator={0}", validator);
            return null;
        }

        public static void AbortCurrentTutorial()
        {
            if (!s_InstanceExists)
                return;

            if (currentTutorial == null)
            {
                Debug.LogError("TutorialManager:EndCurrentTutorial - currentTutorial=null, ignore this if debuuging in Editor mode");
                return;
            }

            CompleteCurrentTutorial();
            //          TutorialHintController.DestroyAll();
            TutorialPanel.DestroyInstance();
        }

        public static void CompleteCurrentTutorial(bool doTutorialComplete = true)
        {
            if (!s_InstanceExists)
                return;

            if (currentTutorial == null)
            {
                Debug.LogError("TutorialManager:CompleteCurrentTutorial - currentTutorial=null, ignore this if debuuging in Editor mode");
                return;
            }

            if (currentTutorial.validator != null && doTutorialComplete)
                currentTutorial.validator.OnTutorialComplete();

            if (currentTutorial.resetOnComplete)
                currentTutorial.Reset();

            if (!s_DebugMode)
                currentTutorial.CompleteTutorial();

            if (tutorialCompletedEvent != null)
                tutorialCompletedEvent(currentTutorial);

            AnalyticsTutorialComplete(currentTutorial.id);
            if (areAllTutorialsCompleted)
                AnalyticsTutorialAllComplete();

            currentTutorial = null;
        }

        public static void CompleteTutorial(int id)
        {
            Tutorial tutorial = GetTutorial(id);
            if (tutorial != null)
                tutorial.isCompleted = true;
        }

        public static void CompleteTutorial(TutorialValidator validator)
        {
            Tutorial tutorial = GetTutorial(validator);
            if (tutorial != null)
                tutorial.isCompleted = true;
        }

        public static bool IsTutorialCompleted(int id)
        {
            if (areAllTutorialsCompleted)
                return true;

            return GetTutorial(id).isCompleted;
        }

        public static bool IsTutorialRunning(int id)
        {
            return currentTutorial != null && currentTutorial.id == id && currentTutorial.isRunning;
        }

        private static bool IsTutorialRunning(Tutorial tutorial)
        {
            return currentTutorial == tutorial && currentTutorial.isRunning;
        }

        public static void SetCurrentTutorialStep(TutorialStep tutorialStep)
        {
            if (currentTutorial == null)
            {
                Debug.LogError("TutorialManager:SetCurrentTutorialStep - currentTutorial=null, ignore this if debuuging in Editor mode");
                return;
            }
            currentTutorial.SetCurrentStep(tutorialStep);
        }

        public static List<TutorialValidator> GetAllValidators()
        {
            List<TutorialValidator> tmp = new List<TutorialValidator>();
            if (s_Instance != null)
            {
                foreach (Tutorial t in s_Instance.tutorials)
                {
                    if (t != null && t.validator != null)
                        tmp.Add(t.validator);
                }
            }
            return tmp;
        }

        public static void CompleteManualSteps()
        {
            if (currentTutorial != null && currentTutorial.isRunning)
            {
                CompleteManualSteps(currentTutorial.currentStep);
                return;
            }

#if UNITY_EDITOR
            // For development only when TutorialStep is drag to edit without using TutorialManager and currentTutorial=null
            TutorialStep ts = FindObjectOfType<TutorialStep>();
            if (ts != null)
                CompleteManualSteps(ts);
#endif
        }

        private static void CompleteManualSteps(TutorialStep ts)
        {
            foreach (TutorialTask tt in ts.tasks)
            {
                if (tt.completeTrigger == TutoriaCompletelTrigger.Manual)
                    tt.DoComplete();
            }
        }

        public static void FireCompleteTrigger(TutoriaCompletelTrigger trigger)
        {
            if (currentTutorial != null && currentTutorial.isRunning)
            {
                FireCompleteTrigger(currentTutorial.currentStep, trigger);
                return;
            }

#if UNITY_EDITOR
            // For development only when TutorialStep is drag to edit without using TutorialManager and currentTutorial=null
            TutorialStep ts = FindObjectOfType<TutorialStep>();
            if (ts != null)
                FireCompleteTrigger(ts, trigger);
#endif
        }

        private static void FireCompleteTrigger(TutorialStep ts, TutoriaCompletelTrigger trigger)
        {
            if (ts != null)
            {
                foreach (TutorialTask tt in ts.tasks)
                    tt.ReceiveCompleteTrigger(trigger);
            }
        }

        // Analytics, add other analytics if needed

        public static void AnalyticsTutorialStart(int tutorialId)
        {
            //Debug.LogFormat("GameAnalytics:TutorialStart({0})", tutorialId);
            UnityEngine.Analytics.AnalyticsEvent.TutorialStart(tutorialId.ToString());
        }

        public static void AnalyticsTutorialComplete(int tutorialId)
        {
            //Debug.LogFormat("GameAnalytics:TutorialComplete({0})", tutorialId);
            UnityEngine.Analytics.AnalyticsEvent.TutorialComplete(tutorialId.ToString());
        }

        public static void AnalyticsTutorialStep(int tutorialId, int stepIndex)
        {
            //Debug.LogFormat("GameAnalytics:TutorialStep({0},{1})", tutorialId, stepIndex);
            UnityEngine.Analytics.AnalyticsEvent.TutorialStep(stepIndex, tutorialId.ToString());
        }

        public static void AnalyticsTutorialAllComplete()
        {
        }

    }

}