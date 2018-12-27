using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace SweatyChair
{
	
	/// <summary>
	/// Display a character dialogue when trigger.
	/// </summary>
    public class TutorialDialogueTask : TutorialTask
	{
	
		[System.Serializable]
		private struct Dialogue
		{
			public string text;
			public string animNameToPlay;
		}

		public static event UnityAction<string> dialogueEvent;

		[SerializeField] private List<Dialogue> _dialogueList = new List<Dialogue>();

		public bool doNotResetPanelOnComplete = false;
		// Value of 0 and below will not autoclose the dialogue
		[SerializeField] private float _autoCloseDelay = 0;

		protected TutorialPanel _tutorialPanel;
		private IEnumerator _autoCloseIEnumerator;
		private float _lastClickTime;
		private int _textIndex = -1;

		public override bool Init()
		{
			_tutorialPanel = TutorialPanel.current;
			if (_tutorialPanel == null) {
				Debug.LogError("TutorialDialogue:Init - Can't get UITutorialPanel instance");
				return false;
			}

			_tutorialPanel.Reset(false);
			_tutorialPanel.Show();

			//ShowNextText();
			return true;
		}

		private void ShowNextText()
		{
			_textIndex++;
			int dialogueLength = _dialogueList.Count;
			if (_textIndex >= dialogueLength) {
				DoComplete();
				return;
			}

			Dialogue dialogue = _dialogueList[_textIndex];

			_tutorialPanel.ToggleBackgroundMask(true);
			_tutorialPanel.ToggleCharacter(true, dialogue.text, dialogue.animNameToPlay);

			if (dialogueEvent != null)
				dialogueEvent(dialogue.text);

			if (_autoCloseDelay > 0 && _textIndex >= dialogueLength - 1) {
				if (_autoCloseIEnumerator != null)
					StopCoroutine(_autoCloseIEnumerator);

				_autoCloseIEnumerator = AutoCloseCoroutine();
				StartCoroutine(_autoCloseIEnumerator);
			}
		}

		private IEnumerator AutoCloseCoroutine()
		{
			yield return new WaitForSecondsRealtime(_autoCloseDelay);
			DoComplete();
        }

        public override bool DoStart()
        {
            if (!base.DoStart())
                return false;

            ShowNextText();

            return true;
        }

        protected override void DoUpdate()
		{
			if (Input.GetMouseButtonDown(0)) {
				if (Time.unscaledTime < _lastClickTime + minEnabledSeconds) // Avoid clicking too fast
				return;
				_lastClickTime = Time.unscaledTime;
				ShowNextText();
			}
		}

		public override void DoComplete()
		{
			StopAllCoroutines();
			base.DoComplete();
		}

		public override void Reset()
		{
			if (_tutorialPanel == null)
				return;
			_tutorialPanel.ToggleCharacter(false);
			if (!doNotResetPanelOnComplete) {
				_tutorialPanel.Reset();
				_tutorialPanel.Hide();
			}
		}

	}

}