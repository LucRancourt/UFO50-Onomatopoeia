using UnityEngine;
using TMPro;

public class LaneFeedbackManager : MonoBehaviour
{
    [SerializeField] TMP_Text[] laneTexts;
    [SerializeField] Color correctColor = Color.green;
    [SerializeField] Color wrongColor = Color.red;
    [SerializeField] Color normalColor = Color.white;
    [SerializeField] float flashDuration = 0.2f;

    public static LaneFeedbackManager Instance;

    void Awake()
    {
        Instance = this;
    }

    public void FlashCorrect(int laneIndex)
    {
        laneTexts[laneIndex].color = correctColor;
        Invoke(nameof(ResetColors), flashDuration);
    }

    public void FlashWrong(int laneIndex)
    {
        laneTexts[laneIndex].color = wrongColor;
        Invoke(nameof(ResetColors), flashDuration);
    }

    void ResetColors()
    {
        foreach (var t in laneTexts)
            t.color = normalColor;
    }
}