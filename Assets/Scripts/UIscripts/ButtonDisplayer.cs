using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonDisplayer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject Info_UI;
    [SerializeField] GameObject[] requireItems;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(GameObject requireItem in requireItems)
        {
            if (Info_UI.GetComponent<ItemInfoDisplayer>().currentDisplayItem == requireItem)
            {
                this.gameObject.GetComponent<Button>().interactable = true;
                return;
            }
        }
        this.gameObject.GetComponent<Button>().interactable = false;
    }
}
