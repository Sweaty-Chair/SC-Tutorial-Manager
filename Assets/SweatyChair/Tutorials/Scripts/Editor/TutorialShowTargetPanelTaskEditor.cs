using UnityEngine;
using UnityEditor;
using System.Collections;
using SweatyChair;

[CustomEditor(typeof(TutorialShowTargetPanelTask))]
public class TutorialShowTargetPanelTaskEditor : TutorialTaskEditor
{

    private TutorialShowTargetPanelTask _tstpt {
		get { return target as TutorialShowTargetPanelTask; }
	}

	protected override void OnPreviewSettingGUI()
	{
		GUILayout.Label("UI", EditorStyles.boldLabel);

		Panel targetPanel = TransformUtils.GetComponentByPath<Panel>(_tstpt.targetPath);
		string targetPath = TransformUtils.ToPath(EditorGUILayout.ObjectField(new GUIContent("Panel in Scene", "The target panel in current scene, this will be converted and saved as path string."), targetPanel, typeof(Panel), true) as Panel);
		if (!string.IsNullOrEmpty(_tstpt.targetPath) && targetPanel == null) { // Set but targetTF not found, this may be because in wrong scene
			GUI.contentColor = Color.red;
			string customTargetPath = EditorGUILayout.TextField(new GUIContent("Not Found!", "The target transform converted from path string is not found in current scene, please drag again or mannually assign the path for dynamically-loaded prefab."), _tstpt.targetPath);
			if (customTargetPath != _tstpt.targetPath) {
				Undo.RegisterUndo(_tstpt, "Reassign Custom Target Path");
				_tstpt.targetPath = customTargetPath;
			}
			GUI.contentColor = Color.white;
		}
		if (!string.IsNullOrEmpty(targetPath) && targetPath != _tstpt.targetPath) {
			Undo.RegisterUndo(_tstpt, "Reassign Target Path");
			_tstpt.targetPath = targetPath;
		}
	}

	protected override void OnOtherSettingGUI()
	{
		bool hideOnComplete = EditorGUILayout.Toggle(new GUIContent("Hide On Complete", "Hide the panel after this tutorial step complete."), _tstpt.hideOnComplete);
		if (hideOnComplete != _tstpt.hideOnComplete) {
			Undo.RegisterUndo(_tstpt, "Reassign Hide On Complete");
			_tstpt.hideOnComplete = hideOnComplete;
		}
	}

	#region Preview

	protected override bool isPreviewShown {
		get { return _tstpt.targetPanel != null; }
	}

	protected override bool canShowPreview {
		get { return !string.IsNullOrEmpty(_tstpt.targetPath) && TransformUtils.GetTransformByPath(_tstpt.targetPath) != null; }
	}

	protected override string cannotShowPreviewWarning {
		get { return "Assign target to enable Preview"; }
	}

	protected override void OnRemovePreviewClick()
	{
		_tstpt.Reset();
	}

	protected override void OnAddPreviewClick()
	{
		_tstpt.Init();
		_tstpt.targetPanel.Show();
	}

	#endregion

}