using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour
{
    CharacterController cc;
    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 horizontalVelocity = new Vector3(cc.velocity.x, 0, cc.velocity.z);
        if (cc.isGrounded == true && horizontalVelocity.magnitude > 60f && GetComponent<AudioSource>().isPlaying == false)
        {
            Debug.Log("velocity: " + cc.velocity.magnitude);
            GetComponent<AudioSource>().volume = Random.Range(0.3f, 0.4f);
            GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.1f);
            GetComponent<AudioSource>().Play();
        }
    }
}
