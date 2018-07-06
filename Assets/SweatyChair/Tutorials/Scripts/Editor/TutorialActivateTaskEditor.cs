using UnityEngine;
using UnityEditor;
using System.Collections;
using SweatyChair;

[CustomEditor(typeof(TutorialActivateTask))]
public class TutorialActivateTaskEditor : TutorialTaskEditor
{

    private TutorialActivateTask _tat {
		get { return target as TutorialActivateTask; }
	}

	protected override void OnPreviewSettingGUI()
	{
		Transform targetTF = TransformUtils.GetTransformByPath(_tat.targetPath);
		string targetPath = TransformUtils.ToPath(EditorGUILayout.ObjectField(new GUIContent("Target in Scene", "The target transform in current scene, this will be converted and saved as path string."), targetTF, typeof(Transform), true) as Transform);
		if (!string.IsNullOrEmpty(_tat.targetPath) && targetTF == null) { // Set but targetTF not found, this may be because in wrong scene
			GUI.contentColor = Color.red;
			string customTargetPath = EditorGUILayout.TextField(new GUIContent("Not Found!", "The target transform converted from path string is not found in current scene, please drag again or mannually assign the path for dynamically-loaded prefab."), _tat.targetPath);
			if (customTargetPath != _tat.targetPath) {
				Undo.RegisterUndo(_tat, "Reassign Custom Target Path");
				_tat.targetPath = customTargetPath;
			}
			GUI.contentColor = Color.white;
		}
		if (!string.IsNullOrEmpty(targetPath) && targetPath != _tat.targetPath) {
			Undo.RegisterUndo(_tat, "Reassign Target Path");
			_tat.targetPath = targetPath;
		}

		GUILayout.BeginHorizontal();
		{
			bool isActivate = EditorGUILayout.Toggle(new GUIContent("Is Activate", "Set the target GameObject to be active or not."), _tat.isActivate);
			GUILayout.Label("Activate target if true, or deactivate if false");

			if (isActivate != _tat.isActivate) {
				Undo.RegisterUndo(_tat, "Reassign Is Activate");
				_tat.isActivate = isActivate;
			}
		}
		GUILayout.EndHorizontal();

		bool checkOnUpdate = EditorGUILayout.Toggle(new GUIContent("Check On Update", "Set the active status in Update, set this for if target GameObject's active status also controlled by another script."), _tat.checkOnUpdate);
		if (checkOnUpdate != _tat.checkOnUpdate) {
			Undo.RegisterUndo(_tat, "Reassign Check On Update");
			_tat.checkOnUpdate = checkOnUpdate;
		}
	}

	protected override void OnOtherSettingGUI()
	{
		bool deactivateOnComplete = EditorGUILayout.Toggle(new GUIContent("Set Prev Activation On Complete", "Set the active status back after this tutorial step complete."), _tat.setPrevActivationOnComplete);
		if (deactivateOnComplete != _tat.setPrevActivationOnComplete) {
			Undo.RegisterUndo(_tat, "Reassign Set Prev Activation On Complete");
			_tat.setPrevActivationOnComplete = deactivateOnComplete;
		}
	}

	#region Preview

	protected override bool isPreviewShown {
		get { return _tat.targetGO != null && _tat.targetGO.activeSelf == _tat.isActivate; }
	}

	protected override bool canShowPreview {
		get { return TransformUtils.GetTransformByPath(_tat.targetPath) != null; }
	}

	protected override string cannotShowPreviewWarning {
		get { return "Assign target to enable Preview"; }
	}

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