using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using SweatyChair.StateManagement;

namespace SweatyChair
{
    
    public enum TutoriaCompletelTrigger
    {
        Auto, // Auto complete, according to satisfy different TutorialStep class conditions, e.g. drag offset in TutorialDragStep, on click in TutorialClickButtonStep
        Manual, // Mannually complete, use TutorialManager.CompleteManualStep
        OnStart, // Immediately complete on start
        OnClick // Complete only when there's a mouse click
    }

    /// <summary>
    /// A base class attached with TutorialInstance.
    /// All TutorialSteps are registered in their TutorialInstance, which monitor them. When all TutorialSteps completed, TutorialInstance trigger the next TutorialInstance and destroy itself.
    /// </summary>
    [RequireComponent(typeof(TutorialStep))]
    public class TutorialTask : MonoBehaviour
    {

        public event UnityAction completed, failed;

        public State skipState;

        // How to complete this tutorial step
        public TutoriaCompletelTrigger completeTrigger;

        // Force complete wait seconds
        public float timeoutSeconds;
        // Complete the whole tutorial if this task timeout
        public bool completeTutorialIfTimeout;

        // Minimum seconds before the tutorial can be completed, to avoid flicker
        public float minEnabledSeconds;

        protected bool isStarted;
        protected bool isCompleted;
        protected float startedTime;

        private void Start()
        {
            if (!Init()) {
                DoFail();
                return;
            }
            DoStart();
			if (skipState != State.None && StateManager.Compare(skipState) || completeTrigger == TutoriaCompletelTrigger.OnStart)
                DoComplete();
        }

        protected virtual void Update()
        {
            if (isCompleted) // No Update if already completed
            return;
            if (isStarted)
                DoUpdate();
        }

        #region Events

        private void OnComplete()
        {
			completed?.Invoke();
		}

        private void OnFail()
        {
			failed?.Invoke();
		}

        #endregion

        // Init, public for Editor
        public virtual bool Init()
        {
            return true;
        }

        // Start to do logic
        [ContextMenu("Start")]
        public virtual bool DoStart()
        {
            if (isStarted)
                return false;
            
            isStarted = true;
            startedTime = Time.unscaledTime;

            if (timeoutSeconds > 0) // If there's timeout
                StartCoroutine(WaitAndCompleteCoroutine());

            return true;
        }

        protected virtual void DoUpdate()
        {
        }

        [ContextMenu("Complete")]
        public virtual void DoComplete()
        {
            if (isCompleted) // Avoid duplicated completed
                return;
            if (minEnabledSeconds > 0 && Time.unscaledTime < startedTime + minEnabledSeconds) {
                StartCoroutine(WaitMinEnabledSecondsAndCompleteCoroutine());
                return;
            }

          //Debug.LogFormat ("{0}:{1}:DoComplete()", name, GetType());

            isCompleted = true;
            enabled = false; // Turn off Update
            OnComplete();
        }

        private IEnumerator WaitAndCompleteCoroutine()
        {
            yield return new WaitForSecondsRealtime(timeoutSeconds);
            DoComplete();
            if (completeTutorialIfTimeout)
                TutorialManager.CompleteCurrentTutorial();
        }

        protected IEnumerator WaitMinEnabledSecondsAndCompleteCoroutine()
        {
            yield return new WaitForSecondsRealtime(Time.unscaledTime - startedTime);
            DoComplete();
        }

        protected virtual void DoFail()
        {
//          Debug.LogFormat ("{0}:{1}:DoFail()", name, GetType ());
            isCompleted = true;
            enabled = false; // Turn off Update
            OnFail();
        }

        public virtual void ReceiveCompleteTrigger(TutoriaCompletelTrigger trigger)
        {
            if (trigger == completeTrigger)
                DoComplete();
        }

        // Destory indicator objects and reset all other modifications
        public virtual void Reset()
        {
        }

        #if UNITY_EDITOR

        // An editor check if this valid, such as if target exists on path
        public virtual bool IsValidate()
        {
            bool tmp = Init();
            Reset();
            return tmp;
        }

        public virtual string GetInvalidateString()
        {
            return string.Empty;
        }

        [ContextMenu("PrintParameters")]
        protected virtual void PrintParameters()
        {
            Debug.Log("isStarted=" + isStarted);
            Debug.Log("isCompleted=" + isCompleted);
        }

        #endif

    }

}