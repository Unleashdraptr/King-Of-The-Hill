using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AI : MonoBehaviour
{
    public enum AIState { Walking, Jumping, Attacking_Player, Attacking_Pylon, Dead }
    public AIState CurrentState;
    //Movement
    public Transform PlayerPos;
    //Will be given to the AI when they spawn in
    public float DirMultiplier;
    public Transform JumpPoint;
    public float JumpDelay;
    public int JumpType;
    public Transform LandingPoint;
    public LayerMask PlayerLayer;

    private bool HasLanded;
    public bool IsAttacking;
    public float Attacking;
    private Animator Animate;
    private Vector3 StayingPos;

    [Header("Class Stats")]
    public int Health;
    public int Attack;
    public float AttackSpeed;
    public int Defense;
    public int Speed;

    private void Start()
    {
        PlayerPos = GameObject.Find("Player").transform;
        Animate = GetComponent<Animator>();
        JumpDelay = 2.5f;
        CurrentState = AIState.Walking;
    }
    // Update is called once per frame
    void Update()
    {
        if (!GameManager.PauseState)
        {
            if (!HasLanded)
            {
                if ((JumpPoint.position.x - transform.position.x) * DirMultiplier <= 0.5f && CurrentState != AIState.Jumping)
                {
                    Animate.SetBool("HasJumped", true);
                    Animate.SetInteger("JumpType", JumpType);
                    CurrentState = AIState.Jumping;
                }
                //This locks the AI from randomly moving or attacking when jumping - A moment of weakness for them
                else if (CurrentState != AIState.Jumping && Attacking <= 0 && CurrentState != AIState.Dead)
                {
                    //Function to check the player position against its own and will setup for the attack
                    if (CheckPlayerPos())
                    {
                        StartAttack();
                        CurrentState = AIState.Attacking_Player;
                    }
                    //Otherwise just move
                    else
                    {
                        transform.Translate(Speed * DirMultiplier * Time.deltaTime, 0f, 0f);
                        CurrentState = AIState.Walking;
                    }
                }
                //Checks to see if they landed yet
                else if (CurrentState == AIState.Jumping && CurrentState != AIState.Dead)
                {
                    JumpDelay -= Time.deltaTime;
                    if (JumpDelay <= 0)
                    {
                        StayingPos = new(LandingPoint.position.x + Random.Range(-1, 1), LandingPoint.position.y, 0);
                        transform.position = StayingPos;
                        CurrentState = AIState.Attacking_Pylon;
                        HasLanded = true;
                    }
                }
            }
            else
            {
                transform.position = StayingPos;
                if (Attacking <= 0)
                {
                    if (CheckPlayerPos())
                        CurrentState = AIState.Attacking_Player;
                    else
                        CurrentState = AIState.Attacking_Pylon;
                    StartAttack();
                }
            }



            if (IsAttacking)
            {
                Attacking -= Time.deltaTime;
                if (Attacking <= 0)
                {
                    if (!CheckPlayerPos())
                    {
                        if (HasLanded)
                            CurrentState = AIState.Attacking_Pylon;
                        else
                        {
                            CurrentState = AIState.Walking;
                            IsAttacking = false;
                            Animate.SetBool("IsAttacking", false);
                        }
                    }
                    else
                    {
                        CurrentState = AIState.Attacking_Player;
                        StartAttack();
                    }
                }
            }
        }
    }
    bool CheckPlayerPos()
    {
        if (Vector2.Distance(new(transform.position.x, transform.position.y), new(PlayerPos.position.x, PlayerPos.position.y)) < 1.5f)
            return true;
        return false;
    }

    public void TakeDmg(int Dmg)
    {
        if (Dmg - Defense <= 0)
            Health -= 1;
        else
            Health -= Dmg - Defense;
        if (Health <= 0)
        {
            CurrentState = AIState.Dead;
            HasLanded = false;
            IsAttacking = false;
            Animate.SetTrigger("HasDied");
            StartCoroutine(Death());
        }
    }
    void StartAttack()
    {
        Animate.SetBool("IsAttacking", true);
        IsAttacking = true;
        Attacking = 1.5f;
        if (CurrentState == AIState.Attacking_Player)
            StartCoroutine(AttackingPlayer());
        else if (CurrentState == AIState.Attacking_Pylon)
            StartCoroutine(AttackingPylon());
    }
    IEnumerator AttackingPlayer()
    {
        IsAttacking = true;
        Attacking = 1.5f;
        yield return new WaitForSeconds(0.5f);
        if (Physics.CheckSphere(transform.position, 2.5f, PlayerLayer))
        {
            StartCoroutine(PlayerPos.GetComponent<PlayerControls>().TakeDmg(Attack));
        }
    }
    IEnumerator AttackingPylon()
    {
        IsAttacking = true;
        Attacking = 1.5f;
        yield return new WaitForSeconds(0.5f);
        GameObject.Find("Pylon").GetComponent<PylonScript>().TakeDmg(Attack);
    }

    IEnumerator Death()
    {
        yield return new WaitForSeconds(1.25f);
        Destroy(gameObject);
    }
}
