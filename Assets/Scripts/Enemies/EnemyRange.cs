using UnityEngine;

public class EnemyRange : MonoBehaviour {

    public float damage, attackDistance;
    private float attackTimer, attackDelay = 3f;
    public AudioClip[] _audioclips;
    public GameObject fireBall;
    private Animator _animator;
    private GameObject _character;

    // Use this for initialization
    void Start()
    {
        _animator = GetComponent<Animator>();
        _character = GameObject.FindGameObjectWithTag("Character");
    }

    // Update is called once per frame
    void Update()
    {
        Flip();
        Attack();
    }

    bool IsAttack()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
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
        Die();
    }

    bool IsCharacterInSight()
    {
        if (Mathf.Abs(_character.transform.position.x - transform.position.x) < attackDistance)
            return true;
        return false;
    }

    void Attack()
    {
        if (IsCharacterInSight() && !_character.GetComponent<CharacterController>().IsDie())
        {
            if (_character.GetComponent<CharacterController>().Timer(ref attackTimer, attackDelay) && !IsDie())
            {
                _animator.Play("Attack");
            }
        }
    }

    public void SpawnFireBall()
    {
        GameObject header = Instantiate(fireBall);
        header.transform.position = transform.GetChild(0).transform.position;
        var xScale = GazeDirection() < 0 ? 1 : -1;
        header.transform.localScale = new Vector3(xScale, transform.localScale.y, transform.localScale.z);
        header.GetComponent<FireBall>().damage = damage;
    }

    float GazeDirection()
    {
        float dir = _character.transform.position.x - transform.position.x;
        return dir < 0 ? -1 : 1;
    }

    void Flip()
    {
        var xScale = GazeDirection() < 0 ? 1 : -1;
        transform.localScale = new Vector3(xScale, transform.localScale.y, transform.localScale.z);
    }

    void Die()
    {
        if (!IsDie())
        {
            _animator.Play("Die");
            GetComponent<CapsuleCollider2D>().enabled = false;
        }
    }

    public void DestroyAfterAnimationEnd()
    {
        Destroy(gameObject);
    }
}
