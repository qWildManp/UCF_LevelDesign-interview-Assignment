using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pursueState : State
{
    public combatState combatState;
    public idleState idleState;
    public override State Tick(EnemyManager enemyManager, EnemyAnimatorManager enemyAnimatorManager)
    {
        if (enemyManager.isPerformingAction)
            return this;
        Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
        enemyManager.distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, transform.position);
        float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
        //if performing stop our move
        if (enemyManager.isPerformingAction)
        {
            enemyAnimatorManager.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
            enemyManager.navMeshAgent.enabled = false;
        }
        else
        {

            if (enemyManager.distanceFromTarget > enemyManager.maxAttackRange)//if distance is not close set killer walk animation
            {
                enemyAnimatorManager.animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);

            }
        }
        HandleRotateTowardsTarget(enemyManager);
        //enemyManager.navMeshAgent.nextPosition = transform.position;
        enemyManager.navMeshAgent.transform.localPosition = Vector3.zero;
        enemyManager.navMeshAgent.transform.localRotation = Quaternion.identity;
        //chase target
        //if within attack range,switch to attack
        if(enemyManager.distanceFromTarget <= enemyManager.maxAttackRange)
        {
            return combatState;
        }
        if(enemyManager.distanceFromTarget >= enemyManager.maxDetectionRange || enemyManager.currentTarget.isPlayerInSafeArea)// if player out of the max dectection range or in safe area,killer stop chasing
        {
                enemyManager.currentTarget = null;//reset killer target
                enemyManager.navMeshAgent.nextPosition = transform.position;
                enemyAnimatorManager.animator.SetFloat("Vertical", 0);
                return idleState;
            
        }
        return this;
    }
    private void HandleRotateTowardsTarget(EnemyManager enemyManager)
    {
        //Rotate manually
        if (enemyManager.isPerformingAction)// if killer is performing stop our move
        {
            Vector3 direction = enemyManager.currentTarget.transform.position - transform.position;
            direction.y = 0;
            direction.Normalize();

            if (direction == Vector3.zero)
            {
                direction = transform.forward;
            }

            Quaternion tartgetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, tartgetRotation, enemyManager.rotationSpeed / Time.deltaTime);
        }
        //Rotate with pathfinding
        else
        {
            Vector3 relativeDirection = transform.InverseTransformDirection(enemyManager.navMeshAgent.desiredVelocity);
            Vector3 targetVelocoty = enemyManager.enemyRigiBody.velocity;

            enemyManager.navMeshAgent.enabled = true; //activate navmesh agent
            enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position); //move to player
            enemyManager.enemyRigiBody.velocity = enemyManager.navMeshAgent.desiredVelocity;
            enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, enemyManager.navMeshAgent.transform.rotation, enemyManager.rotationSpeed / Time.deltaTime);
        }
    }
}
