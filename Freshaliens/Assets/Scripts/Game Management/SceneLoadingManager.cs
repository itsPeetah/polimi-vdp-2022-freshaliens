    using System;
    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MenuManagement;
using UnityEngine.SceneManagement;

public class SceneLoadingManager
{
    public const string LEVEL_SELECTION_SCENE = "Level Selection";

    public static void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public static void LoadMainMenuLevel()
    {
        Debug.Log("Loading MainMenu Scene");
        new WaitForSeconds(1);
        SceneManager.LoadScene("MainMenu");
        MainMenu.Open();
    }

    public static void LoadScene(string sceneName) {
        SceneManager.LoadSceneAsync(sceneName);
    }

    public static void LoadLevelSelection() {
        LoadScene(LEVEL_SELECTION_SCENE);
    }
}
