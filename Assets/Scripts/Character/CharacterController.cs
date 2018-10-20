using UnityEngine;

public class CharacterController : MonoBehaviour {

    public float maxSpeed = 5f, jumpForce = 18f;
    public bool grounded = false, _canMove;
    public LayerMask ground;
    private AudioSource _audioSource;
    private CapsuleCollider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<CapsuleCollider2D>();
    }

    // Use this for initialization
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        CharacterAnimation();
        CharacterFlip();
        CharacterJump();
    }

    void FixedUpdate()
    {
        CharacterMove();
        GroundCheck();
    }

    void CharacterMove()
    {
        if (CanMove(transform.right * Input.GetAxis("Horizontal")))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(Input.GetAxis("Horizontal") * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
            _canMove = true;
        }
        else
            _canMove = false;
    }

    bool CanMove(Vector3 dir)
    {
        float castDistance = 0.2f;
        RaycastHit2D[] hits = Physics2D.CapsuleCastAll(_collider.transform.position, _collider.size, _collider.direction, 0f, dir, castDistance);
        foreach (RaycastHit2D objectHit in hits)
        {
            if (objectHit.transform.tag == "Wall")
            {
                return false;
            }
        }
        return true;
    }

    void CharacterFlip()
    {
        if (Input.GetAxis("Horizontal") != 0)
            GetComponent<SpriteRenderer>().flipX = Input.GetAxis("Horizontal") < 0;
    }

    public void CharacterJump()
    {
        if (grounded && Input.GetButtonDown("Jump"))
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            _audioSource.PlayOneShot(_audioSource.clip);
        }
    }

    void GroundCheck()
    {
        grounded = Physics2D.OverlapCircle(transform.GetChild(0).transform.position, 0.1f, ground); 
    }

    void CharacterAnimation()
    {
        if (grounded)
        {
            GetComponent<Animator>().SetBool("isJump", false);
            if (_canMove && Input.GetButton("Horizontal"))
                GetComponent<Animator>().SetBool("isMove", true);
            else
                GetComponent<Animator>().SetBool("isMove", false);
        }
        else
        {
            GetComponent<Animator>().SetBool("isJump", true);
            GetComponent<Animator>().SetBool("isMove", false);
        }
    }

    public void CharacterDie()
    {
        GetComponent<Animator>().Play("Die");
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<CharacterController>().enabled = false;
    }

    void OnCollisionEnter2D(Collision2D _collider)
    {
        if (_collider.transform.tag == "MovingPlatform")
            transform.parent = _collider.transform;
    }

    void OnCollisionExit2D(Collision2D _collider)
    {
        if (_collider.transform.tag == "MovingPlatform")
            transform.parent = null;
    }
}
