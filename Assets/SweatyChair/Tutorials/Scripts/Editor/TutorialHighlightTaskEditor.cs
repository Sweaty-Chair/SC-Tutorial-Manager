using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;

namespace SweatyChair
{

	[CustomEditor(typeof(TutorialHighlightTask))]
	public class TutorialHighlightTaskEditor : TutorialShowPanelTaskEditor
	{

		protected bool _showTargetSetting = true;
		protected bool _showDisableButtons = true;

		private TutorialHighlightTask _tht => target as TutorialHighlightTask;

		protected override void OnPreviewSettingGUI()
		{
			_showTargetSetting = EditorGUILayout.Foldout(_showTargetSetting, "Target");

			if (_showTargetSetting) {

				EditorGUI.indentLevel++;

				Transform targetTF = TransformUtils.GetTransformByPath(_tht.targetPath);
				Transform newTargetTF = EditorGUILayout.ObjectField(new GUIContent("Target in Scene", "The target transform in current scene, this will be converted and saved as path string."), targetTF, typeof(Transform), true) as Transform;
				string targetPath = newTargetTF.ToPath();
				if (targetTF == null && !string.IsNullOrEmpty(_tht.targetPath))
					GUI.contentColor = Color.red;
				string customTargetPath = EditorGUILayout.TextField(new GUIContent(string.IsNullOrEmpty(_tht.targetPath) ? "Enter Manually" : (targetTF == null ? "Not Found" : " "), "Manually set the target path, use this for dynamically-loaded prefab."), _tht.targetPath);
				if (customTargetPath != _tht.targetPath) {
					EditorUtility.SetDirty(_tht);
					Undo.RegisterCompleteObjectUndo(_tht, "Reassign Custom Target Path");
					_tht.targetPath = customTargetPath;
					targetPath = customTargetPath;
				}
				if (targetTF == null && !string.IsNullOrEmpty(_tht.targetPath))
					GUI.contentColor = Color.white;
				if ((string.IsNullOrEmpty(_tht.targetPath) || newTargetTF != null) && targetPath != _tht.targetPath) {
					EditorUtility.SetDirty(_tht);
					Undo.RegisterCompleteObjectUndo(_tht, "Reassign Target Path");
					_tht.targetPath = targetPath;
				}

				if (!string.IsNullOrEmpty(_tht.targetPath)) {

					bool skipIfTargetInactive = EditorGUILayout.Toggle(new GUIContent("Skip If Target Inactive", "Mark this task complete if finding the target inactive."), _tht.skipIfTargetInactive);
					if (skipIfTargetInactive != _tht.skipIfTargetInactive) {
						EditorUtility.SetDirty(_tht);
						Undo.RegisterCompleteObjectUndo(_tht, "Reassign Skip If Target Inactive");
						_tht.skipIfTargetInactive = skipIfTargetInactive;
					}
					bool cloneTarget = EditorGUILayout.Toggle(new GUIContent("Clone Target", "Clone a target and keep it in original place, use this on scroll slot etc."), _tht.cloneTarget);
					if (cloneTarget != _tht.cloneTarget) {
						EditorUtility.SetDirty(_tht);
						Undo.RegisterCompleteObjectUndo(_tht, "Reassign Clone Target");
						_tht.cloneTarget = cloneTarget;
					}

					if (cloneTarget) {
						bool targetFollowCloneOnUpdate = EditorGUILayout.Toggle(new GUIContent("Target Follow Clone On Update", "Target follows the clone in update, use this on scroll slot etc."), _tht.targetFollowCloneOnUpdate);
						if (targetFollowCloneOnUpdate != _tht.targetFollowCloneOnUpdate) {
							EditorUtility.SetDirty(_tht);
							Undo.RegisterCompleteObjectUndo(_tht, "Reassign Target Follow Clone On Update");
							_tht.targetFollowCloneOnUpdate = targetFollowCloneOnUpdate;
						}
						bool targetCanCatchRaycast = EditorGUILayout.Toggle(new GUIContent("Target Catch Raycast", "Toggle the target raycast, use this if you want tto turn on/off the target's interaction."), _tht.targetCanCatchRaycast);
						if (targetCanCatchRaycast != _tht.targetCanCatchRaycast) {
							EditorUtility.SetDirty(_tht);
							Undo.RegisterCompleteObjectUndo(_tht, "Reassign Target Can Catch Raycast");
							_tht.targetCanCatchRaycast = targetCanCatchRaycast;
						}
						bool putCloneToTutorialPanelInstead = EditorGUILayout.Toggle(new GUIContent("Put Clone To Tutorial Panel Instead", "By default, put original target to tutorial panel, turn this on if you want the opposite."), _tht.putCloneToTutorialPanelInstead);
						if (putCloneToTutorialPanelInstead != _tht.putCloneToTutorialPanelInstead) {
							EditorUtility.SetDirty(_tht);
							Undo.RegisterCompleteObjectUndo(_tht, "Reassign Put Clone To Tutorial Panel Instead");
							_tht.putCloneToTutorialPanelInstead = putCloneToTutorialPanelInstead;
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
				bool handFollowTarget = EditorGUILayout.Toggle(new GUIContent("Follow Target", "Follow the target position, use this if the target may move from its position, e.g. in a layout group."), _tht.handFollowTarget);
				if (handFollowTarget != _tht.handFollowTarget) {
					EditorUtility.SetDirty(_tht);
					Undo.RegisterCompleteObjectUndo(_tht, "Reassign Hand Follow Target");
					_tht.handFollowTarget = handFollowTarget;
					UpdatePreview();
				}

				if (_tht.handFollowTarget) {
					bool handFollowOnUpdate = EditorGUILayout.Toggle(new GUIContent("Follow On Update", "Follow the target position in Update, use this if the target is moving in Update, e.g. in a scroll rect."), _tht.handFollowOnUpdate);
					if (handFollowOnUpdate != _tht.handFollowOnUpdate) {
						EditorUtility.SetDirty(_tht);
						Undo.RegisterCompleteObjectUndo(_tht, "Reassign Hand Follow On Update");
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
				bool textFollowTarget = EditorGUILayout.Toggle(new GUIContent("Follow Target", "Follow the target position, use this if the target may move from its position, e.g. in a layout group."), _tht.textFollowTarget);
				if (textFollowTarget != _tht.textFollowTarget) {
					EditorUtility.SetDirty(_tht);
					Undo.RegisterCompleteObjectUndo(_tht, "Reassign Text Follow Target");
					_tht.textFollowTarget = textFollowTarget;
					UpdatePreview();
				}

				if (_tht.textFollowTarget) {
					bool textFollowOnUpdate = EditorGUILayout.Toggle(new GUIContent("Follow On Update", "Follow the target position in Update, use this if the target is moving in Update, e.g. in a scroll rect."), _tht.textFollowOnUpdate);
					if (textFollowOnUpdate != _tht.textFollowOnUpdate) {
						EditorUtility.SetDirty(_tht);
						Undo.RegisterCompleteObjectUndo(_tht, "Reassign Hand Follow On Update");
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

			bool doNotResetHighlightedObjectOnComplete = EditorGUILayout.Toggle(new GUIContent("Do Not Reset Highlighted Object On Complete", "Not reset highlighted object on step complete. Use this to avoid flicker if next step also use highlight."), _tht.doNotResetHighlightedObjectOnComplete);
			if (doNotResetHighlightedObjectOnComplete != _tht.doNotResetHighlightedObjectOnComplete) {
				EditorUtility.SetDirty(_tht);
				Undo.RegisterCompleteObjectUndo(_tht, "Reassign Do Not Reset Highlighted Object On Complete");
				_tht.doNotResetHighlightedObjectOnComplete = doNotResetHighlightedObjectOnComplete;
			}
			bool removeHighlightedObjectPanelOnComplete = EditorGUILayout.Toggle(new GUIContent("Remove Highlighted Object Panel On Complete", "Remove highlighted object panel on step complete. Use this to make sure the highlight is removed before next step."), _tht.removeHighlightedObjectPanelOnComplete);
			if (removeHighlightedObjectPanelOnComplete != _tht.removeHighlightedObjectPanelOnComplete) {
				EditorUtility.SetDirty(_tht);
				Undo.RegisterCompleteObjectUndo(_tht, "Reassign Remove Highlighted Object Panel On Complete");
				_tht.removeHighlightedObjectPanelOnComplete = removeHighlightedObjectPanelOnComplete;
			}
		}

		protected virtual void OnDisableButtonsSettingGUI()
		{
			_showDisableButtons = EditorGUILayout.Foldout(_showDisableButtons, "Disable Buttons");
			EditorGUI.indentLevel++;

			if (_showDisableButtons) {

				int size = EditorGUILayout.IntField(new GUIContent("Size", "Number of button to be disabled."), _tht.disableButtonPaths.Length);

				if (size != _tht.disableButtonPaths.Length) {
					EditorUtility.SetDirty(_tht);
					Undo.RegisterCompleteObjectUndo(_tht, "Reassign Disable Buttons");
					System.Array.Resize(ref _tht.disableButtonPaths, size);
				}

				for (int i = 0, imax = _tht.disableButtonPaths.Length; i < imax; i++) {
					Button button = TransformUtils.GetComponentByPath<Button>(_tht.disableButtonPaths[i]);
					string buttonPath = TransformUtils.ToPath(EditorGUILayout.ObjectField("Button " + i, button, typeof(Button), true) as Button);
					if (!string.IsNullOrEmpty(_tht.disableButtonPaths[i]) && button == null) { // Set but targetTF not found, this may be because in wrong scene
						GUI.contentColor = Color.red;
						string customButtonPath = EditorGUILayout.TextField("Not Found!", _tht.disableButtonPaths[i]);
						if (customButtonPath != _tht.disableButtonPaths[i]) {
							EditorUtility.SetDirty(_tht);
							Undo.RegisterCompleteObjectUndo(_tht, "Reassign Custom Button Path");
							_tht.disableButtonPaths[i] = customButtonPath;
						}
						GUI.contentColor = Color.white;
					}
					if (!string.IsNullOrEmpty(buttonPath) && buttonPath != _tht.disableButtonPaths[i]) {
						EditorUtility.SetDirty(_tht);
						Undo.RegisterCompleteObjectUndo(_tht, "Reassign Button Path");
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

}