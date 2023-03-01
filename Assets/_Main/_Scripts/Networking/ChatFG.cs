using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class ChatFG : MonoBehaviourPun
{
    public TextMeshProUGUI content;
    public TextMeshProUGUI nickNameUI;
    public TextMeshProUGUI roomNameUI;
    public TMP_InputField _inputF;
    private string _commandDm = "w/";
    private string _commandStart = "/start";
    private string _commandDead = "/dead";
    private string _commandPause = "/pause";
    private string _commandMute = "/mute";
    private string _commandAllMuted= "/mutedplayers";

    private void Start()
    {
        //nickNameUI.text = PhotonNetwork.LocalPlayer.NickName;
        //roomNameUI.text = PhotonNetwork.CurrentRoom.Name;
    }
    public void SendMessageGame()
    {
        var message = _inputF.text;
        if (string.IsNullOrEmpty(message) || string.IsNullOrWhiteSpace(message)) return;
        string[] words = message.Split(' ');
        if (message != _commandDead &&  message != _commandPause && message != _commandStart || message != _commandMute  && message != _commandAllMuted )
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
        else
        {
            /////AUTORIDAD MASTER
            if (message == _commandDead)
            {
                print("HE MUERTO");
                //TODO
            }
            else if (message == _commandStart)
            {
                print("EMPIEZA EL JUEGO ");
                //TODO
            }
            else if (message == _commandPause)
            {
                print("el juego se ha pausado");
                //TODO
            }
            /////AUTORIDAD LOCAL
            else if (message == _commandMute)
            {
                print("te has mutiado ");
                //TODO
            }
            else if (message == _commandAllMuted)
            {
                print("has mutiado a todos");
                //TODO
            }
            _inputF.text = "";
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
}
