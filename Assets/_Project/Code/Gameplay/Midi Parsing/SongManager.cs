using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Code.Core.Audio;
using _Project.Code.Core.General;
using UnityEngine;

public class SongManager : Singleton<SongManager>
{
    public List<TrackData> Tracks = new List<TrackData>();
    public float TempoMultiplier = 1f;
    //public AudioCue BackgroundMusic;
    
    [SerializeField] float _songEndTimeBuffer = 2f;
    [SerializeField] ResultsPageMenu _resultsPage;
    
    private int _finishedTracks;

    public void StartSong()
    {
        _finishedTracks = 0;
        foreach(TrackData track in Tracks)
        {
            track.FinishedTrack.AddListener(CheckForSongCompletion);
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

    private void CheckForSongCompletion(float lastNoteDuration)
    {
        _finishedTracks++;
        
        if(_finishedTracks == 4)
            StartCoroutine(PauseThenResults(lastNoteDuration));

    }

    private IEnumerator PauseThenResults(float lastNoteDuration)
    {
        yield return new WaitForSeconds(FindFirstObjectByType<NoteSpawner>().GetDelayToTopBar() + lastNoteDuration + _songEndTimeBuffer);
        _resultsPage.DisplayResults();
        //UnityEngine.Debug.Log($"song ended: {FindFirstObjectByType<NoteSpawner>().GetDelayToTopBar() + lastNoteDuration + _songEndTimeBuffer}");
    }
}
