using UnityEngine;

public class FireBall : MonoBehaviour {

    [HideInInspector]
    public float damage;
    public float maxSpeed, maxLenght;
    public GameObject explosion;

	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody2D>().velocity = new Vector2(-transform.localScale.x * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
        Destroy(gameObject, maxLenght);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SpawnExplosion()
    {
        GameObject header = Instantiate(explosion);
        header.transform.position = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Character")
            collision.gameObject.GetComponent<CharacterController>().TakeDamage(damage);
        if (collision.gameObject.layer != 11)
        {
            SpawnExplosion();
            Destroy(gameObject);
        }
    }
}
