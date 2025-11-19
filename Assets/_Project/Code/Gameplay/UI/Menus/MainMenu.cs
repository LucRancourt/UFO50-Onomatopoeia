using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

using _Project.Code.Core.ServiceLocator;

public class MainMenu : Menu<MainMenu>
{
    // Variables
    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;

    private bool isDictationEnabled = false;


    // Functions
    protected override void Awake()
    {
        base.Awake();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        playButton.onClick.AddListener(StartGame);
        settingsButton.onClick.AddListener(OpenSettings);
        quitButton.onClick.AddListener(QuitGame);
    }

    private void StartGame()
    {
        DictationRecognizer testDictation = new DictationRecognizer();
        testDictation.DictationError += (string error, int hresult) => { OnDictationError(); } ;

        isDictationEnabled = true;

        testDictation.Start();

        Invoke(nameof(OpenLevelSelect), 0.2f);
    }

    private void OnDictationError()
    {
        isDictationEnabled = false;
        DictationRequestMenu.Instance.Open();
    }

    private void OpenLevelSelect()
    {
        if (!isDictationEnabled) return;

        ServiceLocator.Get<SceneService>().LoadScene("Start Timing Test");
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
