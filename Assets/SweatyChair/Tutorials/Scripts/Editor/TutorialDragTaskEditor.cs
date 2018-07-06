using UnityEngine;
using UnityEditor;
using System.Collections;
using SweatyChair;

[System.Serializable]
[CustomEditor(typeof(TutorialDragTask), true)]
public class TutorialDragTaskEditor : TutorialHighlightTaskEditor
{

	private TutorialDragTask _tod {
		get { return target as TutorialDragTask; }
	}

	protected override bool isCompletedTriggerEditable { get { return false; } }

    protected override TutoriaCompletelTrigger forceCompleteTrigger { get { return TutoriaCompletelTrigger.Auto; } }

	protected override void OnOtherSettingGUI()
	{
		bool constraintDirection = EditorGUILayout.Toggle(new GUIContent("Constraint Direction", "Constraint direction, can drag toward anywhere if false."), _tod.constraintDirection);
		if (constraintDirection != _tod.constraintDirection) {
			Undo.RegisterUndo(_tod, "Reassign Constraint Direction");
			_tod.constraintDirection = constraintDirection;
		}

		if (constraintDirection) {

			float dragDirectionAngle = EditorGUILayout.FloatField(new GUIContent("Drag Direction Angle", "Angle direction to drag, e.g. 135 is toward top-left."), _tod.dragDirectionAngle);
			if (dragDirectionAngle != _tod.dragDirectionAngle) {
				Undo.RegisterUndo(_tod, "Reassign Drag Direction Angle");
				_tod.dragDirectionAngle = dragDirectionAngle;
			}

			bool inverseDirection = EditorGUILayout.Toggle(new GUIContent("Inverse Direction", "Also include for inverse direction or not."), _tod.inverseDirection);
			if (inverseDirection != _tod.inverseDirection) {
				Undo.RegisterUndo(_tod, "Reassign Inverse Direction");
				_tod.inverseDirection = inverseDirection;
			}

		}

		if (_tod.scaleDistanceWithScreenWidth && _tod.dragDistance >= 1)
			EditorGUILayout.HelpBox("Drag distance should be less than 1 when scaling with screen width.", MessageType.Error);
		else if (!_tod.scaleDistanceWithScreenWidth && _tod.dragDistance < 1)
			EditorGUILayout.HelpBox("Drag distance should be more than 1 when not scaling with screen width.", MessageType.Error);

		float dragDistance = EditorGUILayout.FloatField(new GUIContent("Drag Distance", "Drag distance, either absolute screen pixels or percentage to screen width."), _tod.dragDistance);
		if (dragDistance != _tod.dragDistance) {
			Undo.RegisterUndo(_tod, "Reassign Drag Distance");
			_tod.dragDistance = dragDistance;
		}

		bool scaleDistanceWithScreenWidth = EditorGUILayout.Toggle(new GUIContent("Scale Distance With Screen Width", "Drag distance is absolute screen pixel if false, otherwise dragDistance is a percentage in [0,1]."), _tod.scaleDistanceWithScreenWidth);
		if (scaleDistanceWithScreenWidth != _tod.scaleDistanceWithScreenWidth) {
			Undo.RegisterUndo(_tod, "Reassign Scale Distance With Screen Width");
			_tod.scaleDistanceWithScreenWidth = scaleDistanceWithScreenWidth;
		}

		bool accumulateDistance = EditorGUILayout.Toggle(new GUIContent("Accumlate Distance", "Player must finish in ONE drag, or can be completed in few drags (with accumulated distance)"), _tod.accumulateDistance);
		if (accumulateDistance != _tod.accumulateDistance) {
			Undo.RegisterUndo(_tod, "Reassign Accumulate Distance");
			_tod.accumulateDistance = accumulateDistance;
		}

		OnDisableButtonsSettingGUI();
		OnDoNotResetOnCompleteSettingGUI();
	}

}