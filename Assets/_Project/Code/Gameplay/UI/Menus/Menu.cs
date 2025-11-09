using UnityEngine;
using UnityEngine.UI;

using _Project.Code.Core.Audio;
using _Project.Code.Core.General;


public class Menu<T> : Singleton<T> where T : MonoBehaviour
{
    // Variables 
    [SerializeField] private AudioCue clickSFX;

    private Button[] _menuButtons;


    // Functions
    protected override void Awake()
    {
        base.Awake();

        _menuButtons = GetComponentsInChildren<Button>(true);

        foreach (Button button in _menuButtons)
        {
            button.onClick.AddListener(PlayClickSFX);
        }
    }

    private void PlayClickSFX()
    {
        AudioManager.Instance.PlaySound(clickSFX);
    }
}
