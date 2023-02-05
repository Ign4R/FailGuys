using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Barrier : MonoBehaviourPun
{
    private Vector2 dir;
    private Rigidbody _rb;
    [SerializeField] private float offset = 8f;
    [SerializeField] private float _speed;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    [PunRPC]
    public void Movement()
    {
        dir = Vector2.right.normalized;
        dir *= _speed;
        dir.y = 0;
        _rb.velocity = dir;
        if (transform.position.x - offset < -14)
        {
            print("#");
            _speed = Mathf.Abs(_speed);
        }
        if (transform.position.x + offset > 14)
        {
            print("0");
            _speed = -Mathf.Abs(_speed);
        }

    }

   

}
