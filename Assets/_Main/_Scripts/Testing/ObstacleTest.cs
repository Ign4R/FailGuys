using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleTest : MonoBehaviour
{
    [SerializeField] private Vector3 _dir;
    private Rigidbody _rb;
    [SerializeField] private float _speed;
    [SerializeField] private Transform _start;
    [SerializeField] private Transform _end;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();

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

 
    public void Movement()
    {
        transform.position += _dir * _speed * Time.deltaTime;


    }

   
}
