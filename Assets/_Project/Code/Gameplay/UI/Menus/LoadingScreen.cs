using UnityEngine;
using UnityEngine.UI;


public class LoadingScreen : MonoBehaviour
{
    private GameObject _loadingScreen;
    private Slider _loadingBar;


    private void Awake()
    {
        _loadingScreen = transform.GetChild(0).gameObject;
        _loadingBar = GetComponentInChildren<Slider>();
    }

    public void Activate()
    {
        SetLoadValue(0.0f);
        _loadingScreen.SetActive(true);
    }

    public void SetLoadValue(float value)
    {
        _loadingBar.value = value;
    }

    public void Deactivate()
    {
        _loadingScreen.SetActive(false);
    }

}
