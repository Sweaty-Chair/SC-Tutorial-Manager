using UnityEngine;

namespace SweatyChair
{

	/// <summary>
	/// A base class for check if an tutorial can start.
	/// Override this for differnt condition and things to do in a tutorial.
	/// </summary>
	public abstract class TutorialValidator : MonoBehaviour
	{
		// Check the tutorial can start now
		public virtual bool IsValidated()
		{
			return true;
		}

		// Check the tutorial is already completed by return player, this may be similar to IsValidate but with less constraint such as State
		public virtual bool IsCompletedForReturnPlayer()
		{
			return false;
		}

		// Hard-code what should do after tutorial prefab is instantiated
		public virtual void OnTutorialStart()
		{
		}

		// Hard-code what should do after tutorial is completed
		public virtual void OnTutorialComplete()
		{
		}
	}

}