using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTest : MonoBehaviour
{
    public Transform parent;
    public int mouseSensibility = 100;

    private void Start()
    {
        //if (!photonView.IsMine) Destroy(gameObject);
        parent = transform.parent;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }


    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        RotateCamera(mouseX);
    }
   
    public void RotateCamera(float input)
    {
        float mouseX = input * mouseSensibility; //TODO: SACAR HARCODIADO
        //parent.Rotate(Vector3.up, mouseX * Time.deltaTime);
        var temp = parent.rotation;
        parent.rotation = temp * Quaternion.Euler(0, mouseX * Time.deltaTime, 0);
    }
}
