using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenEffects : MonoBehaviour {

    public GameObject blackScreen;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void BlackoutScreen()
    {
        blackScreen.SetActive(true);
        blackScreen.GetComponent<Animator>().Play("Blackout");
    }
}
