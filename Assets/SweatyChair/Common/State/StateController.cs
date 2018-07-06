using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace SweatyChair
{

    public class StateController : EventTrigger
    {

        [SerializeField] private State _targetState = State.None;

        protected override void CallEvent()
        {
            StateManager.Set(_targetState);
        }

    }

}