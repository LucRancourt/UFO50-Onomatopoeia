#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
using UnityEngine;
using UnityEngine.Windows.Speech;
using TMPro;

public class KeywordScript : MonoBehaviour
{
    [SerializeField] string[] keywords;
    [SerializeField] TMP_Text outputText;

    KeywordRecognizer _recognizer;
    float _watchdogTimer;

    void Start()
    {
        _recognizer = new KeywordRecognizer(keywords, ConfidenceLevel.Low);
        _recognizer.OnPhraseRecognized += OnRecognized;
        _recognizer.Start();
    }

    void Update()
    {
        _watchdogTimer += Time.deltaTime;

        if (_watchdogTimer > 3f)
        {
            if (!_recognizer.IsRunning)
                _recognizer.Start();

            _watchdogTimer = 0f;
        }
    }

    void OnDestroy()
    {
        _recognizer.OnPhraseRecognized -= OnRecognized;
        if (_recognizer.IsRunning) _recognizer.Stop();
        _recognizer.Dispose();
    }

    void OnRecognized(PhraseRecognizedEventArgs args)
    {
        if (outputText) outputText.text = args.text;

        Note[] notes = Object.FindObjectsByType<Note>(FindObjectsSortMode.None);

        bool hitSomething = false;
        int laneIndex = -1;

        foreach (var n in notes)
        {
            if (n.Keyword == args.text)
            {
                laneIndex = n.LaneIndex;

                if (n.CanHit)
                {
                    Destroy(n.gameObject);
                    LaneFeedbackManager.Instance.FlashCorrect(laneIndex);
                    hitSomething = true;
                }
            }
        }

        if (!hitSomething)
        {
            for (int i = 0; i < keywords.Length; i++)
                if (keywords[i] == args.text)
                    LaneFeedbackManager.Instance.FlashWrong(i);
        }
    }
}
#endif