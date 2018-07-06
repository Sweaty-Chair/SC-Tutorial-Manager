# SweatyChair Tutorial Manager

## About
A tutorial manager system used in Unity3D games, with the aim to be used across all different games withou coding or with minimal coding. This is been used in [No Humanity](http://www.sweatychair.com/no-humanity), [3DTD - Chicka Invasion](http://www.sweatychair.com/3dtd) and [Friends Must Dungeon](http://www.sweatychair.com/friends-must-dungeon).

## Usage
The tutorial system mainly consists 5 scripts: TutorialManager, TutorialValidator, TutorialInstance and TutorialStep, TutorialPanel:
- [TutorialManager](Assets/SweatyChair/Tutorials/Scripts/TutorialManager.cs): A persistent instance that appears in all scenes, used to create and configure tutorials in Editor.
- [TutorialValidator](Assets/SweatyChair/Tutorials/Scripts/TutorialValidator.cs): A base class that check if the conditions are satisfied to execute a tutorial. Customized validator  should be inherited from this class and implement its own logics. Each tutorial should have 1 or none (if manually trigger) TutorialValidator. All validators are check once per second, avoid putting too complianted login in `IsValidated()` for performance.
- [TutorialStep](Assets/SweatyChair/Tutorials/Scripts/TutorialStep.cs): A controller to keep track of each step of a tutorial. Each tutorial must have at least 1 step.
- [TutorialTask](Assets/SweatyChair/Tutorials/Scripts/TutorialTask.cs): A base class to tell what a tutorial task should do. Each tutorial step has 1 or more tasks (in most case 1 step has 1 task). For example, a tutorial step can have 2 tasks: instantiate a button, and go to next step if the button is clicked. Children task classes include show a text on screen, ask to click a button, spawn a GameObject in game, etc. Customized task should be inherited from this class and implement its own logics.
- [TutorialPanel](Assets/SweatyChair/Tutorials/Scripts/TutorialPanel.cs)): An UI panel to show tutorial text, character, etc. This is only Unity UI compatiable and should be on the topest sorting order.

## Demo
A demo scene is located at [Demo](Assets/SweatyChair/Tutorials/Demo/) folder, open the [Demo.unity](Assets/SweatyChair/Tutorials/Demo/Demo.unity) scene and you can see how TutorialManager instance containing a tutorial. The tutorial has validator call DemoTutorialValidator which always return true in `IsValidated()` and executing the tutorial. A [DemoTutorial.prefab](Assets/SweatyChair/Tutorials/Demo/DemoTutorial.prefab) contains a number of TutorialSteps as children. Note that "Set Complete Condition" is set to "Do Not Set Complete" for demo purpose, so the tutorial will be start again after completion.

## Using with other SW plugins
- GameSave/GameSpark - Check the previous progress of a returning player and skip tutorials if needed. Each TutorialValidator should override `IsCompletedForReturnPlayer()` to determine should that tutorial be skipped (set completed) for the returning player. For example, skip first tutorial if player's already level 2. `IsCompletedForReturnPlayer()` could be same to `IsValidated()` in most cases but not neccessary.
- [StateManager](Assets/SweatyChair/Common/State/StateManager.cs) - Trigger tutorial by game state, or filter the check for particular game states for minimizing the check performance.

## TODO
- A detailed documentation for how to setup and configure settings, plus a guide on how to put custom scripts for tutorials.
- Another demo scene for drag and double drag (two fingers pan) tutorials.
- Merge the tutorial prefab into TutorialManager editor, so that each step can be created and updated in TutorialManager editor screen, instead of doing them in a prefab now.