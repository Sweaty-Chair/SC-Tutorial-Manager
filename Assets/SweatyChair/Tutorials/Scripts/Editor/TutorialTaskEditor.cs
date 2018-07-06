using SweatyChair;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TutorialTask))]
public class TutorialTaskEditor : Editor
{

    private TutorialTask _tt {
        get { return target as TutorialTask; }
    }

    private bool hasPreviewSection {
        get {
            if (!GetType().IsSubclassOf(typeof(TutorialTaskEditor)))
                return false;
            var mi = GetType().GetMethod("OnPreviewSettingGUI");
            return mi == null || mi.DeclaringType == GetType();
        }
    }

    private bool hasOtherSettingSection {
        get {
            if (!GetType().IsSubclassOf(typeof(TutorialTaskEditor)))
                return false;
            var mi = GetType().GetMethod("OnOtherSettingGUI");
            return mi == null || mi.DeclaringType == GetType();
        }
    }

    public override void OnInspectorGUI()
    {
        OnTriggerSettingGUI();

        if (hasPreviewSection) {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            OnPreviewSettingGUI();
            OnPreviewGUI();
        }

        if (hasOtherSettingSection) {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            OnOtherSettingGUI();
        }

        serializedObject.Update();
    }

    #region Settings

    protected virtual bool isCompletedTriggerEditable { get { return true; } }

    protected virtual TutoriaCompletelTrigger forceCompleteTrigger { get { return TutoriaCompletelTrigger.Auto; } }

    protected void OnTriggerSettingGUI()
    {
        State skipState = (State)EditorGUILayout.EnumPopup(new GUIContent("Skip State", "Skip this step for specfic states"), _tt.skipState);
        if (skipState != _tt.skipState) {
            Undo.RegisterCompleteObjectUndo(_tt, "Reassign skip state");
            _tt.skipState = skipState;
        }

        if (isCompletedTriggerEditable) {
            TutoriaCompletelTrigger completeTrigger = (TutoriaCompletelTrigger)EditorGUILayout.EnumPopup(new GUIContent("Complete Trigger", "How this be triggered to complete"), _tt.completeTrigger);
            if (completeTrigger != _tt.completeTrigger) {
                Undo.RegisterCompleteObjectUndo(_tt, "Reassign complete trigger");
                _tt.completeTrigger = completeTrigger;
            }
        } else {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.EnumPopup(new GUIContent("Complete Trigger", string.Format("How this be triggered to complete, not editable for {0}.", target.GetType())), _tt.completeTrigger);
            _tt.completeTrigger = forceCompleteTrigger;
            EditorGUI.EndDisabledGroup();
        }

        float timeoutSeconds = EditorGUILayout.FloatField(new GUIContent("Timeout Seconds", "A timeout seconds to force complete this step, if complete trigger is not fired."), _tt.timeoutSeconds);
        if (timeoutSeconds != _tt.timeoutSeconds) {
            Undo.RegisterCompleteObjectUndo(_tt, "Reassign Timeout Seconds");
            _tt.timeoutSeconds = timeoutSeconds;
        }

        float minEnabledSeconds = EditorGUILayout.FloatField(new GUIContent("Min Enabled Seconds", "A minimum seconds between start and complete, use this to avoid flicker."), _tt.minEnabledSeconds);
        if (minEnabledSeconds != _tt.minEnabledSeconds) {
            Undo.RegisterCompleteObjectUndo(_tt, "Reassign Min Enabled Seconds");
            _tt.minEnabledSeconds = minEnabledSeconds;
        }
    }

    protected virtual void OnPreviewSettingGUI()
    {
    }

    protected virtual void OnOtherSettingGUI()
    {
    }

    #endregion

    #region Preview

    protected virtual bool isPreviewShown {
        get { return false; }
    }

    protected virtual bool canShowPreview {
        get { return true; }
    }

    protected virtual string cannotShowPreviewWarning {
        get { return string.Empty; }
    }

    protected virtual void OnPreviewGUI()
    {
        if (isPreviewShown) {
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("Remove Preview"))
                OnRemovePreviewClick();
            GUI.backgroundColor = Color.white;
        } else {
            if (canShowPreview) {
                GUI.backgroundColor = Color.green;
                if (GUILayout.Button("Preview"))
                    OnAddPreviewClick();
                GUI.backgroundColor = Color.white;
            } else {
                GUI.contentColor = Color.red;
                GUILayout.Label(cannotShowPreviewWarning);
                GUI.contentColor = Color.white;
            }
        }
    }

    protected virtual void OnRemovePreviewClick()
    {
    }

    protected virtual void OnAddPreviewClick()
    {
    }

    protected virtual void UpdatePreview()
    {
    }

    #endregion

}