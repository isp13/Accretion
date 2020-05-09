using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource _audioSource;
    public AudioClip music;
    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        _audioSource = GetComponent<AudioSource>();

    }

    public void PlayMusic()
    {
        if (_audioSource.isPlaying) return;
        _audioSource.clip = music;
        _audioSource.Play(1);
    }

    public void StopMusic()
    {
        _audioSource.Stop();
    }
}
