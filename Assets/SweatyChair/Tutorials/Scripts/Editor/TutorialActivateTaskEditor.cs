using UnityEngine;
using UnityEditor;

namespace SweatyChair
{

	[CustomEditor(typeof(TutorialActivateTask))]
	public class TutorialActivateTaskEditor : TutorialTaskEditor
	{

		private TutorialActivateTask _tat => target as TutorialActivateTask;

		protected override void OnPreviewSettingGUI()
		{
			Transform targetTF = TransformUtils.GetTransformByPath(_tat.targetPath);
			string targetPath = (EditorGUILayout.ObjectField(new GUIContent("Target in Scene", "The target transform in current scene, this will be converted and saved as path string."), targetTF, typeof(Transform), true) as Transform).ToPath();
			if (targetTF == null && !string.IsNullOrEmpty(_tat.targetPath))
				GUI.contentColor = Color.red;
			string customTargetPath = EditorGUILayout.TextField(new GUIContent(string.IsNullOrEmpty(_tat.targetPath) ? "Enter Manually" : (targetTF == null ? "Not Found" : " "), "Manually set the target path, use this for dynamically-loaded prefab."), _tat.targetPath);
			if (customTargetPath != _tat.targetPath) {
				EditorUtility.SetDirty(_tat);
				Undo.RegisterCompleteObjectUndo(_tat, "Reassign Custom Target Path");
				_tat.targetPath = customTargetPath;
				targetPath = customTargetPath;
			}
			if (targetTF == null && !string.IsNullOrEmpty(_tat.targetPath))
				GUI.contentColor = Color.white;
			if ((string.IsNullOrEmpty(_tat.targetPath) || targetTF != null) && targetPath != _tat.targetPath) {
				EditorUtility.SetDirty(_tat);
				Undo.RegisterCompleteObjectUndo(_tat, "Reassign Target Path");
				_tat.targetPath = targetPath;
			}

			bool isActivate = EditorGUILayout.Toggle(new GUIContent("Is Activate", "Set the target GameObject to be active or not."), _tat.isActivate);
			if (isActivate != _tat.isActivate) {
				EditorUtility.SetDirty(_tat);
				Undo.RegisterCompleteObjectUndo(_tat, "Reassign Is Activate");
				_tat.isActivate = isActivate;
			}

			bool checkOnUpdate = EditorGUILayout.Toggle(new GUIContent("Check On Update", "Set the active status in Update, set this for if target GameObject's active status also controlled by another script."), _tat.checkOnUpdate);
			if (checkOnUpdate != _tat.checkOnUpdate) {
				EditorUtility.SetDirty(_tat);
				Undo.RegisterCompleteObjectUndo(_tat, "Reassign Check On Update");
				_tat.checkOnUpdate = checkOnUpdate;
			}
		}

		protected override void OnOtherSettingGUI()
		{
			bool deactivateOnComplete = EditorGUILayout.Toggle(new GUIContent("Set Prev Activation On Complete", "Set the active status back after this tutorial step complete."), _tat.setPrevActivationOnComplete);
			if (deactivateOnComplete != _tat.setPrevActivationOnComplete) {
				EditorUtility.SetDirty(_tat);
				Undo.RegisterCompleteObjectUndo(_tat, "Reassign Set Prev Activation On Complete");
				_tat.setPrevActivationOnComplete = deactivateOnComplete;
			}
		}

		#region Preview

		protected override bool isPreviewShown => _tat.targetGO != null && _tat.targetGO.activeSelf == _tat.isActivate;

		protected override bool canShowPreview => TransformUtils.GetTransformByPath(_tat.targetPath) != null;

		protected override string cannotShowPreviewWarning => "Assign target to enable Preview";

		protected override void OnRemovePreviewClick()
		{
			_tat.targetGO.SetActive(false);
		}

		protected override void OnAddPreviewClick()
		{
			_tat.Init();
		}

		#endregion

	}

}