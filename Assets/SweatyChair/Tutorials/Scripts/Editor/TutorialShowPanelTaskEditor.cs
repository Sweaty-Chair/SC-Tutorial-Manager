using UnityEngine;
using UnityEditor;
using System.Collections;

namespace SweatyChair
{

	[CustomEditor(typeof(TutorialShowPanelTask))]
	public class TutorialShowPanelTaskEditor : TutorialTaskEditor
	{

		protected bool showHandSetting = true;
		protected bool showTextSetting = true;
		protected bool showBackgroundSetting = true;

		private TutorialShowPanelTask _tspt {
			get { return target as TutorialShowPanelTask; }
		}

		// Force complete trigger to be call
		protected override bool isCompletedTriggerEditable { get { return false; } }

		protected override TutoriaCompletelTrigger forceCompleteTrigger { get { return TutoriaCompletelTrigger.Auto; } }

		protected override void OnPreviewSettingGUI()
		{
			OnHandSettingGUI();
			OnTextSettingGUI();
			OnBackgroundMaskSettingGUI();
		}

		protected virtual void OnHandSettingGUI()
		{
			showHandSetting = EditorGUILayout.Foldout(showHandSetting, "Hand");
			EditorGUI.indentLevel++;

			if (showHandSetting) {

				bool showHand = EditorGUILayout.Toggle(new GUIContent("Show Hand", "Show the hand or not."), _tspt.showHand);
				if (showHand != _tspt.showHand) {
					EditorUtility.SetDirty(_tspt);
					Undo.RegisterCompleteObjectUndo(_tspt, "Reassign Show Hand");
					_tspt.showHand = showHand;
					UpdatePreview();
				}

				if (_tspt.showHand) {

					Vector3 handLocalPosition = EditorGUILayout.Vector3Field(new GUIContent("Local Position", "Local position of the hand."), _tspt.handLocalPosition);
					if (handLocalPosition != _tspt.handLocalPosition) {
						EditorUtility.SetDirty(_tspt);
						Undo.RegisterCompleteObjectUndo(_tspt, "Reassign Hand Local Position");
						_tspt.handLocalPosition = handLocalPosition;
						UpdatePreview();
					}

					Vector3 handLocalRotation = EditorGUILayout.Vector3Field(new GUIContent("Local Rotation", "Local rotation of the hand."), _tspt.handLocalRotation);
					if (handLocalRotation != _tspt.handLocalRotation) {
						EditorUtility.SetDirty(_tspt);
						Undo.RegisterCompleteObjectUndo(_tspt, "Reassign Hand Local Rotation");
						_tspt.handLocalRotation = handLocalRotation;
						UpdatePreview();
					}

					Vector3 handLocalScale = EditorGUILayout.Vector3Field(new GUIContent("Local Scale", "Local scale of the hand."), _tspt.handLocalScale);
					if (handLocalScale != _tspt.handLocalScale) {
						EditorUtility.SetDirty(_tspt);
						Undo.RegisterCompleteObjectUndo(_tspt, "Reassign Hand Local Scale");
						_tspt.handLocalScale = handLocalScale;
						UpdatePreview();
					}

				}

			}

			EditorGUI.indentLevel--;
		}

		protected virtual void OnTextSettingGUI()
		{
			showTextSetting = EditorGUILayout.Foldout(showTextSetting, "Text");
			EditorGUI.indentLevel++;

			if (showTextSetting) {

				bool showText = EditorGUILayout.Toggle(new GUIContent("Show Text", "Show text of not."), !string.IsNullOrEmpty(_tspt.text));
				if (showText == string.IsNullOrEmpty(_tspt.text)) {
					EditorUtility.SetDirty(_tspt);
					Undo.RegisterCompleteObjectUndo(_tspt, "Reassign Show Text");
					_tspt.text = showText ? "Tutorial text" : "";
					UpdatePreview();
				}

				if (showText) {

					Vector3 textLocalPosition = EditorGUILayout.Vector3Field(new GUIContent("Local Position", "Local position of the text."), _tspt.textLocalPosition);
					if (textLocalPosition != _tspt.textLocalPosition) {
						EditorUtility.SetDirty(_tspt);
						Undo.RegisterCompleteObjectUndo(_tspt, "Reassign Text Local Position");
						_tspt.textLocalPosition = textLocalPosition;
						UpdatePreview();
					}

					Vector3 textLocalRotation = EditorGUILayout.Vector3Field(new GUIContent("Local Rotation", "Local rotation of the text."), _tspt.textLocalRotation);
					if (textLocalRotation != _tspt.textLocalRotation) {
						EditorUtility.SetDirty(_tspt);
						Undo.RegisterCompleteObjectUndo(_tspt, "Reassign Text Local Rotation");
						_tspt.textLocalRotation = textLocalRotation;
						UpdatePreview();
					}

					Vector2 textSize = EditorGUILayout.Vector2Field(new GUIContent("Size", "Font size of the text."), _tspt.textSize);
					if (textSize != _tspt.textSize) {
						EditorUtility.SetDirty(_tspt);
						Undo.RegisterCompleteObjectUndo(_tspt, "Reassign Text Size");
						_tspt.textSize = textSize;
						UpdatePreview();
					}

					string text = EditorGUILayout.TextField(new GUIContent("Text", "Text string of the text."), _tspt.text);
					if (text != _tspt.text) {
						EditorUtility.SetDirty(_tspt);
						Undo.RegisterCompleteObjectUndo(_tspt, "Reassign Text String");
						_tspt.text = text;
						UpdatePreview();
					}

					Color textColor = EditorGUILayout.ColorField(new GUIContent("Text Color", "Text color of the text."), _tspt.textColor);
					if (textColor != _tspt.textColor) {
						EditorUtility.SetDirty(_tspt);
						Undo.RegisterCompleteObjectUndo(_tspt, "Reassign Text Color");
						_tspt.textColor = textColor;
						UpdatePreview();
					}

					int fontSize = EditorGUILayout.IntField(new GUIContent("Font Size", "Font size of the text."), _tspt.fontSize);
					if (fontSize != _tspt.fontSize) {
						EditorUtility.SetDirty(_tspt);
						Undo.RegisterCompleteObjectUndo(_tspt, "Reassign Font Size");
						_tspt.fontSize = fontSize;
						UpdatePreview();
					}

					TextAnchor alignment = (TextAnchor)EditorGUILayout.EnumPopup(new GUIContent("Alignment", "Alignment of the text."), _tspt.alignment);
					if (alignment != _tspt.alignment) {
						EditorUtility.SetDirty(_tspt);
						Undo.RegisterCompleteObjectUndo(_tspt, "Reassign Text Alignment");
						_tspt.alignment = alignment;
						UpdatePreview();
					}

				}

			}

			EditorGUI.indentLevel--;
		}

