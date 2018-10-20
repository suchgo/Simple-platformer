using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicController : MonoBehaviour {

    public AudioClip[] currentClip;
    private static bool created = false;
    private AudioSource _audioSource;

    void Awake()
    {
        if (!created)
        {
            DontDestroyOnLoad(gameObject);
            created = true;
        }
    }

    // Use this for initialization
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        MusicSelection();
    }

    void MusicSelection()
    {
        if (_audioSource.clip.name != SceneManager.GetActiveScene().name)
        {
            _audioSource.clip = currentClip[SceneManager.GetActiveScene().buildIndex];
            _audioSource.Play();
        }  
    }
}
