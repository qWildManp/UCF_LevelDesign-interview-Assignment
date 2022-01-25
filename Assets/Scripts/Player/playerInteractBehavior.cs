using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerInteractBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    public bool playerHasGun;
    public GameObject bullet;
    public GameObject gunPartical;
    public bool bullectGenerated;
    public float bulletSpeed;
    public float currentTime;
    public float coolDown;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;
        LayerMask layerMask = ~(1 << 9);
        if (Physics.Raycast(ray,out hit, 1000,layerMask))//player pick item
        {
            GameObject obj = hit.collider.gameObject;
            if (obj.GetComponent<RoomItem>())
            {
                RoomItem item = obj.GetComponent<RoomItem>();
                item.SetChecked(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    playerHasGun = true;
                    Destroy(obj);
                }
            }
            if (obj.GetComponent<PuzzleButton>())
            {
                PuzzleButton btn = obj.GetComponent<PuzzleButton>();
                Debug.Log("Hit puzzle btn");
                if (Input.GetKeyDown(KeyCode.E))
                {
                    btn.PressButton();
                }
            }

        }
        if (Physics.Raycast(ray, out hit, 10000, layerMask))//player shoot
        {
                GameObject obj = hit.collider.gameObject;
                if (Input.GetMouseButtonDown(0)&&playerHasGun)
                {
                if(!bullectGenerated){
                    GameObject bullectInstance = Instantiate(bullet);
                    bullectGenerated = true;
                    bullectInstance.transform.position = gunPartical.transform.position;
                    StartCoroutine(BulletMovement(bullectInstance,hit.point));
                }
                Debug.Log("Hit" + obj);
                }        
        }
    }
    protected IEnumerator BulletMovement(GameObject bullet,Vector3 end)
    {

        float sqrRemainDistance = (bullet.transform.position - end).sqrMagnitude;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        while (sqrRemainDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(rb.position, end, bulletSpeed * Time.deltaTime);
            rb.MovePosition(newPosition);
            sqrRemainDistance = (bullet.transform.position - end).sqrMagnitude;
            yield return null;
        }
    }
}
