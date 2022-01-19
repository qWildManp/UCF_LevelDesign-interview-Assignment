using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLocomotionSystem : MonoBehaviour
{
    EnemyManager enemyManager;
    EnemyAnimatorManager enemyAnimatorManager;
    NavMeshAgent navMeshAgent;
    public Rigidbody enemyRigiBody;
    public PlayerStats currentTarget;
    public LayerMask detectionLayer;
    public float distanceFromTarget;
    public float stoppingDistance = 40f;

    public float rotationSpeed = 25;
    private void Awake()
    {
        enemyManager = GetComponent<EnemyManager>();
        enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        enemyRigiBody = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        navMeshAgent.enabled = false;
        enemyRigiBody.isKinematic = false;
    }
    public void HandleDetection()// Detacte player
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, detectionLayer);

        foreach (var collider in colliders)
        {
            if(collider.tag == "Player")
            {

                Vector3 playerPos = collider.transform.position;
                Vector4 targetDirection = playerPos - transform.position;
                Debug.Log(targetDirection);
                float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
                Debug.Log("angle" + viewableAngle);
                if(viewableAngle > enemyManager.minDetectionAngle && viewableAngle < enemyManager.maxDetectionAngle)
                {
                    currentTarget = collider.GetComponentInParent<PlayerStats>();
                }

            }
        }
    }
    public void HandleMoveToTarget()
    {
        if (enemyManager.isPerformingAction)
            return;
        Vector3 targetDirection = currentTarget.transform.position - transform.position;
        distanceFromTarget = Vector3.Distance(currentTarget.transform.position, transform.position);
        float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
        //if performing stop our move
        if (enemyManager.isPerformingAction)
        {
            enemyAnimatorManager.animator.SetFloat("Vertical", 0, 0.1f,Time.deltaTime);
            navMeshAgent.enabled = false;
        }
        else
        {
            Debug.Log("actual dis" + distanceFromTarget);
            Debug.Log("close enough " + (distanceFromTarget <= stoppingDistance));
            
            if(distanceFromTarget > stoppingDistance)//if distance is not close set killer walk animation
            {
                enemyAnimatorManager.animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
              
            }
            else if(distanceFromTarget <= stoppingDistance)//if distance is close enough set killer stop animation
            {
                Debug.Log("stop");
                enemyAnimatorManager.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
            }
        }
        HandleRotateTowardsTarget();
        navMeshAgent.transform.localPosition = Vector3.zero;
        navMeshAgent.transform.localRotation = Quaternion.identity;
    }

    private void HandleRotateTowardsTarget()
    {
        Debug.Log("Handle rotate");
        //Rotate manually
        if (enemyManager.isPerformingAction)// if killer is performing stop our move
        {
            Vector3 direction = currentTarget.transform.position - transform.position;
            direction.y = 0;
            direction.Normalize();

            if(direction == Vector3.zero)
            {
                direction = transform.forward;
            }

            Quaternion tartgetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, tartgetRotation, rotationSpeed / Time.deltaTime);
        }
        //Rotate with pathfinding
        else
        {
            Vector3 relativeDirection = transform.InverseTransformDirection(navMeshAgent.desiredVelocity);
            Vector3 targetVelocoty = enemyRigiBody.velocity;

            navMeshAgent.enabled = true; //activate navmesh agent
            //TODO : killer position does not change
            bool result = navMeshAgent.SetDestination(currentTarget.transform.position); //move to player
            enemyRigiBody.velocity = navMeshAgent.desiredVelocity;
            transform.rotation = Quaternion.Slerp(transform.rotation, navMeshAgent.transform.rotation, rotationSpeed / Time.deltaTime);
        }
        Debug.Log("Finish");
    }
}
