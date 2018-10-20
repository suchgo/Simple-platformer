using UnityEngine;

public class MoveSoundController : MonoBehaviour {

    private AudioSource _audioSource;

    // Use this for initialization
    void Start () {
        _audioSource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        PlayMoveSound();
    }

    void PlayMoveSound()
    {
        if (transform.parent.GetComponent<CharacterController>().grounded)
        {
            if (Input.GetButton("Horizontal") && transform.parent.GetComponent<CharacterController>()._canMove)
            {
                if (!_audioSource.isPlaying)
                    _audioSource.Play();
            }
            else
                _audioSource.Pause();
        }
        else
            _audioSource.Pause();
    }
}
