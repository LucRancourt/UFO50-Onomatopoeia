using UnityEngine;

public class StartMusic : MonoBehaviour
{
    public AudioSource audioSource;

    public void StartSong()
    {
        Debug.Log("START BUTTON PRESSED");
        audioSource.Play();
        SongManager.Instance.StartSong();
    }
}