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
    private float DirMultiplier;
    private Transform[] JumpPoints;
    private Transform[] LandingPoints;
    private bool IsAttacking;
    public float Attacking;

    //Stats
    public int Health;
    public int Attack;
    public float AttackSpeed;
    public int Defense;
    public int Speed;

    private void Start()
    {
        LandingPoints = GameObject.Find("LandingPoints").GetComponentsInChildren<Transform>();
        JumpPoints = GameObject.Find("JumpPoints").GetComponentsInChildren<Transform>();
        CurrentState = AIState.Walking;
    }
    // Update is called once per frame
    void Update()
    {
        //This loops through the designated points at which they'll jump to see if they made it there already
        foreach (Transform Pos in JumpPoints)
        {
            //If it finds one then lock the AI out of any other attacking
            if (transform == Pos)
            {
                AIJump(Pos);
                CurrentState = AIState.Jumping;
            }
        }
        //This locks the AI from randomly moving or attacking when jumping - A moment of weakness for them
        if (CurrentState != AIState.Jumping && Attacking <= 0)
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
            foreach (Transform Pos in LandingPoints)
            {
                if (transform == Pos)
                {
                    CurrentState = AIState.Attacking_Pylon;
                }
            }
        }
    }
    bool CheckPlayerPos()
    {
        return false;
    }
    void AIJump(Transform JumpPoint)
    {

    }
}
