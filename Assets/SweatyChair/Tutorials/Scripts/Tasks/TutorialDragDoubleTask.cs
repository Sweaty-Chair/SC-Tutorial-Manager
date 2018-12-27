/// <summary>
/// Tutorial on teaching drag with TWO fingers.
/// </summary>

using UnityEngine;
using System.Collections;

namespace SweatyChair
{
	
    public class TutorialDragDoubleTask : TutorialDragTask
	{

		public override bool DoStart()
		{
			if (!base.DoStart())
				return false;
			// Input touches may be carried from last tutorial step, do a init record here
			if (Input.touchCount == 2)
				_dragStartPosition = (Input.GetTouch(0).position + Input.GetTouch(1).position) / 2;
			return true;
		}

		public override void SetupTutorialPanel()
		{
			base.SetupTutorialPanel();
			tutorialPanel.SetHandAnimation("HandDragDouble");
		}

		protected override void DoUpdate()
		{
			float draggedDistance = 0;

			if (Input.touchCount >= 2) {

				Touch touchOne = Input.GetTouch(0);
				Touch touchTwo = Input.GetTouch(1);

				if (touchTwo.phase == TouchPhase.Began) // Reset the drag start position
					_dragStartPosition = (touchOne.position + touchTwo.position) / 2;

				if ((accumulateDistance && (touchOne.phase == TouchPhase.Moved || touchTwo.phase == TouchPhase.Moved)) ||
					(touchOne.phase == TouchPhase.Ended || touchTwo.phase == TouchPhase.Ended)) {

					Vector2 centerPosition = (touchOne.position + touchTwo.position) / 2;
					Vector2 dragVector = centerPosition - _dragStartPosition;
					if (accumulateDistance)
						_dragStartPosition = centerPosition;

					if (constraintDirection) {
						draggedDistance = Vector2.Dot(dragVector, _dragDirection);
						if (inverseDirection)
							draggedDistance = Mathf.Abs(draggedDistance);
					} else {
						draggedDistance = dragVector.magnitude;
					}

				}

			}

			#if UNITY_EDITOR
			if (Input.GetMouseButton(0) && Input.GetMouseButton(1))
			if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
				_dragStartPosition = (Vector2)Input.mousePosition;
			if ((accumulateDistance && Input.GetMouseButton(0) && Input.GetMouseButton(1)) ||
				((Input.GetMouseButtonUp(0) && Input.GetMouseButton(1)) || (Input.GetMouseButtonUp(1) && Input.GetMouseButton(0)))) {

				Vector2 dragVector = (Vector2)Input.mousePosition - _dragStartPosition;
				if (accumulateDistance)
					_dragStartPosition = (Vector2)Input.mousePosition;

				if (constraintDirection) {
					draggedDistance = Vector2.Dot(dragVector, _dragDirection);
					if (inverseDirection)
						draggedDistance = Mathf.Abs(draggedDistance);
				} else {
					draggedDistance = dragVector.magnitude;
				}
			}
			#endif

			if (draggedDistance == 0)
				return;

			if (accumulateDistance)
				_accumulatedDistance += draggedDistance;
			else
				_accumulatedDistance = draggedDistance;

			if (_accumulatedDistance >= _dragThreshold)
				DoComplete();

		}

	}

}