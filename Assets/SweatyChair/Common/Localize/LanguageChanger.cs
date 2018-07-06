using UnityEngine;
using SweatyChair;

public class LanguageChanger : MonoBehaviour
{

	[SerializeField] private Language _lanuage;

	void Awake()
	{
		LocalizeUtils.SetLanguage(_lanuage);
	}

}