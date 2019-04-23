using UnityEngine;

public class MoveSoundController : MonoBehaviour {

    private AudioSource _audioSource;
    private float _attackTimer, _attackDelay = 0.34f;
    private byte i = 0;
    public AudioClip[] audioClips;

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
        if (transform.parent.GetComponent<CharacterController>().Timer(ref _attackTimer, _attackDelay))
        {
            if (transform.parent.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Run"))
            {
                _audioSource.PlayOneShot(audioClips[i]);
                i += 1;
                if (i > 1)
                    i = 0;
                _attackTimer = Time.time;
            }
        }
    }
}
