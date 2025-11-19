using UnityEngine;
using UnityEngine.UI;
using _Project.Code.Core.ServiceLocator;

public class MainMenuTest : Menu<MainMenu>
{
    [Header("Main Menu Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button howToPlayButton;

    [Header("Panels")]
    [SerializeField] private GameObject levelSelectPanel;
    [SerializeField] private GameObject howToPlayPanel;

    [Header("Level Select Buttons")]
    [SerializeField] private Button level1Button;
    [SerializeField] private Button level2Button;
    [SerializeField] private Button level3Button;

    protected override void Awake()
    {
        base.Awake();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        playButton.onClick.AddListener(OpenLevelSelect);
        settingsButton.onClick.AddListener(OpenSettings);
        quitButton.onClick.AddListener(QuitGame);
        howToPlayButton.onClick.AddListener(OpenHowToPlay);

        level1Button.onClick.AddListener(() => LoadLevel("GameSceneTest1"));
        level2Button.onClick.AddListener(() => LoadLevel("GameSceneTest2"));
        level3Button.onClick.AddListener(() => LoadLevel("GameSceneTest3"));
    }

    private void OpenLevelSelect()
    {
        howToPlayPanel.SetActive(false);
        levelSelectPanel.SetActive(true);
    }

    private void OpenHowToPlay()
    {
        levelSelectPanel.SetActive(false);
        howToPlayPanel.SetActive(true);
    }

    private void LoadLevel(string sceneName)
    {
        ServiceLocator.Get<SceneService>().LoadScene(sceneName);
    }

    private void OpenSettings()
    {
        SettingsMenu.Instance.OpenMenu();
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}