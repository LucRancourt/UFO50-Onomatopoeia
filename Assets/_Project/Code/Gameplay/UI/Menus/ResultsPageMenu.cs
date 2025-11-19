using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using _Project.Code.Core.ServiceLocator;


public class ResultsPageMenu : Menu<ResultsPageMenu>
{
    [Header("General")]
    [SerializeField] private GameObject panel;
    [SerializeField] private Button returnToMainMenuButton;
    [SerializeField] private float delayBetweenStats = 2.0f;

    [Header("Stat Texts")]
    [SerializeField] private TextMeshProUGUI hitsText;
    [SerializeField] private TextMeshProUGUI missesText;
    [SerializeField] private TextMeshProUGUI accuracyText;


    private void Start()
    {
        returnToMainMenuButton.onClick.AddListener(ReturnToMainMenu);
        panel.SetActive(false);
    }

    public void DisplayResults()
    {
        if (panel.activeSelf) return;

        hitsText.text = "";
        missesText.text = "";
        accuracyText.text = "";

        returnToMainMenuButton.gameObject.SetActive(false);

        panel.SetActive(true);

        StartCoroutine(ShowStat());
    }

    IEnumerator ShowStat()
    {

        yield return new WaitForSeconds(delayBetweenStats);

        float hits = ScoreManager.Instance.NumberOfHits;
        hitsText.text = "Hits: " + hits;

        yield return new WaitForSeconds(delayBetweenStats);

        float misses = ScoreManager.Instance.NumberOfMisses;
        missesText.text = "Misses: " + misses;

        yield return new WaitForSeconds(delayBetweenStats);

        float accuracy = Mathf.Round((hits / (float)(hits + misses)) * 10000f) / 100f;
        accuracyText.text = "Accuracy: " + accuracy +"%";

        yield return new WaitForSeconds(delayBetweenStats);

        returnToMainMenuButton.gameObject.SetActive(true);
    }

    private void ReturnToMainMenu()
    {
        ServiceLocator.Get<SceneService>().LoadScene("MainMenu");
    }
}
