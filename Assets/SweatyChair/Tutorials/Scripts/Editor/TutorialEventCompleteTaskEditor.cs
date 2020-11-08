using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace SweatyChair
{

	[CustomEditor(typeof(TutorialEventCompleteTask))]
	public class TutorialEventCompleteTaskEditor : TutorialTaskEditor
	{

		protected bool showTargetSetting = true;

		private TutorialEventCompleteTask _tect {
			get { return target as TutorialEventCompleteTask; }
		}

		protected override bool isCompletedTriggerEditable { get { return false; } }

		protected override TutoriaCompletelTrigger forceCompleteTrigger { get { return TutoriaCompletelTrigger.Auto; } }

		protected override void OnOtherSettingGUI()
		{
			Transform targetTF = TransformUtils.GetTransformByPath(_tect.targetPath);
			string targetPath = (EditorGUILayout.ObjectField(new GUIContent("Target in Scene", "The target transform in current scene, this will be converted and saved as path string."), targetTF, typeof(Transform), true) as Transform).ToPath();
			if (targetTF == null && !string.IsNullOrEmpty(_tect.targetPath))
				GUI.contentColor = Color.red;
			string customTargetPath = EditorGUILayout.TextField(string.IsNullOrEmpty(_tect.targetPath) ? "Enter Manually" : (targetTF == null ? "Not Found" : " "), _tect.targetPath);
			if (customTargetPath != _tect.targetPath) {
				EditorUtility.SetDirty(_tect);
				Undo.RegisterCompleteObjectUndo(_tect, "Reassign Custom Target Path");
				_tect.targetPath = customTargetPath;
				targetPath = customTargetPath;
			}
			if (targetTF == null && !string.IsNullOrEmpty(_tect.targetPath))
				GUI.contentColor = Color.white;
			if ((string.IsNullOrEmpty(_tect.targetPath) || targetTF != null) && targetPath != _tect.targetPath) {
				EditorUtility.SetDirty(_tect);
				Undo.RegisterCompleteObjectUndo(_tect, "Reassign Target Path");
				_tect.targetPath = targetPath;
			}

			if (targetTF != null) {

				Component[] components = targetTF.GetComponents<Component>();
				int savedComponentIndex = -1;
				GUIContent[] componentContents = new GUIContent[components.Length];
				for (int i = 0, imax = components.Length; i < imax; i++) {
					componentContents[i] = new GUIContent(components[i].GetType().ToString(), "");
					if (components[i].GetType().ToString() == _tect.targetComponentName)
						savedComponentIndex = i;
				}

				int selectedComponentIndex = EditorGUILayout.Popup(new GUIContent("Component", "The script that contain the variable."), savedComponentIndex, componentContents);
				if (selectedComponentIndex != savedComponentIndex) {
					EditorUtility.SetDirty(_tect);
					Undo.RegisterCompleteObjectUndo(_tect, "Reassign Component");
					_tect.targetComponentName = components[selectedComponentIndex].GetType().ToString();
				}

				if (!string.IsNullOrEmpty(_tect.targetComponentName)) {

					if (selectedComponentIndex >= components.Length)
						selectedComponentIndex = components.Length - 1;
					Component c = components.Length > 0 ? components[selectedComponentIndex] : null;
					FieldInfo[] fieldInfos = components[selectedComponentIndex].GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

					int savedFieldIndex = 0;
					GUIContent[] fieldContents = new GUIContent[fieldInfos.Length + 1]; // First null
					fieldContents[0] = new GUIContent("<null>", "");
					for (int i = 0, imax = fieldInfos.Length; i < imax; i++) {
						fieldContents[i + 1] = new GUIContent(fieldInfos[i].Name, "");
						if (fieldInfos[i].Name == _tect.fieldName)
							savedFieldIndex = i + 1;
					}

					int selectedObjectIndex = EditorGUILayout.Popup(new GUIContent("Field Object", "The variable field name."), savedFieldIndex, fieldContents);
					if (selectedObjectIndex != savedFieldIndex) {
						EditorUtility.SetDirty(_tect);
						Undo.RegisterCompleteObjectUndo(_tect, "Reassign Field Object");
						_tect.fieldName = fieldInfos[selectedObjectIndex].Name;
					}

					EventInfo[] eventInfos;
					if (string.IsNullOrEmpty(_tect.fieldName) || c == null) {
						eventInfos = c.GetType().GetEvents(BindingFlags.Public | BindingFlags.Instance);
					} else {
						object obj = (object)c.GetType().GetField(_tect.fieldName, BindingFlags.Public | BindingFlags.Instance).GetValue(c);
						eventInfos = obj.GetType().GetEvents(BindingFlags.Public | BindingFlags.Instance);
					}

					if (eventInfos.Length == 0) {
						GUI.contentColor = Color.red;
						EditorGUILayout.LabelField("Event", "Not Found");
						GUI.contentColor = Color.white;
					} else {
						int savedEventIndex = -1;
						GUIContent[] actionsContents = new GUIContent[eventInfos.Length];
						for (int i = 0, imax = eventInfos.Length; i < imax; i++) {
							actionsContents[i] = new GUIContent(eventInfos[i].Name, "");
							if (eventInfos[i].Name == _tect.eventName)
								savedEventIndex = i;
						}

						int selectedActionIndex = EditorGUILayout.Popup(new GUIContent("Event", "The event field name."), savedEventIndex, actionsContents);
						if (selectedActionIndex != savedEventIndex) {
							EditorUtility.SetDirty(_tect);
							Undo.RegisterCompleteObjectUndo(_tect, "Reassign Event");
							_tect.eventName = eventInfos[selectedActionIndex].Name;
						}
					}

				}

			}
		}

	}

}