		protected virtual void OnBackgroundMaskSettingGUI()
		{
			showBackgroundSetting = EditorGUILayout.Foldout(showBackgroundSetting, "Background Mask");
			EditorGUI.indentLevel++;

			if (showBackgroundSetting) {

				bool showBackgroundMask = EditorGUILayout.Toggle(new GUIContent("Show Background Mask", "Show background mask or not."), _tspt.showBackgroundMask);
				if (showBackgroundMask != _tspt.showBackgroundMask) {
					EditorUtility.SetDirty(_tspt);
					Undo.RegisterCompleteObjectUndo(_tspt, "Reassign Show Background Mask");
					_tspt.showBackgroundMask = showBackgroundMask;
					UpdatePreview();
				}

				if (showBackgroundMask) {
					int alpha = (int)EditorGUILayout.Slider(new GUIContent("Alpha", "Background mask color alpha."), _tspt.alpha, 0, 255);
					if (alpha != _tspt.alpha) {
						EditorUtility.SetDirty(_tspt);
						Undo.RegisterCompleteObjectUndo(_tspt, "Reassign Background Mask Alpha");
						_tspt.alpha = alpha;
						UpdatePreview();
					}
					bool clickToComplete = EditorGUILayout.Toggle(new GUIContent("Click To Complete", "Click the background mask to complete the tutorial step or not."), _tspt.clickToComplete);
					if (clickToComplete != _tspt.clickToComplete) {
						EditorUtility.SetDirty(_tspt);
						Undo.RegisterCompleteObjectUndo(_tspt, "Reassign Show Background Click To Complete");
						_tspt.clickToComplete = clickToComplete;
					}

					if (clickToComplete) {
						float timeBeforeClickable = EditorGUILayout.FloatField(new GUIContent("Time Before Clickable", "Seconds disallow clicking background mask after OnEnable."), _tspt.timeBeforeClickable);
						if (timeBeforeClickable != _tspt.timeBeforeClickable) {
							EditorUtility.SetDirty(_tspt);
							Undo.RegisterCompleteObjectUndo(_tspt, "Reassign Show Background Time Before Clickable");
							_tspt.timeBeforeClickable = timeBeforeClickable;
						}
					}
				}
			}

			EditorGUI.indentLevel--;
		}

		protected override void OnOtherSettingGUI()
		{
			OnDoNotResetOnCompleteSettingGUI();
		}

		protected virtual void OnDoNotResetOnCompleteSettingGUI()
		{
			bool doNotResetPanelOnComplete = EditorGUILayout.Toggle(new GUIContent("Do Not Hide Panel On Complete", "Do not reset and hide the panel after tutorial step complete. This is mainly used when next tutorial step also use the tutorial panel, so avoid flicker of the tutorial panel."), _tspt.doNotHidePanelOnComplete);
			if (doNotResetPanelOnComplete != _tspt.doNotHidePanelOnComplete) {
				EditorUtility.SetDirty(_tspt);
				Undo.RegisterCompleteObjectUndo(_tspt, "Reassign Do Not Reset Panel On Complete");
				_tspt.doNotHidePanelOnComplete = doNotResetPanelOnComplete;
			}
		}

		#region Preview

		protected override bool isPreviewShown {
			get { return _tspt.tutorialPanel != null; }
		}

		protected override void OnRemovePreviewClick()
		{
			_tspt.ResetBackgroundMask();
			_tspt.DestroyTutorialPanel();
		}

		protected override void OnAddPreviewClick()
		{
			_tspt.Init();
			UpdatePreview();
		}

		protected override void UpdatePreview()
		{
			if (_tspt.tutorialPanel != null)
				_tspt.SetupTutorialPanel();
		}

		#endregion

	}

}