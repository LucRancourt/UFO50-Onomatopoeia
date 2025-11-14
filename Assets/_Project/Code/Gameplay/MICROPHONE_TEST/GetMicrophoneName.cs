using UnityEngine;

public class GetMicrophoneName : MonoBehaviour
{
    void Start()
    {
        foreach (var d in Microphone.devices)
            Debug.Log("Mic: " + d);
    }
}
