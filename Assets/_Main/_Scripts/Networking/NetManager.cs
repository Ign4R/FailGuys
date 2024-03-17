using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class NetManager : MonoBehaviourPunCallbacks
{
   
    public TMP_InputField _nickInputF;
    public TMP_InputField _roomInputF;
    public TMP_InputField _roomJoinInputF;
    public TMP_InputField _playersInputF;
    public Button _createRoom_b;
    public Button _joinRoom_b;
    public TextMeshProUGUI status;
    public GameObject nameSetMenu;
    public GameObject roomOptionMenu;
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        _createRoom_b.interactable = false;
        _joinRoom_b.interactable = true;
        status.text = "Connecting To Master";
    }
    public void OnEndSetName()
    {
        var message = _nickInputF.text;
        if (string.IsNullOrEmpty(message) || string.IsNullOrWhiteSpace(message)) return;
        nameSetMenu.SetActive(false);
        roomOptionMenu.SetActive(true);
    }
    /// GAME = ROOM
    public void CreateGame()
    {
        if (status.text == "Disconnected") return;
        if (string.IsNullOrEmpty(_roomInputF.text) || string.IsNullOrWhiteSpace(_roomInputF.text))
        {
            status.text = "Write Room Name";
            return;
        }
        if (string.IsNullOrEmpty(_playersInputF.text) || string.IsNullOrWhiteSpace(_playersInputF.text))
        {
            status.text = "Write Max Players";
            return;
        }
        if (byte.Parse(_playersInputF.text) > 4)
        {
            status.text = "Max Players is 4";
            return;
        }
        RoomOptions options = new RoomOptions();
        int maxPJ = byte.Parse(_playersInputF.text) + 1;
        options.MaxPlayers = (byte)maxPJ;
        options.IsOpen = true;
        options.IsVisible = true;
        PhotonNetwork.JoinOrCreateRoom(_roomInputF.text, options, TypedLobby.Default);
        PhotonNetwork.NickName = _nickInputF.text;
        _createRoom_b.interactable = false;
    }

    public void JoinGame()
    {
        if (string.IsNullOrEmpty(_roomJoinInputF.text) || string.IsNullOrWhiteSpace(_roomJoinInputF.text))
        {
            status.text = "Write Room Name";
            return;
        }
        PhotonNetwork.JoinRoom(_roomJoinInputF.text);
        PhotonNetwork.NickName = _nickInputF.text;
        _joinRoom_b.interactable = false;
    }
    public override void OnConnectedToMaster()
    {
        _createRoom_b.interactable = false;
        PhotonNetwork.JoinLobby();
        status.text = "Connecting To Lobby";
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        status.text = "Disconnected";
    }

    public override void OnJoinedLobby()
    {
        _createRoom_b.interactable = true;
        status.text = "Connected To Lobby";
    }
    public override void OnLeftLobby()
    {
        status.text = "Lobby failed";
    }
    public override void OnCreatedRoom()
    {
        status.text = "Created Room";
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        status.text = "Created Room failed";
        _createRoom_b.interactable = false;
    }

    public override void OnJoinedRoom()
    {
        status.text = "Joined Room";
        PhotonNetwork.LoadLevel("GameHybrid");
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        status.text = "Joined Room Random failed";
        _createRoom_b.interactable = false;
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        _joinRoom_b.interactable = true;
        status.text = "ROOM DOES NOT EXIST";
    }
    
}