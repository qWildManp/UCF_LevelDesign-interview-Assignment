using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using TMPro;
public class PlayerStats : MonoBehaviour
{
    GameObject UI;
    [SerializeField]private int currenthealth;
    [SerializeField]private int maxhealth;
    [SerializeField] bool playerDead;
    public bool isPlayerInSafeArea;
    [SerializeField] GameObject playerCamera;
    private Animator playerCameraAnimator;
    // Start is called before the first frame update
    void Start()
    {
        UI = GameObject.Find("Canvas");
        playerDead = false;
        playerCameraAnimator = playerCamera.GetComponent<Animator>();
        playerCameraAnimator.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(currenthealth == 0)
        {
            playerDead = true;
        }
        if (playerDead)
        {
            playerCameraAnimator.enabled = true;
            GetComponent<FirstPersonController>().enabled = false;
            playerCameraAnimator.Play("PlayerDeath");
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            Invoke("ActivePlayerDiedUI", 1f);
        }
    }
    private void ActivePlayerDiedUI()
    {
        UI.GetComponent<MsgDisplayer>().ActivePlayerDiedUI();
    }
    public void GetHit()
    {
        if(currenthealth > 0)
            currenthealth -= 1;
    }
    public int GetPlayerCurrentHealth()
    {
        return currenthealth;
    }
    public void ResetPlayer()
    {
        playerDead = false;
    }
    public bool Healing()
    {
        if (currenthealth < maxhealth)
        {
            currenthealth += 1;
            return true;
        }
        else
        {
            UI.GetComponent<MsgDisplayer>().SetMessage("I don't need it");
            return false;
        }
    }
}
