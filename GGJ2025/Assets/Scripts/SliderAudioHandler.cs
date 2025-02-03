using UnityEngine;
using UnityEngine.EventSystems;

public class SliderAudioHandler : MonoBehaviour
{
    public AudioSource audioSource;

    // Unity Event Trigger expects BaseEventData
    public void OnPointerUp(BaseEventData eventData)
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}

