using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour {
    
    public GameObject losePanel;
    public GameObject inGameMenuPanel;
    public GameObject character;

    // Use this for initialization
    void Start () {
        SetCursorState(CursorLockMode.Locked);
    }

    // Update is called once per frame
    void Update () {
        ShowInGameMenu();
    }

    public void SetCursorState(CursorLockMode wantedMode)
    {
        Cursor.lockState = wantedMode;
        // Hide cursor when locking
        Cursor.visible = (CursorLockMode.Locked != wantedMode);
    }

    public void ShowLosePanel()
    {
        losePanel.SetActive(true);
        SetCursorState(CursorLockMode.None);
    }

    void ShowInGameMenu()
    {
        if (!inGameMenuPanel.activeInHierarchy && !losePanel.activeInHierarchy)
        {
            if (Input.GetButtonDown("Cancel"))
            {
                SetCursorState(CursorLockMode.None);
                inGameMenuPanel.SetActive(true);
                Time.timeScale = 0;
                character.GetComponent<CharacterController>().enabled = false;
            }
        }
    }
}
