/// <summary>
/// Tutorial to set hand animation manually.
/// </summary>

namespace SweatyChair
{
    public class TutorialAnimateHandTask : TutorialShowPanelTask
    {
        public string handAnim = "HandClick";

        public override void SetupTutorialPanel()
        {
            base.SetupTutorialPanel();
            tutorialPanel.SetHandAnimation(handAnim);
        }
    }
}