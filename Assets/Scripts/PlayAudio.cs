using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    public AudioSource audio;
    public void playButton()
    {
        audio.Play();
    }
}
