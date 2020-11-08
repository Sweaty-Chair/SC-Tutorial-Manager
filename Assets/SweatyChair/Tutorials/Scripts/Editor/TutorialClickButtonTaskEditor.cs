using UnityEditor;

namespace SweatyChair
{

	[CustomEditor(typeof(TutorialClickButtonTask))]
	public class TutorialClickButtonTaskEditor : TutorialHighlightTaskEditor
	{

		private TutorialClickButtonTask _tcbt => target as TutorialClickButtonTask;

	}

}