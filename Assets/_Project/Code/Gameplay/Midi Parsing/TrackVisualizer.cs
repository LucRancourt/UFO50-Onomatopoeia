using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TrackVisualizer : MonoBehaviour
{
    private SpriteRenderer _sprite;
    public TrackData TrackData;

    void Start()
    {
        _sprite = gameObject.GetComponent<SpriteRenderer>();
        _sprite.enabled = false;
    }

    public void RegisterTrack(TrackData trackData)
    {
        TrackData = trackData;
        TrackData.PlayedNote.AddListener((id, noteName, duration) => StartCoroutine(ShowNote(id, noteName, duration)));
    }

    void OnDisable()
    {
        TrackData.PlayedNote.RemoveAllListeners();  //for now
    }

    public IEnumerator ShowNote(int trackID, string noteName, float noteDuration)       //Do stuff with track here. Or replace this class with the track
    {
        _sprite.enabled = true;
        yield return new WaitForSeconds(noteDuration/2);
        _sprite.enabled = false;
    }
}
