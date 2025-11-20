using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NoteVisuals : MonoBehaviour
{
    [SerializeField] Image bubbleImage;
    [SerializeField] TMP_Text keywordText;
    [SerializeField] GameObject scorePopupPrefab;

    public void Init(string keyword, Sprite bubbleSprite)
    {
        bubbleImage.sprite = bubbleSprite;
        keywordText.text = keyword;
    }

    public void ShowScorePopup()
    {
        GameObject popup = Instantiate(scorePopupPrefab, transform.parent);
        popup.GetComponent<RectTransform>().anchoredPosition =
            GetComponent<RectTransform>().anchoredPosition;
    }
}