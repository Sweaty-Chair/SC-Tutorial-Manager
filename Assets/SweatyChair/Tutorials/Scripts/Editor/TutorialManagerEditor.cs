﻿using SweatyChair;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(TutorialManager))]
public class TutorialManagerEditor : Editor
{

    private bool _confirmDelete = false;
    private bool _confirmMoveUp = false, _confirmMoveDown = false;
    private bool _confirmValidate = false;
    private static List<TutorialTask> _invalidateTaskList;

    private TutorialManager _tm {
        get { return target as TutorialManager; }
    }

    private int _tutorialIndex = -1;

    private int tutorialIndex {
        get {
            if (_tutorialIndex == -1)
                _tutorialIndex = EditorPrefs.GetInt("TutorialManagerEditorIndex");
            return _tutorialIndex;
        }
        set {
            _tutorialIndex = value;
            EditorPrefs.SetInt("TutorialManagerEditorIndex", value);
        }
    }

    public override void OnInspectorGUI()
    {
        Tutorial tutorial = null;

        if (_tm.tutorials == null || _tm.tutorials.Count == 0) {
            tutorialIndex = 0;
        } else {
            tutorialIndex = Mathf.Clamp(tutorialIndex, 0, _tm.tutorials.Count - 1);
            tutorial = _tm.tutorials[tutorialIndex];
        }

        if (_confirmMoveUp) {

            // Show the confirmation dialog
            GUILayout.Label("Move up '" + tutorial.name + "'? Player completed tutorial will be effected.");
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            GUILayout.BeginHorizontal();
            {
                GUI.backgroundColor = Color.green;

                if (GUILayout.Button("Cancel"))
                    _confirmMoveUp = false;

                GUI.backgroundColor = Color.red;

                if (GUILayout.Button("Confirm")) {
                    Undo.RegisterCompleteObjectUndo(_tm, "Move Up Tutorial");
                    Tutorial prevTutorial = _tm.tutorials[tutorialIndex - 1];
                    _tm.tutorials[tutorialIndex - 1] = tutorial;
                    _tm.tutorials[tutorialIndex] = prevTutorial;
                    RearrangeIds();
                    tutorialIndex--;
                    _confirmMoveUp = false;
                }

                GUI.backgroundColor = Color.white;
            }
            GUILayout.EndHorizontal();

        } else if (_confirmMoveDown) {

            // Show the confirmation dialog
            GUILayout.Label("Move down '" + tutorial.name + "'? Player completed tutorial will be effected.");
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            GUILayout.BeginHorizontal();
            {
                GUI.backgroundColor = Color.green;

                if (GUILayout.Button("Cancel"))
                    _confirmMoveDown = false;

                GUI.backgroundColor = Color.red;

                if (GUILayout.Button("Confirm")) {
                    Undo.RegisterCompleteObjectUndo(_tm, "Move Down Tutorial");
                    Tutorial nextTutorial = _tm.tutorials[tutorialIndex + 1];
                    _tm.tutorials[tutorialIndex] = nextTutorial;
                    _tm.tutorials[tutorialIndex + 1] = tutorial;
                    RearrangeIds();
                    tutorialIndex++;
                    _confirmMoveDown = false;
                }

                GUI.backgroundColor = Color.white;
            }
            GUILayout.EndHorizontal();

        } else if (_confirmDelete) {
            
            // Show the confirmation dialog
            GUILayout.Label("Are you sure you want to delete '" + tutorial.name + "'?");
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            GUILayout.BeginHorizontal();
            {
                GUI.backgroundColor = Color.green;

                if (GUILayout.Button("Cancel"))
                    _confirmDelete = false;
                
                GUI.backgroundColor = Color.red;

                if (GUILayout.Button("Delete")) {
                    Undo.RegisterCompleteObjectUndo(_tm, "Delete Tutorial");
                    _tm.tutorials.RemoveAt(tutorialIndex);
                    RearrangeIds();
                    _confirmDelete = false;
                }

                GUI.backgroundColor = Color.white;
            }
            GUILayout.EndHorizontal();

        } else if (_confirmValidate) {

            // Show the confirmation dialog
            GUILayout.Label("This will open other scenes and current scene won't be save, continue?");
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            GUILayout.BeginHorizontal();
            {
                GUI.backgroundColor = Color.red;

                if (GUILayout.Button("Cancel"))
                    _confirmValidate = false;

                GUI.backgroundColor = Color.green;

                if (GUILayout.Button("Continue")) {
                    ValidateTutorials();
                    _confirmValidate = false;
                }

                GUI.backgroundColor = Color.white;
            }
            GUILayout.EndHorizontal();

        } else if (_invalidateTaskList != null) {

            GUI.contentColor = Color.red;
            GUILayout.Label(_invalidateTaskList.Count + " TutorialTask invalid:");
            GUI.contentColor = Color.white;

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            foreach (TutorialTask tt in _invalidateTaskList) {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(tt.transform.parent.name + "/" + tt.transform.name + ":" + tt.GetType().ToString().Replace("SweatyChair.", ""));
                    if (GUILayout.Button("Select", GUILayout.Width(60)))
                        Selection.activeGameObject = tt.gameObject;
                }
                GUILayout.EndHorizontal();
                EditorGUI.indentLevel++;
                GUI.contentColor = Color.red;
                GUILayout.Label(tt.GetInvalidateString());
                GUI.contentColor = Color.white;
                EditorGUI.indentLevel--;
            }

            GUI.backgroundColor = Color.green;

            if (GUILayout.Button("Clear"))
                _invalidateTaskList = null;

            GUI.backgroundColor = Color.white;

        } else {

            // "New" button
            GUI.backgroundColor = Color.green;

            if (GUILayout.Button("New Tutorial")) {
                
                Undo.RegisterCompleteObjectUndo(_tm, "Add Tutorial");

                Tutorial t = new Tutorial();
                t.id = (_tm.tutorials.Count > 0) ? _tm.tutorials[_tm.tutorials.Count - 1].id + 1 : 1;
                _tm.tutorials.Add(t);
                tutorialIndex = _tm.tutorials.Count - 1;

                if (tutorial != null) {
                    t.checkStateMask = tutorial.checkStateMask;
                    t.triggerCondition = tutorial.triggerCondition;
                    t.validator = tutorial.validator;
                    t.sceneName = tutorial.sceneName;
                    t.prefab = tutorial.prefab;
                    t.isCore = tutorial.isCore;
                    t.setCompleteCondition = tutorial.setCompleteCondition;
                }

                tutorial = t;
            }
            GUI.backgroundColor = Color.white;

            if (tutorial != null) {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                // Navigation section
                GUILayout.BeginHorizontal();
                {
                    if (tutorialIndex == 0)
                        GUI.color = Color.grey;
                    if (GUILayout.Button("<<")) {
                        _confirmDelete = false;
                        tutorialIndex--;
                    }
                    GUI.color = Color.white;
                    tutorialIndex = EditorGUILayout.IntField(tutorialIndex + 1, GUILayout.Width(40)) - 1;
                    GUILayout.Label("/ " + _tm.tutorials.Count, GUILayout.Width(40));
                    if (tutorialIndex + 1 == _tm.tutorials.Count)
                        GUI.color = Color.grey;
                    if (GUILayout.Button(">>")) {
                        _confirmDelete = false;
                        tutorialIndex++;
                    }
                    GUI.color = Color.white;
                }
                GUILayout.EndHorizontal();

                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                // Tutorial ID and delete tutorial button
                GUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField(new GUIContent("ID", "Used in saving completed tutorials and analytics, do not change swipe order in release version."), new GUIContent(tutorial.id.ToString(), ""));

                    GUI.backgroundColor = Color.red;

                    if (tutorialIndex > 0 && GUILayout.Button("↑", GUILayout.Width(20)))
                        _confirmMoveUp = true;
                    if (tutorialIndex < (_tm.tutorials.Count - 1) && GUILayout.Button("↓", GUILayout.Width(20)))
                        _confirmMoveDown = true;
                    if (GUILayout.Button("Delete", GUILayout.Width(60)))
                        _confirmDelete = true;
                    
                    GUI.backgroundColor = Color.white;
                }
                GUILayout.EndHorizontal();

                // Trigger condition
                Tutorial.TriggerCondition triggerCondition = (Tutorial.TriggerCondition)EditorGUILayout.EnumPopup(new GUIContent("Trigger Condition", "How this tutorial be triggered?"), tutorial.triggerCondition);
                if (triggerCondition != tutorial.triggerCondition) {
                    Undo.RegisterCompleteObjectUndo(_tm, "Reassign tutorial trigger condition");
                    tutorial.triggerCondition = triggerCondition;
                }
                switch (tutorial.triggerCondition) {
                    case Tutorial.TriggerCondition.AlwaysTrigger:
                        EditorGUILayout.HelpBox("This will always trigger the tutoral, if it's not yet completed. Normally only the FIRST tutorial should be set to this.", MessageType.Info);
                        break;

                    case Tutorial.TriggerCondition.Mannual:
                        EditorGUILayout.HelpBox(string.Format("Call TutorialManager.StartTutorial({0}) to start this tutorial.", tutorial.id), MessageType.Info);
                        break;

                    case Tutorial.TriggerCondition.Validator:

                        // Check state mask
                        int checkStateMask = (int)(State)EditorGUILayout.EnumMaskField(new GUIContent("Check State", "Only check in specific states"), (State)tutorial.checkStateMask);
                        if (checkStateMask < 0) // If "Everything" is set, set bits to be all-1s with length of State
                            checkStateMask = (1 << System.Enum.GetValues(typeof(State)).Length) - 1;
                        if (checkStateMask != tutorial.checkStateMask) {
                            Undo.RegisterCompleteObjectUndo(_tm, "Reassign check state");
                            tutorial.checkStateMask = (int)checkStateMask;
                        }

                        // Tutorial validator
                        if (tutorial.validator == null)
                            EditorGUILayout.HelpBox("Required a customized validator, otherwise trigger condition is same as manual.", MessageType.Warning);
                        TutorialValidator validator = EditorGUILayout.ObjectField(new GUIContent("Validator", "A custom script for this tutorial controlling trigger time, OnStart, OnComplete, etc."), tutorial.validator, typeof(TutorialValidator), true) as TutorialValidator;
                        if (validator != tutorial.validator) {
                            Undo.RegisterCompleteObjectUndo(_tm, "Reassign tutorial validator");
                            tutorial.validator = validator;
                        }
                        break;
                }

                // Scene name
                string sceneName = EditorGUILayout.TextField(new GUIContent("Scene Name", "Load a scene just before this tutorial trigger, leave empty for not loading any."), tutorial.sceneName);
                if (sceneName != tutorial.sceneName) {
                    Undo.RegisterCompleteObjectUndo(_tm, "Reassign tutorial scene name");
                    tutorial.sceneName = sceneName;
                }

                // Tutorial prefab
                if (tutorial.prefab == null)
                    EditorGUILayout.HelpBox("Required", MessageType.Warning);
                GameObject prefab = EditorGUILayout.ObjectField(new GUIContent("Prefab", "The tutorial prefab that contain logic scripts TutorialInstance and TutorialSteps.") , tutorial.prefab, typeof(GameObject), false) as GameObject;
                if (prefab != tutorial.prefab) {
                    Undo.RegisterCompleteObjectUndo(_tm, "Reassign tutorial prefab");
                    tutorial.prefab = prefab;
                }

                // Is Core
                bool isCore = EditorGUILayout.Toggle(new GUIContent("Is Core", "A boolean just for showing core tutorial completion in UI for some games, no actual function here."), tutorial.isCore);
                if (isCore != tutorial.isCore) {
                    Undo.RegisterCompleteObjectUndo(_tm, "Reassign tutorial is core");
                    tutorial.isCore = isCore;
                }

                // Set Complete condition
                Tutorial.SetCompleteCondition setCompleteCondition = (Tutorial.SetCompleteCondition)EditorGUILayout.EnumPopup(new GUIContent("Set Complete Condition", "When should this tutorial be marked as completed?"), tutorial.setCompleteCondition);
                if (setCompleteCondition != tutorial.setCompleteCondition) {
                    Undo.RegisterCompleteObjectUndo(_tm, "Reassign tutorial set complete condition");
                    tutorial.setCompleteCondition = setCompleteCondition;
                }

                // Reset On Complete
                bool resetOnComplete = EditorGUILayout.Toggle(new GUIContent("Reset On Complete", "Tutorial should be reset after complete, so another tutorial can be triggered. Only turn this off if you don't want any tutorial automacitally run after this tutorial."), tutorial.resetOnComplete);
                if (resetOnComplete != tutorial.resetOnComplete)
                {
                    Undo.RegisterCompleteObjectUndo(_tm, "Reassign reset tutorial on complete");
                    tutorial.resetOnComplete = resetOnComplete;
                }

                // Is completed and reset
                GUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField(new GUIContent("Completed", "Is the tutorial completed, this is local and saved in PlayerPrefs."), new GUIContent(tutorial.isCompleted.ToString(), ""));

                    GUI.backgroundColor = Color.green;

                    if (tutorial.isCompleted) {
                        if (GUILayout.Button("Reset", GUILayout.Width(60)))
                            tutorial.isCompleted = false;
                    } else {
                        if (GUILayout.Button("Complete", GUILayout.Width(60)))
                            tutorial.isCompleted = true;
                    }

                    GUI.backgroundColor = Color.white;
                }
                GUILayout.EndHorizontal();

