using UnityEngine;

namespace SweatyChair
{

	/// <summary>
	/// Tutorial task on teaching drag with ONE finger, with hightlighting a particular UI object in the scene.
	/// </summary>
	public class TutorialDragTask : TutorialHighlightTask
	{

		// Constraint direction, can drag toward anywhere if false
		public bool constraintDirection = true;
		// Angle direction to drag, e.g. 135 is toward top-left
		public float dragDirectionAngle = 135;
		// Also include for inverse direction or not
		public bool inverseDirection = false;

		// Drag distance
		public float dragDistance = 100;
		// Drag distance is absolute screen pixel if false, otherwise dragDistance is a percentage in [0,1]
		public bool scaleDistanceWithScreenWidth = false;
		// Player must finish in ONE drag, or can be completed in few drags (with accumulated distance)
		public bool accumulateDistance = true;

		protected Vector2 _dragDirection;
		protected float _dragThreshold;

		protected Vector2 _dragStartPosition;
		protected float _accumulatedDistance;

		public override bool Init()
		{
			if (scaleDistanceWithScreenWidth && dragDistance >= 1) {
				Debug.LogErrorFormat("{0}:{1}:Init - Drag distance is not yet probably, it should be less than 1 when scaling with screen width.", name, GetType());
				return false;
			} else if (!scaleDistanceWithScreenWidth && dragDistance < 1) {
				Debug.LogErrorFormat("{0}:{1}:Init - Drag distance is not yet probably, it should be larger than 1 when not scaling with screen width.", name, GetType());
				return false;
			}

			_dragDirection = new Vector2(Mathf.Cos(Mathf.Deg2Rad * dragDirectionAngle), Mathf.Sin(Mathf.Deg2Rad * dragDirectionAngle));
			_dragThreshold = scaleDistanceWithScreenWidth ? dragDistance * Screen.width : dragDistance;

			checkTarget = !string.IsNullOrEmpty(targetPath);

			return base.Init();
		}

		public override void SetupTutorialPanel()
		{
			base.SetupTutorialPanel();
			tutorialPanel.SetHandAnimation("HandDrag");
		}

		protected override void DoUpdate()
		{
			if (Input.GetMouseButtonDown(0))
				_dragStartPosition = (Vector2)Input.mousePosition;

			if ((accumulateDistance && Input.GetMouseButton(0)) || Input.GetMouseButtonUp(0)) { // Either check whenever pressed for accumlate, or check on button up only

				Vector2 dragVector = (Vector2)Input.mousePosition - _dragStartPosition;
				_dragStartPosition = (Vector2)Input.mousePosition;

				float draggedDistance = 0;
				if (constraintDirection) {
					draggedDistance = Vector2.Dot(dragVector, _dragDirection);
					if (inverseDirection)
						draggedDistance = Mathf.Abs(draggedDistance);
				} else {
					draggedDistance = dragVector.magnitude;
				}

				if (accumulateDistance)
					_accumulatedDistance += draggedDistance;
				else
					_accumulatedDistance = draggedDistance;

				if (_accumulatedDistance >= _dragThreshold)
					DoComplete();

			}
		}

	}

}