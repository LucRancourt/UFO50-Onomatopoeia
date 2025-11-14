using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Melanchall.DryWetMidi.Multimedia;
using System.Globalization;

/*
    Using DryWetMidi Library plug-in
    https://melanchall.github.io/drywetmidi/api/Melanchall.DryWetMidi.Common.html
*/



public class MidiParser : MonoBehaviour
{
    [SerializeField] Object midiAsset;
    private string _path;

    private MidiFile _midiFile;
    private List<TrackChunk> _tracks;

    private short _ticksPerQuarter;
    private float _microSecPerQuarter;
    private const int MICROSECONDS_PER_SECOND = 1000000;

    void Start()
    {
        //Try to read the provided midi file
        _path = AssetDatabase.GetAssetPath(midiAsset);
        if (Path.GetExtension(_path) != ".mid")
        {
            throw new System.Exception("Invalid file extension (expected .mid)");
        }

        LoadMidiInfo();

        foreach (TrackChunk track in _tracks)
        {
            StartCoroutine(DisplayTrackNotes(track));
        }
    }
    
    private void LoadMidiInfo()
    {
        // Load MIDI file and separate tracks
        _midiFile = MidiFile.Read(_path);
        _tracks = _midiFile.GetTrackChunks().ToList();

        Debug.Log($"Num of tracks:{_tracks.Count}");

        //Read tempo-related information
        _microSecPerQuarter = _midiFile.GetTempoMap().GetTempoChanges().First().Value.MicrosecondsPerQuarterNote;    //assuming that tempo never changes for now
        _ticksPerQuarter = (_midiFile.TimeDivision as TicksPerQuarterNoteTimeDivision).TicksPerQuarterNote;

        Debug.Log($"ms/Quarter: {_microSecPerQuarter} || tick/Quarter {_ticksPerQuarter}");

    }


    public IEnumerator DisplayTrackNotes(TrackChunk track)
    {
        if (track.GetNotes().Count == 0)       //if it not a track with notes
            yield return 0;

        float secondsPerTick = (_microSecPerQuarter / _ticksPerQuarter) / MICROSECONDS_PER_SECOND;
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
