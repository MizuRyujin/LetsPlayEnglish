using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/* 
    Scene build index order is important:

        - Main Menu always index 1;
        - Loading Screen index 2;
        - Gameplay scene index 3;

    Levels should be placed from index 4 upwards
*/

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager Instance { get; private set; }

    public int ScenesInBuild => SceneManager.sceneCountInBuildSettings;
    public event Action<float> WhileLoading;
    private List<AsyncOperation> _scenesToLoad;
    private int _testLevelIndex;

    [SerializeField] private bool _isTesting;
    [SerializeField] private int _levelToTest;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }
        _scenesToLoad = new List<AsyncOperation>();

#if UNITY_ANDROID && !UNITY_EDITOR
        _isTesting = false;
#endif

    }

    private void Start()
    {
        if (_isTesting)
        {
            LoadLevelToTest(_levelToTest);
            return;
        }

        LoadMainMenu();
    }

    private void LoadLevelToTest(int levelToTest)
    {
        // Default goes to first level
        _testLevelIndex = 4 + levelToTest;
        if (_testLevelIndex > SceneManager.sceneCountInBuildSettings - 1)
        {
            _testLevelIndex = 4;
            Debug.LogWarning("No level with that index exists. Please check " +
            "the build settings");
        }
        StartGame(_testLevelIndex);
    }

    private void LoadMainMenu()
    {
        _scenesToLoad.Add(SceneManager.LoadSceneAsync(1));
    }

    /// <summary>
    /// Starts the game. To be used from main menu, start button
    /// </summary>
    public void StartGame(int sceneIndex)
    {
        _scenesToLoad.Clear(); // Clear any previous operations

        _scenesToLoad.Add(SceneManager.LoadSceneAsync(2)); // Loading Screen
        _scenesToLoad.Add(SceneManager.LoadSceneAsync(
                                    sceneIndex, LoadSceneMode.Additive)); // Specified Level
        _scenesToLoad.Add(SceneManager.LoadSceneAsync(
                                    3, LoadSceneMode.Additive)); // GameplayScene
        StartCoroutine(TrackLoadProgress());
    }

    /// <summary>
    /// Method to be called from a button in the main menu. Should have a 
    /// transition before the actual method call.
    /// </summary>
    /// <param name="sceneIndex">Scene to be loaded.</param>
    public void LoadLevel(int sceneIndex)
    {
        if (sceneIndex > SceneManager.sceneCountInBuildSettings)
        {
            ReturnToMenu();
        }
        SceneManager.UnloadSceneAsync(sceneIndex - 1);
        SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive).completed +=
            _ => SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(sceneIndex));
    }

    public void RestartLevel(int sceneIndex)
    {
        if (_isTesting)
        {
            sceneIndex = _testLevelIndex;
        }

        SceneManager.UnloadSceneAsync(sceneIndex);
        SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive).completed +=
            _ => SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(sceneIndex));
    }

    public void ReturnToMenu()
    {
        _scenesToLoad.Clear();

        _scenesToLoad.Add(SceneManager.LoadSceneAsync(2));
        _scenesToLoad.Add(SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive));
        StartCoroutine(TrackLoadProgress());
    }

    private IEnumerator TrackLoadProgress()
    {
        float progress = 0f;
        for (int i = 0; i < _scenesToLoad.Count; i++)
        {
            while (!_scenesToLoad[i].isDone)
            {
                progress += (0.01f + _scenesToLoad[i].progress) / _scenesToLoad.Count;
                WhileLoading?.Invoke(progress);
                yield return null;
            }
        }
        // if (SceneManager.GetSceneByBuildIndex(3).isLoaded) // If gameplay scene is loaded
        // {
        //     SceneManager.UnloadSceneAsync(2).completed += _ =>
        //         SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(3));
        // }
        // else
        // {
            SceneManager.UnloadSceneAsync(2).completed += _ =>
                Resources.UnloadUnusedAssets();
        // }
        GameManager.Instance.PauseResumeGame();
    }
}
