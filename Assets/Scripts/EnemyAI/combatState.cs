using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class combatState : State
{
    public attackState attackState;
    public pursueState pursueState;
    public override State Tick(EnemyManager enemyManager, EnemyAnimatorManager enemyAnimatorManager)
    {
        Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
        float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
        if(enemyManager.currentRecoveryTime <= 0 && enemyManager.distanceFromTarget <= enemyManager.maxAttackRange)
        {
            return attackState;
        }
        else if(enemyManager.distanceFromTarget > enemyManager.maxAttackRange)
        {
            return pursueState;
        }
        else
        {
            return this;
        }
    }
}
