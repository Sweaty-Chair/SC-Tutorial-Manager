using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SweatyChair
{
	
	public class TimeScaleEnabler : MonoBehaviour
	{

		private float _lastTimeScale = 0;

		private void OnEnable()
		{
			_lastTimeScale = Time.timeScale;
			Time.timeScale = 1;
		}

		private void OnDisable()
		{
			Time.timeScale = _lastTimeScale;
		}

	}

}