using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//TODO 
public class DragRotate : MonoBehaviour
{
    private bool onDrag = false;
    [SerializeField]private float speed = 6f;
    [SerializeField ] private float Rspeed;
    private float tempSpeed;
    private float axisX;
    private float axisY;
    private float pitch;
    private float roll;
    private float cXY;

    private void OnMouseDown()
    {
        Debug.Log("Mouse Down");
        axisX = 0f;
        axisY = 0f;
    }
    private void OnMouseDrag()
    {
        Debug.Log("Mouse drag");
        onDrag = true;
        axisX = Input.GetAxis("Mouse X");
        axisY = -Input.GetAxis("Mouse Y");
        cXY = Mathf.Sqrt(axisX * axisX + axisY * axisY);
        if(cXY == 0f)
        {
            cXY = 1f;
        }
    }
    private float Rigid()
    {
        if (onDrag)
        {
            tempSpeed = speed;
        }
        else
        {
            if (tempSpeed > 0)
                tempSpeed -= speed * 2 * Time.deltaTime / cXY;
            else
                tempSpeed = 0;
        }
        return tempSpeed;
    }
    // Start is called before the first frame update
    void Start()
    {
        roll = 0f;
        pitch = 0f;
    }
    public void ResetRotation()
    {
        pitch = 0;
        roll = 0;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(Mathf.Abs(roll + axisX) > 30)
        {
            var sign = Mathf.Abs(roll + axisX) / (roll + axisX);
            roll = sign * 30;
        }
        else
        {
            roll += axisX * Rigid();
        }
        if (Mathf.Abs(pitch + axisY) > 30)
        {
            var sign = Mathf.Abs(pitch + axisY) / (pitch + axisY);
            pitch = sign * 30;
        }
        else
        {
            pitch += axisY * Rigid();
        }
        Quaternion quanternion = Quaternion.Euler(pitch, 0, roll);
        transform.localRotation =Quaternion.RotateTowards(transform.localRotation ,quanternion , Time.deltaTime * Rspeed);
        if (!Input.GetMouseButton(0))
        {
            onDrag = false;
        }
    }
}
