using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MasterManager : MonoBehaviourPunCallbacks
{
    Dictionary<Player, CharacterModel> _dicChars = new Dictionary<Player, CharacterModel>();
    Dictionary<CharacterModel, Player> dicPJ = new Dictionary<CharacterModel, Player>();
    public static MasterManager _instance;
    [SerializeField] private Transform[] spawns;
    [SerializeField] private Material[] mats;
    private int countPJ;
    private float _timeElapsed=0f;
    public TMP_Text timerText;
    private bool starting;

    public static MasterManager Instance
    {
        get
        {
            return _instance;
        }
    }


    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;

        }
    }
    private void Start()
    {
        starting = false;
    }
    private void Update()
    {
      
        if (PhotonNetwork.PlayerList.Length == 2|| PhotonNetwork.PlayerList.Length == 4)
        {
            
            Timer();

        }
    }
    public void RPCMaster(string name, params object[] p)
    {
        photonView.RPC(name, PhotonNetwork.MasterClient, p);
    }
    [PunRPC]
    public void RequestConnectPlayer(Player client)
    {
        StartCoroutine(InitialTimer(client));
        countPJ = PhotonNetwork.PlayerList.Length;
        GameObject obj = PhotonNetwork.Instantiate("Character0", spawns[countPJ - 1].position, Quaternion.identity);
        obj.transform.Rotate(0, 180, 0);
        var character = obj.GetComponent<CharacterModel>();
        photonView.RPC("SetSkin", RpcTarget.AllBuffered, countPJ-1, character.photonView.ViewID);
        photonView.RPC("OnComponentPlayer", client, character.photonView.ViewID);
        _dicChars[client] = character;
        dicPJ[character] = client;
        character.OnDie += DieHandlerModel;

    }

    [PunRPC]
    public void OnComponentPlayer(int id)
    {
        var pv = PhotonView.Find(id);
        var model = pv.GetComponent<CharacterModel>();
        model.Cam.SetActive(true);      
    }
    [PunRPC]
    public void SetSkin(int countPJ,int id)
    {
        var pv = PhotonView.Find(id);
        var render = pv.GetComponent<CharacterView>();
        render.SetSkin(mats[countPJ]);
    }


    [PunRPC]
    public void RequestMove(Player client, Vector3 dir)
    {

        if (_dicChars.ContainsKey(client))
        {
            var character = _dicChars[client];
            Vector3 x = character.transform.right * dir.x;
            Vector3 z = character.transform.forward * dir.z;
            Vector3 dirFinal = (x + z).normalized;
            character.Move(dirFinal);

            
        }
    }
    [PunRPC]
    public void StartGame(bool value)
    {
        foreach (var item in _dicChars)
        {
            item.Value.Rb.isKinematic = value;
        }

    }
    [PunRPC]
    public void UpdateAnimMove(Player client, float V)
    {
        if (_dicChars.ContainsKey(client) && Time.timeScale!=0f)
        {
            var character = _dicChars[client];
            character.GetComponent<CharacterView>().Anim.SetFloat("RunVertical", V);

        }
    }
    [PunRPC]
    public void UpdateAnimJump(Player client, bool isJumping)
    {
        if (_dicChars.ContainsKey(client) && Time.timeScale != 0f)
        {
            var character = _dicChars[client];
            character.GetComponent<CharacterView>()?.Anim.SetBool("Jumping", isJumping); //TODO JUMP CUANDP CHARACTER.JUMP<<<<

        }
    }
    [PunRPC]
    public void RequestJump(Player client)
    {
        if (_dicChars.ContainsKey(client))
        {
            var character = _dicChars[client];
            character.Jump();

        }
    }

    [PunRPC]
    public void RequestRotateCam(Player client, float mousePos)
    {
        if (_dicChars.ContainsKey(client))
        {
            var character = _dicChars[client];
            var pjCam = character.Cam.GetComponent<CameraScript>();
            pjCam.RotateCamera(mousePos);
        }
    }
    public void DieHandlerModel(CharacterModel character)
    {
        if (dicPJ.ContainsKey(character))
        {
            var client = dicPJ[character];
            _dicChars.Remove(client);
            dicPJ.Remove(character);
            character.photonView.RPC("LoseGame", client);
            if (_dicChars.Count == 1)
            {
                foreach (var item in _dicChars)
                {
                    //Gano
                    item.Value.photonView.RPC("WinGame", item.Key);
                    break;
                }
            }

        }

    }
    [PunRPC]
    public void DisconnectGame(Player client)
    {
        if (_dicChars.ContainsKey(client))
        {
            var character = _dicChars[client];
            character.DisconnectPlayer();
        }
    }

    [PunRPC]
    public void PauseGame(float value)
    {
        Time.timeScale = value;
    }
    public void Timer()
    {
        _timeElapsed += Time.deltaTime;
        var minutes = (int)(_timeElapsed / 60f);
        var seconds = (int)(_timeElapsed - minutes * 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes,seconds);

    }

    [PunRPC]
    public void UpdateUITimer(float current)
    {
        _timeElapsed = current;
    }
    public IEnumerator InitialTimer(Player client)
    {
        while (_timeElapsed != 0)
        {
            yield return new WaitForSecondsRealtime(2f);
            photonView.RPC("UpdateUITimer", client, _timeElapsed); //RPC.Others
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (_dicChars.ContainsKey(otherPlayer))
            {
                var character = _dicChars[otherPlayer];
                _dicChars.Remove(otherPlayer);
                PhotonNetwork.Destroy(character.gameObject);
            }
        }
       
    }

}
