using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

// public struct CustomData : INetworkStruct
// {
//     public NetworkArray<int> decList;
// }
public class TestNetwork : NetworkBehaviour
{
    [Networked]
    public int PlayerId { get; private set; }
    [Networked]
    public string Nickname { get; private set; }

    [Networked]
    [Capacity(6)]
    public NetworkArray<int> DecList {get;}
    public int[] dec = new int[]{1,2,3,4,5,101};
    [Networked]
    public int Score {get; private set;}

    [Networked]
    public bool IsHost{get; private set;}
    public PlayerRef playerRef;

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RpcSendMessage(int id)
    {
        Debug.Log(id);
    }

    public void Init(int playerId, string playerName, PlayerRef playerRef)
    {
        Debug.Log("Player Count in Init : " + FusionConnection.Instance.playerCount );
        PlayerId = playerId;
        Nickname = playerName;
        this.playerRef = playerRef;
        for (int i = 0; i < dec.Length; i++)
        {
            DecList.Set(i, dec[i]);
        }

        if (Object.HasInputAuthority)
        {
            UIManager.Instance.button_SendMessage.onClick.AddListener(() =>
            {
                RpcStartGame();
            });
            SetPlayerInfo(false);
        }
        else
        {
            SetPlayerInfo(true);
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RpcSendMessageTest(int playerId)
    {
        Debug.Log(Object.HasStateAuthority);
        UIManager.Instance.SetStatusMessage($"hi from {playerId}");
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RpcStartGame()
    {
        UIManager.Instance.SetServerMessage("Game Start");
    }

    public override void Spawned()
    {
        base.Spawned();
        Debug.LogWarning("Spawned");

        if (!Object.HasInputAuthority)
        {
            Debug.Log("Nickname : " + Nickname);
            Debug.LogWarning("another Player");
            SetPlayerInfo(true);
        }
        else
        {
            
        }
    }

    public int playerCount;

    public void SetPlayerInfo(bool isOtherPlayer)
    {
        if (isOtherPlayer)
        {
            Debug.Log("other Player id : " + PlayerId);
            Debug.Log("other Player Name : " + Nickname);
            Debug.Log("Player Count in Set PlayerInfo: " + FusionConnection.Instance.playerCount);
        }
        else
        {
            IsHost = FusionConnection.Instance.isHost;
            SessionInfo sessionInfo = FusionConnection.Instance.runner.SessionInfo;
            if (sessionInfo.MaxPlayers == sessionInfo.PlayerCount)
            {
                Debug.Log("Last Player");
                UIManager.Instance.SetServerMessage("Last Player");
            }
        }

        Debug.Log($"{Object.HasStateAuthority} in line number : 122");

        PlayerInfo playerInfo = new()
        {
            playerRef = this.playerRef,
            nickname = this.Nickname,
            decList = new int[] { 1, 2, 3, 4, 5 }
        };

        FusionConnection.Instance.SetPlayerList(PlayerId, playerInfo);
    }
}
