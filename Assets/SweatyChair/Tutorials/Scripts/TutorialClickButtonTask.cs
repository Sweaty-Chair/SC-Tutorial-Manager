using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace SweatyChair
{
	
	/// <summary>
	/// Works like TutorialShowPanel, plus highlight and follow an in-game UI/world object.
	/// </summary>
    public class TutorialClickButtonTask : TutorialHighlightTask
	{

		private Button _targetButton;

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

		protected override void SetupBackgroundMask()
		{
			base.SetupBackgroundMask();

			if (_targetButton != null && _targetButton.interactable)
				_targetButton.onClick.AddListener(DoComplete);
		}

		public override void ResetBackgroundMask()
		{
			base.ResetBackgroundMask();

			if (!showBackgroundMask)
				return;

			if (_targetButton != null && _targetButton.interactable)
				_targetButton.onClick.RemoveListener(DoComplete);
		}

        protected override void RepositionHand()
        {
            base.RepositionHand();

            tutorialPanel.SetHandAnimation("HandClick"); // Make sure hand is playing click animation, e.g. after tutoial drag executed
        }
    }

}