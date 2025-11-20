using UnityEngine;
using UnityEngine.Windows.Speech;

public class DictationStatusChecker
{
    private DictationRecognizer dictationRecognizer;

    public bool WasSuccessful { get; private set; }

    private const int SPERR_SPEECH_PRIVACY_POLICY_NOT_ACCEPTED = unchecked((int)0x80045509);

    public void Initialize()
    {
        WasSuccessful = true;

        dictationRecognizer = new DictationRecognizer();

        dictationRecognizer.DictationError += OnDictationError;
        dictationRecognizer.DictationComplete += OnDictationComplete;

        dictationRecognizer.Start();
    }

    private void OnDictationError(string error, int hresult)
    {
        if (hresult == SPERR_SPEECH_PRIVACY_POLICY_NOT_ACCEPTED)
        {
            if (dictationRecognizer.Status == SpeechSystemStatus.Running)
                dictationRecognizer.Stop();
        }
        else
        {
            Application.Quit();
        }

        WasSuccessful = false;
    }

    private void OnDictationComplete(DictationCompletionCause cause)
    {
        if (cause == DictationCompletionCause.UnknownError)
        {
            WasSuccessful = false;
        }
    }

    public void Destroy()
    {
        if (dictationRecognizer != null)
        {
            dictationRecognizer.DictationError -= OnDictationError;
            dictationRecognizer.DictationComplete -= OnDictationComplete;
            dictationRecognizer.Dispose();
        }
    }
}