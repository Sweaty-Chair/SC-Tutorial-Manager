using UnityEngine;
using UnityEngine.UI;

namespace SweatyChair
{
	
	/// <summary>
	/// Call to TutorialAssistant when satifying a condition.
	/// </summary>
    public abstract class EventTrigger : MonoBehaviour
	{
	
		public enum UnityEvent
		{
			CallAfterSeconds,
			CallAfterFrames,
			Start,
			Update,
			FixedUpdate,
			LateUpdate,
			OnDestroy,
			OnEnable,
			OnDisable,
			OnControllerColliderHit,
			OnParticleCollision,
			OnJointBreak,
			OnBecameInvisible,
			OnBecameVisible,
			OnTriggerEnter,
			OnTriggerExit,
			OnTriggerStay,
			OnCollisionEnter,
			OnCollisionExit,
			OnCollisionStay,
			OnTriggerEnter2D,
			OnTriggerExit2D,
			OnTriggerStay2D,
			OnCollisionEnter2D,
			OnCollisionExit2D,
			OnCollisionStay2D,
			OnClick
		}

        [SerializeField] private UnityEvent _targetEvent = UnityEvent.Start;

        [Tooltip("Used only when target event set to CallAfterSeconds")]
        [SerializeField] private float _afterSeconds = 0;
        [Tooltip("Used only when target event set to CallAfterFrames")]
        [SerializeField] private int _afterFrames = 0;

		private int _startFrame;
		private float _startTime;

        protected abstract void CallEvent();

        private void CheckCallEvent(UnityEvent unityEvent)
		{
			if (unityEvent == _targetEvent) {
                CallEvent();
				_afterFrames = 0;
				_afterSeconds = 0.0f;
				_startTime = float.PositiveInfinity;
				_startFrame = int.MinValue;
			}
		}

		public void Start()
		{
			_startTime = Time.unscaledTime;
			_startFrame = _afterFrames;
			CheckCallEvent(UnityEvent.Start);
			if (_targetEvent == UnityEvent.OnClick) {
				Button b = GetComponent<Button>();
				if (b != null)
					b.onClick.AddListener(OnClick);
			}
		}

		public void Update()
		{
			CheckCallEvent(UnityEvent.Update);
			CallAfterSeconds();
			CallAfterFrames();
		}

		private void CallAfterFrames()
		{
			if (_afterFrames > 0 && (_startFrame + _afterFrames) <= Time.frameCount)
				CheckCallEvent(UnityEvent.CallAfterFrames);
		}

		private void CallAfterSeconds()
		{
			if ((_startTime + _afterSeconds) <= Time.unscaledTime)
				CheckCallEvent(UnityEvent.CallAfterSeconds);
		}

		public void OnDisable()
		{
			CheckCallEvent(UnityEvent.OnDisable);
		}

		public void OnEnable()
		{
			CheckCallEvent(UnityEvent.OnEnable);
		}

		public void OnDestroy()
		{
			CheckCallEvent(UnityEvent.OnDestroy);
		}

		public void FixedUpdate()
		{
			CheckCallEvent(UnityEvent.FixedUpdate);
		}

		public void LateUpdate()
		{
			CheckCallEvent(UnityEvent.LateUpdate);
		}

		public void OnControllerColliderHit()
		{
			CheckCallEvent(UnityEvent.OnControllerColliderHit);
		}

		public void OnParticleCollision()
		{
			CheckCallEvent(UnityEvent.OnParticleCollision);
		}

		public void OnJointBreak()
		{
			CheckCallEvent(UnityEvent.OnJointBreak);
		}

		public void OnBecameInvisible()
		{
			CheckCallEvent(UnityEvent.OnBecameInvisible);
		}

		public void OnBecameVisible()
		{
			CheckCallEvent(UnityEvent.OnBecameVisible);
		}

		public void OnTriggerEnter()
		{
			CheckCallEvent(UnityEvent.OnTriggerEnter);
		}

		public void OnTriggerExit()
		{
			CheckCallEvent(UnityEvent.OnTriggerExit);
		}

		public void OnTriggerStay()
		{
			CheckCallEvent(UnityEvent.OnTriggerStay);
		}

		public void OnCollisionEnter()
		{
			CheckCallEvent(UnityEvent.OnCollisionEnter);
		}

		public void OnCollisionExit()
		{
			CheckCallEvent(UnityEvent.OnCollisionExit);
		}

		public void OnCollisionStay()
		{
			CheckCallEvent(UnityEvent.OnCollisionStay);
		}

		public void OnTriggerEnter2D()
		{
			CheckCallEvent(UnityEvent.OnTriggerEnter2D);
		}

		public void OnTriggerExit2D()
		{
			CheckCallEvent(UnityEvent.OnTriggerExit2D);
		}

		public void OnTriggerStay2D()
		{
			CheckCallEvent(UnityEvent.OnTriggerStay2D);
		}

		public void OnCollisionEnter2D()
		{
			CheckCallEvent(UnityEvent.OnCollisionEnter2D);
		}

		public void OnCollisionExit2D()
		{
			CheckCallEvent(UnityEvent.OnCollisionExit2D);
		}

		public void OnCollisionStay2D()
		{
			CheckCallEvent(UnityEvent.OnCollisionStay2D);
		}

		public void OnClick()
		{
			CheckCallEvent(UnityEvent.OnClick);
		}

	}

}