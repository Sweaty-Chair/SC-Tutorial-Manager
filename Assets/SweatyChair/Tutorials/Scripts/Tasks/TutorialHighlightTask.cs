using UnityEngine;
using UnityEngine.UI;
using SweatyChair.UI;
using System.ComponentModel;

namespace SweatyChair
{

	/// <summary>
	/// Works like TutorialShowPanel, plus highlight and follow an in-game UI/world object.
	/// </summary>
	public class TutorialHighlightTask : TutorialShowPanelTask
	{

		// Highlight target path in scene
		public string targetPath;
		// Check and skip this step if target not active
		public bool skipIfTargetInactive = false;
		// Clone a target GameObject, used if target is within a grid
		public bool cloneTarget = true;
		// Target follow the target clone OnUpdate
		public bool targetFollowCloneOnUpdate;
		// Target can catch raycasts
		public bool targetCanCatchRaycast = true;

		// By default, put original target to tutorial panel, turn this on if you want the opposite
		public bool putCloneToTutorialPanelInstead;

		// Hand and text follow the target position
		public bool handFollowTarget = true, textFollowTarget = true;
		// Hand and text follow the target position on Update, or OnEnable only as default
		public bool handFollowOnUpdate = false, textFollowOnUpdate = false;
		// Disable buttons
		public string[] disableButtonPaths = new string[0];

		// Do not reset highlighed object on complete
		public bool doNotResetHighlightedObjectOnComplete = false;
		// Remove highlighted object panel on complete
		public bool removeHighlightedObjectPanelOnComplete = false;

		// Check target iv valide in initialization
		protected bool checkTarget = true;
		// Traget Transform obtained from targetPath
		protected Transform targetTF;

		// Disable Colliders array from disableColliderPaths
		protected Button[] _disableButtons = new Button[0];

		// Cached original and clone parameters
		protected Transform _targetParentTF;
		protected int _targetSiblingIndex;
		protected GameObject _targetCloneGO;
		protected bool _cachedRaycastTarget;

		public override bool Init()
		{
			if (checkTarget) {

				if (string.IsNullOrEmpty(targetPath)) {
					Debug.LogErrorFormat("{0}:{1}:Init - Target object not set, targetPath={2}", name, GetType(), targetPath);
					return false;
				}

				targetTF = TransformUtils.GetTransformByPath(targetPath);
				if (targetTF == null) {
					Debug.LogErrorFormat("{0}:{1}:Init - Target transform not found, targetPath={2}", name, GetType(), targetPath);
					return false;
				}

				if (skipIfTargetInactive && !targetTF.gameObject.activeInHierarchy) {
					Debug.LogErrorFormat("{0}:{1}:Init - Target object not active", name, GetType());
					return false;
				}

			}

			_disableButtons = new Button[disableButtonPaths.Length];
			for (int i = 0, imax = disableButtonPaths.Length; i < imax; i++) {
				Button b = TransformUtils.GetComponentByPath<Button>(disableButtonPaths[i]);
				if (b == null)
					Debug.LogErrorFormat("{0}:{1}:Init - Disable collider not found, disableButtonPaths={2}", name, GetType(), disableButtonPaths[i]);
				_disableButtons[i] = b;
			}

			tutorialPanel = TutorialPanel.current;
			if (tutorialPanel == null) {
				Debug.LogErrorFormat("{0}:{1}:Init - TutorialPanel instance not found, targetPath={2}", name, GetType(), targetPath);
				return false;
			}

			tutorialPanel.Reset(!showBackgroundMask);

			return true;
		}

		protected override void DoUpdate()
		{
			if (targetFollowCloneOnUpdate)
				RepositionTarget();
			if (showHand && handFollowTarget && handFollowOnUpdate)
				RepositionHand();
			if (showText && textFollowTarget && textFollowOnUpdate)
				RepositionText();
		}

		public override void SetupTutorialPanel()
		{
			base.SetupTutorialPanel();

			RepositionHand();
			RepositionText();

			tutorialPanel.ToggleCharacter(false);

			SetupTarget();
		}

		private void RepositionTarget()
		{
			if (_targetCloneGO != null) {
				if (putCloneToTutorialPanelInstead)
					 _targetCloneGO.transform.position = targetTF.position;
				else
					targetTF.position = _targetCloneGO.transform.position;
			}
		}

		protected virtual void RepositionHand()
		{
			if (showHand) {
				Vector3 newHandLocalPosition = handLocalPosition;
				if (handFollowTarget) { // Calculate the hand position if follow target
					if (isTargetUI) {
						newHandLocalPosition = GetFollowTargetUIOffset() + handLocalPosition;
					} else {
						tutorialPanel.SetHandPosition(GetFollowTargetUIPosition());
						newHandLocalPosition = tutorialPanel.GetHandLocalPosition() + handLocalPosition;
					}
				}
				tutorialPanel.SetHandTransform(newHandLocalPosition, handLocalRotation, handLocalScale);
			}
		}

		private void RepositionText()
		{
			if (showText) {
				Vector3 newTextLocalPosition = textLocalPosition;
				if (textFollowTarget) { // Calculate the text position if follow target
					if (isTargetUI) {
						newTextLocalPosition = GetFollowTargetUIOffset() + textLocalPosition;
					} else {
						tutorialPanel.SetTextPosition(GetFollowTargetUIPosition());
						newTextLocalPosition = tutorialPanel.GetTextLocalPosition() + textLocalPosition;
					}
				}
				tutorialPanel.SetTextTransform(newTextLocalPosition, textLocalRotation, textSize);
			}
		}

