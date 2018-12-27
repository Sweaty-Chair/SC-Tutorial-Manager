using UnityEngine;
using System.Collections;

namespace SweatyChair
{
	
	/// <summary>
	/// Display an UI when trigger.
	/// </summary>
	public class TutorialShowTargetPanelTask : TutorialTask
	{

		// UI target path in scene
		public string targetPath;

		// Traget UIBase obtained from targetPath
		public Panel targetPanel;

		// Complete Settings
		public bool hideOnComplete = false;

		public override bool Init()
		{
			if (!string.IsNullOrEmpty(targetPath)) {
				Transform targetTF = TransformUtils.GetTransformByPath(targetPath);
				if (targetTF == null) {
					Debug.LogError(string.Format("{0}:{1}:Init - Can't find target Panel with targetPath={2}", name, GetType(), targetPath));
					return false;
				}
				targetPanel = targetTF.GetComponent<Panel>();
				if (targetPanel == null) {
					Debug.LogError(string.Format("{0}:{1}:Init - Target doesn't contain an Panel component, targetPath={2}", name, GetType(), targetPath));
					return false;
				}
			}

			return base.Init();
		}

		public override bool DoStart()
		{
			if (!base.DoStart())
				return false;

			targetPanel.Show();

			return true;
		}

		public override void Reset()
		{
			if (targetPanel == null)
				return;
			if (hideOnComplete)
				targetPanel.Hide();
			targetPanel = null; // Unset it
		}

	}

}