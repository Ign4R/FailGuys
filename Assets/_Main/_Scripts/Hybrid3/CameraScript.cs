using Photon.Pun;
using UnityEngine;

public class CameraScript : MonoBehaviourPun
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


    void LateUpdate()
    {
        //var newPosition = parent.transform.position;
        //transform.position = newPosition;


    }
    public void RotateCamera(float input)
    {
        float mouseX = input * mouseSensibility; //TODO: SACAR HARCODIADO
        //parent.Rotate(Vector3.up, mouseX * Time.deltaTime);
        var temp = parent.rotation;
        parent.rotation = temp * Quaternion.Euler(0, mouseX * Time.deltaTime, 0);

    }
}
