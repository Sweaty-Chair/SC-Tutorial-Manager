using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;

namespace SweatyChair
{

    [System.Serializable]
    public class Tutorial
    {

        public enum TriggerCondition
        {
            AlwaysTrigger,
            Mannual,
            Validator
        }

        public enum SetCompleteCondition
        {
            SetCompleteOnStart,
            SetCompleteOnEnd,
            DoNotSetComplete
        }

        private const string PREFS_PREFX_TUTORIAL_COMPLETED = "Tutorial";
        public const string TUTORIAL_RESOURCE_PATH = "Tutorials/";

        public int id;
        public TriggerCondition triggerCondition = TriggerCondition.Validator;

        public int checkStateMask;
        public TutorialValidator validator;

        public string sceneName;
        public GameObject prefab;

        // Is a core tutorial, this is mainly for UI
        public bool isCore = true;

        // Should this tutorial be reset on complete, so next tutorial can be trigger
        public bool resetOnComplete = true;

        // When to set the tutorial as completed
        public SetCompleteCondition setCompleteCondition = SetCompleteCondition.SetCompleteOnEnd;

        public GameObject tutorialGO { get; private set; }

        public TutorialStep currentStep { get; private set; }

        public bool isRunning {
            get { return tutorialGO != null && currentStep != null; }
        }

        public bool isCompleted {
            get { return PlayerPrefs.GetInt(PREFS_PREFX_TUTORIAL_COMPLETED + id) != 0; }
            set {
                if (value)
                    PlayerPrefs.SetInt(PREFS_PREFX_TUTORIAL_COMPLETED + id, 1);
                else
                    PlayerPrefs.DeleteKey(PREFS_PREFX_TUTORIAL_COMPLETED + id);
            }
        }

        public bool isValidated {
            get {
                if (isCompleted) // No need to validate if already complete
                    return false;
                if (triggerCondition == TriggerCondition.Mannual)
                    return false;
                else if (triggerCondition == TriggerCondition.AlwaysTrigger)
                    return true;
                if (!StateManager.Compare(checkStateMask)) // Only validate when in check state
                    return false;
                if (validator == null) // False if no validator
                    return false;
                return validator.IsValidated();
            }
        }

        public string name { // For editor distinguish only
            get { return prefab != null ? prefab.name : (validator != null ? validator.GetType().ToString() : string.Empty); }
        }

        public void SetCurrentStep(TutorialStep ts)
        {
            currentStep = ts;
        }

        public void Reset()
        {
            if (tutorialGO != null) {
                GameObject.Destroy(tutorialGO);
                tutorialGO = null;
            }

            currentStep = null;
        }

        public void StartTutorial()
        {
            TutorialManager.AnalyticsTutorialStart(id);

            if (setCompleteCondition == SetCompleteCondition.SetCompleteOnStart) { // Set this tutorial completed when trigger?
                #if UNITY_EDITOR
                if (!TutorialManager.s_DebugMode) // Don't set completed if in debug mode in Editor
                #endif
                isCompleted = true;
            }

            if (prefab != null) {
                GameObject go = GameObjectUtils.AddChild(null, prefab);
                go.name = prefab.name;
                tutorialGO = go;
                TutorialStep.ResetStepNumber();
            }

            // Do pre-load things for each tutorial
            if (validator != null)
                validator.OnTutorialStart();
        }

        public void StartTask(int index)
        {
            if (currentStep != null)
                currentStep.StartTask(index);
            else
                Debug.LogErrorFormat("Tutorial:StartTask - Could not start task {0}, current step is null", index);
        }

        public void CompleteTask(int index)
        {
            if (currentStep != null)
                currentStep.CompleteTask(index);
            else
                Debug.LogError("Tutorial:CompleteInstanceStep - Failed, currentInstance=null");
        }

        public void CompleteStep()
        {
            if (currentStep != null)
                currentStep.Complete();
            else
                Debug.LogError("Tutorial:CompleteInstance - Failed, currentInstance=null");
        }

        public void CompleteSteps(int count)
        {
            if (count <= 0)
                return;
            if (count == 1)
                CompleteStep();
            TutorialStep ts = currentStep;
            for (int i = 0; i < count; i++) {
                TutorialStep tutorialStepToComplete = ts;
                if (tutorialStepToComplete != null) {
                    ts = tutorialStepToComplete.nextStep;
                    tutorialStepToComplete.Complete();
                } else {
                    Debug.LogErrorFormat("Tutorial:CompleteSteps - No tutorial step for index={0}", i);
                }
            }
        }

        public void CompleteTutorial()
        {
            if (setCompleteCondition == SetCompleteCondition.SetCompleteOnEnd)
                isCompleted = true;
        }

        public void CheckCompleteForReturnPlayer()
        {
            if (isCompleted) // Already completed, return
                return;
            if (validator != null) {
                if (validator.IsCompletedForReturnPlayer())
                    isCompleted = true;
            }
        }

        public override string ToString() // For debug
        {
            return string.Format("[Tutorial: id={0}, prefab={1}, tutorialGO={2}, currentStep={3}, validator={4}, isRunning={5}, isCompleted={6}, isValidated={7}]", id, prefab, tutorialGO, currentStep, validator, isRunning, isCompleted, isValidated);
        }

    }

}