using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    [SerializeField] Button _button;
    
    void OnEnable()
    {
        _button.gameObject.SetActive(true);
        _button.enabled = true;
    }

    public void OnClick()
    {
        _button.gameObject.SetActive(false);
    }
}
