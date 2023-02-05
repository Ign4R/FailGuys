using Photon.Pun;
using UnityEngine;

public class CharacterController : MonoBehaviourPun
{
    private CharacterModel model;
    private CharacterView _view;
    private float timePressed;
    private void Awake()
    {
        model = GetComponent<CharacterModel>();
        _view = GetComponent<CharacterView>();
    }
    private void Start()
    {
      
    }

    private void Update()
    {


    }
}


