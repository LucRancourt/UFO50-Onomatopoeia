#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
using UnityEngine;
using UnityEngine.Windows.Speech;

public class KeywordGameplayListener : MonoBehaviour
{
    [SerializeField] string[] keywords;

    KeywordRecognizer _rec;
    float _timer;

    void Start()
    {
        Init();
    }

    void Init()
    {
        _rec = new KeywordRecognizer(keywords, ConfidenceLevel.Low);
        _rec.OnPhraseRecognized += OnRec;
        _rec.Start();
    }

    void DestroyRec()
    {
        if (_rec == null) return;
        _rec.OnPhraseRecognized -= OnRec;
        if (_rec.IsRunning) _rec.Stop();
        _rec.Dispose();
        _rec = null;
    }

    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer > 5f)
        {
            if (!_rec.IsRunning)
            {
                DestroyRec();
                Invoke(nameof(Init), 0.2f);
            }

            _timer = 0;
        }
    }

    void OnDestroy()
    {
        DestroyRec();
    }

    void OnRec(PhraseRecognizedEventArgs args)
    {
        Note[] notes = Object.FindObjectsByType<Note>(FindObjectsSortMode.None);

        bool hit = false;

        foreach (var n in notes)
        {
            if (n.Keyword == args.text)
            {
                var laneIndex = n.LaneIndex;

                if (n.CanHit)
                {
                    Destroy(n.gameObject);
                    LaneFeedbackManager.Instance.FlashCorrect(laneIndex);
                    hit = true;
                }
            }
        }

        if (!hit)
        {
            for (int i = 0; i < keywords.Length; i++)
                if (keywords[i] == args.text)
                    LaneFeedbackManager.Instance.FlashWrong(i);
        }
    }
}
#endif