using UnityEngine;
using UnityEditor;

namespace SweatyChair
{

	[CustomEditor(typeof(TutorialWaitForActiveTask))]
	public class TutorialWaitForActiveTaskEditor : TutorialTaskEditor
	{

		private TutorialWaitForActiveTask _twfat => target as TutorialWaitForActiveTask;

		protected override void OnPreviewSettingGUI()
		{
			Transform targetTF = TransformUtils.GetTransformByPath(_twfat.targetPath);
			string targetPath = (EditorGUILayout.ObjectField(new GUIContent("Target in Scene", "The target transform in current scene, this will be converted and saved as path string."), targetTF, typeof(Transform), true) as Transform).ToPath();
			if (targetTF == null && !string.IsNullOrEmpty(_twfat.targetPath))
				GUI.contentColor = Color.red;
			string customTargetPath = EditorGUILayout.TextField(new GUIContent(string.IsNullOrEmpty(_twfat.targetPath) ? "Enter Manually" : (targetTF == null ? "Not Found" : " "), "Manually set the target path, use this for dynamically-loaded prefab."), _twfat.targetPath);
			if (customTargetPath != _twfat.targetPath) {
				EditorUtility.SetDirty(_twfat);
				Undo.RegisterCompleteObjectUndo(_twfat, "Reassign Custom Target Path");
				_twfat.targetPath = customTargetPath;
				targetPath = customTargetPath;
			}
			if (targetTF == null && !string.IsNullOrEmpty(_twfat.targetPath))
				GUI.contentColor = Color.white;
			if ((string.IsNullOrEmpty(_twfat.targetPath) || targetTF != null) && targetPath != _twfat.targetPath) {
				EditorUtility.SetDirty(_twfat);
				Undo.RegisterCompleteObjectUndo(_twfat, "Reassign Target Path");
				_twfat.targetPath = targetPath;
			}

			bool isActive = EditorGUILayout.Toggle(new GUIContent("Is Active", "Check if the target GameObject to be active for true, or inactive for false."), _twfat.isActive);
			if (isActive != _twfat.isActive) {
				EditorUtility.SetDirty(_twfat);
				Undo.RegisterCompleteObjectUndo(_twfat, "Reassign Is Activate");
				_twfat.isActive = isActive;
			}
		}

		#region Preview

		protected override bool canShowPreview => false;

		protected override void OnRemovePreviewClick()
		{
			_twfat.targetGO.SetActive(false);
		}

		protected override void OnAddPreviewClick()
		{
			_twfat.Init();
		}

		#endregion

	}

}