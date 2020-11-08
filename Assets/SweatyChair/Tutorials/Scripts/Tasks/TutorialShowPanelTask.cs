using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using SweatyChair.UI;

namespace SweatyChair
{

	/// <summary>
	/// Display tutorial panel with hand and text when trigger.
	/// </summary>
	public class TutorialShowPanelTask : TutorialTask
	{

		// Hand
		public bool showHand = true;
		public Vector3 handLocalPosition, handLocalRotation, handLocalScale = Vector3.one;

		// Text
		public Vector3 textLocalPosition, textLocalRotation;
		public Vector2 textSize = new Vector2(200, 100);
		public string text;
		public Color textColor = Color.black;
		public int fontSize = 60;
		public TextAnchor alignment = TextAnchor.MiddleLeft;

		protected bool showText { get { return !string.IsNullOrEmpty(text); } }

		// Background mask
		public bool showBackgroundMask = true;
		public int alpha = 168;
		public bool clickToComplete = true;
		public float timeBeforeClickable = 0f;

		// Other
		public bool doNotHidePanelOnComplete = false;

		public TutorialPanel tutorialPanel;

		public override bool Init()
		{
			tutorialPanel = TutorialPanel.current;
			if (tutorialPanel == null) {
				Debug.LogErrorFormat("{0}:{1}:Init - Can't get TutorialPanel instance", name, GetType());
				return false;
			}

			tutorialPanel.Reset(!showBackgroundMask);

			return true;
		}

		public override bool DoStart()
		{
			if (!base.DoStart())
				return false;

			SetupTutorialPanel();

			return true;
		}

		public override void DoComplete()
		{
			base.DoComplete();
			if (!doNotHidePanelOnComplete)
				tutorialPanel.Hide();
		}

		public virtual void SetupTutorialPanel()
		{
			tutorialPanel.Show();
			tutorialPanel.ToggleHand(showHand);
			if (showHand)
				tutorialPanel.SetHandTransform(handLocalPosition, handLocalRotation, handLocalScale);
			if (showText)
				tutorialPanel.SetTextTransform(textLocalPosition, textLocalRotation, textSize);
			tutorialPanel.SetText(text, textColor, fontSize, alignment);

			SetupBackgroundMask();
		}

		protected virtual void SetupBackgroundMask()
		{
			tutorialPanel.ToggleBackgroundMask(showBackgroundMask, alpha);
			if (clickToComplete) {
				Button backgroundButton = tutorialPanel.GetBackgroundButton();
				if (backgroundButton != null) {
					backgroundButton.onClick.AddListener(DoComplete);
					if (timeBeforeClickable != 0) {
						backgroundButton.interactable = false;
						IEnumerator setButtonInteractableCoroutine = SetBackgroundButtonInteractableDelayed(backgroundButton, timeBeforeClickable);
						StartCoroutine(setButtonInteractableCoroutine);
					}
				}
			}
		}

		protected IEnumerator SetBackgroundButtonInteractableDelayed(Button button, float delay)
		{
			yield return new WaitForSecondsRealtime(delay);
			button.interactable = true;
		}

		public virtual void ResetBackgroundMask()
		{
		}

		public override void Reset()
		{
			if (tutorialPanel == null)
				return;
			if (clickToComplete) {
				Button backgroundButton = tutorialPanel.GetBackgroundButton();
				if (backgroundButton != null) {
					backgroundButton.onClick.RemoveListener(DoComplete);
					if (timeBeforeClickable != 0) {
						StopAllCoroutines();
						backgroundButton.interactable = true;
					}
				}
			}
			ResetBackgroundMask();
			if (!doNotHidePanelOnComplete) {
				tutorialPanel.Reset();
				tutorialPanel.Hide();
			}
		}

		public void DestroyTutorialPanel()
		{
			StopAllCoroutines();
			DestroyImmediate(tutorialPanel.gameObject);
		}

	}

}