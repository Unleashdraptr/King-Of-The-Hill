using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class PylonScript : MonoBehaviour
{
    private Animator Animate;
    public int Hp;
    public int Defense;


    public Slider Healthbar;
    private bool Timer;
    private float CountDown;
    // Start is called before the first frame update
    void Start()
    {
        Animate = GetComponent<Animator>();
        CountDown = 10;
    }

    // Update is called once per frame
    void Update()
    {
        Healthbar.value = Hp;
        if (Hp <= 0)
        {
            Hp = 0;
            Timer = true;
            GameManager.PauseState = true;
        }
        if (CountDown <= 0 && Hp == 0)
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().LostGame();
        }
        if (Timer)
            CountDown -= Time.deltaTime;
    }

    public void TakeDmg(int Attack)
    {
        if (Attack - Defense <= 0)
            Hp -= 1;   
        else
            Hp -= Attack - Defense;
    }
}
