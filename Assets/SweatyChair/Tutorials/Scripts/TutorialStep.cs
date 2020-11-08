using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using SweatyChair.UI;

namespace SweatyChair
{

    /// <summary>
	/// A step in a tutorial, a step should contain at least ONE tutorial task.
	/// </summary>
    public class TutorialStep : MonoBehaviour
    {

        // Step count on current tutorial instance, for game analytics
        private static int _stepNumber = 1;
        public event UnityAction completed;

        [HideInInspector] public TutorialTask[] tasks;

        [SerializeField] private TutorialStep _nextStep;
        [SerializeField] private string _sceneName;

        private int _assistantCount = 0;
        private int _completedCount = 0;

        public TutorialStep nextStep => _nextStep;

        public static void ResetStepNumber()
        {
            _stepNumber = 1;
        }

        private void Awake()
        {
            tasks = GetComponents<TutorialTask>();
            _assistantCount = tasks.Length;
            foreach (TutorialTask tt in tasks) {
                tt.completed  += OnStepComplete;
                tt.failed  += OnStepFail;
            }
        }

        private void Start()
        {
            TutorialManager.SetCurrentTutorialStep(this);
            if (!string.IsNullOrEmpty(_sceneName) && SceneManager.GetActiveScene().name != _sceneName)
                SceneManager.LoadScene(_sceneName);
        }

        private void Update()
        {
            if (Input.GetMouseButtonUp(0))
                TutorialManager.FireCompleteTrigger(TutoriaCompletelTrigger.OnClick);
            // TODO: on drag
        }

        public void Activate()
        {
            //Debug.LogFormat("{0}/{1}:TutorialStep - tutorial step started", transform.parent.name, transform.name);
            gameObject.SetActive(true);
        }

        public void OnStepComplete()
        {
            _completedCount++;
            if (_assistantCount <= _completedCount)
                Complete();
        }

        public void OnStepFail()
        {
            Complete();
        }

        public void StartTask(int index)
        {
            index = Mathf.Clamp(index, 0, index);
            if (tasks.Length > index && tasks[index] != null)
                tasks[index].DoStart();
            else
                Debug.LogErrorFormat("TutorialStep:StartTask - Task {0} does not exist", index);
        }

        public void CompleteTask(int index)
        {
            index = Mathf.Max(0, index);
            if (tasks.Length > index && tasks[index] != null)
                tasks[index].DoComplete();
            else
                Debug.LogErrorFormat("TutorialStep:CompleteTask - Task {0} does not exist", index);
        }

        public void Complete()
        {
			//Debug.LogFormat("{0}/{1}:TutorialStep - tutorial step completed", transform.parent.name, transform.name);

			foreach (TutorialTask tt in tasks)
                tt.Reset();

            if (TutorialManager.currentTutorial != null)
                TutorialManager.AnalyticsTutorialStep(TutorialManager.currentTutorial.id, _stepNumber++);

            if (_nextStep != null) {
                _nextStep.Activate();
            } else { // Whole tutorial done
                TutorialManager.CompleteCurrentTutorial();
                TutorialPanel.DestroyInstance();
            }

			completed?.Invoke();

			Destroy(gameObject);
        }

    }

}