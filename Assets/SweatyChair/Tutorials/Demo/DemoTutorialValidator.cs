using UnityEngine;

namespace SweatyChair
{
	
    // This is a demo tutorial validator, a validator has custom logic on 
    // checking if a tutorial should be execute, as well as what to do when
    // tutorial start and complete
	public class DemoTutorialValidator : TutorialValidator
	{

        public override bool IsValidated()
        {
            // If true, execute the tutorial, put logic here such as player reach level 2
            return true;
        }

		public override void OnTutorialStart()
		{
            Debug.Log("Demo tutorial started.");
            // Do anything here, such as give player some items that they cau use in the tutorial
		}

		public override void OnTutorialComplete()
		{
            Debug.Log("Demo tutorial completed.");
            // Do anything here, such as reward the player for completing the tutorial
		}

	}

}