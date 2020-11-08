using UnityEngine;
using UnityEngine.UI;

namespace SweatyChair
{
	
	/// <summary>
	/// Works like TutorialShowPanel, plus highlight and follow an in-game UI/world object.
	/// </summary>
    public class TutorialClickButtonTask : TutorialHighlightTask
	{

		protected Button _targetButton;

        private bool _isHandAnimationPlaying = false;

		public override bool Init()
		{
			bool tmp = base.Init();

			if (!tmp)
				return false;

			_targetButton = targetTF.GetComponent<Button>();
			if (_targetButton == null) {
				Debug.LogErrorFormat("{0}:{1}:Init - Target Button not found, targetPath={2}", name, GetType(), targetPath);
				return false;
			}

			return true;
		}

		protected override void SetupTarget()
		{
			base.SetupTarget();

			Button addCompletCallbackButton = _targetButton;
			if (putCloneToTutorialPanelInstead)
				addCompletCallbackButton = _targetCloneGO?.GetComponent<Button>();
			if (addCompletCallbackButton != null && addCompletCallbackButton.interactable)
				addCompletCallbackButton.onClick.AddListener(DoComplete);
		}

		protected override void ResetTarget()
		{
			base.ResetTarget();

			Button addCompletCallbackButton = _targetButton;
			if (putCloneToTutorialPanelInstead)
				addCompletCallbackButton = _targetCloneGO?.GetComponent<Button>();
			if (addCompletCallbackButton != null && addCompletCallbackButton.interactable)
				addCompletCallbackButton.onClick.RemoveListener(DoComplete);
		}

        protected override void RepositionHand()
        {
            base.RepositionHand();

            // Avoid animation play callback every frame.
            if (!_isHandAnimationPlaying) {
                _isHandAnimationPlaying = true;
                tutorialPanel.SetHandAnimation("HandClick"); // Make sure hand is playing click animation, e.g. after tutoial drag executed
            }
        }
    }

}