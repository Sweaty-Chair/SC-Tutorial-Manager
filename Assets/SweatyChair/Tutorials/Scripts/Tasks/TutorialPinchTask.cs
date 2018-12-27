/// <summary>
/// Tutorial on teaching pinch (mostly for zooming).
/// </summary>

using UnityEngine;
using System.Collections;

namespace SweatyChair
{
	
    public class TutorialPinchTask : TutorialShowPanelTask
	{

		public enum PinchAction
		{
			PinchIn,
			PinchOut,
			Both
		}

		// The pinching action to check
		public PinchAction checkPinchAction = PinchAction.Both;

		// Pinch distance
		public float pinchDistance = 100;
		// Pinch distance is absolute screen pixel if false, otherwise pinchDistance is a percentage in [0,1]
		public bool scaleDistanceWithScreenWidth = false;
		// Player must finish in ONE pinch, or can be completed in few pinches (with accumulated distance)
		public bool accumulateDistance = true;

		private float _pinchThreshold;

		private float _pinchStartDistance;
		private float _accumulatedDelta;

		public override bool Init()
		{
			if (scaleDistanceWithScreenWidth && pinchDistance >= 1) {
				Debug.LogErrorFormat("{0}:{1}:Init - Pinch distance is not yet probably, it should be less than 1 when scaling with screen width.", name, GetType());
				return false;
			} else if (!scaleDistanceWithScreenWidth && pinchDistance < 1) {
				Debug.LogErrorFormat("{0}:{1}:Init - Pinch distance is not yet probably, it should be larger than 1 when not scaling with screen width.", name, GetType());
				return false;
			}

			_pinchThreshold = scaleDistanceWithScreenWidth ? pinchDistance * Screen.width : pinchDistance;

			return base.Init();
		}

		public override bool DoStart()
		{
			if (!base.DoStart())
				return false;
			// Input touches may be carried from last tutorial step, do a init record here
			if (Input.touchCount == 2)
				_pinchStartDistance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
			return true;
		}

		public override void SetupTutorialPanel()
		{
			base.SetupTutorialPanel();
			tutorialPanel.SetHandAnimation("HandPinch");
		}

		protected override void DoUpdate()
		{
			float pinchDelta = 0;

			if (Input.touchCount == 2) {
				float pinchedDistance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
				if (Input.GetTouch(1).phase == TouchPhase.Began)
					_pinchStartDistance = pinchedDistance;
				pinchDelta = _pinchStartDistance - pinchedDistance;
			}

			#if UNITY_EDITOR
			float deadZone = 0.01f;
			if (Input.GetAxis("Mouse ScrollWheel") < -deadZone || Input.GetAxis("Mouse ScrollWheel") > deadZone) {
				float delta = Input.GetAxis("Mouse ScrollWheel");
				pinchDelta = delta * 100 * 15; // 15 is hardcoded here, this should match the pinch speed, e.g. zoom speed for mouse wheel in zoom camera
			}
			#endif

			if (pinchDelta == 0)
				return;

			bool isPassed = false;

			switch (checkPinchAction) {
				case PinchAction.PinchIn:
					if (accumulateDistance)
						_accumulatedDelta += pinchDelta < 0 ? pinchDelta : 0;
					else
						_accumulatedDelta = pinchDelta;
					isPassed = _accumulatedDelta < 0 && -_accumulatedDelta > _pinchThreshold;
					break;
				case PinchAction.PinchOut:
					if (accumulateDistance)
						_accumulatedDelta += pinchDelta > 0 ? pinchDelta : 0;
					else
						_accumulatedDelta = pinchDelta;
					isPassed = _accumulatedDelta > 0 && _accumulatedDelta > _pinchThreshold;
					break;
				case PinchAction.Both:
					if (accumulateDistance)
						_accumulatedDelta += Mathf.Abs(pinchDelta);
					else
						_accumulatedDelta = pinchDelta;
					isPassed = Mathf.Abs(pinchDelta) > _pinchThreshold;
					break;
			}

			if (isPassed)
				DoComplete();

		}

	}

}