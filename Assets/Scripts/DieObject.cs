using UnityEngine;

public class DieObject : MonoBehaviour {

    GameObject character, gameController;

	// Use this for initialization
	void Start () {
        character = GameObject.FindGameObjectWithTag("Character");
        gameController = GameObject.FindGameObjectWithTag("GameController");
    }
	
	// Update is called once per frame
	void Update () {

    }

    void OnCollisionEnter2D(Collision2D _collision)
    {
        if (_collision.transform.tag == "Character")
        {
            character.GetComponent<CharacterController>().CharacterDie();
            gameController.GetComponent<Game>().ShowLosePanel();
        }
    }

    void OnTriggerEnter2D(Collider2D _collider)
    {
        if (_collider.transform.tag == "Character")
        {
            character.GetComponent<CharacterController>().CharacterDie();
            gameController.GetComponent<Game>().ShowLosePanel();
        }
    }
}
