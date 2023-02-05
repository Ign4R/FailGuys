using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Obstacle : MonoBehaviourPun
{
    [SerializeField] private Vector3 _dir;
    private Rigidbody _rb;
    [SerializeField] private float _speed;
    [SerializeField] private Transform _start;
    [SerializeField] private Transform _end;
    private Animator _anim;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
       // StartCoroutine(MovementForSeconds());

    }
    private void Update()
    {
        if (Vector3.Distance(transform.position, _start.position) < 1) //TODO: SINCRONIZAR REBOTE
        {
            _speed = Mathf.Abs(_speed);
        }
        if (Vector3.Distance(transform.position, _end.position) < 1)
        {
            _speed = -Mathf.Abs(_speed);
        }
        Movement();
        
    }

    //[PunRPC]
    public void Movement()
    {
        _anim.SetBool("Speed", true);
        var dir = _dir.normalized;
        dir *= _speed;
        _rb.velocity = dir;     

    }

    //public IEnumerator MovementForSeconds()
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSecondsRealtime(2f);
    //        photonView.RPC("Movement", PhotonNetwork.LocalPlayer);
    //    }
       
    //}

}
