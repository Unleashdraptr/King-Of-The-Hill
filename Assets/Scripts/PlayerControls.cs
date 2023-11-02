using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public enum MoveState { IDLE, WALK, JUMP, FALL, DEATH, WEAK_ATTACKING, STRONG_ATTACKING };
    public MoveState moveState;


    //General Player settings
    public float Speed;
    public float JumpSpeed;
    public LayerMask FloorMask;
    public Transform Feet;
    public Rigidbody rb;
    Vector3 PlayerMoveInput;
    private Animator Anim;


    public int Hp;
    public int Defense;
    public float Ab1CoolDown;
    public float Ab2CoolDown;
    public GameObject MagicBoltPrefab;


    private float AttackTimer;
    private bool Attacking;

    void Start()
    {
        //Set the state to idle
        moveState = MoveState.IDLE;
        rb = transform.GetComponent<Rigidbody>();
        Attacking = false;
        Anim = GetComponent<Animator>();
    }


    void FixedUpdate()
    {
        Vector3 MoveDir = transform.TransformDirection(PlayerMoveInput) * Speed;
        MoveDir.y = rb.velocity.y;
        rb.MovePosition(transform.position + MoveDir * Time.deltaTime);
    }



    void Update()
    {
        if (!GameManager.PauseState)
        {
            if (GameObject.Find("Pylon").GetComponent<PylonScript>().Hp <= 0 || Hp <= 0)
            {
                StartCoroutine(Death());
            }
            PlayerMoveInput = new(Input.GetAxis("Horizontal"), 0, 0);
            if (!Attacking)
            {
                if (PlayerMoveInput.Equals(default) && rb.velocity.y == 0)
                {
                    Anim.SetBool("IsIdle", true);
                    moveState = MoveState.IDLE;
                }
                else if (rb.velocity.y > 0)
                    moveState = MoveState.JUMP;
                else if (rb.velocity.y < 0)
                    moveState = MoveState.FALL;
                else
                {
                    moveState = MoveState.WALK;
                    Anim.SetBool("IsIdle", false);
                }
            }
            else
            {
                AttackTimer -= Time.deltaTime;
                if (AttackTimer <= 0)
                {
                    Attacking = false;
                    Anim.SetBool("IsAttacking", false);
                    Anim.SetBool("IsHeavyAttack", false);
                }
            }
            Anim.SetBool("IsAttacking", false);
            UpdateAnimator();

            //Jump
            if (Input.GetKeyDown(KeyCode.Space) && Physics.CheckSphere(Feet.position, 0.1f, FloorMask))
            {
                rb.AddForce(Vector3.up * JumpSpeed, ForceMode.Impulse);
            }
            //Soft Attack
            if (Input.GetKeyDown(KeyCode.Mouse0) && !Attacking && Ab1CoolDown <= 0)
            {
                StartCoroutine(WeakAttack());
                moveState = MoveState.WEAK_ATTACKING;
                AttackTimer = 0.5f;
                Attacking = true;
                Anim.SetBool("IsAttacking", true);
                Ab1CoolDown = 0.5f;
            }
            //Strong Attack
            if (Input.GetKeyDown(KeyCode.Q) && !Attacking && Physics.CheckSphere(Feet.position, 0.1f, FloorMask) && Ab2CoolDown <= 0)
            {
                StartCoroutine(StrongAttack());
                moveState = MoveState.STRONG_ATTACKING;
                AttackTimer = 1f;
                Attacking = true;
                Anim.SetBool("IsAttacking", true);
                Anim.SetBool("IsHeavyAttack", true);
                Ab2CoolDown = 8;
            }
        }
    }

    IEnumerator Death()
    {
        Anim.SetBool("IsPylonDead", true);
        yield return new WaitForSeconds(1f);
        GameObject.Find("GameManager").GetComponent<GameManager>().LostGame();
        Time.timeScale = 0;
    }
    IEnumerator WeakAttack()
    {
        yield return new WaitForSeconds(0.35f);
        GameObject MagicBolt = Instantiate(MagicBoltPrefab, transform.position, Quaternion.identity);

        Vector3 mousePos = GameObject.Find("Main Camera").GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);

        Vector3 Rotation = mousePos - MagicBolt.transform.position;

        float rotZ = Mathf.Atan2(Rotation.y, Rotation.x) * Mathf.Rad2Deg;

        MagicBolt.transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }

    IEnumerator StrongAttack()
    {
        yield return new WaitForSeconds(0.85f);
        Vector3 mousePos = GameObject.Find("Main Camera").GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
        for (int i = 0; i < 10; i++)
        {
            GameObject MagicBolt = Instantiate(MagicBoltPrefab, transform.position, Quaternion.identity);
            Vector3 Rotation = mousePos - MagicBolt.transform.position;
            float rotZ = Mathf.Atan2(Rotation.y, Rotation.x) * Mathf.Rad2Deg;

            MagicBolt.transform.rotation = Quaternion.Euler(0, 0, rotZ + ((i * 10) - 50));
        }
    }

    public IEnumerator TakeDmg(int Dmg)
    {
        Anim.SetBool("HasBeenHit", true);
        if (Dmg - Defense <= 0)
            Hp -= 1;
        else
            Hp -= Dmg - Defense;
        yield return new WaitForSeconds(0.15f);
        Anim.SetBool("HasBeenHit", false);
    }

    void UpdateAnimator()
    {
        if (Input.GetAxis("Horizontal") < 0)
        {
            Anim.SetInteger("XVel", -1);
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (Input.GetAxis("Horizontal") > 0)
        {
            Anim.SetInteger("XVel", 1);
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else
            Anim.SetInteger("XVel", 0);

        Anim.SetFloat("YVel", rb.velocity.y);
        if (Physics.CheckSphere(Feet.position, 0.1f, FloorMask))
            Anim.SetBool("IsTouchingGround", true);
        else
            Anim.SetBool("IsTouchingGround", false);
    }
}

