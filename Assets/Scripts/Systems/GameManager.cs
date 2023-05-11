using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool IsPaused { get; private set; }

    public event Action OnPauseGame;
    public event Action OnStartLevel;


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
        Initialize();
    }

    private void Initialize()
    {
        Cursor.lockState = CursorLockMode.Confined;
        IsPaused = true;
    }

    // Update is called once per frame
    void Update()
    {
        KeyboardPause();
    }

    private void KeyboardPause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            IsPaused = !IsPaused;
            OnPauseGame?.Invoke();
        }
    }

    public void LoadLevel(int buildIndex)
    {

    }

    public void StartGame()
    {

    }
    public void PauseResumeGame()
    {
        IsPaused = !IsPaused;
        Time.timeScale = Time.timeScale == 0 ? 0f : 1f;
        OnPauseGame?.Invoke();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
