using UnityEngine;
using UnityEngine.UI;

public class LosePanelAnimation : MonoBehaviour {

    public Animator TextAnimator;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void StartTextAnimation()
    {
        TextAnimator.Play("Text");
        GetComponent<Button>().enabled = true;
    }
}
