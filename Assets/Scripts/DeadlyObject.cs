using UnityEngine;

public class DeadlyObject : MonoBehaviour {

    GameObject character;

	// Use this for initialization
	void Start () {
        character = GameObject.FindGameObjectWithTag("Character");
    }
	
	// Update is called once per frame
	void Update () {

    }

    void OnCollisionEnter2D(Collision2D _collision)
    {
        if (_collision.transform.tag == "Character")
        {
            character.GetComponent<CharacterController>().TakeDamage(character.GetComponent<CharacterController>().hp);
        }
    }

    void OnTriggerEnter2D(Collider2D _collider)
    {
        if (_collider.transform.tag == "Character")
        {
            character.GetComponent<CharacterController>().TakeDamage(character.GetComponent<CharacterController>().hp);
        }
    }
}
