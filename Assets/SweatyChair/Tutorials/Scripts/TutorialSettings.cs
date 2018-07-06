using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TutorialSettings", menuName = "SweatyChair/Tutorial Settings")]
public class TutorialSettings : ScriptableObjectSingleton<TutorialSettings>
{
	public string canvasPath = "";
}