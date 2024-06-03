using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Fusion;
using Unity.VisualScripting;

public class UIManager : NetworkBehaviour, IPlayerJoined
{
    public static UIManager Instance;
    public Button button_ConnectLobby;
    public Button button_SendMessage;
    public GameObject playerPrefab;
    public Canvas canvas;

    public TextMeshProUGUI text_Status;
    public TextMeshProUGUI text_ServerMessage;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        

        button_ConnectLobby.onClick.AddListener(() =>
        {
            //FusionConnection.Instance.ConnectToLobby("asd");
            FusionConnection.Instance.ConnectToRunner("asd");
        });

        button_SendMessage.onClick.AddListener(()=>
        {
            FusionConnection.Instance.RPC_SendMessage("Hi");
        });
    }


    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_SendMessage(string message)
    {
        Debug.Log("RPC Send Message");
        text_ServerMessage.text = message;
    }

    public void PlayerJoined(PlayerRef player)
    {
        Debug.Log("Player Joined");
        text_ServerMessage.text = "Player Joined";
    }

    public void SetServerMessage(string message)
    {
        text_ServerMessage.text = message;
    }

    public void SetStatusMessage(string message)
    {
        text_Status.text = message;
    }
}
