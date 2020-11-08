using UnityEngine;
using UnityEditor;
using SweatyChair.UI;

namespace SweatyChair
{

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
			string targetPath = (EditorGUILayout.ObjectField(new GUIContent("Panel in Scene", "The target panel in current scene, this will be converted and saved as path string."), targetPanel, typeof(Panel), true) as Panel).ToPath();
			if (targetPanel == null && !string.IsNullOrEmpty(_tstpt.targetPath))
				GUI.contentColor = Color.red;
			string customTargetPath = EditorGUILayout.TextField(new GUIContent(string.IsNullOrEmpty(_tstpt.targetPath) ? "Enter Manually" : (targetPanel == null ? "Not Found" : " "), "Manually set the target path, use this for dynamically-loaded prefab."), _tstpt.targetPath);
			if (customTargetPath != _tstpt.targetPath) {
				EditorUtility.SetDirty(_tstpt);
				Undo.RegisterCompleteObjectUndo(_tstpt, "Reassign Custom Target Path");
				_tstpt.targetPath = customTargetPath;
				targetPath = customTargetPath;
			}
			if (targetPanel == null && !string.IsNullOrEmpty(_tstpt.targetPath))
				GUI.contentColor = Color.white;
			if ((string.IsNullOrEmpty(_tstpt.targetPath) || targetPanel != null) && targetPath != _tstpt.targetPath) {
				EditorUtility.SetDirty(_tstpt);
				Undo.RegisterCompleteObjectUndo(_tstpt, "Reassign Target Path");
				_tstpt.targetPath = targetPath;
			}
		}

		protected override void OnOtherSettingGUI()
		{
			bool hideOnComplete = EditorGUILayout.Toggle(new GUIContent("Hide On Complete", "Hide the panel after this tutorial step complete."), _tstpt.hideOnComplete);
			if (hideOnComplete != _tstpt.hideOnComplete) {
				EditorUtility.SetDirty(_tstpt);
				Undo.RegisterCompleteObjectUndo(_tstpt, "Reassign Hide On Complete");
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

}