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

        try
        {
            dictationRecognizer.Start();
            Debug.Log("Attempting to start dictation... (If successful, dictation is enabled)");
        }
        catch (System.Exception e)
        {
            Debug.Log($"Start failed immediately: {e.Message}");
        }
    }

    private void OnDictationError(string error, int hresult)
    {
        if (hresult == SPERR_SPEECH_PRIVACY_POLICY_NOT_ACCEPTED)
        {
            Debug.Log("Dictation is disabled in Windows Privacy Settings.");
            Debug.LogErrorFormat("Dictation error: {0}; HResult = {1}. User needs to enable dictation in Settings > Privacy > Speech.", error, hresult);
            // You can stop the recognizer here as it won't work
            if (dictationRecognizer.Status == SpeechSystemStatus.Running)
            {
                dictationRecognizer.Stop();
            }
        }
        else
        {
            Debug.Log($"Other dictation error: {error}");
            Debug.LogErrorFormat("Dictation error: {0}; HResult = {1}.", error, hresult);
        }

        WasSuccessful = false;
    }

    private void OnDictationComplete(DictationCompletionCause cause)
    {
        if (cause == DictationCompletionCause.Complete)
        {
            // The recognizer started and stopped normally, indicating settings are likely okay.
            // Note: This does not mean the user is actively using Win+H dictation at that moment.
            Debug.Log("Dictation session completed normally, settings are fine.");
        }
        else if (cause == DictationCompletionCause.UnknownError)
        {
            // This might happen if the app closed unexpectedly or had another issue
            Debug.Log("Dictation session was incomplete.");
            WasSuccessful = false;
        }
        // Other causes like PauseLimitExceeded, Timeout, etc.
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