using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System;
using Fusion.Sockets;

public class PlayerInfo
{
    public PlayerRef playerRef;
    public string nickname;
    public int[] decList;
}
public class FusionConnection : MonoBehaviour, IPlayerJoined, INetworkRunnerCallbacks
{
    public static FusionConnection Instance;
    public bool connectOnAwake = false;
    public NetworkRunner runner;

    public GameObject playerPrefab;

    public Dictionary<int, PlayerInfo> dic_PlayerInfos = new();

    public string playerName;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        if (connectOnAwake == true)
        {

        }
    }

    public async void ConnectToLobby(string playerName)
    {
        if (runner == null)
        {
            runner = gameObject.AddComponent<NetworkRunner>();
        }

        await runner.JoinSessionLobby(SessionLobby.Shared);
    }

    public async void ConnectToRunner(string playerName)
    {
        this.playerName = playerName;

        if (runner == null)
        {
            runner = gameObject.AddComponent<NetworkRunner>();
        }

        var result = await runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            SessionName = "test",
            PlayerCount = 4
        });

        if (result.Ok)
        {
            UIManager.Instance.SetStatusMessage("Connect Success");
        }
        else
        {
            UIManager.Instance.SetStatusMessage("Connect Failed");
        }
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {

    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {

    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {

    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {

    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {

    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {

    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {

    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {

    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {

    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {

    }


    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_SendMessage(int playerId, PlayerInfo playerInfo)
    {
        Debug.Log("RPC Send Message");
        dic_PlayerInfos.Add(playerId, playerInfo);
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("OnPlayerJoined");
        //UIManager.Instance.SetServerMessage(player.PlayerId.ToString());

        if (player == runner.LocalPlayer)
        {
            Debug.Log("This player is me");
            UIManager.Instance.SetServerMessage("This player is me");
        }
        else
        {
            Debug.Log("This player is not me");
            UIManager.Instance.SetServerMessage("This player is not me");
        }

        
        //RPC_SendMessage("hi");
        //RPC_SendMessage(player.PlayerId.ToString());

    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {

    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {

    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {

    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {

    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {

    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        Debug.Log("Session List Updated");
        UIManager.Instance.SetServerMessage("Session List Updated");
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {

    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {

    }

    public void PlayerJoined(PlayerRef player)
    {
        Debug.Log("me");
    }
}
