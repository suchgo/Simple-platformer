using UnityEngine;

public class AttackTrigger : MonoBehaviour {
    [HideInInspector]
    public GameObject target;
    public string tagName;
    [HideInInspector]
    public bool resetTimer;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}



    void OnTriggerStay2D(Collider2D _collider)
    {
        if (_collider.transform.tag == tagName)
        {
            target = _collider.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D _collider)
    {
        if (_collider.transform.tag == tagName)
        {
            target = null;
        }
    }

    void OnTriggerEnter2D(Collider2D _collider)
    {
        if (_collider.transform.tag == tagName)
        {
            resetTimer = true;
        }
    }
}
