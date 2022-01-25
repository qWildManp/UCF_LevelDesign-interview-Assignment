using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienBehavior : MonoBehaviour
{
    public int health;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LossHealth()
    {
        health -= 1;
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
