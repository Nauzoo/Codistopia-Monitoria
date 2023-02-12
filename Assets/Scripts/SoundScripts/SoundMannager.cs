using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMannager : MonoBehaviour
{
    public AudioClip Sfx_PcOn;
    public AudioClip Sfx_PcOff;
    public AudioClip Sfx_PcError;
    public AudioClip Sfx_PcSuccess;
    public AudioClip Sfx_femaleVoice;
    public AudioClip Sfx_maleVoice;
    public AudioClip Sfx_computerType;
    public AudioClip Sfx_confirm;
    public AudioClip Sfx_Save;


    public static SoundMannager Instance;
    [SerializeField] private AudioSource _fxSource;

    private void Awake()
    {
        if (SoundMannager.Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void PlaySound(AudioClip clip)
    {
        _fxSource.PlayOneShot(clip);

    }
}