		private bool isTargetUI {
			get { return targetTF.gameObject.layer == LayerMask.NameToLayer("UI"); }
		}

		private Vector3 GetFollowTargetUIOffset()
		{
			return tutorialPanel.transform.InverseTransformPoint(targetTF.position);
		}

		private Vector3 GetFollowTargetUIPosition()
		{
			Vector3 screenPointUnscaled = Camera.main.WorldToScreenPoint(targetTF.position);
			Canvas canvas = FindObjectOfType<Canvas>();
			return screenPointUnscaled / canvas.scaleFactor;
		}

		protected virtual void SetupTarget()
		{
			if (targetTF != null && targetTF.gameObject.layer == LayerMask.NameToLayer("UI")) { // Only do for UI

				_targetParentTF = targetTF.parent;
				RectTransform targetRT = targetTF.GetComponent<RectTransform>();
				_targetSiblingIndex = targetRT.GetSiblingIndex();

				if (cloneTarget && _targetCloneGO == null) { // Position the target clone to target's position
					_targetCloneGO = targetTF.parent.gameObject.AddChild(targetTF.gameObject);
					RectTransform targetCloneRT = _targetCloneGO.GetComponent<RectTransform>();
					if (targetCloneRT != null) {
						targetCloneRT.SetSiblingIndex(_targetSiblingIndex);
						targetCloneRT.sizeDelta = targetRT.sizeDelta;
					}
				}

				// Move target to tutorial panel
				if (targetRT != null) {
					Transform transformToMoveInTutorialPanel = targetTF;
					if (putCloneToTutorialPanelInstead) {
						transformToMoveInTutorialPanel = _targetCloneGO.transform;
						// Add canvas sorting
						Canvas canvas = transformToMoveInTutorialPanel.GetComponent<Canvas>();
						if (canvas != null) canvas.sortingOrder += 1000;
						// Remove TweenOnEnable
						TweenOnEnable tweenOnEnable = transformToMoveInTutorialPanel.GetComponent<TweenOnEnable>();
						
						if (tweenOnEnable != null) {
							tweenOnEnable.ForceCancelTween();
							Destroy(tweenOnEnable);
						}
					}
					transformToMoveInTutorialPanel.SetParent(tutorialPanel.transform, true);

					RectTransform rectTransformToMoveInTutorialPanel = transformToMoveInTutorialPanel.GetComponent<RectTransform>();
					rectTransformToMoveInTutorialPanel.SetSiblingIndex(1); // Just after background mask

					// Set if target should catch raycasts and store original state
					Graphic graphicToMoveInTutorialPanel = rectTransformToMoveInTutorialPanel.GetComponent<Graphic>();
					if (graphicToMoveInTutorialPanel != null) {
						_cachedRaycastTarget = graphicToMoveInTutorialPanel.raycastTarget;
						graphicToMoveInTutorialPanel.raycastTarget = targetCanCatchRaycast;
					}

					// Make sure it sets it to the right width.
					if (cloneTarget && _targetCloneGO != null) { // Position the target clone to target's position	
						RectTransform subTf = (putCloneToTutorialPanelInstead) ? _targetCloneGO.GetComponent<RectTransform>() : targetTF.GetComponent<RectTransform>();
						RectTransform coreTf = (putCloneToTutorialPanelInstead) ? targetTF.GetComponent<RectTransform>() : _targetCloneGO.GetComponent<RectTransform>();
						
						LayoutGroup parentGroup = coreTf.GetComponentInParent<LayoutGroup>();
						if(parentGroup != null)
							LayoutRebuilder.ForceRebuildLayoutImmediate(parentGroup.GetComponent<RectTransform>());

						subTf.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, coreTf.rect.width);
						subTf.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, coreTf.rect.height);

					}

					// Try copy the data from target to clone
					_targetCloneGO?.SendMessage("CopyFrom", targetTF.gameObject, SendMessageOptions.DontRequireReceiver);
				}

			}

			foreach (Button b in _disableButtons) {
				if (b != null)
					b.interactable = false;
			}
		}

		public override void Reset()
		{
			base.Reset();
			ResetTarget();
		}

		protected virtual void ResetTarget()
		{
			if (doNotResetHighlightedObjectOnComplete)
				return;

			if (_targetCloneGO != null)
				GameObjectUtils.Destroy(_targetCloneGO);
			if (_targetParentTF != null && !putCloneToTutorialPanelInstead) {
				targetTF.SetParent(_targetParentTF, true);
				RectTransform targetRT = targetTF.GetComponent<RectTransform>();
				if (targetRT != null) {
					targetRT.SetSiblingIndex(_targetSiblingIndex);
					// Reset target can catch raycast to original state
					Graphic targetGraphic = targetRT.GetComponent<Graphic>();
					if (targetGraphic)
						targetGraphic.raycastTarget = _cachedRaycastTarget;
				}
			}

			foreach (Button b in _disableButtons) {
				if (b != null)
					b.interactable = true;
			}
		}

	}

}