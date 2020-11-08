using UnityEngine;
using UnityEditor;
using System.Collections;

namespace SweatyChair
{

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
				EditorUtility.SetDirty(_tit);
				Undo.RegisterCompleteObjectUndo(_tit, "Reassign Prefab");
				_tit.prefab = prefab;
			}

			if (_tit.prefab != null) {
				Transform parentTF = TransformUtils.GetTransformByPath(_tit.parentPath);
				string parentPath = (EditorGUILayout.ObjectField(new GUIContent("Parent in Scene", "The parent transform in current scene, this will be converted and saved as path string."), parentTF, typeof(Transform), true) as Transform).ToPath();
				if (parentTF == null && !string.IsNullOrEmpty(_tit.parentPath))
					GUI.contentColor = Color.red;
				string customTargetPath = EditorGUILayout.TextField(new GUIContent(string.IsNullOrEmpty(_tit.parentPath) ? "Enter Manually" : (parentTF == null ? "Not Found" : " "), "Manually set the target path, use this for dynamically-loaded prefab."), _tit.parentPath);
				if (customTargetPath != _tit.parentPath) {
					EditorUtility.SetDirty(_tit);
					Undo.RegisterCompleteObjectUndo(_tit, "Reassign Custom Parent Path");
					_tit.parentPath = customTargetPath;
					parentPath = customTargetPath;
				}
				if (parentTF == null && !string.IsNullOrEmpty(_tit.parentPath))
					GUI.contentColor = Color.white;
				if ((string.IsNullOrEmpty(_tit.parentPath) || parentTF != null) && parentPath != _tit.parentPath) {
					EditorUtility.SetDirty(_tit);
					Undo.RegisterCompleteObjectUndo(_tit, "Reassign Parent Path");
					_tit.parentPath = parentPath;
				}

				Vector3 localPosition = EditorGUILayout.Vector3Field(new GUIContent("Local Position", "Local position of the instantiated prefab."), _tit.localPosition);
				if (localPosition != _tit.localPosition) {
					EditorUtility.SetDirty(_tit);
					Undo.RegisterCompleteObjectUndo(_tit, "Reassign Local Position");
					_tit.localPosition = localPosition;
					UpdatePreview();
				}

				Vector3 localRotation = EditorGUILayout.Vector3Field(new GUIContent("Local Rotation", "Local rotation of the instantiated prefab."), _tit.localRotation);

				if (localRotation != _tit.localRotation) {
					EditorUtility.SetDirty(_tit);
					Undo.RegisterCompleteObjectUndo(_tit, "Reassign Local Rotation");
					_tit.localRotation = localRotation;
					UpdatePreview();
				}
			}
		}

		protected override void OnOtherSettingGUI()
		{
			float completeWaitSeconds = EditorGUILayout.FloatField(new GUIContent("Complete Wait Seconds", "Added waiting seconds before this tutorial step complete."), _tit.completeWaitSeconds);
			if (completeWaitSeconds != _tit.completeWaitSeconds) {
				EditorUtility.SetDirty(_tit);
				Undo.RegisterCompleteObjectUndo(_tit, "Reassign Complete Wait Seconds");
				_tit.completeWaitSeconds = completeWaitSeconds;
			}

			bool destroyOnComplete = EditorGUILayout.Toggle(new GUIContent("Destroy On Complete", "Destroy the instantiated prefab after this tutorial step complete."), _tit.destroyOnComplete);
			if (destroyOnComplete != _tit.destroyOnComplete) {
				EditorUtility.SetDirty(_tit);
				Undo.RegisterCompleteObjectUndo(_tit, "Reassign Destroy On Complete");
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

}