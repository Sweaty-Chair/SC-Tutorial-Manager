using UnityEngine;

namespace SweatyChair
{
	
	/// <summary>
	/// Call to TutorialAssistant when satifying a condition.
	/// </summary>
    public class TutorialCompleteStepTrigger : EventTrigger
	{
	
        protected override void CallEvent()
        {
            TutorialManager.CompleteManualSteps();
        }

	}

}