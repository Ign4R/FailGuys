using Photon.Pun;
using Photon.Voice.PUN;
using UnityEngine;
using TMPro;
using System.Collections;
using Photon.Realtime;
using UnityEngine.UI;

public class CharacterView : MonoBehaviourPun
{
    private Animator _anim;
   
    public Image _speakerUI;
    public Animator Anim { get => _anim; private set => _anim = value; }
    private SkinnedMeshRenderer skinnedMesh;
    public int CharacterID { get; private set; }
    public SkinnedMeshRenderer SkinnedMesh { get => skinnedMesh; set => skinnedMesh = value; }





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

    //public void IsRecording(bool record)
    //{
    //    if (photonView.IsMine)
    //    {
    //       _micUI.enabled = record;

    //    }
    //}

    void Update()
    {

      

    }



}
