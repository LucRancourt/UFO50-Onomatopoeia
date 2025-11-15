using System.Collections.Generic;
using _Project.Code.Core.General;
using UnityEngine.Rendering;

public class SongManager : Singleton<SongManager>
{
    public List<TrackData> Tracks = new List<TrackData>();

    public void StartSong()
    {
        foreach(TrackData track in Tracks)
        {
            StartCoroutine(track.PlayTrack());
        }
    }
}
