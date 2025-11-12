using System.Collections;
using System.Collections.Generic;
using System.IO;
using MidiParser;
using UnityEditor;
using UnityEngine;

public class MidiVisualizer : MonoBehaviour
{
    [SerializeField] Object midiAsset;
    private MidiFile _midiFile;
    private int _BPM = 120;         //TEMP!! the BPM can be read from each track using the tempo event I think.
    private float _secondsPerTick;

    public const string FILE_PATH_PREFIX = "Assets/_Project/Audio/MidiFiles/";

    void Start()
    {
        //Try to read the rpovided midi file
        string path = AssetDatabase.GetAssetPath(midiAsset);
        if (Path.GetExtension(path) != ".mid")
        {
            throw new System.Exception("Invalid file extension (expected .mid)");
        }

        _midiFile = new MidiFile(AssetDatabase.GetAssetPath(midiAsset));
        _secondsPerTick = (60f / _BPM) / _midiFile.TicksPerQuarterNote;  
        Debug.Log($"Seconds per Tick: {_secondsPerTick} || Ticks per quarter: {_midiFile.TicksPerQuarterNote}");


        for(int i = 0; i < _midiFile.TracksCount; i++)
        {
            StartCoroutine(DisplayTrackNotes(_midiFile.Tracks[i], i));
        }

    }

    public List<Note> ReadNotesInTrack(MidiTrack midiTrack)
    {
        List<Note> notes = new List<Note>();

        float ticksSinceLastNote = 0;

        foreach (var midiEvent in midiTrack.MidiEvents)
        {
            ticksSinceLastNote += midiEvent.Time;   //Time since last event
            if (midiEvent.MidiEventType == MidiEventType.NoteOn && midiEvent.Velocity > 0)
            {
                float startSec = ticksSinceLastNote * _secondsPerTick;
                notes.Add(new Note() { startDelay = startSec, });
            }
        }
        return notes;
    }
    
    public IEnumerator DisplayTrackNotes(MidiTrack midiTrack, int trackIdentifier)
    {
        List<Note> notes = ReadNotesInTrack(midiTrack);
        float timeElapsed = 0;
        foreach (Note note in notes)
        {
            timeElapsed += Time.deltaTime;
            yield return new WaitForSeconds(note.startDelay - timeElapsed);
            Debug.Log(trackIdentifier);
        }
    }


}

public class Note
{
    public float startDelay;
}

