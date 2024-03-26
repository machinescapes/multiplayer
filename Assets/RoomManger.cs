using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class RoomManger : MonoBehaviourPunCallbacks
{
    public static RoomManger instance;

    public GameObject player;
    [Space]
    public Transform[] spawnPoints;


    [Space]
    public GameObject roomCam;

    [Space]
    public GameObject nameUI;
    public GameObject connectingUI;








    private string nickname = "unnamed";

    public string roomNameTojoin = "test";


    [HideInInspector]
    public int kills = 0;
    [HideInInspector]
    public int deaths = 0;








    void Awake()
    {
        instance = this;
    }


    public void ChangeNickname(string _name)
    {
        nickname = _name;
    }


    public void JoinRoomButtonPressed()
    {
        Debug.Log("connecting..");

        PhotonNetwork.JoinOrCreateRoom(roomNameTojoin, roomOptions: null, typedLobby: null);


        nameUI.SetActive(false);
        connectingUI.SetActive(true);
    }







    public override void OnJoinedRoom()
    {


        base.OnJoinedRoom();

        Debug.Log(message: "connecd in a room");


        roomCam.SetActive(false);

        spawnPlayer();
    }


    public void spawnPlayer()
    {
        Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];

        GameObject _player = PhotonNetwork.Instantiate(player.name, spawnPoint.position, Quaternion.identity);
        _player.GetComponent<PlayerSetup>().IslocalPlayer();
        _player.GetComponent<Heath>().isLocalPlayer = true;

        _player.GetComponent<PhotonView>().RPC("SetNickname", RpcTarget.AllBuffered, nickname);
        PhotonNetwork.LocalPlayer.NickName = nickname;
    }



    public void SetHashes()
    {
        try
        {
            Hashtable hash = PhotonNetwork.LocalPlayer.CustomProperties;
            hash["kills"] = kills;
            hash["deaths"] = deaths;
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
        catch
        {
            // nothing

        }






    }




}
