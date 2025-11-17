using UnityEngine;

public class Note : MonoBehaviour
{
    [SerializeField] int laneIndex;
    [SerializeField] string keyword;
    [SerializeField] float speed;
    [SerializeField] RectTransform hitZoneTop;
    [SerializeField] RectTransform hitZoneBottom;
    private NoteSpawner _spawner;
    

    RectTransform _rt;
    bool _canHit;
    public bool CanHit => _canHit;
    public string Keyword => keyword;
    public int LaneIndex => laneIndex;

    void Awake()
    {
        _rt = GetComponent<RectTransform>();
    }

    void Update()
    {
        _rt.anchoredPosition += Vector2.down * (speed * Time.deltaTime);

        float y = _rt.anchoredPosition.y;
        float topY = hitZoneTop.anchoredPosition.y;
        float bottomY = hitZoneBottom.anchoredPosition.y;

        _canHit = y <= topY && y >= bottomY;

        if (y < bottomY)
            _spawner.MissedNote(this);
    }

    public void Setup(NoteSetupData data)
    {
        laneIndex = data.LaneIndex;
        keyword = data.Keyword;
        speed = data.Speed;
        hitZoneTop = (RectTransform)data.HitTop;
        hitZoneBottom = (RectTransform)data.HitBottom;
        _spawner = data.Spawner;
    }
}