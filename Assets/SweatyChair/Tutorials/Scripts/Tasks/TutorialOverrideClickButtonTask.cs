using UnityEngine.UI;

namespace SweatyChair
{

	/// <summary>
	/// Works like <see cref="TutorialClickButtonTask"/> however you can override the button functionality with a new function. Useful for displaying different prompts.
	/// </summary>
	public class TutorialOverrideClickButtonTask : TutorialClickButtonTask
	{

		#region Variables

		public Button.ButtonClickedEvent onButtonClickOverride = new Button.ButtonClickedEvent();

		private Button.ButtonClickedEvent _backupButtonClickEvent;

		#endregion

		#region Setup / Reset Target

		protected override void SetupTarget()
		{
			// Backup then clear our old target button effects
			_backupButtonClickEvent = _targetButton.onClick;
			_targetButton.onClick = onButtonClickOverride;

			// Setup
			base.SetupTarget();
		}

		protected override void ResetTarget()
		{
			if (_targetButton != null) {
				// Reset our Click event back to our backup
				_targetButton.onClick = _backupButtonClickEvent;
				_backupButtonClickEvent = new Button.ButtonClickedEvent();

			}
			// Reset
			base.ResetTarget();
		}

		#endregion

	}
}
