using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class QuadModel : MonoBehaviourPun
{
    [SerializeField] private Animation anim;
    private float timer;
    private bool startTimer = false;
    [SerializeField]private float timerToDestroy=3f;

    private void Update()
    {
        if(startTimer)
        timer = timer += Time.deltaTime;
        if (timer >= timerToDestroy && startTimer)
        {
            timer = 0;          
            PhotonNetwork.Destroy(gameObject);
            startTimer = false;

        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (photonView.IsMine)
        {
            if (collision.gameObject.tag != "Obstacle")
            {
                
                photonView.RPC("UpdateAnimQuad", RpcTarget.All);
                startTimer = true;
            }
        }
       

    }

    [PunRPC]
    public void UpdateAnimQuad()
    {
        anim.Play("QuadDestroy");
    }
}


