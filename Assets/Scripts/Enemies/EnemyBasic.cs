using UnityEngine;

public class EnemyBasic : MonoBehaviour
{

    public float maxSpeed = 5f, hp, damage, attackDistance;
    public LayerMask ground;
    private float attackTimer, attackDelay = 1.5f, _hp;
    public AudioClip[] _audioclips;
    private Animator _animator;
    private GameObject _character;

    // Use this for initialization
    void Start () {
        _animator = GetComponent<Animator>();
        _character = GameObject.FindGameObjectWithTag("Character");
        _hp = 1f;
    }
	
	// Update is called once per frame
	void Update () {
        Move();
        Flip();
        Attack();
        Die();
    }

    bool IsAttack()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            return true;
        return false;
    }

    bool IsAttacked()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Hurt"))
            return true;
        return false;
    }

    bool IsDie()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Die"))
            return true;
        return false;
    }

    public void TakeDamage(float value)
    {
        _hp -= value / hp;
        _animator.Play("Hurt");
        GetComponent<AudioSource>().PlayOneShot(_audioclips[0]);
        attackTimer = Time.time;
    }

    void Attack()
    {
        if (transform.GetChild(1).GetComponent<AttackTrigger>().resetTimer)
        {
            attackTimer = Time.time;
            transform.GetChild(1).GetComponent<AttackTrigger>().resetTimer = false;
        }
        if (_character.GetComponent<CharacterController>().Timer(ref attackTimer, attackDelay) && !IsAttacked() && !IsDie())
        {
            if (transform.GetChild(1).GetComponent<AttackTrigger>().target != null && !transform.GetChild(1).GetComponent<AttackTrigger>().target.GetComponent<CharacterController>().IsDie())
            {
                transform.GetChild(1).GetComponent<AttackTrigger>().target.GetComponent<CharacterController>().TakeDamage(damage);
                _animator.Play("Attack");
                GetComponent<AudioSource>().PlayOneShot(_audioclips[2]);
            }
        }
    }

    float MoveDirection()
    {
        float dir = _character.transform.position.x - transform.position.x;
        return dir < 0 ? -1: 1;
    }

    bool Grounded()
    {
        return Physics2D.OverlapCircle(transform.GetChild(0).transform.position, 0.1f, ground);
    }

    void Move()
    {
        if (!IsDie())
        {
            if (!IsAttack() && !IsAttacked() && Mathf.Abs(_character.transform.position.x - transform.position.x) > attackDistance && Mathf.Abs(_character.transform.position.x - transform.position.x) < 30f && Grounded())
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(MoveDirection() * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
                _animator.SetBool("isMove", true);
            }
            else
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
            if (GetComponent<Rigidbody2D>().velocity.x == 0)
                _animator.SetBool("isMove", false);
        }
    }

    void Flip()
    {
        var xScale = MoveDirection() < 0 ? 1 : -1;
        transform.localScale = new Vector3(xScale, transform.localScale.y, transform.localScale.z);
    }

    void Die()
    {
        if (_hp <= 0 && !IsDie())
        {
            _animator.Play("Die");
            GetComponent<AudioSource>().PlayOneShot(_audioclips[1]);
            GetComponent<CapsuleCollider2D>().enabled = false;
            Destroy(GetComponent<Rigidbody2D>());
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    public void DestroyAfterAnimationEnd()
    {
        Destroy(gameObject, 2f);
    }
}
