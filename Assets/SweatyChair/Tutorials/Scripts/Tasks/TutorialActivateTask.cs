using UnityEngine;

namespace SweatyChair
{

	/// <summary>
	/// Activate a game object in current scene.
	/// </summary>
    public class TutorialActivateTask : TutorialTask
	{

		public string targetPath;

		public bool isActivate = true;
		public bool checkOnUpdate = false;

		public bool setPrevActivationOnComplete = false;

		public GameObject targetGO;

		private bool _prevActivation = false;

		public override bool Init()
		{
			if (string.IsNullOrEmpty(targetPath)) {
				Debug.LogErrorFormat("{0}:{1}:Init - Target object not set, targetPath={2}", name, GetType(), targetPath);
				return false;
			}

			Transform targetTF = TransformUtils.GetTransformByPath(targetPath);
			if (targetTF == null) {
				Debug.LogErrorFormat("{0}:{1}:Init - Target transform not found, targetPath={2}", name, GetType(), targetPath);
				return false;
			}

			targetGO = targetTF.gameObject;

			return true;
		}

		public override bool DoStart()
		{
			if (!base.DoStart())
				return false;

            if (completeTrigger == TutoriaCompletelTrigger.Auto) // Set complete on beginning
                DoComplete();

			_prevActivation = targetGO.activeSelf;
			targetGO.SetActive(isActivate);

			return true;
		}

		protected override void DoUpdate()
		{
			if (checkOnUpdate)
				targetGO.SetActive(isActivate);
		}

		public override void Reset()
		{
			if (setPrevActivationOnComplete)
				targetGO.SetActive(_prevActivation);
		}

		#if UNITY_EDITOR

		public override bool IsValidate()
		{
			if (!string.IsNullOrEmpty(targetPath) && TransformUtils.GetTransformByPath(targetPath) == null)
				return false;
			return true;
		}

		#endif
	
	}

}