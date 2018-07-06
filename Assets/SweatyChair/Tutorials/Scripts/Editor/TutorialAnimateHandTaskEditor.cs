using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using SweatyChair;

[CustomEditor(typeof(TutorialAnimateHandTask))]
public class TutorialAnimateHandTaskEditor : TutorialShowPanelTaskEditor
{
    private TutorialAnimateHandTask _taht
    {
        get { return target as TutorialAnimateHandTask; }
    }

    protected override void OnPreviewSettingGUI()
    {
        string handAnim = EditorGUILayout.TextField("Hand animation to use", _taht.handAnim);
        if (handAnim != _taht.handAnim)
        {
            Undo.RegisterCompleteObjectUndo(_taht, "Reassign Hand Anim");
            _taht.handAnim = handAnim;
        }

        EditorGUI.indentLevel++;

        base.OnPreviewSettingGUI();
    }
}