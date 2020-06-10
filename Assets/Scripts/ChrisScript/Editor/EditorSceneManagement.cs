using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEditor;

public class EditorSceneManagement : MonoBehaviour
{
    [MenuItem("SceneLoader/Load - Main Menu", priority = 0)]

    public static void LoadMainMenu()
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            EditorSceneManager.OpenScene("Assets/Scenes/Levels/MainMenu.unity");
    }

    [MenuItem("SceneLoader/Load Chris - Inventory Scene", priority = 50)]

   public static void LoadChris1()
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            EditorSceneManager.OpenScene("Assets/Scenes/Chris_Experiment/InventoryScene.unity");
    }
    [MenuItem("SceneLoader/Load Kenny - Grid Prototype Scene", priority = 100)]

    public static void LoadKenny1()
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            EditorSceneManager.OpenScene("Assets/Scenes/Kenny_Experiment/GridPrototypeScene.unity");
    }
    [MenuItem("SceneLoader/Load Tamiko - 2DArrScene Scene", priority = 150)]

    public static void LoadTamiko()
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            EditorSceneManager.OpenScene("Assets/Scenes/Tamiko_Experiments/2DArrScene.unity");
    }
}
