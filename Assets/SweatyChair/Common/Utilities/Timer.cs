using System.Collections;
using System;
using UnityEngine;

namespace SweatyChair
{
    [Serializable]
    public class Timer : IDisposable
    {
        public Action onComplete;

		/// <summary>
		/// Duration used when the timer is Started, Stopped or Reset. (Do not get this confused with 'initialDuration')
		/// </summary>
		public float duration = 1f;

        private float _duration = 1f;
		/// <summary>
		/// Initial duration of the current timer.
		/// </summary>
		public float initialDuration { get { return _duration; } }
        
        private float _timer = 1f;
        private bool _isPaused = true;

        public float timeLeft { get { return _timer; } }

        public float elapsedTime { get { return _duration - _timer; } }

        public Timer(float duration = 1f, Action onComplete = null, bool useTimeManager = true)
        {
            this.duration = duration;
            this.onComplete = onComplete;
            Stop();

            if (useTimeManager)
                TimeManager.AddManagedTimer(this);
        }

        /// <summary>
        /// Resets the timer and starts counting down.
        /// </summary>
        public void Start()
        {
            Reset();
            Resume();
        }

        /// <summary>
        /// Pauses and resets the timer.
        /// </summary>
        public void Stop()
        {
            _isPaused = true;
            Reset();
        }

        /// <summary>
        /// Resets the timer without pausing it.
        /// </summary>
        public void Reset()
        {
            _duration = duration;
            _timer = _duration;
        }

		/// <summary>
		/// Interpolates the timer between its initial duration and 0 by factor.
		/// </summary>
		/// <param name="factor">Interpolant factor (between 0 and 1)</param>
		public void Sample(float factor)
		{
			_timer = Mathf.Lerp(_duration, 0f, Mathf.Clamp01(factor));
			Update();
		}

        public void Pause()
        {
            _isPaused = true;
        }

        public void Resume()
        {
            _isPaused = false;
        }

        public void TogglePaused()
        {
            if (_isPaused)
                Resume();
            else
                Pause();
        }

		public void TogglePaused(bool isPaused)
		{
			_isPaused = isPaused;
		}

        public void SetOnCompleteCallback(Action onComplete)
        {
            this.onComplete = onComplete;
        }

		/// <summary>
		/// Updates the current state of the timer. Must be called every frame (eg. in a MonoBehaviour's Update) for accurate time-keeping.
		/// </summary>
        public void Update()
        {
            if (!_isPaused && _timer > 0f)
            {
                _timer -= Time.deltaTime;

                if (_timer <= 0f)
                {
					_timer = 0f;
                    if (onComplete != null)
                        onComplete();

					Pause();
                }
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    TimeManager.RemoveManagedTimer(this);
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Timer() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}