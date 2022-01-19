using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="A.I./Enemey Actions/Attack Action")]
public class EnemyAttackAction : EnemyAction
{
    public int attackScore = 3;
    public float recoveryTime = 2;

    public float maxAttackAngle = 35;
    public float minAttackAngle = -35;

    public float minDistanceNeedToAttack = 0;
    public float maxDistanceNeedToAttack = 60;
}
