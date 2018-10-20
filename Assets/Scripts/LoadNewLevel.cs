using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNewLevel : MonoBehaviour {

    public string levelName;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        
    }

    public void LoadLevel()
    {
        if (levelName != "")
            SceneManager.LoadScene(levelName);
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
