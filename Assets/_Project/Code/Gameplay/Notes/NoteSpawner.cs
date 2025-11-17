using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    [SerializeField] GameObject notePrefab;
    [SerializeField] RectTransform[] lanePositions;
    [SerializeField] List<string> laneKeywords;
    [SerializeField] float spawnInterval;
    [SerializeField] float moveSpeed;
    [SerializeField] RectTransform hitZoneTop;
    [SerializeField] RectTransform hitZoneBottom;
    [SerializeField] RectTransform canvasTransform;
    [SerializeField] float spawnY;
    [SerializeField] bool useMidi;
    private KeywordGameplayListener _keywordListener;

    float _t;


    private void Start()
    {
        _keywordListener = new KeywordGameplayListener(laneKeywords);

        _keywordListener.OnNoteHit += HitNote;
        _keywordListener.OnFalseHit += FalseHit;
    }

    private void OnDestroy()
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

        rt.anchoredPosition = new Vector2(
            lanePositions[lane].anchoredPosition.x,
            spawnY
        );

        obj.GetComponent<Note>().Setup(new NoteSetupData
        {
            LaneIndex = lane,
            Keyword = laneKeywords[lane],
            Speed = moveSpeed,
            HitTop = hitZoneTop,
            HitBottom = hitZoneBottom,
            Spawner = this
        });

        _keywordListener.AddNextNote(obj.GetComponent<Note>());
    }

    private void HitNote(Note note)
    {
        LaneFeedbackManager.Instance.FlashCorrect(note.LaneIndex);

        MissedNote(note);
    }

    private void FalseHit(string word)
    {
        LaneFeedbackManager.Instance.FlashWrong(laneKeywords.FindIndex(x => x.Contains(word, System.StringComparison.OrdinalIgnoreCase)));
    }

    public void MissedNote(Note note)
    {
        _keywordListener.RemoveNote(note);
        Destroy(note.gameObject);
    }

    public float GetDelayToTopBar()     //calculates the time it will take for spawned notes to reach the top bar
    {
        float distance = Mathf.Abs(spawnY - hitZoneTop.anchoredPosition.y);
        return distance / moveSpeed;
    }
}