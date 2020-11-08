using UnityEngine;

namespace SweatyChair
{

	/// <summary>
	/// A tutorial task to instantiate a prefab.
	/// </summary>
    public class TutorialInstantiateTask : TutorialTask
	{

		// Prefab settings
		public GameObject prefab;
		public string parentPath;
		public Vector3 localPosition, localRotation;

		// Complete Settings
		public float completeWaitSeconds = 0;
		public bool destroyOnComplete = false;

		public GameObject objectGO;

		public override bool Init()
		{
			if (prefab == null) {
				Debug.LogErrorFormat("{0}:{1}:Init - Prefab not set", name, GetType());
				return false;
			}

			if (!string.IsNullOrEmpty(parentPath) && TransformUtils.GetTransformByPath(parentPath) == null) {
				Debug.LogErrorFormat("{0}:{1}:Init - Parent not found, parentPath={2}", name, GetType(), parentPath);
				return false;
			}

			return true;
		}

		public override bool DoStart()
		{
			if (!base.DoStart())
				return false;

			DoInstantiate();

			return true;
		}

		public void DoInstantiate()
		{
			objectGO = GameObjectUtils.AddChildToPath(parentPath, prefab);
			SetObjectTransform();
		}

		public void SetObjectTransform() // Public for Editor
		{
			Transform tf = objectGO.transform;
			tf.localPosition = localPosition;
			tf.localRotation = Quaternion.Euler(localRotation);
		}

		public override void Reset()
		{
			if (objectGO == null)
				return;
			if (destroyOnComplete)
				DestroyObject();
		}

		public void DestroyObject()
		{
			if (Application.isPlaying)
				Destroy(objectGO);
			else
				DestroyImmediate(objectGO);
		}

	}

}