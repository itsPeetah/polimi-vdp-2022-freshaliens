    using System;
    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MenuManagement;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private static int _nextLevel = 0;
    private static int _maxLevel = 2;
    private static int _levelPlayed = -1;

    private static string SceneName(int level)
    {
        // this should load the level to play
        return "Level" + level;
    }
    
    public static void LoadLevel(int level)
    {
        if (level < _maxLevel)
        {
            _levelPlayed = level;
            _nextLevel = level+1;
            SceneManager.LoadScene(SceneName(_levelPlayed));        
        }
    }

    public static void LoadFirstLevel()
    {
        Debug.Log("Loading First Level");
        _levelPlayed = 0;
        _nextLevel = 1;
        SceneManager.LoadScene(SceneName(_levelPlayed));
    }

    public static bool CompletedAllLevels()
    {
        return (_nextLevel == _maxLevel);
    }
    
    public static void LoadNextLevel()
    {
        if (_nextLevel < _maxLevel)
        {
            _levelPlayed = _nextLevel;
            _nextLevel = _nextLevel + 1;
            SceneManager.LoadScene(SceneName(_levelPlayed));
        }
    }

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
}
