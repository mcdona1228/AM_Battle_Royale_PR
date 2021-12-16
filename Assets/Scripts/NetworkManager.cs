using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public int maxPlayers = 10;
    // instance
    public static NetworkManager instance;
    void Awake()
    {
        if (instance != null && instance != this)
            gameObject.SetActive(false);
        else
        {
            // set the instance
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    void Start()
    {
        // connect to the master server
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("We've connected to the master server!");
        PhotonNetwork.JoinLobby();
    }
    public void CreateRoom(string roomName)
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = (byte)maxPlayers;
        PhotonNetwork.CreateRoom(roomName, options);
        Debug.Log("Room Created");
    }
    // attempts to join a room
    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }
    [PunRPC]
    public void ChangeScene(string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        PhotonNetwork.LoadLevel("Menu");
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        GameManager.instance.alivePlayers--;
        GameUI.instance.UpdatePlayerInfoText();
        if (PhotonNetwork.IsMasterClient)
        {
            GameManager.instance.CheckWinCondition();
        }
    }

}
