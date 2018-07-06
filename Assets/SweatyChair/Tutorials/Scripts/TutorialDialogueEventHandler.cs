using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SweatyChair
{
	
	public class TutorialDialogueEventHandler : MonoBehaviour
	{
	
		[System.Serializable]
		public class TutorialDialogueEvent
		{
			[SerializeField] private string _triggerText;
			[SerializeField] private UnityEvent _action;

			public bool Compare(string triggerText)
			{
				return _triggerText == triggerText;
			}

			public void Invoke()
			{
				if (_action != null)
					_action.Invoke();
			}
		}

		[SerializeField] private List<TutorialDialogueEvent> tutorialDialogueEventList = new List<TutorialDialogueEvent>();

		private void Awake()
		{
			TutorialDialogueTask.dialogueEvent += DialogueCheck;
		}

		private void OnDestroy()
		{
            TutorialDialogueTask.dialogueEvent -= DialogueCheck;
		}

		private void DialogueCheck(string dialogue)
		{
			foreach (TutorialDialogueEvent dialogueEvent in tutorialDialogueEventList) {
				if (dialogueEvent.Compare(dialogue)) {
					dialogueEvent.Invoke();
					break;
				}
			}
		}

	}

}