                if (Application.isPlaying) {
                    if (GUILayout.Button("Debug Force Start")) {
                        TutorialManager.currentTutorial = tutorial;
                        tutorial.StartTutorial();
                    }
                }
            }

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            int skipCheckingStateMask = (int)(State)EditorGUILayout.EnumMaskField(new GUIContent("Skip Checking State", "A global state checking mask, to stop triggering ANY Tutorial in specific states."), (State)_tm.skipCheckingStateMask);
            if (skipCheckingStateMask < 0) // If "Everything" is set, set bits to be all-1s with length of State
                skipCheckingStateMask = (1 << System.Enum.GetValues(typeof(State)).Length) - 1;
            if (skipCheckingStateMask != _tm.skipCheckingStateMask) {
                Undo.RegisterCompleteObjectUndo(_tm, "Reassign skip checking state");
                _tm.skipCheckingStateMask = (int)skipCheckingStateMask;
            }

            // Debug mode
            GUILayout.BeginHorizontal();
            {
                bool debugMode = EditorGUILayout.Toggle(new GUIContent("Debug Mode", "Would not set tutorials to completed if this enabled."), _tm.debugMode);
                if (debugMode != _tm.debugMode) {
                    Undo.RegisterCompleteObjectUndo(_tm, "Reassign debug mode");
                    _tm.debugMode = debugMode;
                }
                GUILayout.Label("If ON, tutorials won't be set completed");
            }
            GUILayout.EndHorizontal();

            // Debug info
            if (TutorialManager.currentTutorial != null) {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                EditorGUILayout.LabelField("Current Tutorial", TutorialManager.currentTutorial.ToString());
            }

            // Validate tutorials
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            GUI.backgroundColor = Color.green;
            if (GUILayout.Button(new GUIContent("Validate All Tutorials", "Check all prefab references are validate in all tutorials.")))
                _confirmValidate = true;
            GUI.backgroundColor = Color.white;

        }

        serializedObject.Update();
    }

    private void RearrangeIds()
    {
        for (int i = 0; i < _tm.tutorials.Count; i++)
            _tm.tutorials[i].id = i + 1;
    }

    private void ValidateTutorials()
    {
        _invalidateTaskList = new List<TutorialTask>();
        foreach (Tutorial t in _tm.tutorials) {
            if (!string.IsNullOrEmpty(t.sceneName) && EditorSceneManager.GetActiveScene().name != t.sceneName)
                EditorSceneManager.OpenScene("Assets/Scenes/" + t.sceneName + ".unity");
            if (t.prefab == null)
                continue;
            foreach (Transform tf in t.prefab.transform) {
                foreach (TutorialTask tt in tf.GetComponents<TutorialTask> ()) {
                    if (!tt.IsValidate())
                        _invalidateTaskList.Add(tt);
                }
            }
            // Destroy the spawned tutorial panel
            TutorialPanel tp = GameObject.FindObjectOfType<TutorialPanel>();
            if (tp != null)
                Destroy(tp.gameObject);
        }
    }

}