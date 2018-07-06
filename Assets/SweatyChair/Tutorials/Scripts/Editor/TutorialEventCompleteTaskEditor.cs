using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using SweatyChair;

[CustomEditor(typeof(TutorialEventCompleteTask))]
public class TutorialEventCompleteTaskEditor : TutorialTaskEditor
{

    protected bool showTargetSetting = true;

    private TutorialEventCompleteTask _tect
    {
        get { return target as TutorialEventCompleteTask; }
    }

    protected override bool isCompletedTriggerEditable { get { return false; } }

    protected override TutoriaCompletelTrigger forceCompleteTrigger { get { return TutoriaCompletelTrigger.Auto; } }

    protected override void OnOtherSettingGUI()
    {
        Transform targetTF = TransformUtils.GetTransformByPath(_tect.targetPath);
        Transform newTargetTF = EditorGUILayout.ObjectField(new GUIContent("Target in Scene", "The target transform in current scene, this will be converted and saved as path string."), targetTF, typeof(Transform), true) as Transform;
        if (targetTF != newTargetTF)
        {
            Undo.RegisterCompleteObjectUndo(_tect, "Reassign Target Path");
            _tect.targetPath = targetTF.ToPath();
            targetTF = newTargetTF;
        }
        if (targetTF == null)
        { // Not set or not found in scene, show option for mannually set
            GUI.contentColor = Color.red;
            string targetPath = EditorGUILayout.TextField(new GUIContent("Not Found!", "The target transform converted from path string is not found in current scene, please drag again or mannually assign the path for dynamically-loaded prefab."), _tect.targetPath);
            if (targetPath != _tect.targetPath)
            {
                Undo.RegisterCompleteObjectUndo(_tect, "Reassign Target Path");
                _tect.targetPath = targetPath;
            }
            GUI.contentColor = Color.white;
        }

        if (targetTF != null)
        {

            Component[] components = targetTF.GetComponents<Component>();
            int savedComponentIndex = -1;
            GUIContent[] componentContents = new GUIContent[components.Length];
            for (int i = 0, imax = components.Length; i < imax; i++)
            {
                componentContents[i] = new GUIContent(components[i].GetType().ToString(), "");
                if (components[i].GetType().ToString() == _tect.targetComponentName)
                    savedComponentIndex = i;
            }

            int selectedComponentIndex = EditorGUILayout.Popup(new GUIContent("Component", "The script that contain the variable."), savedComponentIndex, componentContents);
            if (selectedComponentIndex != savedComponentIndex)
            {
                Undo.RegisterCompleteObjectUndo(_tect, "Reassign Component");
                _tect.targetComponentName = components[selectedComponentIndex].GetType().ToString();
            }

            if (!string.IsNullOrEmpty(_tect.targetComponentName))
            {

                if (selectedComponentIndex >= components.Length)
                    selectedComponentIndex = components.Length - 1;
                Component c = components.Length > 0 ? components[selectedComponentIndex] : null;
                FieldInfo[] fieldInfos = components[selectedComponentIndex].GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

                int savedFieldIndex = 0;
                GUIContent[] fieldContents = new GUIContent[fieldInfos.Length + 1]; // First null
                fieldContents[0] = new GUIContent("<null>", "");
                for (int i = 0, imax = fieldInfos.Length; i < imax; i++)
                {
                    fieldContents[i + 1] = new GUIContent(fieldInfos[i].Name, "");
                    if (fieldInfos[i].Name == _tect.fieldName)
                        savedFieldIndex = i + 1;
                }

                int selectedObjectIndex = EditorGUILayout.Popup(new GUIContent("Field Object", "The variable field name."), savedFieldIndex, fieldContents);
                if (selectedObjectIndex != savedFieldIndex)
                {
                    Undo.RegisterCompleteObjectUndo(_tect, "Reassign Field Object");
                    _tect.fieldName = fieldInfos[selectedObjectIndex].Name;
                }

                EventInfo[] eventInfos;
                if (string.IsNullOrEmpty(_tect.fieldName) || c == null)
                {
                    eventInfos = c.GetType().GetEvents(BindingFlags.Public | BindingFlags.Instance);
                }
                else
                {
                    object obj = (object)c.GetType().GetField(_tect.fieldName, BindingFlags.Public | BindingFlags.Instance).GetValue(c);
                    eventInfos = obj.GetType().GetEvents(BindingFlags.Public | BindingFlags.Instance);
                }

                if (eventInfos.Length == 0)
                {
                    GUI.contentColor = Color.red;
                    EditorGUILayout.LabelField("Event", "Not Found");
                    GUI.contentColor = Color.white;
                }
                else
                {
                    int savedEventIndex = -1;
                    GUIContent[] actionsContents = new GUIContent[eventInfos.Length];
                    for (int i = 0, imax = eventInfos.Length; i < imax; i++)
                    {
                        actionsContents[i] = new GUIContent(eventInfos[i].Name, "");
                        if (eventInfos[i].Name == _tect.eventName)
                            savedEventIndex = i;
                    }

                    int selectedActionIndex = EditorGUILayout.Popup(new GUIContent("Event", "The event field name."), savedEventIndex, actionsContents);
                    if (selectedActionIndex != savedEventIndex)
                    {
                        Undo.RegisterCompleteObjectUndo(_tect, "Reassign Event");
                        _tect.eventName = eventInfos[selectedActionIndex].Name;
                    }
                }

            }

        }
    }

}