using UnityEngine;

namespace _Project.Code.Core.Audio
{
    [CreateAssetMenu(fileName = "NewAudioCue", menuName = "Scriptable Objects/Audio/AudioCue")]
    public class AudioCue : ScriptableObject
    {
        [field: SerializeField] public AudioClip Clip { get; private set; }
        [field: SerializeField] public bool Loop { get; private set; } = false;
        [field: SerializeField, Range(0.0f, 1.0f)] public float Volume { get; private set; } = 1.0f;
        [field: SerializeField, Range(0.1f, 3.0f)] public float Pitch { get; private set; } = 1.0f;
    }
}

