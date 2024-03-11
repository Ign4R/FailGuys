using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Voice.PUN;
using UnityEngine.UI;
using System;
using TMPro;

public class ControllerHyb : MonoBehaviour
{
 
    public Image _micUI;
    public MasterManager masterManager;
    public PhotonVoiceView voiceObject;
    public static Action<bool> OnRecorder;
    public TMP_InputField _inputF;

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
        StartCoroutine(UpdateSpeaker());    
    }
    private void Update()
    {
  
        float V = Input.GetAxisRaw("Vertical");
        Vector3 dir = new Vector3(0, 0, V);
        float mouseX = Input.GetAxis("Mouse X");

        if (_inputF.enabled)
        {
            V = 0;
        }

        if (!_inputF.enabled) 
        {
            if (dir != Vector3.zero)
            {
                MasterManager.Instance.RPCMaster("RequestMove", PhotonNetwork.LocalPlayer, dir);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                MasterManager.Instance.RPCMaster("RequestJump", PhotonNetwork.LocalPlayer);
                MasterManager.Instance.RPCMaster("UpdateAnimJump", PhotonNetwork.LocalPlayer, true);
            }
        }
       
        if (V <= 1f)
        {
            MasterManager.Instance.RPCMaster("UpdateAnimMove", PhotonNetwork.LocalPlayer, V);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            MasterManager.Instance.RPCMaster("UpdateAnimJump", PhotonNetwork.LocalPlayer, false);
        }

        if (Input.GetAxisRaw("Mouse X") != 0)
        {
            MasterManager.Instance.RPCMaster("RequestRotateCam", PhotonNetwork.LocalPlayer, mouseX);
        }



    }

  
    public IEnumerator UpdateSpeaker()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(0.5f);
            _micUI.enabled = voiceObject.IsRecording;
            MasterManager.Instance.RPCMaster("UpdateSpeakerVoice", PhotonNetwork.LocalPlayer, _micUI.enabled);
        }

    }

}
