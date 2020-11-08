using UnityEditor;
using UnityEngine;

namespace SweatyChair
{

	[CustomEditor(typeof(TutorialOverrideClickButtonTask))]
	public class TutorialOverrideClickButtonTaskEditor : TutorialHighlightTaskEditor
	{
		#region Variables

		private SerializedProperty _buttonEventProp;

		private TutorialOverrideClickButtonTask _tocbt {
			get { return target as TutorialOverrideClickButtonTask; }
		}

		#endregion

		#region OnEnable / Disable

		protected void OnEnable()
		{
			_buttonEventProp = serializedObject.FindProperty("onButtonClickOverride");
		}

		#endregion

		#region Override Editor Drawing

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorUtils.DrawScriptField<TutorialClickButtonTask>(_tocbt);
			base.OnInspectorGUI();
			OnButtonOverrideSettingGUI();

			serializedObject.ApplyModifiedProperties();
		}

		protected virtual void OnButtonOverrideSettingGUI()
		{
			EditorUtils.HorizontalLine(Color.grey);
			GUILayout.Label("On Button Click Override");
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField(_buttonEventProp, true);
			EditorGUI.indentLevel--;
		}

		#endregion

	}

}
