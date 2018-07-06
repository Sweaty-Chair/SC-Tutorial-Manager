using UnityEngine;
using UnityEditor;
using System.Collections;
using SweatyChair;

[CustomEditor(typeof(TutorialAnimateTask))]
public class TutorialAnimateTaskEditor : TutorialHighlightTaskEditor
{

    private TutorialAnimateTask _tat {
		get { return target as TutorialAnimateTask
            ; }
	}

	protected override bool isCompletedTriggerEditable { get { return false; } }

    protected override TutoriaCompletelTrigger forceCompleteTrigger { get { return TutoriaCompletelTrigger.Auto; } }

	protected override void OnOtherSettingGUI()
	{
		AnimationClip addAnimationClip = EditorGUILayout.ObjectField("Add Animation Clip", _tat.animationClip, typeof(AnimationClip), false) as AnimationClip;
		if (addAnimationClip != _tat.animationClip) {
			Undo.RegisterUndo(_tat, "Reassign Add Animation Clip");
			_tat.animationClip = addAnimationClip;
		}

		if (addAnimationClip != null) {
			Transform animationTF = TransformUtils.GetTransformByPath(_tat.animationPath);
			string animationPath = TransformUtils.ToPath(EditorGUILayout.ObjectField("Animation Transform in Scene", animationTF, typeof(Transform), true) as Transform);
			if (!string.IsNullOrEmpty(_tat.animationPath) && animationTF == null) { // Set but targetTF not found, this may be because in wrong scene
				GUI.contentColor = Color.red;
				string customAnimationPath = EditorGUILayout.TextField("Not Found!", _tat.animationPath);
				if (customAnimationPath != _tat.animationPath) {
					Undo.RegisterUndo(_tat, "Reassign Custom Animation Path");
					_tat.animationPath = customAnimationPath;
				}
				GUI.contentColor = Color.white;
			}
			if (animationTF != null) {
				GUI.contentColor = Color.green;
				if (animationTF.GetComponent<Animation>() == null)
					GUILayout.Label("An Animation component will be added in runtime");
				else
					GUILayout.Label("The animation clip will be added to Animation in runtime");
				GUI.contentColor = Color.white;
			}
			if (!string.IsNullOrEmpty(animationPath) && animationPath != _tat.animationPath) {
				Undo.RegisterUndo(_tat, "Reassign Animation Path");
				_tat.animationPath = animationPath;
			}
		}

		OnDisableButtonsSettingGUI();
		OnDoNotResetOnCompleteSettingGUI();
	}

}