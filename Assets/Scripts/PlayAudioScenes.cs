using UnityEngine;
using UnityEngine.UI;

public class PlayAudioScenes : MonoBehaviour
{
    [SerializeField]
    public AudioClip clip;

    [SerializeField]
    public AudioSource source;

    public void PlaySpeech()
    {
        clip.LoadAudioData();
        source.PlayOneShot(clip);
        
    }
}
