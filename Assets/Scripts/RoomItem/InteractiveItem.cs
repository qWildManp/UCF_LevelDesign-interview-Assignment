using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class InteractiveItem : MonoBehaviour
{
    protected bool isChecked;
    protected Animator itemAnimator;
    protected GameObject player;
    protected Transform playerCapsule;
    protected Transform playerCamera;
    protected string msg;
    protected void SetHightLight(bool result)
    {
        Outline[] objOutlines = GetComponentsInChildren<Outline>();
        foreach (var outline in objOutlines)
        {
            outline.enabled = result;
        }
    }
    public void SetChecked(bool result)
    {
        isChecked = result;
    }
}
