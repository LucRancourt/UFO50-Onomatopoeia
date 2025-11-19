using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;


public class DictationRequestMenu : Menu<DictationRequestMenu>
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Button button;

    private void Start()
    {
        Close();

        button.onClick.AddListener(RequestDictationPermission);
    }

    public void Open()
    {
        panel.SetActive(true);
    }

    private void Close()
    {
        panel.SetActive(false);
    }

    private void RequestDictationPermission()
    {
        Process.Start("ms-settings:privacy-speech");
        Close();
    }
}
