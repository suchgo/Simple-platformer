using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PlayAgainButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Button>().Select();
        GetComponent<Button>().enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
}
