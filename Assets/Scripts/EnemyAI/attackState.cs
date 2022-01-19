using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackState : State
{
    public combatState combatState;
    public EnemyAttackAction[] enemyAttacks;
    public EnemyAttackAction currentAttack;
    public idleState idleState;
    public override State Tick(EnemyManager enemyManager, EnemyAnimatorManager enemyAnimatorManager)
    {
        float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
        Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
        float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
        //check for attack range
        if (enemyManager.isPerformingAction)
            return this;
        if (enemyManager.currentTarget.GetPlayerCurrentHealth() <= 0)
            return idleState;
        if (currentAttack != null)
        {
            if (enemyManager.distanceFromTarget < currentAttack.minDistanceNeedToAttack)
            {
                return this;
            }
            else if (enemyManager.distanceFromTarget < currentAttack.maxDistanceNeedToAttack)
            {
                //if within range do attack
                if (enemyManager.viewableAngle <= currentAttack.maxAttackAngle
                    && enemyManager.viewableAngle >= currentAttack.minAttackAngle)
                {
                    if (enemyManager.currentRecoveryTime <= 0 && enemyManager.isPerformingAction == false)
                    {
                        enemyAnimatorManager.animator.SetFloat("Vertical", 0);
                        enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
                        enemyManager.isPerformingAction = true;
                        enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
                        currentAttack = null;
                        return combatState;
                    }
                }
            }
        }
        else
        {
            GetNewAttack(enemyManager);
        }
        return combatState;
    }
    private void GetNewAttack(EnemyManager enemyManager)
    {
        Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
        float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
        enemyManager.distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, transform.position);

        int maxScore = 0;

        for (int i = 0; i <enemyAttacks.Length; i++){
            EnemyAttackAction enemyAttackAction = enemyAttacks[i];
            Debug.Log(enemyAttacks);
            if(enemyManager.distanceFromTarget <= enemyAttackAction.maxDistanceNeedToAttack
                && enemyManager.distanceFromTarget >= enemyAttackAction.minDistanceNeedToAttack)
            {
                if(viewableAngle<= enemyAttackAction.maxAttackAngle
                    && viewableAngle >= enemyAttackAction.minAttackAngle)
                {
                    maxScore += enemyAttackAction.attackScore;
                }
            }
        }

        int randomValue = Random.Range(0, maxScore);
        int tempScore = 0;

        for (int i = 0; i < enemyAttacks.Length; i++)
        {
            EnemyAttackAction enemyAttackAction = enemyAttacks[i];
            if (enemyManager.distanceFromTarget <= enemyAttackAction.maxDistanceNeedToAttack
                && enemyManager.distanceFromTarget >= enemyAttackAction.minDistanceNeedToAttack)
            {
                if (viewableAngle <= enemyAttackAction.maxAttackAngle
                    && viewableAngle >= enemyAttackAction.minAttackAngle)
                {
                    if (currentAttack != null)
                        return;
                    tempScore += enemyAttackAction.attackScore;
                    if(tempScore > randomValue)
                    {
                        currentAttack = enemyAttackAction;
                    }
                }
            }
        } 
    }
}

