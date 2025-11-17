using System;
using System.Collections.Generic;
using _Project.Code.Core.General;

public class SongManager : Singleton<SongManager>
{
    public List<TrackData> Tracks = new List<TrackData>();
    public float TempoMultiplier = 1f;

    public void StartSong()
    {
        foreach(TrackData track in Tracks)
        {
            StartCoroutine(track.PlayTrack());
        }
    }

    public float GetFirstNoteDelay()        //This is actually kind of useless but I'm gonna leave it here for the time being
    {
        if(Tracks.Count <= 0)
        {
           throw new System.Exception("There are no tracks");
        }

        float timing = 100f;
        foreach(TrackData track in Tracks)
        {
            timing = Math.Min(track.GetWaitToFirstNote(), timing);
        }

        return timing;
    }
}
