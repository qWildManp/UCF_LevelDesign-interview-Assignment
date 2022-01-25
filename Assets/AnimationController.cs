using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public GameObject flashlight_animator;
    public GameObject gun_animator;
    public Animator activeAnimator;
    public CharacterController cc;
    public playerInteractBehavior playerInteract;
    // Start is called before the first frame update
    void Start()
    {
        activeAnimator = flashlight_animator.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInteract.playerHasGun)
        {
            PutOffHand();
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (gun_animator.activeInHierarchy)
            {
                Shoot();
            }
        }
        Walk();
    }

    public void Walk()
    {
        if(cc.velocity.magnitude > 0)
        {
            flashlight_animator.GetComponent<Animator>().SetBool("walk", true);
            gun_animator.GetComponent<Animator>().SetBool("walk", true);
        }else if (cc.velocity.magnitude == 0) 
        {
            flashlight_animator.GetComponent<Animator>().SetBool("walk", false);
            gun_animator.GetComponent<Animator>().SetBool("walk", false);
        }
            
    }

    public void PutOffHand()
    {
        flashlight_animator.GetComponent<Animator>().SetBool("hasGun", true);
    }
    public void PutOnGun()
    {
        gun_animator.SetActive(true);
        gun_animator.GetComponent<Animator>().SetBool("hasGun", true);
    }

    public void Shoot()
    {
        
        gun_animator.GetComponent<Animator>().SetBool("shoot", true);
    }

    public void ShootFinish()
    {
        gun_animator.GetComponent<Animator>().SetBool("shoot", false);
    }
}
