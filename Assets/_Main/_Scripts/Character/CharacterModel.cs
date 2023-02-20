using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterModel : MonoBehaviourPun
{
    [SerializeField] private GameObject canvasWin;
    [SerializeField] private GameObject canvasLose;
    [SerializeField] private float speed;
    [SerializeField] private float modifiedSpeed;
    [SerializeField] private GameObject _cam;
    [SerializeField] private GameObject _voice;
    private CharacterView _cv;
    public GameObject Cam { get => _cam; set => _cam = value; }
    public GameObject Voice { get => _voice; set => _voice = value; }

    public Action<CharacterModel> OnDie;

    private Rigidbody _rb;
    private int jumpHeight = 5;
    private bool touchGround;




    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _cv = GetComponent<CharacterView>();
    }
    private void Start()
    {       
        modifiedSpeed = Vector3.one.x;
    }
    private void Update()
    {
       
    }
    public void Move(Vector3 dir)
    {
        dir *= (speed * modifiedSpeed);
        dir.y = _rb.velocity.y;
        _rb.velocity = dir;

    }

    public void Jump()
    {

        if (touchGround)
        {
            touchGround = false;
            _rb.velocity = new Vector3(_rb.velocity.x, jumpHeight, _rb.velocity.z);
        }

    }

    public void ChangeSpeed(float _speed)
    {
        modifiedSpeed = _speed;

    }

    [PunRPC]
    public void LoseGame()
    {
        canvasLose.SetActive(true);
    }

    [PunRPC]
    public void WinGame()
    {
        canvasWin.SetActive(true);
        Time.timeScale = 0f;
    }


    private void OnCollisionEnter(Collision collision)
    {

        if (photonView.IsMine)
        {
            if (collision.gameObject.tag == "Floor")
            {
                touchGround = true;
                print("touch ground");
            }
           
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine)
        {
            if (other.gameObject.tag == "Dead")
            {
                _rb.constraints = RigidbodyConstraints.FreezePosition;
                print("am dead");
                OnDie(this);
            }
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (photonView.IsMine)
        {
            if (collision.gameObject.tag == "Obstacle")
            {
                touchGround = false;

            }

        }
    }





}
