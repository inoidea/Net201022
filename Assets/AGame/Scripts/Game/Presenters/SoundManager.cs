using UnityEngine;

public class SoundManager : MonoBehaviour
{

    [SerializeField] private AudioSource _audioSource;

    public static AudioSource AudioSource { get; private set; }
    public static bool IsPlaying { get; set; }


    private void Awake()
    {
        AudioSource = _audioSource;
    }
}