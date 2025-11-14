using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    [SerializeField] GameObject notePrefab;
    [SerializeField] RectTransform[] lanePositions;
    [SerializeField] string[] laneKeywords;
    [SerializeField] float spawnInterval;
    [SerializeField] float moveSpeedMin;
    [SerializeField] float moveSpeedMax;
    [SerializeField] RectTransform hitZoneTop;
    [SerializeField] RectTransform hitZoneBottom;
    [SerializeField] RectTransform canvasTransform;
    [SerializeField] float spawnY;

    float _t;

    void Update()
    {
        _t += Time.deltaTime;
        if (_t >= spawnInterval)
        {
            _t = 0f;
            int lane = Random.Range(0, lanePositions.Length);

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
                Speed = Random.Range(moveSpeedMin, moveSpeedMax),
                HitTop = hitZoneTop,
                HitBottom = hitZoneBottom
            });
        }
    }
}