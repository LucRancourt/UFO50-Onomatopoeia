using UnityEngine;

public class MidiLaneMapper : MonoBehaviour
{
    public int[] trackToLane;
    public NoteSpawner spawner;
    bool _initialized;

    void Start()
    {
        Debug.Log("MidiLaneMapper START CALLED");
    }
    
    void Update()
    {
        if (!_initialized && SongManager.Instance.Tracks.Count > 0)
        {
            _initialized = true;

            foreach (var t in SongManager.Instance.Tracks)
                t.PlayedNote.AddListener(OnMidiNote);

            Debug.Log("Mapper connected to " + SongManager.Instance.Tracks.Count + " tracks");
        }
    }

    void OnMidiNote(int trackID, string noteName, float duration)
    {
        Debug.Log("MIDI EVENT ARRIVED: track = " + trackID);

        if (trackID < 0 || trackID >= trackToLane.Length) return;

        int lane = trackToLane[trackID];
        if (lane >= 0)
            spawner.SpawnOne(lane);
    }
}