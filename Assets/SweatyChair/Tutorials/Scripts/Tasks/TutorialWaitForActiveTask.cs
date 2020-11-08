using System.Collections;
using UnityEngine;

namespace SweatyChair
{

	/// <summary>
	/// Wait for a game object being active in current scene.
	/// </summary>
    public class TutorialWaitForActiveTask : TutorialTask
	{

		public string targetPath;

		public bool isActive = true;

		public GameObject targetGO;

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

			StartCoroutine(CheckTargetActiveCoroutine());

			return true;
		}

		private IEnumerator CheckTargetActiveCoroutine()
		{
			yield return new WaitForSeconds(0.2f);
			if (targetGO.activeInHierarchy == isActive)
				DoComplete();

			StartCoroutine(CheckTargetActiveCoroutine());
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