using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    [SerializeField] GameObject notePrefab;
    [SerializeField] RectTransform[] lanePositions;
    [SerializeField] KeywordSet laneKeywords;
    [SerializeField] Sprite[] laneBubbleSprites;
    [SerializeField] float spawnInterval;
    [SerializeField] float moveSpeed;
    [SerializeField] RectTransform hitZoneTop;
    [SerializeField] RectTransform hitZoneBottom;
    [SerializeField] RectTransform canvasTransform;
    [SerializeField] float spawnY;
    [SerializeField] bool useMidi;

    private KeywordGameplayListener _keywordListener;
    float _t;

    void Start()
    {
        _keywordListener = new KeywordGameplayListener(laneKeywords);
        _keywordListener.OnNoteHit += HitNote;
        _keywordListener.OnFalseHit += FalseHit;
    }

    void OnDestroy()
    {
        _keywordListener.OnNoteHit -= HitNote;
        _keywordListener.OnFalseHit -= FalseHit;
        _keywordListener.Dispose();
        _keywordListener = null;
    }

    void Update()
    {
        if (useMidi) return;

        _t += Time.deltaTime;
        if (_t >= spawnInterval)
        {
            _t = 0f;
            int lane = Random.Range(0, lanePositions.Length);
            SpawnOne(lane);
        }
    }

    public void SpawnOne(int lane)
    {
        GameObject obj = Instantiate(notePrefab, canvasTransform);
        RectTransform rt = obj.GetComponent<RectTransform>();

        rt.anchoredPosition = new Vector2(lanePositions[lane].anchoredPosition.x, spawnY);

        var note = obj.GetComponent<Note>();

        note.Setup(new NoteSetupData
        {
            LaneIndex = lane,
            Keyword = laneKeywords.GetOnomatopoeia(lane),
            Speed = moveSpeed,
            HitTop = hitZoneTop,
            HitBottom = hitZoneBottom,
            Spawner = this,
            BubbleSprite = laneBubbleSprites[lane]
        });

        _keywordListener.AddNextNote(note);
    }

    private void HitNote(Note note)
    {
        LaneFeedbackManager.Instance.FlashCorrect(note.LaneIndex);

        note.GetComponent<NoteVisuals>().ShowScorePopup();

        ScoreManager.Instance.AddHit();
        DestroyNote(note);
    }

    private void FalseHit(string word)
    {
        int index = laneKeywords.GetIndex(word);

        if (index == -1) return;

        LaneFeedbackManager.Instance.FlashWrong(index);
    }

    public void MissedNote(Note note)
    {
        ScoreManager.Instance.AddMiss();
        DestroyNote(note);
    }

    private void DestroyNote(Note note)
    {
        _keywordListener.RemoveNote(note);
        Destroy(note.gameObject);
    }

    public float GetDelayToTopBar()
    {
        float distance = Mathf.Abs(spawnY - hitZoneTop.anchoredPosition.y);
        return distance / moveSpeed;
    }
}
