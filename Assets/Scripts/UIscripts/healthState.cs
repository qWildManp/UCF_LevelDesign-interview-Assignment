using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthState : MonoBehaviour
{
    [SerializeField] PlayerStats playerStats;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int currenthealth = playerStats.GetPlayerCurrentHealth();
        if(1 < currenthealth &&currenthealth < 4)
        {
            transform.GetChild(0).GetComponent<Image>().color = new Color32(212, 190, 0, 100);
        }
        else if(currenthealth == 1)
        {
            transform.GetChild(0).GetComponent<Image>().color = new Color32(195, 0, 0, 100);
        }
        else if(currenthealth == 0)
        {
            transform.GetChild(0).GetComponent<Image>().color = new Color32(0, 0, 0, 100);
        }
        else
        {
            transform.GetChild(0).GetComponent<Image>().color = new Color32(0, 197, 12, 100);
        }
    }
}
