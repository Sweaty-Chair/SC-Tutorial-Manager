using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;
using SweatyChair;

[CustomEditor(typeof(TutorialHighlightTask))]
public class TutorialHighlightTaskEditor : TutorialShowPanelTaskEditor
{

	protected bool showTargetSetting = true;
	protected bool showDisableButtons = true;

    private TutorialHighlightTask _tht {
		get { return target as TutorialHighlightTask; }
	}

	protected override void OnPreviewSettingGUI()
	{
		showTargetSetting = EditorGUILayout.Foldout(showTargetSetting, "Target");

		if (showTargetSetting) {
			
			EditorGUI.indentLevel++;

			Transform targetTF = TransformUtils.GetTransformByPath(_tht.targetPath);
			string targetPath = TransformUtils.ToPath(EditorGUILayout.ObjectField("Target in Scene", targetTF, typeof(Transform), true) as Transform);
			if (!string.IsNullOrEmpty(_tht.targetPath) && targetTF == null) { // Set but targetTF not found, this may be because in wrong scene
				GUI.contentColor = Color.red;
				string customTargetPath = EditorGUILayout.TextField("Not Found!", _tht.targetPath);
				if (customTargetPath != _tht.targetPath) {
					Undo.RegisterUndo(_tht, "Reassign Custom Target Path");
					_tht.targetPath = customTargetPath;
				}
				GUI.contentColor = Color.white;
			}
			if (!string.IsNullOrEmpty(targetPath) && targetPath != _tht.targetPath) {
				Undo.RegisterUndo(_tht, "Reassign Target Path");
				_tht.targetPath = targetPath;
			}

			if (!string.IsNullOrEmpty(targetPath)) {
				
				bool skipIfTargetInactive = EditorGUILayout.Toggle("Skip If Target Inactive", _tht.skipIfTargetInactive);
				if (skipIfTargetInactive != _tht.skipIfTargetInactive) {
					Undo.RegisterUndo(_tht, "Reassign Skip If Target Inactive");
					_tht.skipIfTargetInactive = skipIfTargetInactive;
				}

				bool cloneTarget = EditorGUILayout.Toggle("Clone Target", _tht.cloneTarget);
				if (cloneTarget != _tht.cloneTarget) {
					Undo.RegisterUndo(_tht, "Reassign Clone Target");
					_tht.cloneTarget = cloneTarget;
				}

				if (cloneTarget) {
					bool targetFollowCloneOnUpdate = EditorGUILayout.Toggle("Target Follow Clone On Update", _tht.targetFollowCloneOnUpdate);
					if (targetFollowCloneOnUpdate != _tht.targetFollowCloneOnUpdate) {
						Undo.RegisterUndo(_tht, "Reassign Target Follow Clone On Update");
						_tht.targetFollowCloneOnUpdate = targetFollowCloneOnUpdate;
					}

                    bool targetCanCatchRaycast = EditorGUILayout.Toggle("Target Catch Raycast", _tht.targetCanCatchRaycast);
                    if (targetCanCatchRaycast != _tht.targetCanCatchRaycast)
                    {
                        Undo.RegisterUndo(_tht, "Reassign Target Can Catch Raycast");
                        _tht.targetCanCatchRaycast = targetCanCatchRaycast;
                    }
                }

			}

			EditorGUI.indentLevel--;
		}

		base.OnPreviewSettingGUI();
	}

	protected override void OnHandSettingGUI()
	{
		base.OnHandSettingGUI();

		EditorGUI.indentLevel++;

		if (showHandSetting && _tht.showHand) {
			bool handFollowTarget = EditorGUILayout.Toggle("Follow Target", _tht.handFollowTarget);
			if (handFollowTarget != _tht.handFollowTarget) {
				Undo.RegisterUndo(_tht, "Reassign Hand Follow Target");
				_tht.handFollowTarget = handFollowTarget;
				UpdatePreview();
			}

			if (_tht.handFollowTarget) {
				bool handFollowOnUpdate = EditorGUILayout.Toggle("Follow On Update", _tht.handFollowOnUpdate);
				if (handFollowOnUpdate != _tht.handFollowOnUpdate) {
					Undo.RegisterUndo(_tht, "Reassign Hand Follow On Update");
					_tht.handFollowOnUpdate = handFollowOnUpdate;
				}
			}
		}

		EditorGUI.indentLevel--;
	}

