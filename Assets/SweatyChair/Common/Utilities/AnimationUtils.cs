using System;
using System.Collections;
using UnityEngine;

namespace SweatyChair
{
    public static class AnimationUtils
    {
        public static IEnumerator Play(Animation animation, string clipName, bool useTimeScale = true, Action onComplete = null)
        {
            if (!useTimeScale)
            {
                AnimationState _currState = animation[clipName];
                bool isPlaying = true;
                float _progressTime = 0f;
                float _timeAtLastFrame = 0f;
                float _timeAtCurrentFrame = 0f;
                float deltaTime = 0f;
                
                animation.Play(clipName);
                _timeAtLastFrame = Time.realtimeSinceStartup;

                while (isPlaying)
                {
					if (!animation)
						break;

                    _timeAtCurrentFrame = Time.realtimeSinceStartup;
                    deltaTime = _timeAtCurrentFrame - _timeAtLastFrame;
                    _timeAtLastFrame = _timeAtCurrentFrame;

                    _progressTime += deltaTime;
                    _currState.normalizedTime = _progressTime / _currState.length;
                    animation.Sample();

                    if (_progressTime >= _currState.length)
                    {
                        if (_currState.wrapMode != WrapMode.Loop)
                            isPlaying = false;
                        else
                            _progressTime = 0.0f;
                    }

                    yield return new WaitForEndOfFrame();
                }

                yield return null;

                if (onComplete != null)
                    onComplete();
            }
            else
                animation.Play(clipName);
        }
    }
}