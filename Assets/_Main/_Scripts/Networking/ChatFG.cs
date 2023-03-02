using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Voice.PUN;
using Photon.Voice.Unity;

public class ChatFG : MonoBehaviourPun
{
    public TextMeshProUGUI content;
    public TextMeshProUGUI nickNameUI;
    public TextMeshProUGUI roomNameUI;
    public TMP_InputField _inputF;
    private string _commandDm = "w/";
    private string _commandStart = "/start";
    private string _commandDead = "/exit";
    private string _commandPause = "/pause";
    private string _commandMute = "/mute";
    private string _commandAllMuted= "/mutedplayers";
    private float valueTimeScale;
    private Recorder pvoice; 

    private void Start()
    {
        valueTimeScale = 1f;
        pvoice= FindObjectOfType<Recorder>();
        //nickNameUI.text = PhotonNetwork.LocalPlayer.NickName;
        //roomNameUI.text = PhotonNetwork.CurrentRoom.Name;
    }
    public void SendMessageGame()
    {
        var message = _inputF.text;
        if (string.IsNullOrEmpty(message) || string.IsNullOrWhiteSpace(message)) return;
        string[] words = message.Split(' ');
        if (message != _commandDead &&  message != _commandPause && message != _commandStart && message != _commandMute  && message != _commandAllMuted )
        {
            if (words.Length > 2 && words[0] == _commandDm)
            {
                var target = words[1];
                foreach (var currPlayer in PhotonNetwork.PlayerList)
                {
                    if (target == currPlayer.NickName)
                    {
                        var currMessage = string.Join(" ", words, 2, words.Length - 2);
                        photonView.RPC("GetChatMessage", currPlayer, PhotonNetwork.NickName, currMessage, true);
                        GetChatMessage(PhotonNetwork.NickName, currMessage);
                        return;
                    }


                }
                content.text += "<color=black>" + "NO EXISTE ESTE USUARIO" + "</color>" + "\n";
                _inputF.text = "";
            }
            else
            {
                photonView.RPC("GetChatMessage", RpcTarget.All, PhotonNetwork.NickName, message, false);
                _inputF.text = "";
            }
        }
        if (message == _commandDead)
        {
            print("dead player");
            MasterManager.Instance.RPCMaster("DisconnectGame", PhotonNetwork.LocalPlayer);
            _inputF.text = "";
            //TODO
        }
        else if (message == _commandStart)
        {
            print("EMPIEZA EL JUEGO ");
            MasterManager.Instance.RPCMaster("StartGame", false);
            _inputF.text = "";
            //TODO
        }
        else if (message == _commandPause)
        {
            if (valueTimeScale == 1)
            {
                valueTimeScale = 0f;
                MasterManager.Instance.RPCMaster("PauseGame", valueTimeScale);
                _inputF.text = "";
            }
            else
            {
                valueTimeScale = 1f;
                MasterManager.Instance.RPCMaster("PauseGame", valueTimeScale);
                _inputF.text = "";
            }

            //TODO
        }
        /////AUTORIDAD LOCAL
        else if (message == _commandMute)
        {
            print("te has mutiado ");
            photonView.RPC("MeMuted", RpcTarget.All);

            //TODO
        }
        else if (message == _commandAllMuted)
        {
            photonView.RPC("MutedAll", RpcTarget.All, false);
            //TODO
        }


    }

    [PunRPC]
    public void GetChatMessage(string nameClient, string message, bool dm = false)
    {
        string color;
        if (PhotonNetwork.NickName == nameClient)
        {
            color = "<color=green>";

        }
        else if (dm)
        {
            color = "<color=yellow>";
        }
        else
        {
            color = "<color=red>";
        }
        content.text += color + nameClient + ":" + "</color>" + message + "\n";
    }

    [PunRPC]
    public void MeMuted()
    {
        bool pvoiceBool = pvoice.RecordingEnabled;
        if (pvoiceBool == false)
        {
            pvoice.RecordingEnabled = true;
            print("transmit true");
        }
        if (pvoiceBool == true)
        { 
            pvoice.RecordingEnabled = false;
            print("transmit false");
        }
    }

    //[PunRPC]
    //public void MutedAll(bool value)
    //{
    //    pvoice.SpeakerInUse.enabled = value;
    //}
}
