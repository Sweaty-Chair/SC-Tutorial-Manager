using UnityEngine;

namespace SweatyChair
{

	/// <summary>
	/// Works like TutorialHighlight, plus animate the higlighted UI object.
	/// </summary>
	public class TutorialAnimateTask : TutorialHighlightTask
	{

		public AnimationClip animationClip;
		public string animationPath;

		private Animation _addedAnimation;

		public override bool Init()
		{
			if (!base.Init())
				return false;
			if (targetTF.gameObject.layer != LayerMask.NameToLayer("UI")) {
				Debug.LogError(string.Format("{0}:{1}:Init - Target object is not an UI, targetPath={2}", name, GetType(), targetPath));
				return false;
			}
			return true;
		}

		public override void SetupTutorialPanel()
		{
			base.SetupTutorialPanel();
			if (Application.isPlaying) // Skip in editor
			PlayAnimation();
		}

		private void PlayAnimation()
		{
			if (animationClip == null)
				return;
		
			Transform animationTF = targetTF;
			if (!string.IsNullOrEmpty(animationPath))
				animationTF = TransformUtils.GetTransformByPath(animationPath);

			if (animationTF == null)
				return;

			Animation animation = animationTF.GetComponent<Animation>();

			if (animation != null) { // There's already animation component attached

				bool containClip = false;
				animation.clip = animationClip;
				animation.Play();

			} else {

				if (_addedAnimation != null)
					Destroy(_addedAnimation);
			
				_addedAnimation = animationTF.gameObject.AddComponent<Animation>();
				_addedAnimation.clip = animationClip;
				_addedAnimation.playAutomatically = true;

				// Re-active the animation
				animationTF.gameObject.SetActive(false);
				animationTF.gameObject.SetActive(true);

			}
		}

		public override void Reset()
		{
			base.Reset();

			if (_addedAnimation != null)
				Destroy(_addedAnimation);
		}

	}

}