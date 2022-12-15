using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadingManager
{
    public const string LEVEL_SELECTION_SCENE = "Level Selection";
    public const string INTRO_CUTSCENE_SCENE = "IntroCutScene";

    public static void ReloadLevel()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }

    public static void LoadMainMenuLevel()
    {
        Debug.Log("Loading MainMenu Scene");
        new WaitForSeconds(1);
        SceneManager.LoadScene("MainMenu");
    }

    public static void LoadScene(string sceneName) {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.completed += (_) => { Time.timeScale = 1; };
    }

    public static void LoadFirstScene() {
        string firstSceneToLoad = PlayerData.Instance.HasPlayedBefore ? LEVEL_SELECTION_SCENE : INTRO_CUTSCENE_SCENE;
        LoadScene(firstSceneToLoad);
    }

    public static void LoadLevelSelection() {
        LoadScene(LEVEL_SELECTION_SCENE);
    }
}