	protected override void OnTextSettingGUI()
	{
		base.OnTextSettingGUI();

		EditorGUI.indentLevel++;

		if (showTextSetting && !string.IsNullOrEmpty(_tht.text)) {
			bool textFollowTarget = EditorGUILayout.Toggle("Follow Target", _tht.textFollowTarget);
			if (textFollowTarget != _tht.textFollowTarget) {
				Undo.RegisterUndo(_tht, "Reassign Text Follow Target");
				_tht.textFollowTarget = textFollowTarget;
				UpdatePreview();
			}

			if (_tht.textFollowTarget) {
				bool textFollowOnUpdate = EditorGUILayout.Toggle("Follow On Update", _tht.textFollowOnUpdate);
				if (textFollowOnUpdate != _tht.textFollowOnUpdate) {
					Undo.RegisterUndo(_tht, "Reassign Hand Follow On Update");
					_tht.textFollowOnUpdate = textFollowOnUpdate;
				}
			}
		}

		EditorGUI.indentLevel--;
	}

	protected override void OnOtherSettingGUI()
	{
		base.OnOtherSettingGUI();
		OnDisableButtonsSettingGUI();
	}

	protected override void OnDoNotResetOnCompleteSettingGUI()
	{
		base.OnDoNotResetOnCompleteSettingGUI();

		bool doNotResetHighlightedObjectOnComplete = EditorGUILayout.Toggle("Do Not Reset Highlighted Object On Complete", _tht.doNotResetHighlightedObjectOnComplete);
		if (doNotResetHighlightedObjectOnComplete != _tht.doNotResetHighlightedObjectOnComplete) {
			Undo.RegisterUndo(_tht, "Reassign Do Not Reset Highlighted Object On Complete");
			_tht.doNotResetHighlightedObjectOnComplete = doNotResetHighlightedObjectOnComplete;
		}
		bool removeHighlightedObjectPanelOnComplete = EditorGUILayout.Toggle("Remove Highlighted Object Panel On Complete", _tht.removeHighlightedObjectPanelOnComplete);
		if (removeHighlightedObjectPanelOnComplete != _tht.removeHighlightedObjectPanelOnComplete) {
			Undo.RegisterUndo(_tht, "Reassign Remove Highlighted Object Panel On Complete");
			_tht.removeHighlightedObjectPanelOnComplete = removeHighlightedObjectPanelOnComplete;
		}
	}

	protected virtual void OnDisableButtonsSettingGUI()
	{
		showDisableButtons = EditorGUILayout.Foldout(showDisableButtons, "Disable Buttons");
		EditorGUI.indentLevel++;

		if (showDisableButtons) {

			int size = EditorGUILayout.IntField("Size", _tht.disableButtonPaths.Length);

			if (size != _tht.disableButtonPaths.Length) {
				Undo.RegisterUndo(_tht, "Reassign Disable Buttons");
				System.Array.Resize<string>(ref _tht.disableButtonPaths, size);
			}

			for (int i = 0, imax = _tht.disableButtonPaths.Length; i < imax; i++) {
				Button button = TransformUtils.GetComponentByPath<Button>(_tht.disableButtonPaths[i]);
				string buttonPath = TransformUtils.ToPath<Button>(EditorGUILayout.ObjectField("Button " + i, button, typeof(Button), true) as Button);
				if (!string.IsNullOrEmpty(_tht.disableButtonPaths[i]) && button == null) { // Set but targetTF not found, this may be because in wrong scene
					GUI.contentColor = Color.red;
					string customButtonPath = EditorGUILayout.TextField("Not Found!", _tht.disableButtonPaths[i]);
					if (customButtonPath != _tht.disableButtonPaths[i]) {
						Undo.RegisterUndo(_tht, "Reassign Custom Button Path");
						_tht.disableButtonPaths[i] = customButtonPath;
					}
					GUI.contentColor = Color.white;
				}
				if (!string.IsNullOrEmpty(buttonPath) && buttonPath != _tht.disableButtonPaths[i]) {
					Undo.RegisterUndo(_tht, "Reassign Button Path");
					_tht.disableButtonPaths[i] = buttonPath;
				}
			}
		}

		EditorGUI.indentLevel--;
	}

	#region Preview

	protected override bool canShowPreview {
		get { return !string.IsNullOrEmpty(_tht.targetPath) && TransformUtils.GetTransformByPath(_tht.targetPath) != null; }
	}

	protected override string cannotShowPreviewWarning {
		get { return "Assign target to enable Preview"; }
	}

	#endregion

}