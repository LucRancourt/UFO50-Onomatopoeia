using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    [SerializeField] GameObject notePrefab;
    [SerializeField] RectTransform[] lanePositions;
    [SerializeField] string[] laneKeywords;
    [SerializeField] float spawnInterval;
    [SerializeField] float moveSpeed;
    [SerializeField] RectTransform hitZoneTop;
    [SerializeField] RectTransform hitZoneBottom;
    [SerializeField] RectTransform canvasTransform;
    [SerializeField] float spawnY;
    [SerializeField] bool useMidi;

    float _t;

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
            HitBottom = hitZoneBottom
        });
    }
}