using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMannager : MonoBehaviour
{
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
