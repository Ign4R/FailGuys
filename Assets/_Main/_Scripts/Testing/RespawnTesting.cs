using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnTesting : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.transform.position = Vector3.zero;
    }
}
