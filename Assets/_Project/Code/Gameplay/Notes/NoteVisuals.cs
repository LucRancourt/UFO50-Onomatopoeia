using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NoteVisuals : MonoBehaviour
{
    [SerializeField] Image bubbleImage;
    [SerializeField] TMP_Text keywordText;
    [SerializeField] Sprite[] bubbleSprites;
    [SerializeField] GameObject scorePopupPrefab;

    public void Init(string keyword)
    {
        bubbleImage.sprite = bubbleSprites[Random.Range(0, bubbleSprites.Length)];
        keywordText.text = keyword;
    }

    public void ShowScorePopup()
    {
        GameObject popup = Instantiate(scorePopupPrefab, transform.parent);
        popup.GetComponent<RectTransform>().anchoredPosition =
            GetComponent<RectTransform>().anchoredPosition;
    }
}