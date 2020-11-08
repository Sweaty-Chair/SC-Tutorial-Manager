using UnityEngine;
using UnityEngine.UI;

namespace SweatyChair.UI
{

	/// <summary>
	/// A tutorial panel, this is instantiated in runtime.
	/// </summary>
	public class TutorialPanel : Panel
	{

		// Prefab path in Resources folder
		public const string RESOURCES_PREFAB_PATH = "TutorialPanel";

		private static TutorialPanel _instance;
		// Cached canvas used for comparing to new canvas and deciding whether to create a new Tutorial Panel.
		private static Canvas _cachedCanvas;

		// Parts GameObjects
		[SerializeField] private GameObject _backgroundMaskGO, _handGO, _characterGO;
		// Background Mask
		[SerializeField] private Image _backgroundImage;
		// Hand
		[SerializeField] private RectTransform _handHolderRF;
		[SerializeField] private Animation _handAnimation;
		// Content
		[SerializeField] private RectTransform _textRF;
		[SerializeField] private Text _contentText;
		// Character dialogue
		[SerializeField] private RawImage _characterImage;
		[SerializeField] private Text _characterText;
		[SerializeField] private Animation _characterAnim;

		private string _defaultHandAnimationClipName;

		// Get TutorialPanel instance, instantiate one if there has no tutorial panel been created.
		public static TutorialPanel current {
			get {
				if (_instance != null) {
					return _instance;
				} else {
#if UNITY_EDITOR
					// In editor, try find the already spawned panel first
					TutorialPanel tmp = FindObjectOfType<TutorialPanel>();
					if (tmp != null)
						return tmp;
#endif
					_cachedCanvas = null;
					if (!string.IsNullOrEmpty(TutorialSettings.current.canvasPath)) { // Find the specified Canvas
						_cachedCanvas = TransformUtils.GetComponentByPath<Canvas>(TutorialSettings.current.canvasPath);
					} else if (PanelManager.instance != null) {
						_cachedCanvas = PanelManager.instance.GetComponent<Canvas>();
					}
					if (_cachedCanvas == null) // Try find the default Canvas object
						_cachedCanvas = TransformUtils.GetComponentByPath<Canvas>("Canvas");
					if (_cachedCanvas == null) // Find the first Canvas
						_cachedCanvas = FindObjectOfType<Canvas>();
					if (_cachedCanvas != null) {
						GameObject go = _cachedCanvas.gameObject.AddChild(RESOURCES_PREFAB_PATH, false);
						if (go != null) {
							go.name = "TutorialPanel"; // Remove "(Clone)"
							_instance = go.GetComponent<TutorialPanel>();
							return _instance;
						}
					} else {
						Debug.LogError("TutorialPanel:current - Cannot find Canvas");
					}
					return null;
				}
			}
		}

		public override void Init()
		{
			base.Init();

			_defaultHandAnimationClipName = _handAnimation.clip.name;

			InitTransform();

			// All default off
			ToggleBackgroundMask(false);
			ToggleHand(false);
			SetText(string.Empty);
			ToggleCharacter(false);
		}

		/// <summary>
		/// Creates a tutorial panel on the Canvas that the target is rendering onto, else gets TutorialPanel instance or instantiates one on the first found Canvas.
		/// </summary>
		/// <param name="targetTF">Target Transform to create a TutorialPanel for.</param>
		public static TutorialPanel GetCurrent(Transform targetTF)
		{
			if (targetTF != null) {
				Canvas targetCanvas = targetTF.GetComponentInParent<Canvas>();
				if (targetCanvas != _cachedCanvas) {
					GameObject go = targetCanvas.gameObject.AddChild(RESOURCES_PREFAB_PATH);
					if (go)
						return go.GetComponent<TutorialPanel>();
				}
			}
			return current;
		}

		public override void Toggle(bool doShow)
		{
			base.Toggle(doShow);
			if (!doShow) {
				ToggleBackgroundMask(false);
				ToggleHand(false);
				ToggleCharacter(false);
			} else {
				InitTransform();
			}
		}

