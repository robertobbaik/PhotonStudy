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

    public void OnClick_JoinRoom()
    {
        GameStart(GameMode.Shared, "");
    }

    public async void GameStart(GameMode mode, string roomName)
    {
        networkRunner.ProvideInput = true;

        var startGameArgs = new StartGameArgs()
        {
            GameMode = mode,
            SessionName = roomName,
            Scene = SceneRef.FromIndex(0),
        };

        await networkRunner.StartGame(startGameArgs);

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
