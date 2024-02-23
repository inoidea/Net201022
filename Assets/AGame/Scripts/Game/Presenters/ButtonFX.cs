using UnityEngine;

public class ButtonFX : MonoBehaviour
{
    public AudioSource _audioSource;
    public AudioClip _hoverAudioClip;

    public void HoverSound()
    {
        _audioSource.PlayOneShot(_hoverAudioClip);
    }
}