		private void InitTransform()
		{
			// When instantiating prefabs using Tutorial, all rect transform settings are messed up. So here we are fixing that
			RectTransform selfRectTF = transform as RectTransform;
			if (selfRectTF == null) {
				Debug.LogErrorFormat("'{0}' : Unable to set rectTransform size, our transform is not a rect transform", gameObject.name);
				return;
			}

			// Reset our scale
			selfRectTF.localScale = Vector3.one;

			// Plus our anchored position
			selfRectTF.anchorMin = Vector2.zero;
			selfRectTF.anchorMax = Vector2.one;
			selfRectTF.anchoredPosition = Vector2.zero;
			selfRectTF.sizeDelta = Vector2.zero;
			selfRectTF.offsetMin = Vector2.zero;
			selfRectTF.offsetMax = Vector2.zero;
		}

		#region Background Mask and Spotlight

		public void ToggleBackgroundMask(bool isShown, int alpha = -1)
		{
			if (_backgroundImage == null)
				return;
			_backgroundMaskGO.SetActive(isShown);
			if (isShown) {
				if (alpha == -1)
					alpha = 168;
				_backgroundImage.color = new Color(0, 0, 0, alpha / 255f);
			}
		}

		public Button GetBackgroundButton()
		{
			if (_backgroundImage != null)
				return _backgroundImage.GetComponent<Button>();
			return null;
		}

		#endregion

		#region Hand

		public void ToggleHand(bool isShown)
		{
			_handGO.SetActive(isShown);
		}

		public void SetHandPosition(Vector3 handPosition)
		{
			_handHolderRF.position = handPosition;
		}

		public Vector3 GetHandLocalPosition()
		{
			return _handHolderRF.localPosition;
		}

		public void SetHandTransform(Vector3 handLocalPosition, Vector3 handLocalRotation, Vector3 handLocalScale)
		{
			_handHolderRF.localPosition = handLocalPosition;
			_handHolderRF.localRotation = Quaternion.Euler(handLocalRotation);
			_handHolderRF.localScale = handLocalScale;
			_handHolderRF.SetAsLastSibling();
		}

		public void SetHandAnimation(string clipName)
		{
			if (string.IsNullOrEmpty(clipName))
				return;

			TimeManager.Start(AnimationUtils.Play(_handAnimation, clipName, false));
		}

		#endregion

		#region Text

		public void SetTextPosition(Vector3 textPosition)
		{
			_textRF.position = textPosition;
		}

		public Vector3 GetTextLocalPosition()
		{
			return _textRF.localPosition;
		}

		public void SetTextTransform(Vector3 textLocalPosition, Vector3 textLocalRotation, Vector2 textSize)
		{
            _textRF.gameObject.SetActive(true); //Make sure the gameobject is enabled if string was empty before.
			_textRF.localPosition = textLocalPosition;
			_textRF.localRotation = Quaternion.Euler(textLocalRotation);
			_textRF.sizeDelta = textSize;
			_textRF.SetAsLastSibling();
		}

		public void SetText(string text)
		{
			_contentText.text = LocalizeUtils.Get(TermCategory.Tutorial, text);
		}

		public void SetText(string text, Color color, int fontSize, TextAnchor alignment)
		{
			_contentText.text = LocalizeUtils.Get(TermCategory.Tutorial, text);
			_contentText.color = color;
			_contentText.alignment = alignment;
			_contentText.fontSize = fontSize;
            if (string.IsNullOrEmpty(text)) //If string is empty hide the textbox.
            {
                _textRF.gameObject.SetActive(false);
            }
		}

		#endregion

		#region Character Dialogue

		public void ToggleCharacter(bool isShown, string text = "", string animNameToPlay = "")
		{
			if (_characterGO != null) {
				_characterGO.SetActive(isShown);
				if (isShown) {
					_characterText.text = LocalizeUtils.Get(TermCategory.Tutorial, text);
					if (_characterAnim != null) {
						if (_characterAnim.GetClip(animNameToPlay))
							TimeManager.Start(AnimationUtils.Play(_characterAnim, animNameToPlay, false));
						else
							_characterAnim.Play();
					}
				}
			}
		}

		#endregion

		public void Reset(bool hideBackgroundMask = true)
		{
			if (hideBackgroundMask)
				ToggleBackgroundMask(false);
			ToggleHand(false);
			SetHandAnimation(_defaultHandAnimationClipName);
			SetText(string.Empty);
		}

		public static void DestroyInstance()
		{
			if (_instance != null)
				Destroy(_instance.gameObject);
		}

		private void OnDisable()
		{
			StopAllCoroutines();
		}

	}

}