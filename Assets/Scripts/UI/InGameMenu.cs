using UnityEngine;

public class InGameMenu : MonoBehaviour {

    public GameObject character;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Cancel"))
        {
            Resume();
        }
    }

    public void Resume()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
        character.transform.GetChild(0).GetComponent<MoveSoundController>().enabled = true;
        character.GetComponent<CharacterController>().enabled = true;
    }

    public void Exit()
    {
        Time.timeScale = 1;
    }
}
