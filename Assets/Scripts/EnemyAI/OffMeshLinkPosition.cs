using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OffMeshLinkPosition : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public bool hasCheckedGroundPosition;
    [SerializeField] public bool innerRefPoint;
    [SerializeField] private OffMeshLinkPosition outterPoint;
    [SerializeField] public bool outterRefPoint;
    void Start()
    {
        hasCheckedGroundPosition = false;
        if(innerRefPoint)
            GetComponent<OffMeshLink>().enabled = false;
    }

    void Update(){
        if(!hasCheckedGroundPosition){
            //hasCheckedGroundPosition = true;
            TestHit();
        }
        else
        {
            if (innerRefPoint)
            {
                if(outterPoint.hasCheckedGroundPosition)
                //GetComponent<OffMeshLink>().enabled = false;
                    GetComponent<OffMeshLink>().enabled = true;
            }

        }
    }

    // Update is called once per frame
    void TestHit()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Ground");
        RaycastHit hitResult;
        Debug.DrawRay(transform.position, Vector3.down * 500f);
        
        if(Physics.Raycast(transform.position, Vector3.down, out hitResult, 500f, layerMask)){
            Debug.Log("hit: " + hitResult.collider.name);
            hasCheckedGroundPosition = true;
            transform.position =hitResult.point;
            transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y + 0.1f, transform.localPosition.z);//adjust the navmesh connection point
        }
    }
}
