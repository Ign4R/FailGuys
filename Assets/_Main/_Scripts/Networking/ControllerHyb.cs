using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Voice.PUN;
using UnityEngine.UI;
using System;

public class ControllerHyb : MonoBehaviour
{
    public MasterManager masterManager;
    public PhotonVoiceView voiceObject;
    public static Action<bool> OnRecorder;


    private void Awake()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            //Destroy(this);
        }
       
    }
    private void Start()
    {
        MasterManager.Instance.RPCMaster("RequestConnectPlayer", PhotonNetwork.LocalPlayer);
        voiceObject = PhotonNetwork.Instantiate("VoiceObject", Vector3.zero, Quaternion.identity).GetComponent<PhotonVoiceView>();

    }
    private void Update()
    {
       
        MasterManager.Instance.RecordingVoice(PhotonNetwork.LocalPlayer, voiceObject.IsRecording);
       


        float V = Input.GetAxisRaw("Vertical");
        Vector3 dir = new Vector3(0, 0, V);
        float mouseX = Input.GetAxis("Mouse X");

        if (dir != Vector3.zero && PhotonNetwork.PlayerList.Length == 2 || PhotonNetwork.PlayerList.Length == 4)
        {
            MasterManager.Instance.RPCMaster("RequestMove", PhotonNetwork.LocalPlayer, dir);
        }

        if (V <= 1f) 
        {
            MasterManager.Instance.RPCMaster("UpdateAnimMove", PhotonNetwork.LocalPlayer, V);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            MasterManager.Instance.RPCMaster("RequestJump", PhotonNetwork.LocalPlayer);
            MasterManager.Instance.RPCMaster("UpdateAnimJump", PhotonNetwork.LocalPlayer,true);
        }
        else if(Input.GetKeyUp(KeyCode.Space)) MasterManager.Instance.RPCMaster("UpdateAnimJump", PhotonNetwork.LocalPlayer, false);

        if (Input.GetAxisRaw("Mouse X") != 0)
        {
            
            MasterManager.Instance.RPCMaster("RequestRotateCam", PhotonNetwork.LocalPlayer, mouseX);
        }

    }

  
    public IEnumerator UpdateSpeaker()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(1f);
            MasterManager.Instance.photonView.RPC("SpeakerVoice", PhotonNetwork.LocalPlayer, PhotonNetwork.LocalPlayer, voiceObject.IsRecording);
        }

    }

}
