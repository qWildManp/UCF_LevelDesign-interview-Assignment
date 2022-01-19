using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkToObject : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float linkSpeed;
    [SerializeField] private GameObject KillerObject;
    private OffMeshLinkData _currLink;
    private bool isLinking;
    private float originalSpeed;
    [SerializeField] private float countDown;
    private float currentCountDown;
    private NavMeshAgent agent;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = KillerObject.GetComponent<NavMeshAgent>();
        originalSpeed = agent.speed;
        isLinking = false;
        countDown = 3;
        currentCountDown = countDown;
        agent.autoTraverseOffMeshLink = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void FixedUpdate(){
        if(agent.isOnOffMeshLink){
            if (!isLinking)
            {
                isLinking = true;
                _currLink = agent.currentOffMeshLinkData;
            }
            var newPos = Vector3.Lerp(_currLink.startPos, _currLink.endPos, countDown);
            newPos.y += 2f * Mathf.Sin(Mathf.PI * countDown);
            //Update transform position
            transform.position = newPos;
            currentCountDown -= Time.deltaTime;
            if (currentCountDown <= 0)
            {
                transform.position = _currLink.endPos;
                isLinking = false;
                agent.CompleteOffMeshLink();
                agent.isStopped = false;
            }
        }
        else if(agent.isOnNavMesh && isLinking)
        {
            isLinking = false;
            agent.velocity = Vector3.zero;
            agent.speed = originalSpeed;
        }
    }

    public void MoveToTarget(){
        animator.SetBool("walk", true);
        agent = KillerObject.GetComponent<NavMeshAgent>();
        agent.destination = target.transform.position;
        /*
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(target.transform.position, path);
        Debug.Log(path.status);
        if (path.status == NavMeshPathStatus.PathComplete || path.status == NavMeshPathStatus.PathPartial){
            Debug.Log("Path Found!");
            agent.SetPath(path);
        }
        else{
            Debug.Log("Path Not Found!");
        }*/
    }
}
