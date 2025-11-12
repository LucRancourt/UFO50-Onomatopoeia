using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Melanchall.DryWetMidi.Multimedia;

/*
    Using DryWetMidi Library plug-in
    https://melanchall.github.io/drywetmidi/api/Melanchall.DryWetMidi.Common.html
*/



public class NewMidiVisualizer : MonoBehaviour
{
    [SerializeField] Object midiAsset;
    private MidiFile _midiFile;
    private List<TrackChunk> _tracks;

    private short _ticksPerQuarter;
    private float _microSecPerQuarter;

    void Start()
    {
        //Try to read the provided midi file
        string path = AssetDatabase.GetAssetPath(midiAsset);
        if (Path.GetExtension(path) != ".mid")
        {
            throw new System.Exception("Invalid file extension (expected .mid)");
        }

        // Load MIDI file and separate tracks
        _midiFile = MidiFile.Read(path);
        _tracks = _midiFile.GetTrackChunks().ToList();

        _microSecPerQuarter = _midiFile.GetTempoMap().GetTempoChanges().First().Value.MicrosecondsPerQuarterNote;    //assuming that tempo never changes for now
        _ticksPerQuarter = (_midiFile.TimeDivision as TicksPerQuarterNoteTimeDivision).TicksPerQuarterNote;

        Debug.Log($"ms/Quarter: {_microSecPerQuarter} || tick/Quarter {_ticksPerQuarter}");

        foreach(TrackChunk track in _tracks)
        {
            StartCoroutine(DisplayTrackNotes(track));
        }
    }


    public IEnumerator DisplayTrackNotes(TrackChunk track)
    {
        float secondsPerTick = (_microSecPerQuarter / _ticksPerQuarter) / 1000000f;
        Debug.Log($"Seconds per tick: {secondsPerTick}");
        long currentTime = 0;
        
        foreach (var note in track.GetNotes())
        {
            var waitTime = note.Time - currentTime;
            yield return new WaitForSeconds(waitTime * secondsPerTick);
            
            Debug.Log($"Played note: {note.NoteName}");
            currentTime = note.Time;
        }
    }
}
