using UnityEngine;
using UnityEngine.SceneManagement;

using _Project.Code.Core.ServiceLocator;


public class SceneService : MonoBehaviourService
{
    [SerializeField] private string startScreen = "MainMenu";
    private LoadingScreen _loadingScreen;
    public string CurrentSceneName { get; private set; }


    public override void Initialize()
    {
        CurrentSceneName = SceneManager.GetActiveScene().name;

        SceneManager.sceneLoaded += SetActiveScene;
    }

    public void Start()
    {
        _loadingScreen = FindFirstObjectByType<LoadingScreen>();
        LoadScene(startScreen);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }

    public async void LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        if (_loadingScreen != null)
        {
            _loadingScreen.Activate();

            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / 0.9f);
                _loadingScreen.SetLoadValue(progress);

                await System.Threading.Tasks.Task.Yield();
            }

            _loadingScreen.Deactivate();
        }
        else
        {
            while (!operation.isDone)
            {
                await System.Threading.Tasks.Task.Yield();
            }
        }
    }

    public void ReloadCurrentScene()
    {
        LoadScene(CurrentSceneName);
    }

    private void SetActiveScene(Scene scene, LoadSceneMode mode)
    {
        SceneManager.SetActiveScene(scene);
    }

    public override void Dispose()
    {
    }
}
