using Photon.Pun;
using Photon.Voice.PUN;
using UnityEngine;
using TMPro;
using System.Collections;
using Photon.Realtime;
using UnityEngine.UI;

public class CharacterView : MonoBehaviourPun
{
    [SerializeField]private Animator _anim;
   
    public Image _speakerUI;
    public Animator Anim { get => _anim; private set => _anim = value; }
    [SerializeField] private SkinnedMeshRenderer _skinnedMesh;
    public int CharacterID { get; private set; }
    public SkinnedMeshRenderer SkinnedMesh { get => _skinnedMesh; set => _skinnedMesh = value; }

    private void Start()
    {
        CharacterID = photonView.OwnerActorNr;
    }

    [PunRPC]
    public void SetSkin(Color skinColor)
    {
        _skinnedMesh.materials[0].SetColor("_EmissionColor", skinColor);
        _skinnedMesh.materials[1].color = skinColor;
    }

    [PunRPC]
    public void IsRecording(bool record)
    {
        _speakerUI.enabled = record;
    }

    void Update()
    {

      

    }



}
