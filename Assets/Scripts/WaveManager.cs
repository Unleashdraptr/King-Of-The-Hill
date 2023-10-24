using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    
    public int WaveNum;
    public float Countdown;
    private bool CountDowning;
    public float WaveTimer;
    private bool WaveTimered;

    private Transform EnemyStorage;
    public Transform[] JumpPoints;
    public Transform[] LandingPoints;

    [Header("This is what enemies will spawn is stored (Hover over for more info)")]
    [Tooltip("Give it a name, what it'll spawn for the enemy, in the  Wave Spawns array add 21 (leave element 0 blank)")]
    public WaveNumbers[] WaveNums;
    // Start is called before the first frame update
    void Start()
    {
        JumpPoints = GameObject.Find("JumpPoints").GetComponentsInChildren<Transform>();
        LandingPoints = GameObject.Find("LandingPoints").GetComponentsInChildren<Transform>();

        Countdown = 10;
        CountDowning = true;
        EnemyStorage = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (CountDowning)
            Countdown -= 1 * Time.deltaTime;
        if(WaveTimered)
            WaveTimer -= 1 * Time.deltaTime;

        if ((WaveTimer <= 0 || EnemyStorage.transform.childCount == 0) && CountDowning != true && WaveNum != 20)
        {
            Countdown = 10;
            WaveTimered = false;
            CountDowning = true;
        }
        if(Countdown <= 0 && CountDowning)
        {
            CountDowning = false;
            WaveNum += 1;
            WaveTimered = true;
            WaveTimer = 30;
            for(int i = 0; i < WaveNums.Length; i++)
            {
                StartCoroutine(SpawnEnemies(WaveNums[i].EnemyToInstantiate, WaveNums[i].WaveSpawns[WaveNum]));
            }
        }
    }

    IEnumerator SpawnEnemies(GameObject EnemyToInstantiate, int Repetition)
    {
        //Spawn All Enemies
        for (int i = 0; i < Repetition; i++)
        {
            GameObject Enemy = Instantiate(EnemyToInstantiate, EnemyStorage);

            int Placement = Random.Range(1, 100);
            Vector3 Pos = new(0,0,0);
            int Dir = 0;
            switch (Placement)
            {
                case < 25:
                    //Top Left
                    Pos = new(-22.25f, 2.5f, 0);
                    Dir = 1;
                    Enemy.GetComponent<Enemy_AI>().JumpPoint = JumpPoints[1];
                    Enemy.GetComponent<Enemy_AI>().LandingPoint = LandingPoints[1];
                    Enemy.GetComponent<Enemy_AI>().JumpType = 1;
                    break;
                case < 50:
                    //Top Right
                    Pos = new(22.25f, 2.5f, 0);
                    Dir = -1;
                    Enemy.GetComponent<Enemy_AI>().JumpPoint = JumpPoints[2];
                    Enemy.GetComponent<Enemy_AI>().LandingPoint = LandingPoints[2];
                    Enemy.GetComponent<Enemy_AI>().JumpType = 2;
                    break;
                case < 75:
                    //Bottom Left
                    Pos = new(-22.25f, -9.75f, 0);
                    Dir = 1;
                    Enemy.GetComponent<Enemy_AI>().JumpPoint = JumpPoints[3];
                    Enemy.GetComponent<Enemy_AI>().LandingPoint = LandingPoints[1];
                    Enemy.GetComponent<Enemy_AI>().JumpType = 3;
                    break;
                case < 100:
                    //Bottom Right
                    Pos = new(22.25f, -9.75f, 0);
                    Dir = -1;
                    Enemy.GetComponent<Enemy_AI>().JumpPoint = JumpPoints[4];
                    Enemy.GetComponent<Enemy_AI>().LandingPoint = LandingPoints[2];
                    Enemy.GetComponent<Enemy_AI>().JumpType = 4;
                    break;
            }
            Enemy.transform.position = Pos;
            Enemy.GetComponent<Enemy_AI>().DirMultiplier = Dir;
            yield return new WaitForSeconds(1f);
        }
    }    
}

//This is where each Enemy types data will be stored. Once it has its section Give it it's name and then change the array length to 20.
[System.Serializable]
public class WaveNumbers
{
    public string ClassName;
    public GameObject EnemyToInstantiate;
    public int[] WaveSpawns;
}
