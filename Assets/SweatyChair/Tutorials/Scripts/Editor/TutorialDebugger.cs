using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using SweatyChair;

public static class TutorialDebugger
{

    [MenuItem("Debug/Tutorial/Complete All Tutorials")]
    public static void CompleteAllTutorials()
    {
        ToggleAllTutorialsComplete(true);
    }

    [MenuItem("Debug/Tutorial/Reset All Tutorials")]
    public static void ResetAllTutorials()
    {
        ToggleAllTutorialsComplete(false);
    }

    private static void ToggleAllTutorialsComplete(bool isCompleted)
    {
        GameObject tmGO = GameObject.Find("TutorialManager");
        bool isInScene = true;

        if (tmGO == null)
        {
            tmGO = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/SweatyChair/Tutorials/TutorialManager.prefab");
            isInScene = false;
        }

        TutorialManager tm = tmGO.GetComponent<TutorialManager>();
        foreach (Tutorial t in tm.tutorials)
            t.isCompleted = isCompleted;

        Selection.activeObject = tmGO;

        if (isInScene)
            Debug.Log("All tutorials marked as " + (isCompleted ? "" : "not ") + "completed, please apply and save the TutorialManager prefab.");
        else
            Debug.Log("All tutorials marked as " + (isCompleted ? "" : "not ") + "completed in TutorialManager prefab, make sure you check it out.");
    }

    [MenuItem("Debug/Tutorial/Start First Tutorial")]
    public static void StartFirstTutorial()
    {
        TutorialManager.StartTutorial(1);
    }

    [MenuItem("Debug/Tutorial/Print Parameters")]
    public static void PrintParameters()
    {
        Debug.Log("TutorialManager.currentTutorial=" + TutorialManager.currentTutorial);
        Debug.Log("TutorialManager.isFirstTutorialCompleted=" + TutorialManager.isFirstTutorialCompleted);
        Debug.Log("TutorialManager.areCoreTutorialsCompleted=" + TutorialManager.areCoreTutorialsCompleted);
        Debug.Log("TutorialManager.totalCoreTutorials=" + TutorialManager.totalCoreTutorials);
        Debug.Log("TutorialManager.totalCoreTutorialsCompleted=" + TutorialManager.totalCoreTutorialsCompleted);
        Debug.Log("TutorialManager.isAllTutorialsCompleted=" + TutorialManager.areAllTutorialsCompleted);
    }

}