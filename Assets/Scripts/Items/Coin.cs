using UnityEngine;

public class Coin : MonoBehaviour
{

    private float startPosY, range, speed, alphaValue, flickerTimer, flickerDelay;
    private bool isPickUp = false;
    private Animator _animator;
    private Vector3 newPos;

    // Use this for initialization
    void Start()
    {
        _animator = GetComponent<Animator>();
        startPosY = transform.position.y;
        range = 4f;
        speed = 8f;
        flickerDelay = 0.03f;
    }

    // Update is called once per frame
    void Update()
    {
        PickUp();
    }

    void PickUp()
    {
        if (isPickUp)
        {
            newPos = new Vector2(transform.position.x, startPosY + range);
            if (Mathf.Abs(newPos.y - transform.position.y) <= 0.2f)
            {
                range = 0;
                speed /= 1.5f;
            }
            transform.position = Vector2.Lerp(transform.position, newPos, speed * Time.deltaTime);
            if (Mathf.Abs(transform.position.y - startPosY) <= 0.2f)
                Destroy(gameObject);
            if (Timer(ref flickerTimer, flickerDelay))
            {
                alphaValue = alphaValue == 1 ? 0 : 1;
                GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, alphaValue);
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Character" && !isPickUp)
        {
            isPickUp = true;
            _animator.speed *= 3;
            GetComponent<AudioSource>().PlayOneShot(GetComponent<AudioSource>().clip);
        }
    }

    bool Timer(ref float timer, float delay)
    {
        if (Time.time - timer >= delay)
        {
            timer = Time.time;
            return true;
        }
        else
            return false;
    }
}
