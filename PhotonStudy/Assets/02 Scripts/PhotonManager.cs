using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Photon.Realtime;
using UnityEngine.UI;
using Fusion.Sockets;
using System;
using TMPro;
using System.Threading.Tasks;
using Unity.VisualScripting;

public class PhotonManager : MonoBehaviour, INetworkRunnerCallbacks
{
    public Button button_JoinRoom;
    public Button button_SendMessage;
    public TextMeshProUGUI text_Status;
    public TextMeshProUGUI text_RoomName;
    public GameObject playerPrefab;
    public NetworkRunner networkRunner;
    public delegate void OnJoinCallBack(string nickname, PlayerRef playerRef);
    public static OnJoinCallBack onJoinCallBack;

    public Dictionary<string, string> dic_UserList = new();
    public string nickname;

    public List<string> nicknameList = new();

    void Start()
    {
        nickname = "123";

        button_JoinRoom.onClick.AddListener(async () =>
        {
            Task task = JoinLobby(networkRunner);
            await task;
        });

        button_SendMessage.onClick.AddListener(async () =>
        {
            //OnClick_SendMessage();
            Task task = StartSharedRoom(networkRunner);
            await task;
        });
    }

    public void Test2()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            RpcJoinMessage(networkRunner, "efg");
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RpcJoinMessage()
    {
        text_Status.text = "Message from Ipanema";
    }

    public void OnClick_JoinRoom()
    {
        Debug.Log("OnClick");
        text_RoomName.text = "On Click";
        //GameStart(GameMode.Shared, "");
    }

    public void TestSessionProperty()
    {

    }

    public async Task JoinLobby(NetworkRunner runner)
    {
        var result = await runner.JoinSessionLobby(SessionLobby.Shared);

        if (result.Ok)
        {
            text_Status.text = "Success Join Lobby";
            Debug.Log("OK");
        }
        else
        {
            text_Status.text = "Failed Join Lobby";
            Debug.Log("Not Ok");
        }
    }

    public void GetUserId()
    {
        foreach (var player in networkRunner.ActivePlayers)
        {
            string userId = networkRunner.GetPlayerUserId(player);
            Debug.Log($"PlayerId : {userId}");
        }

        Debug.Log(networkRunner.GetPlayerUserId());

        text_Status.text = networkRunner.GetPlayerUserId();
    }

    public async Task StartSharedRoom(NetworkRunner runner)
    {
        var customProps = new Dictionary<string, SessionProperty>
        {
            ["maxLevel"] = 2,
            ["minLevel"] = 1
        };

        var result = await runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            CustomLobbyName = "MyCustomLobby",
            SessionProperties = customProps
        });

        if (result.Ok)
        {
            // all good
        }
        else
        {
            Debug.LogError($"Failed to Start: {result.ShutdownReason}");
        }
    }
    // public async void GameStart(GameMode mode, string roomName)
    // {
    //     networkRunner.ProvideInput = true;

    //     var customProps = new Dictionary<string, SessionProperty>
    //     {
    //         ["maxLevel"] = 2,
    //         ["minLevel"] = 1
    //     };


    //     text_RoomName.text = roomName;

    //     Debug.Log(networkRunner.IsConnectedToServer);
    // }

    [Rpc]
    public static void RpcJoinMessage(NetworkRunner networkRunner, string nickname)
    {
        Debug.Log(nickname);
        Debug.Log(networkRunner.GetPlayerUserId());

        
    }

    // [Rpc]
    // public static void RPC_TestMessage(NetworkRunner runner, string message)
    // {
    //     Debug.Log(nickname);
    //     Debug.Log(networkRunner.GetPlayerUserId());

    //     text_Status.text = $"{nickname} has Join Game";
    // }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RpcTestMessage(string message)
    {
        text_Status.text = message;
        Debug.Log(message);
    }

    public void PlayerLeft(PlayerRef player)
    {
        Debug.LogWarning("I NetworkRunner Callback : PlayerLeft");
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        Debug.LogWarning("I NetworkRunner Callback : Object Exit");
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        Debug.LogWarning("I NetworkRunner Callback : Object Enter");
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.LogWarning("I NetworkRunner Callback : Join " + player.PlayerId);

        if (runner.UserId == networkRunner.GetPlayerUserId())
        {
            text_Status.text = "It's me";
        }
        else
        {
            text_Status.text = "Another Player";
        }
    }

    public void Test1()
    {
        Debug.Log("Test 1");
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.LogWarning("I NetworkRunner Callback : Left " + player.PlayerId);
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {

    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {

    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {

    }

    public void OnConnectedToServer(NetworkRunner runner)
    {

    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {

    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {

    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {

    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {

    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        for (int i = 0; i < sessionList.Count; i++)
        {
            Debug.Log(sessionList[i].Properties["minLevel"]);
        }
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {

    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {

    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {

    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {

    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {

    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {

    }
}
