using UnityEngine;
using UnityEditor;
using System.Collections;
using SweatyChair;

[CustomEditor(typeof(TutorialClickButtonTask))]
public class TutorialClickButtonTaskEditor : TutorialHighlightTaskEditor
{

    private TutorialClickButtonTask _tcbt {
		get { return target as TutorialClickButtonTask; }
	}

}