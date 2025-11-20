using UnityEngine.Windows.Speech;
using System;
using System.Collections.Generic;
using System.Diagnostics;


public class KeywordGameplayListener : IDisposable
{
    private DictationRecognizer _dictationRecognizer;

    private KeywordSet _possibleNotes;
    private List<Note> _upcomingNotes = new List<Note>();

    public event Action<Note> OnNoteHit;
    public event Action<string> OnFalseHit;


    public KeywordGameplayListener(KeywordSet notes)
    { 
        _possibleNotes = notes;

        _dictationRecognizer = new DictationRecognizer();
        _dictationRecognizer.AutoSilenceTimeoutSeconds = 100000.0f;
        _dictationRecognizer.InitialSilenceTimeoutSeconds = 100000.0f;

        _dictationRecognizer.DictationHypothesis += CheckNote;

        _dictationRecognizer.DictationComplete += (completionCause) => { ResetDictation(); };
        _dictationRecognizer.DictationError += (error, hresult) => { ResetDictation(); };

        _dictationRecognizer.Start();
    }

    private void CheckNote(string hypoText)
    {
        string word = GetLastWord(hypoText);
        UnityEngine.Debug.Log(word);

        string onomatopoeia = _possibleNotes.GetOnomatopoeia(word);
        UnityEngine.Debug.Log("Ono " + onomatopoeia);
        if (onomatopoeia != null)
        {
            UnityEngine.Debug.Log("Yoooooo");
            Note noteFound = _upcomingNotes.Find(x => x.Keyword.Contains(onomatopoeia, StringComparison.OrdinalIgnoreCase));

            if (noteFound != null)
            {
                UnityEngine.Debug.Log("wwwww");
                if (!noteFound.CanHit) return;

                UnityEngine.Debug.Log("aaaaaaaaaa");
                OnNoteHit?.Invoke(noteFound);
            }
            else
            {
                OnFalseHit?.Invoke(word);
            }
        }
    }

    private void ResetDictation()
    {
        if (_dictationRecognizer.Status == SpeechSystemStatus.Running)
            _dictationRecognizer.Stop();

        _dictationRecognizer.Start();
    }

    public void AddNextNote(Note newNote) { _upcomingNotes.Add(newNote); }
    public void RemoveNote(Note noteToRemove) { _upcomingNotes.Remove(noteToRemove); }

    private bool IsWordInList(List<string> list, string word)
    {
        return list.Find(x => x.Contains(word, StringComparison.OrdinalIgnoreCase)) != null;
    }

    private string GetLastWord(string inputString)
    {
        if (string.IsNullOrEmpty(inputString))
        {
            return string.Empty;
        }

        int lastSpaceIndex = inputString.LastIndexOf(' ');

        if (lastSpaceIndex == -1) // No spaces, so the whole string is one word
        {
            return inputString;
        }
        else
        {
            return inputString.Substring(lastSpaceIndex + 1);
        }
    }

    public void Dispose()
    {
        if (_dictationRecognizer == null) return;

        _dictationRecognizer.DictationHypothesis -= CheckNote;

        _dictationRecognizer.Stop();

        _dictationRecognizer.Dispose();
        _dictationRecognizer = null;
    }
}