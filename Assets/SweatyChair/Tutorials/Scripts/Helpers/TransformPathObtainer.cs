using UnityEngine;
using SweatyChair;

public class TransformPathObtainer : MonoBehaviour
{

#if UNITY_EDITOR

	[UnityEditor.MenuItem("Sweaty Chair/Utilities/Copy Path")]
	private static void CopyActiveObjectPath()
	{
		if (UnityEditor.Selection.activeObject == null || !(UnityEditor.Selection.activeObject is GameObject)) {
			Debug.LogError("Please select a object in scene first.");
		} else {
			GameObject gameObject = (GameObject)UnityEditor.Selection.activeObject;
			if (gameObject != null) {
				CopyTransformPath(gameObject.transform);
			}
		}
	}

	[UnityEditor.MenuItem("CONTEXT/Transform/Copy Path")]
	private static void CopyTransformPath(UnityEditor.MenuCommand menuCommand)
	{
		CopyTransformPath(menuCommand.context as Transform);
	}

	[UnityEditor.MenuItem("CONTEXT/GameObject/Copy Path")]
	private static void CopyGameObjectPath(UnityEditor.MenuCommand menuCommand)
	{
		CopyTransformPath((menuCommand.context as GameObject).transform);
	}

#endif

	private static void CopyTransformPath(Transform transform)
	{
		string path = transform.ToPath();
		Debug.LogFormat("Copied '{0}' to clipboard", path);
		TextEditor te = new TextEditor {
			text = path
		};
		te.SelectAll();
		te.Copy();
	}

	[ContextMenu("Copy Path")]
	private void CopyPath()
	{
		CopyTransformPath(transform);
	}

}
