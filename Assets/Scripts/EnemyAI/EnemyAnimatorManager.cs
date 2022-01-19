using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorManager : MonoBehaviour
{
    public Animator animator;
    private weaponBehavior weapon;
    EnemyManager enemyManager;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        weapon = GetComponentInChildren<weaponBehavior>();
        enemyManager = GetComponentInParent<EnemyManager>();
    }
    public void PlayTargetAnimation(string targetAnim, bool isInteracting)
    {
        animator.applyRootMotion = isInteracting;
        animator.SetBool("IsInteracting", isInteracting);
        animator.CrossFade(targetAnim, 0.2f);
    }

    private void OnAnimatorMove()// can the velocity of killer speed
    {
        float delta = Time.deltaTime;
        enemyManager.enemyRigiBody.drag = 0;
        Vector3 deltaPos = animator.deltaPosition;
        deltaPos.y = 0;
        Vector3 velocity = deltaPos / delta;
        enemyManager.enemyRigiBody.velocity = velocity;

    }

    private void SetWeaponHitboxEnabled(){
        weapon.EnableWeaponCollider(true);
    }

    private void SetWeaponHitboxDisabled(){
        weapon.EnableWeaponCollider(false);
    }

}
