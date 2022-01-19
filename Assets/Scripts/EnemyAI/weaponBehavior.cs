using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponBehavior : MonoBehaviour
{
    public EnemyManager enemy;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInParent<Animator>();
    }

    public void EnableWeaponCollider(bool enabled){
        GetComponent<Collider>().enabled = enabled;
    }

    // Update is called once per frame
    void Update()
    {/*
        if(animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")){
            GetComponent<Collider>().enabled = true;
        }
        else{
            GetComponent<Collider>().enabled = false;
        }*/
        /*
       if(enemy.currenState.name == "attackState" || enemy.currenState.name == "combatState")
        {
            GetComponent<Collider>().enabled = true;
        }
        else
        {
            GetComponent<Collider>().enabled = false;
        }*/
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("hit:" + other.name);
            other.GetComponent<PlayerStats>().GetHit();
        }
    }
}
