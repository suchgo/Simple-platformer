using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour {
    
    public GameObject losePanel;
    public GameObject inGameMenuPanel;
    public GameObject character;

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update () {
        ShowInGameMenu();
    }

    public void ShowLosePanel()
    {
        losePanel.SetActive(true);
    }

    void ShowInGameMenu()
    {
        if (!inGameMenuPanel.activeInHierarchy && !losePanel.activeInHierarchy)
        {
            if (Input.GetButtonDown("Cancel"))
            {
                inGameMenuPanel.SetActive(true);
                Time.timeScale = 0;
                character.transform.GetChild(0).GetComponent<MoveSoundController>().enabled = false;
                character.GetComponent<CharacterController>().enabled = false;
            }
        }
    }
}
