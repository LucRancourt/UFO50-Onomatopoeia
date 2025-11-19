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

    private bool isDictationEnabled = false;
    private DictationRecognizer dictationTest;

    protected override void Awake()
    {
        base.Awake();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        playButton.onClick.AddListener(StartDictationTest);
        settingsButton.onClick.AddListener(OpenSettings);
        quitButton.onClick.AddListener(QuitGame);
        howToPlayButton.onClick.AddListener(OpenHowToPlay);

        level1Button.onClick.AddListener(() => LoadLevel("Start Timing Test"));
        level2Button.onClick.AddListener(() => LoadLevel("CarelessWhisper"));
        level3Button.onClick.AddListener(() => LoadLevel("MiiChannel"));
    }

    private void StartDictationTest()
    {
        dictationTest = new DictationRecognizer();
        dictationTest.DictationError += (string error, int hresult) => { OnDictationError(); };

        isDictationEnabled = true;
        dictationTest.Start();

        Invoke(nameof(OpenLevelSelectAfterTest), 0.3f);
    }

    private void OnDictationError()
    {
        isDictationEnabled = false;
        DictationRequestMenu.Instance.Open();
    }

    private void OpenLevelSelectAfterTest()
    {
        if (!isDictationEnabled)
            return;

        dictationTest.Stop();
        dictationTest.Dispose();

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
