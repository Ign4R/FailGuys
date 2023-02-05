using Photon.Pun;
using Photon.Voice.PUN;
using UnityEngine;
using TMPro;

public class CharacterView : MonoBehaviourPun
{
    private Animator _anim;
   [SerializeField] private GameObject _micUI;
    [SerializeField] private GameObject _speakerUI;
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

        if (_voiceView.IsRecording)
        {
            _micUI.SetActive(true);
            print("recording voice");
        }
        if (_voiceView.IsSpeaking)
        {
            _speakerUI.SetActive(true);
            print("IsSpeaking voice");
        }
        else
        {
            _speakerUI.SetActive(false);
        }


    }
}
