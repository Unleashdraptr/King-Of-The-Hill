using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AI : MonoBehaviour
{
    public enum AIState {Walking, Jumping, Attacking_Player, Attacking_Pylon }
    public AIState CurrentState;
    //Movement
    public Transform PlayerPos;
    //Will be given to the AI when they spawn in
    public float DirMultiplier;
    public Transform JumpPoint;
    public float JumpDelay;
    public Transform LandingPoint;

    private bool IsAttacking;
    private float Attacking;

    [Header("Class Stats")]
    public int Health;
    public int Attack;
    public float AttackSpeed;
    public int Defense;
    public int Speed;

    private void Start()
    {
        JumpDelay = 1.75f;
        CurrentState = AIState.Walking;
    }
    // Update is called once per frame
    void Update()
    {
        if ((JumpPoint.position.x - transform.position.x) * DirMultiplier <= 0.5f && CurrentState != AIState.Jumping)
        {
            AIJump();
            CurrentState = AIState.Jumping;
        }
        //This locks the AI from randomly moving or attacking when jumping - A moment of weakness for them
        else if (CurrentState != AIState.Jumping && Attacking <= 0)
        {
            //Function to check the player position against its own and will setup for the attack
            if (CheckPlayerPos())
            {
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
        else
        {
            JumpDelay -= Time.deltaTime;
            if (JumpDelay <= 0)
            {
                transform.position = new(LandingPoint.position.x + Random.Range(-1, 1), LandingPoint.position.y, 0);
                CurrentState = AIState.Attacking_Pylon;
            }
        }
    }
    bool CheckPlayerPos()
    {
        return false;
    }
    void AIJump()
    {
        //Start the Animation here
    }
}
