using Photon.Pun;
using Photon.Voice.PUN;
using UnityEngine;
using TMPro;
using System.Collections;
using Photon.Realtime;

public class CharacterView : MonoBehaviourPun
{
    private Animator _anim;
    public GameObject _micUI;
    public GameObject _speakerUI;
    public Animator Anim { get => _anim; private set => _anim = value; }
    private SkinnedMeshRenderer skinnedMesh;
    public int CharacterID { get; private set; }
    public SkinnedMeshRenderer SkinnedMesh { get => skinnedMesh; set => skinnedMesh = value; }

    public PhotonVoiceView _voiceView; 



    private void Awake()
    {
        _anim = GetComponent<Animator>();
        SkinnedMesh = GetComponent<SkinnedMeshRenderer>();
        
        
    }
    private void Start()
    {
       
        CharacterID = photonView.OwnerActorNr;
    }

    [PunRPC]
    public void SetSkin(Material meshMat)
    {
        skinnedMesh.material = meshMat;
    }

    void Update()
    {

        if (_voiceView.IsRecording) //TODO: Problema aqui
        {
            _micUI.SetActive(true);
            print("recording voice");

        }

        if (_voiceView.IsSpeaking) //TODO: Problema aqui
        {
            _speakerUI.SetActive(true);
            print("speaking voice");

        }


    }



}
