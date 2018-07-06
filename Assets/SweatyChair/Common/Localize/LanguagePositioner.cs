using UnityEngine;

namespace SweatyChair
{

	[System.Serializable]
	public class LanguagePosition
	{
		public Language language = Language.English;
		public Vector3 localPosition = Vector3.zero;
	}

	public class LanguagePositioner : MonoBehaviour
	{

		[SerializeField] private LanguagePosition[] languagePositions;

		void Start()
		{
			Reposition();
		}

		[ContextMenu("Execute")]
		public void Reposition()
		{
			foreach (LanguagePosition languagePosition in languagePositions) {
				if (languagePosition.language == LocalizeUtils.currentLanguage) {
					transform.localPosition = languagePosition.localPosition;
					break;
				}
			}
		}
	
	}

}