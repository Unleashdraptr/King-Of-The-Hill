using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public WaveNumbers[] WaveNums; 
    private int WaveNum;
    private List<int> EnemyNumbers = new List<int>();

    float Countdown;
    private GameObject EnemyStorage;

    // Start is called before the first frame update
    void Start()
    {
        UpdateNumbers();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void UpdateNumbers()
    {
        EnemyNumbers.Clear();
        for (int i = 0; i < 20; i++)
        {
            EnemyNumbers.Add(WaveNums[i].WaveSpawns[WaveNum]);
        }
    }
}

//This is where each Enemy types data will be stored. Once it has its section Give it it's name and then change the array length to 20.
[System.Serializable]
public class WaveNumbers
{
    public string ClassName;
    public int[] WaveSpawns;
}
