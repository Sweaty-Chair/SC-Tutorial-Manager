using UnityEngine;
using UnityEditor;
using System.Collections;
using SweatyChair;

[CustomEditor(typeof(TutorialInstantiateTask))]
public class TutorialInstantiateTaskEditor : TutorialTaskEditor
{

    private TutorialInstantiateTask _tit {
		get { return target as TutorialInstantiateTask; }
	}

	protected override void OnPreviewSettingGUI()
	{
		GUILayout.Label("Object", EditorStyles.boldLabel);

		GameObject prefab = EditorGUILayout.ObjectField(new GUIContent("Prefab", "The prefab to be instantiate."), _tit.prefab, typeof(GameObject), false) as GameObject;
		if (prefab != _tit.prefab) {
			Undo.RegisterUndo(_tit, "Reassign Prefab");
			_tit.prefab = prefab;
		}

		if (_tit.prefab != null) {
			Transform parentTF = TransformUtils.GetTransformByPath(_tit.parentPath);
			string parentPath = TransformUtils.ToPath(EditorGUILayout.ObjectField(new GUIContent("Parent in Scene", "The parent transform in current scene, this will be converted and saved as path string."), parentTF, typeof(Transform), true) as Transform);
			if (!string.IsNullOrEmpty(_tit.parentPath) && parentTF == null) { // Set but parentTF not found, this may be because in wrong scene
				GUI.contentColor = Color.red;
				string customParentPath = EditorGUILayout.TextField(new GUIContent("Not Found!", "The target transform converted from path string is not found in current scene, please drag again or mannually assign the path for dynamically-loaded prefab."), _tit.parentPath);
				if (customParentPath != _tit.parentPath) {
					Undo.RegisterUndo(_tit, "Reassign Custom Parent Path");
					_tit.parentPath = customParentPath;
				}
				GUI.contentColor = Color.white;
			}
			if (!string.IsNullOrEmpty(parentPath) && parentPath != _tit.parentPath) {
				Undo.RegisterUndo(_tit, "Reassign Parent Path");
				_tit.parentPath = parentPath;
			}

			Vector3 localPosition = EditorGUILayout.Vector3Field(new GUIContent("Local Position", "Local position of the instantiated prefab."), _tit.localPosition);
			if (localPosition != _tit.localPosition) {
				Undo.RegisterUndo(_tit, "Reassign Local Position");
				_tit.localPosition = localPosition;
				UpdatePreview();
			}

			Vector3 localRotation = EditorGUILayout.Vector3Field(new GUIContent("Local Rotation", "Local rotation of the instantiated prefab."), _tit.localRotation);

			if (localRotation != _tit.localRotation) {
				Undo.RegisterUndo(_tit, "Reassign Local Rotation");
				_tit.localRotation = localRotation;
				UpdatePreview();
			}
		}
	}

	protected override void OnOtherSettingGUI()
	{
		float completeWaitSeconds = EditorGUILayout.FloatField(new GUIContent("Complete Wait Seconds", "Added waiting seconds before this tutorial step complete."),_tit.completeWaitSeconds);
		if (completeWaitSeconds != _tit.completeWaitSeconds) {
			Undo.RegisterUndo(_tit, "Reassign Complete Wait Seconds");
			_tit.completeWaitSeconds = completeWaitSeconds;
		}

		bool destroyOnComplete = EditorGUILayout.Toggle(new GUIContent("Destroy On Complete", "Destroy the instantiated prefab after this tutorial step complete."), _tit.destroyOnComplete);
		if (destroyOnComplete != _tit.destroyOnComplete) {
			Undo.RegisterUndo(_tit, "Reassign Destroy On Complete");
			_tit.destroyOnComplete = destroyOnComplete;
		}
	}

	#region Preview

	protected override bool isPreviewShown {
		get { return _tit.objectGO != null; }
	}

	protected override bool canShowPreview {
		get { return _tit.prefab != null; }
	}

	protected override string cannotShowPreviewWarning {
		get { return "Assign prefab to enable Preview"; }
	}

	protected override void OnRemovePreviewClick()
	{
		_tit.DestroyObject();
	}

	protected override void OnAddPreviewClick()
	{
		_tit.Init();
		_tit.DoInstantiate();
		UpdatePreview();
	}

	protected override void UpdatePreview()
	{
		if (_tit.objectGO != null)
			_tit.SetObjectTransform();
	}

	#endregion

}