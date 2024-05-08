using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;
using Fusion.Sockets;
using System;
using TMPro;

public class PhotonManager : MonoBehaviour, IPlayerJoined, IPlayerLeft
{
    public Button button_JoinRoom;
    public TextMeshProUGUI text_Status;
    public TextMeshProUGUI text_RoomName;
    public GameObject playerPrefab;
    public NetworkRunner networkRunner;

    void Start()
    {
        button_JoinRoom.onClick.AddListener(() =>
        {
            OnClick_JoinRoom();
        });
    }

    void Update()
    {
        
    }

    public void OnClick_JoinRoom()
    {

    }

    public async void GameStart(GameMode mode, string roomName, string sceneName)
    {
        
    }

    public void PlayerJoined(PlayerRef player)
    {
        string message = "Join Player : " + player.PlayerId;
        text_Status.text = message;
    }

    public void PlayerLeft(PlayerRef player)
    {
        throw new NotImplementedException();
    }
}
