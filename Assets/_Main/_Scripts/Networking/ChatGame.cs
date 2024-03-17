using TMPro;
using Photon.Pun;
using Photon.Voice.Unity;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChatGame : MonoBehaviourPun
{
    public TextMeshProUGUI content;
    public TMP_InputField _inputF;
    private string _commandDm = "w/";
    private string _commandStart = "/start";
    private string _commandDead = "/exit";
    private string _commandPause = "/pause";
    private string _commandMute = "/mute";
    private string _commandAllMuted= "/mutedAll";
    private float valueTimeScale;
    private Recorder pVoice;

    private void Start()
    {
        _inputF.enabled = false;
        valueTimeScale = 1f;
        pVoice= FindObjectOfType<Recorder>();
        //nickNameUI.text = PhotonNetwork.LocalPlayer.NickName;
        //roomNameUI.text = PhotonNetwork.CurrentRoom.Name;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            ChatModeOn();
        }
     
    }
    public void ChatModeOn()
    {
        var eventSystem = EventSystem.current;
        if (!eventSystem.alreadySelecting && !_inputF.enabled)
        {
            eventSystem.SetSelectedGameObject(null);
            _inputF.enabled = true;
            _inputF.ActivateInputField();
        }



    }
    public void SendMessageGame()
    {
        var message = _inputF.text;
        if (string.IsNullOrEmpty(message) || string.IsNullOrWhiteSpace(message)) return;
        string[] words = message.Split(' ');
        if (message != _commandDead && message != _commandPause && message != _commandStart && message != _commandMute && message != _commandAllMuted)
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
                        ChatModeOff();
                        return;
                    }

                }
                content.text += "<color=black>" + "NO EXISTE ESTE USUARIO" + "</color>" + "\n";

            }
            else
            {
                photonView.RPC("GetChatMessage", RpcTarget.All, PhotonNetwork.NickName, message, false);

            }
        }
        if (message == _commandDead)
        {
            print("dead player");
            MasterManager.Instance.RPCMaster("DisconnectGame", PhotonNetwork.LocalPlayer);

            //TODO
        }
        else if (message == _commandStart)
        {
            print("EMPIEZA EL JUEGO ");
            MasterManager.Instance.RPCMaster("StartGame");

            //TODO
        }
        else if (message == _commandPause)
        {
            if (valueTimeScale == 1)
            {
                valueTimeScale = 0f;
                MasterManager.Instance.RPCMaster("PauseGame", valueTimeScale);

            }
            else if (valueTimeScale == 0) 
            {
                valueTimeScale = 1f;
                MasterManager.Instance.RPCMaster("PauseGame", valueTimeScale);

            }

            //TODO
        }
        /////AUTORIDAD LOCAL
        else if (message == _commandMute)
        {
            print("te has mutiado");
            photonView.RPC("MeMuted", RpcTarget.All);

        }
        else if (message == _commandAllMuted)
        {
            photonView.RPC("MutedAll", RpcTarget.All, false);

        }
        ChatModeOff();
    }
    public void ChatModeOff()
    {
        _inputF.text = "";
        _inputF.DeactivateInputField();
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
        pVoice.RecordingEnabled = !pVoice.RecordingEnabled;
    }

    [PunRPC]
    public void MutedAll(bool value)
    {
        pVoice.enabled = value;
    }
}
