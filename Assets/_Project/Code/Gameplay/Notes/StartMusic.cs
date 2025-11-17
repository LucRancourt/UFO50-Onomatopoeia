using System.Collections;
using UnityEngine;

public class StartMusic : MonoBehaviour
{
    public AudioSource audioSource;
    public float AudioDelayBuffer = 0f;

    public void StartSong()
    {
        Debug.Log("START BUTTON PRESSED");
        audioSource.pitch = SongManager.Instance.TempoMultiplier;
        SongManager.Instance.StartSong();
        NoteSpawner spawner = FindFirstObjectByType<NoteSpawner>();
        StartCoroutine(DelayBackgroundAudio(spawner.GetDelayToTopBar()+AudioDelayBuffer));
    }

    private IEnumerator DelayBackgroundAudio(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        audioSource.Play();
    }
}