using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFootSteps : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, Vector3.down * 200);
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit, 200))
        {
            if (hit.collider.tag == "ground")
            {
                float speed = GetComponent<Rigidbody>().velocity.magnitude;
                if(GetComponent<AudioSource>().isPlaying == false && speed >2)
                {
                    GetComponent<AudioSource>().volume = Random.Range(0.8f, 1);
                    GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.1f);
                    GetComponent<AudioSource>().Play();
                }
            }
        }
    }
}
