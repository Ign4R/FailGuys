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
    private string _commandDm= "w/";

    private void Start()
    {
        nickNameUI.text = PhotonNetwork.LocalPlayer.NickName;
        roomNameUI.text = PhotonNetwork.CurrentRoom.Name;
    }
    public void SendMessageGame()
    {
        var message = _inputF.text;
        if (string.IsNullOrEmpty(message) || string.IsNullOrWhiteSpace(message)) return;
        string[] words = message.Split(' ');
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
            photonView.RPC("GetChatMessage", RpcTarget.All, PhotonNetwork.NickName, message,false);
            _inputF.text = "";
        }
    }

    [PunRPC]
    public void GetChatMessage(string nameClient, string message, bool dm=false)
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
