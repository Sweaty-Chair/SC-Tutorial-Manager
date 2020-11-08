using UnityEngine;
using UnityEditor;
using System.Collections;

namespace SweatyChair
{

	[System.Serializable]
	[CustomEditor(typeof(TutorialPinchTask), true)]
	public class TutorialPinchTaskEditor : TutorialShowPanelTaskEditor
	{

		private TutorialPinchTask _topt {
			get { return target as TutorialPinchTask; }
		}

		protected override bool isCompletedTriggerEditable { get { return false; } }

		protected override TutoriaCompletelTrigger forceCompleteTrigger { get { return TutoriaCompletelTrigger.Auto; } }

		protected override void OnOtherSettingGUI()
		{
			TutorialPinchTask.PinchAction checkPinchAction = (TutorialPinchTask.PinchAction)EditorGUILayout.EnumPopup(new GUIContent("Check Pinch Action", "Which pinch action to complete this tutorial step."), _topt.checkPinchAction);
			if (checkPinchAction != _topt.checkPinchAction) {
				EditorUtility.SetDirty(_topt);
				Undo.RegisterCompleteObjectUndo(_topt, "Reassign Check Pinch Action");
				_topt.checkPinchAction = checkPinchAction;
			}

			if (_topt.scaleDistanceWithScreenWidth && _topt.pinchDistance >= 1)
				EditorGUILayout.HelpBox("Pinch distance should be less than 1 when scaling with screen width.", MessageType.Error);
			else if (!_topt.scaleDistanceWithScreenWidth && _topt.pinchDistance < 1)
				EditorGUILayout.HelpBox("Pinch distance should be more than 1 when not scaling with screen width.", MessageType.Error);

			float pinchDistance = EditorGUILayout.FloatField(new GUIContent("Pinch Distance", "Pinch distance, either absolute screen pixels or percentage to screen width."), _topt.pinchDistance);
			if (pinchDistance != _topt.pinchDistance) {
				EditorUtility.SetDirty(_topt);
				Undo.RegisterCompleteObjectUndo(_topt, "Reassign Pinch Distance");
				_topt.pinchDistance = pinchDistance;
			}

			bool scaleDistanceWithScreenWidth = EditorGUILayout.Toggle(new GUIContent("Scale Distance With Screen Width", "Pinch distance is absolute screen pixel if false, otherwise dragDistance is a percentage in [0,1]."), _topt.scaleDistanceWithScreenWidth);
			if (scaleDistanceWithScreenWidth != _topt.scaleDistanceWithScreenWidth) {
				EditorUtility.SetDirty(_topt);
				Undo.RegisterCompleteObjectUndo(_topt, "Reassign Scale Distance With Screen Width");
				_topt.scaleDistanceWithScreenWidth = scaleDistanceWithScreenWidth;
			}

			bool accumulateDistance = EditorGUILayout.Toggle(new GUIContent("Accumlate Distance", "Player must finish in ONE pinch, or can be completed in few pinches (with accumulated distance)"), _topt.accumulateDistance);
			if (accumulateDistance != _topt.accumulateDistance) {
				EditorUtility.SetDirty(_topt);
				Undo.RegisterCompleteObjectUndo(_topt, "Reassign Accumulate Distance");
				_topt.accumulateDistance = accumulateDistance;
			}

			OnDoNotResetOnCompleteSettingGUI();
		}

	}

}