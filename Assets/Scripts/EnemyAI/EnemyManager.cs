using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyManager : MonoBehaviour
{
    EnemyAnimatorManager enemyAnimatorManager;

    public State currenState;
    public PlayerStats currentTarget;
    public NavMeshAgent navMeshAgent;
    public Rigidbody enemyRigiBody;

    public bool isPerformingAction;
    public float distanceFromTarget;
    public float rotationSpeed = 25;
    public float maxAttackRange = 60;
    public float viewableAngle;

    [Header("A.I. Settings")]
    public float maxDetectionRange;
    public float detectionRadius;
    //The higher, and lower, respetetively these angle are, the greater detection FIELD OF VIEW
    public float maxDetectionAngle = 50;
    public float minDetectionAngle = -50;
    public float currentRecoveryTime = 0;
    void Awake()
    {
        enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        navMeshAgent.enabled = true;
        enemyRigiBody = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        enemyRigiBody.isKinematic = false;
    }
    private void Update()
    {
        HandleRecoveryTimer();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        HandleStateMachine();
    }

    private void HandleStateMachine()
    {
        if(currenState != null)
        {
            State nextState = currenState.Tick(this, enemyAnimatorManager);

            if(nextState != null)
            {
                SwitchToNextState(nextState);
            }
        }
    }
    
    private void SwitchToNextState(State state)
    {
        currenState = state;
    }
    private void HandleRecoveryTimer()
    {
        if(currentRecoveryTime > 0)
        {
            currentRecoveryTime -= Time.deltaTime;
        }

        if (isPerformingAction)
        {
            if(currentRecoveryTime <= 0)
            {
                isPerformingAction = false;
            }
        }
    }
}
