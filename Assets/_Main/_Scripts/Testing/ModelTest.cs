using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelTest : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private float modifiedSpeed;


    private Rigidbody _rb;
    private int jumpHeight = 5;
    private bool touchGround;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        modifiedSpeed = Vector3.one.x;
    }
    private void Update()
    {

        //MODE TESTING
        ///
        float V = Input.GetAxisRaw("Vertical");
        Vector3 dir = new Vector3(0, 0, V);
        Move(dir);

    }
    public void Move(Vector3 dir)
    {
        Vector3 x = transform.right * dir.x;
        Vector3 z = transform.forward * dir.z;
        Vector3 dirFinal = (x + z).normalized;
        dirFinal *= (speed * modifiedSpeed);
        dirFinal.y = _rb.velocity.y;
        _rb.velocity = dirFinal;

    }


    public void Jump()
    {

        if (touchGround)
        {
            touchGround = false;
            _rb.velocity = new Vector3(_rb.velocity.x, jumpHeight, _rb.velocity.z);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Floor")
        {
            touchGround = true;
            print("touch ground");
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Dead")
        {
            _rb.constraints = RigidbodyConstraints.FreezePosition;
            print("am dead");
            //OnDie(this);
        }
    }
    private void OnCollisionStay(Collision collision)
    {

        if (collision.gameObject.tag == "Obstacle")
        {
            touchGround = false;

        }


    }
}
