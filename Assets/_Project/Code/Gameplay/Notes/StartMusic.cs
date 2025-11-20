using System.Collections;
using TMPro;
using UnityEngine;

public class StartMusic : MonoBehaviour
{
    public AudioSource audioSource;
    public float AudioDelayBuffer = 0f;
    [SerializeField] GameObject _timerCanvas;

    private const float START_DELAY = 3.5f;
    private const float COUNTDOWN_START = 3f;

    public void StartSong()
    {
        Debug.Log("START BUTTON PRESSED");
        audioSource.pitch = SongManager.Instance.TempoMultiplier;
        //SongManager.Instance.BackgroundMusic.SetPitch(SongManager.Instance.TempoMultiplier);
        
        float totalAudioDelay = FindFirstObjectByType<NoteSpawner>().GetDelayToTopBar() + AudioDelayBuffer;
        StartCoroutine(DelayedStart(totalAudioDelay));
    }

    private IEnumerator DelayedStart(float audioDelay)
    {
        _timerCanvas.SetActive(true);
        float spawnDelay = 0f;

        if(audioDelay < START_DELAY)
        {
            spawnDelay = START_DELAY - audioDelay;                      //To make countdown at least three seconds
        }
        
        StartCoroutine(StartCountdownTimer(spawnDelay + audioDelay));   //start the countdown timer

        yield return new WaitForSeconds(spawnDelay);            //Start Spawning Notes
        SongManager.Instance.StartSong();
        
        
        yield return new WaitForSeconds(audioDelay);            //Play the audio once the notes reach the top bar
        _timerCanvas.SetActive(false);
        audioSource.Play();
        //AudioManager.Instance.PlayMusic(SongManager.Instance.BackgroundMusic, false);
    }

    private IEnumerator StartCountdownTimer(float totalDelay)
    {
        TextMeshProUGUI text = _timerCanvas.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        text.enabled = false;

        yield return new WaitForSeconds(totalDelay - COUNTDOWN_START);
        totalDelay = COUNTDOWN_START;
        text.enabled = true;

        while(totalDelay > 0)
        {
            text.text = Mathf.Ceil(totalDelay).ToString("0");
            yield return new WaitForSeconds(1f);
            totalDelay -= 1f;
        }
    }
}