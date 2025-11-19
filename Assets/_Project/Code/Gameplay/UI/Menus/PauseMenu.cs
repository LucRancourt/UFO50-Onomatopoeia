using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using _Project.Code.Core.ServiceLocator;
using _Project.Code.Core.Events;


public class PauseMenu : Menu<PauseMenu>
{
    // Variables
    [Header("Pause Settings")]
    [SerializeField] private GameObject pauseMenu;

    [Header("Buttons")]
    [SerializeField] private Button resumeGame;
    [SerializeField] private Button openSettingsMenu;
    [SerializeField] private Button returnToMainMenu;

    private AudioSource _musicSource;


    // Functions
    protected override void Awake()
    {
        base.Awake();

        pauseMenu.SetActive(false);

        resumeGame.onClick.AddListener(ResumeClicked);
        openSettingsMenu.onClick.AddListener(OpenSettingsMenu);
        returnToMainMenu.onClick.AddListener(ReturnToMainMenu);
    }

    private void Start()
    {
        EventBus.Instance.Subscribe<PauseInputEvent>(this, OnPauseInputEvent);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _musicSource = FindMusicSource();
    }

    private AudioSource FindMusicSource()
    {
        StartMusic sm = FindFirstObjectByType<StartMusic>();
        if (sm != null)
            return sm.audioSource;

        return null;
    }

    private void OnPauseInputEvent(PauseInputEvent evt)
    {
        if (Time.timeScale == 0.0f)
            UnPauseGame();
        else
            PauseGame();
    }

    private void PauseGame()
    {
        Time.timeScale = 0.0f;

        if (_musicSource != null)
            _musicSource.Pause();

        pauseMenu.SetActive(true);
    }

    private void ResumeClicked()
    {
        EventBus.Instance.Publish(new PauseInputEvent());
    }

    private void UnPauseGame()
    {
        pauseMenu.SetActive(false);

        Time.timeScale = 1.0f;

        if (_musicSource != null)
            _musicSource.UnPause();
    }

    private void OpenSettingsMenu()
    {
        SettingsMenu.Instance.OpenMenu();
    }

    private void ReturnToMainMenu()
    {
        UnPauseGame();
        ServiceLocator.Get<SceneService>().LoadScene("MainMenu");
    }
}
