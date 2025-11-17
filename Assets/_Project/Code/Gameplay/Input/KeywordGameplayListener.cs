using UnityEngine.Windows.Speech;
using System;
using System.Collections.Generic;


public class KeywordGameplayListener : IDisposable
{
    private DictationRecognizer _DictationRecognizer;

    private List<string> _possibleNotes;
    private List<Note> _upcomingNotes = new List<Note>();

    public event Action<Note> OnNoteHit;
    public event Action<string> OnFalseHit;


    public KeywordGameplayListener(List<string> notes)
    { 
        _possibleNotes = notes;

        _DictationRecognizer = new DictationRecognizer();
        _DictationRecognizer.AutoSilenceTimeoutSeconds = 100000.0f;
        _DictationRecognizer.InitialSilenceTimeoutSeconds = 100000.0f;

        _DictationRecognizer.DictationHypothesis += CheckNote;

        _DictationRecognizer.Start();
    }

    private void CheckNote(string hypoText)
    {
        string word = GetLastWord(hypoText);
        UnityEngine.Debug.Log(word);

        if (IsWordInList(_possibleNotes, word))
        {
            Note noteFound = _upcomingNotes.Find(x => x.Keyword.Contains(word, StringComparison.OrdinalIgnoreCase));

            if (noteFound != null)
            {
                if (!noteFound.CanHit) return;

                OnNoteHit?.Invoke(noteFound);
            }
            else
            {
                OnFalseHit?.Invoke(word);
            }
        }
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
        if (_DictationRecognizer == null) return;

        _DictationRecognizer.DictationHypothesis -= CheckNote;

        _DictationRecognizer.Stop();

        _DictationRecognizer.Dispose();
        _DictationRecognizer = null;
    }
}