using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buletBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    public float currentTime;
    public float stayTime;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if(currentTime >= stayTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<AlienBehavior>())
        {
            other.GetComponent<AlienBehavior>().LossHealth();
        }
    }
}
