using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class AudioManager : MonoBehaviour
{
    /// <summary>
    /// Компонент, позволяющий работать с музыкой в Unity
    /// </summary>
    private AudioSource _audioSource;

    /// <summary>
    /// Звуковая дорожка для проигрывания
    /// </summary>
    public AudioClip music;

    /// <summary>
    /// Говорит приложению не прерывать скрипт при открытии новой сцены
    /// </summary>
    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        _audioSource = GetComponent<AudioSource>();

    }

    /// <summary>
    /// Включает музыку
    /// </summary>
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
