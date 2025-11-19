using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.Collections;
using System.Linq;
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

    public UnityEvent<int, string, float> PlayedNote = new UnityEvent<int, string, float>();    //track ID,  note name, duration of note
    public UnityEvent<float> FinishedTrack = new UnityEvent<float>();                           //float: duration of last note

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
        float lastNoteLength = 0;
        foreach (var note in _track.GetNotes())
        {
            var waitTime = note.Time - currentTime;
            yield return new WaitForSeconds((waitTime * _secondsPerTick) / SongManager.Instance.TempoMultiplier);
            currentTime = note.Time;
            
            lastNoteLength = note.Length * _secondsPerTick;
            PlayedNote.Invoke(TrackID, note.NoteName.ToString(), lastNoteLength);
        }

        FinishedTrack.Invoke(lastNoteLength);

        IsPlaying = false;
    }
    
    public float QuarterNoteLength()
    {
        return _secondsPerTick * _ticksPerQuarter;
    }

    public float GetWaitToFirstNote(){
        return (_track.GetNotes().First().Time * _secondsPerTick) / SongManager.Instance.TempoMultiplier;
    }
}
