using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class CharacterController : MonoBehaviour {

    public float maxSpeed = 5f, jumpForce = 18f, hp, damage;
    public bool grounded = false;
    public LayerMask ground;
    public Image hpBar;
    public TextMeshProUGUI hpText;
    public AudioClip[] _audioclips;
    [HideInInspector]
    public Animator _animator;
    private float _attackTimer, _attackDelay = 0.4f, _hp;
    private bool isSpawnDust = false, airAttackEnd = false, isCrouch = false;
    private AudioSource _audioSource;
    private Animator _cameraAnimator;
    private GameObject _gameController;
    private CapsuleCollider2D _collider;

    // Use this for initialization
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        _cameraAnimator = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>();
        _gameController = GameObject.FindGameObjectWithTag("GameController");
        _collider = GetComponent<CapsuleCollider2D>();
        _hp = 1f;
    }

    void Update()
    {
        CharacterAnimation();
        CharacterFlip();
        CharacterJump();
        CharacterDie();
        Attack();
        SpawnDust();
        Cast();
        Crouch();
    }

    void FixedUpdate()
    {
        CharacterMove();
        GroundCheck();
    }

    void CharacterMove()
    {
        if (!IsAttack() && !IsAttacked() && !IsCast() && !isCrouch)
            GetComponent<Rigidbody2D>().velocity = new Vector2(Input.GetAxis("Horizontal") * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
        else
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
    }

    bool IsStay()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            return true;
        return false;
    }

    void Crouch()
    {
        if (Input.GetButton("Vertical") && IsStay())
        {
            if (Input.GetAxis("Vertical") < 0)
            {
                _collider.size = new Vector2(_collider.size.x, _collider.size.y - 1f);
                _collider.offset = new Vector2(_collider.offset.x, _collider.offset.y - 0.5f);
                isCrouch = true;
                _animator.Play("Crouch");
                _animator.SetBool("isCrouch", true);
            }
        }
        else
        if (isCrouch)
        {
            if (Input.GetButtonUp("Vertical") || IsAttack() || IsAttacked())
            {
                _collider.size = new Vector2(_collider.size.x, _collider.size.y + 1f);
                _collider.offset = new Vector2(_collider.offset.x, _collider.offset.y + 0.5f);
                isCrouch = false;
                _animator.SetBool("isCrouch", false);
            }
        }
    }

    void CharacterFlip()
    {
        if (Input.GetButton("Horizontal"))
        {
            if (!IsAttack() && !IsAttacked() && !IsCast())
            {
                var xScale = Input.GetAxis("Horizontal") < 0 ? -1 : 1;
                transform.localScale = new Vector3(xScale, transform.localScale.y, transform.localScale.z);
            }
        }
    }

    public void CharacterJump()
    {
        if (grounded && Input.GetButtonDown("Jump") && !IsAttack() && !IsAttacked() && !IsCast() && !isCrouch)
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            _audioSource.PlayOneShot(_audioclips[0]);
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || _animator.GetCurrentAnimatorStateInfo(0).IsName("JumpToIdle"))
                _animator.Play("JumpAfterIdle");
            else if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
                _animator.Play("JumpAfterRun");
        }
    }

    bool IsCast()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("CastStart") || _animator.GetCurrentAnimatorStateInfo(0).IsName("CastMid") || _animator.GetCurrentAnimatorStateInfo(0).IsName("CastEnd"))
            return true;
        return false;
    }

    void Cast()
    {
        if (grounded && !isCrouch)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                _animator.Play("CastStart");
                _animator.SetBool("isCast", true);
            }
            else if (Input.GetButtonUp("Fire2"))
                _animator.SetBool("isCast", false);
        }
    }

    public bool IsDie()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Die"))
            return true;
        return false;
    }

    bool IsAttack()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1") || _animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2") || _animator.GetCurrentAnimatorStateInfo(0).IsName("Attack3") || _animator.GetCurrentAnimatorStateInfo(0).IsName("AirAttack1") || _animator.GetCurrentAnimatorStateInfo(0).IsName("AirAttack2") || _animator.GetCurrentAnimatorStateInfo(0).IsName("AirAttack3") || _animator.GetCurrentAnimatorStateInfo(0).IsName("AirAttack3Mid") || _animator.GetCurrentAnimatorStateInfo(0).IsName("AirAttack3End"))
            return true;
        return false;
    }

    bool IsAttacked()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Hurt"))
            return true;
        return false;
    }

    void Attack()
    {
        if (Input.GetButtonDown("Fire1") && !IsCast() &&  !IsAttacked())
        {
            if (grounded)
            {
                if (Timer(ref _attackTimer, _attackDelay))
                {
                    if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
                    {
                        _attackDelay = 0.5f;
                        _animator.Play("Attack3");
                        _audioSource.PlayOneShot(_audioclips[3]);
                    }
                    else if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
                    {
                        _attackDelay = 0.4f;
                        _animator.Play("Attack2");
                        _audioSource.PlayOneShot(_audioclips[2]);
                    }
                    else
                    {
                        _attackDelay = 0.4f;
                        _animator.Play("Attack1");
                        _audioSource.PlayOneShot(_audioclips[1]);
                    }
                    GetAttackTrigger(1);
                }
            }
            else
            {
                if (Timer(ref _attackTimer, _attackDelay) && !airAttackEnd)
                {
                    if (_animator.GetCurrentAnimatorStateInfo(0).IsName("AirAttack2"))
                    {
                        airAttackEnd = true;
                        _animator.Play("AirAttack3");
                        _audioSource.PlayOneShot(_audioclips[1]);
                        GetComponent<Rigidbody2D>().gravityScale = 3f;
                    }
                    else if (_animator.GetCurrentAnimatorStateInfo(0).IsName("AirAttack1"))
                    {
                        _attackDelay = 0.2f;
                        _animator.Play("AirAttack2");
                        _audioSource.PlayOneShot(_audioclips[2]);
                    }
                    else
                    {
                        _attackDelay = 0.3f;
                        _animator.Play("AirAttack1");
                        _audioSource.PlayOneShot(_audioclips[1]);
                    }
                    GetAttackTrigger(1);
                }
            }
        }
    }

    void GetAttackTrigger(byte triggerIndex)
    {
        if (transform.GetChild(triggerIndex).GetComponent<AttackTrigger>().target != null)
        {
            StartCoroutine(SlowdownWhenAttacking(triggerIndex));
        }
    }

    IEnumerator SlowdownWhenAttacking(byte triggerIndex)
    {
        Time.timeScale = 0.1f;
        yield return new WaitForSeconds(0.02f);
        Time.timeScale = 1f;
        _cameraAnimator.SetTrigger("Attack");
        if (transform.GetChild(triggerIndex).GetComponent<AttackTrigger>().target.GetComponent<EnemyBasic>())
            transform.GetChild(triggerIndex).GetComponent<AttackTrigger>().target.GetComponent<EnemyBasic>().TakeDamage(damage);
        else
            if (transform.GetChild(triggerIndex).GetComponent<AttackTrigger>().target.GetComponent<EnemyRange>())
            transform.GetChild(triggerIndex).GetComponent<AttackTrigger>().target.GetComponent<EnemyRange>().TakeDamage(damage);
    }

    public void HangInTheAair()
    {
        GetComponent<Rigidbody2D>().gravityScale = 0f;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    public void ResetGravityScale()
    {
        if(!grounded)
            GetComponent<Rigidbody2D>().gravityScale = 1f;
        airAttackEnd = true;
    }

    public void AirAttackWhenAnimEnd()
    {
        airAttackEnd = true;
        _audioSource.PlayOneShot(_audioclips[6]);
        GetAttackTrigger(2);
    }

    public void TakeDamage(float value)
    {
        if (IsCast())
            value *= 0.5f;
        else
        {
            _animator.Play("Hurt");
            _audioSource.PlayOneShot(_audioclips[4]);
        }
        _hp -= value / hp;
        ResetGravityScale();
        if (!grounded)
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    public void HpBarUpdate()
    {
        hpBar.fillAmount = Mathf.Lerp(hpBar.fillAmount, _hp, Time.deltaTime * 5);
        hpText.text = ((int)(hpBar.fillAmount * 100)).ToString();
    }

    void GroundCheck()
    {
        grounded = Physics2D.OverlapCircle(transform.GetChild(0).transform.position, 0.1f, ground);
    }

    void CharacterAnimation()
    {
        if (grounded)
        {
            airAttackEnd = false;
            _animator.SetBool("isJump", false);
            if (Input.GetButton("Horizontal"))
                _animator.SetBool("isMove", true);
            else
                _animator.SetBool("isMove", false);
        }
        else
        {
            _animator.SetBool("isJump", true);
            _animator.SetBool("isMove", false);
        }
    }

    public void CharacterDie()
    {
        if (_hp <= 0)
        {
            _animator.Play("Die");
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<CharacterController>().enabled = false;
        }
    }

    public void Lose()
    {
        _gameController.GetComponent<Game>().ShowLosePanel();
        _animator.enabled = false;
    }

    void SpawnDust()
    {
        if (grounded)
        {
            if (isSpawnDust)
            {
                //_cameraAnimator.SetTrigger("Jump");
                _audioSource.PlayOneShot(_audioclips[5]);
                isSpawnDust = false;
            }
        }
        else
            isSpawnDust = true;
    }

    void OnCollisionEnter2D(Collision2D _collider)
    {
        if (_collider.transform.tag == "MovingPlatform")
            transform.parent = _collider.transform;
        if (_collider.gameObject.layer == 8)
            GetComponent<Rigidbody2D>().gravityScale = 3f;
    }

    void OnCollisionExit2D(Collision2D _collider)
    {
        if (_collider.transform.tag == "MovingPlatform")
            transform.parent = null;
        if (_collider.gameObject.layer == 8)
            GetComponent<Rigidbody2D>().gravityScale = 1f;
    }

    public bool Timer(ref float timer, float delay)
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
