using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

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

    private DictationStatusChecker checker;
    
    protected override void Awake()
    {
        base.Awake();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        playButton.onClick.AddListener(StartDictationTest);
        settingsButton.onClick.AddListener(OpenSettings);
        quitButton.onClick.AddListener(QuitGame);
        howToPlayButton.onClick.AddListener(OpenHowToPlay);

        level1Button.onClick.AddListener(() => LoadLevel("JingleBells"));
        level2Button.onClick.AddListener(() => LoadLevel("CarelessWhisper"));
        level3Button.onClick.AddListener(() => LoadLevel("MiiChannel"));
    }

    private void StartDictationTest()
    {
        checker = new DictationStatusChecker();
        checker.Initialize();

        Invoke(nameof(CheckDictationStatus), 1.0f);
    }

    private void CheckDictationStatus()
    {
        bool success = checker.WasSuccessful;

        checker.Destroy();
        checker = null;

        if (!success)
        {
            DictationRequestMenu.Instance.Open();
            return;
        }

        Invoke(nameof(OpenLevelSelectAfterTest), 1.0f);
    }

    private void OpenLevelSelectAfterTest()
    {
        levelSelectPanel.SetActive(true);
        howToPlayPanel.SetActive(false);
    }

    private void OpenHowToPlay()
    {
        howToPlayPanel.SetActive(true);
        levelSelectPanel.SetActive(false);
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
