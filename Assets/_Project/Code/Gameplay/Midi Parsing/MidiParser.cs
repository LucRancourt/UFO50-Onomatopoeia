using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Linq;

/*
    Using DryWetMidi Library plug-in
    https://melanchall.github.io/drywetmidi/api/Melanchall.DryWetMidi.Common.html
*/



public class MidiParser : MonoBehaviour
{
    [SerializeField] Object midiAsset;
    [SerializeField] GameObject _visualizerPrefab;
    private string _path;

    private MidiFile _midiFile;
    private List<TrackData> _tracks = new List<TrackData>();

    private short _ticksPerQuarter;
    private float _microSecPerQuarter;
    private const int MICROSECONDS_PER_SECOND = 1000000;

    void Start()
    {
        //1. Try to read the provided midi file
        _path = AssetDatabase.GetAssetPath(midiAsset);
        if (Path.GetExtension(_path) != ".mid")
        {
            throw new System.Exception("Invalid file extension (expected .mid)");
        }

        //2. Read Track and Tempo data
        LoadMidiInfo();

        //3. Create TrackData *only* for tracks with playable notes.
        int i = 0;
        float tempo = (_microSecPerQuarter / _ticksPerQuarter) / MICROSECONDS_PER_SECOND;

        foreach (TrackChunk trackChunk in _midiFile.GetTrackChunks().ToList())
        {
            if (trackChunk.GetNotes().Count > 0)
            {
                _tracks.Add(new TrackData(trackChunk, i, tempo, _ticksPerQuarter));
                i++;
            }
        }

        //4. TESTING ONLY !!
        SetupVisualizers();

        foreach (TrackData track in _tracks)
        {
            StartCoroutine(track.PlayTrack());
        }
    }

    private void LoadMidiInfo()
    {
        // Load MIDI file and separate tracks
        _midiFile = MidiFile.Read(_path);

        Debug.Log($"Num of tracks:{_midiFile.GetTrackChunks().ToList().Count}");

        //Read tempo-related information
        _microSecPerQuarter = _midiFile.GetTempoMap().GetTempoChanges().First().Value.MicrosecondsPerQuarterNote;    //assuming that tempo never changes for now
        _ticksPerQuarter = (_midiFile.TimeDivision as TicksPerQuarterNoteTimeDivision).TicksPerQuarterNote;
    }
    #region TESTING WITH VISUALIZER

    private void SetupVisualizers()
    {
        float trackWidth = gameObject.GetComponent<BoxCollider2D>().size.x / _tracks.Count;
        float leftBound = gameObject.GetComponent<BoxCollider2D>().bounds.min.x;

        Vector3 position = new Vector3();
        for (int i = 0; i < _tracks.Count; i++)
        {
            TrackVisualizer visualizer = Instantiate(_visualizerPrefab).GetComponent<TrackVisualizer>();
            position.x = leftBound + (i * trackWidth) + (trackWidth / 2f);

            visualizer.transform.position = position;
            visualizer.RegisterTrack(_tracks[i]);
        }
    }
    
    #endregion
}
