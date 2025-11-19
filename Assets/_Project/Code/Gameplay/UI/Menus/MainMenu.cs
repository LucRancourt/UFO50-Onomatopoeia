using UnityEngine;
using UnityEngine.UI;

using _Project.Code.Core.ServiceLocator;

public class MainMenu : Menu<MainMenu>
{
    // Variables
    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;

    private DictationStatusChecker checker;

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
        checker = new DictationStatusChecker();
        checker.Initialize();

        Invoke(nameof(Check), 1.0f);
    }

    private void Check()
    {
        bool success = checker.WasSuccessful;

        checker.Destroy();
        checker = null;

        if (!success)
        {
            DictationRequestMenu.Instance.Open();
            return;
        }

        Invoke(nameof(OpenLevelSelect), 1.0f);
    }

    private void OpenLevelSelect()
    {
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
