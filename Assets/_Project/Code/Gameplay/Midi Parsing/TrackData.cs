using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class TrackData
{
    /*
        FIELDS
    */
    public bool IsPlaying { get; private set; }
    public int TrackID { get; private set; }
    
    private TrackChunk _track;
    public float _secondsPerTick, _ticksPerQuarter;      //For tempo

    public UnityEvent<int, string> PlayedNote = new UnityEvent<int, string>();  //track ID and note name

    /*
        CONSTRUCTOR
    */
    public TrackData(TrackChunk trackChunk, int id, float secondsPerTick, float ticksPerQuarter)
    {
        _track = trackChunk;
        TrackID = id;
        _secondsPerTick = secondsPerTick;
        _ticksPerQuarter = ticksPerQuarter;
    }

    /*
        FUNCTIONS
    */
    public IEnumerator PlayTrack()
    {
        IsPlaying = true;

        long currentTime = 0;
        foreach (var note in _track.GetNotes())
        {
            var waitTime = note.Time - currentTime;
            yield return new WaitForSeconds(waitTime * _secondsPerTick);
            currentTime = note.Time;

            PlayedNote.Invoke(TrackID, note.NoteName.ToString());
            Debug.Log($"{TrackID}: Played note: {note.NoteName}");
        }

        IsPlaying = false;
    }
    
    public float QuarterNoteLength()
    {
        return _secondsPerTick * _ticksPerQuarter;
    }
}
