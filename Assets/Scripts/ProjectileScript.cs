using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public int Damage;
    public float KillTime;

    private void Update()
    {
        transform.Translate(10 * Time.deltaTime, 0 ,0);
        KillTime -= Time.deltaTime;
        if(KillTime <= 0)
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            other.GetComponent<Enemy_AI>().TakeDmg(Damage);
            Destroy(gameObject);
        }
    }
}
