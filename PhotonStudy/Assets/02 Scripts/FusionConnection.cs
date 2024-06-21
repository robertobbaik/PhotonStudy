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
public class FusionConnection : MonoBehaviour, INetworkRunnerCallbacks
{
    public static FusionConnection Instance;
    public bool connectOnAwake = false;
    public NetworkRunner runner;
    public TestNetwork testNetwork;
    public GameObject playerPrefab;

    public int myScore;

    public Dictionary<int, PlayerInfo> dic_PlayerInfos = new();
    public List<CellUserList> userList = new();

    public bool isLastPlayer;
    public int networkInitPlayerCount;
    public string playerName;
    public int playerCount;
    public bool isHost;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        myScore = 1000;

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

        this.playerName = playerName;

        await runner.JoinSessionLobby(SessionLobby.Shared);

    }

    public void SetRoomProperty()
    {
       Debug.Log(runner.SessionInfo.IsOpen);
       runner.SessionInfo.IsOpen = false;
       Debug.Log(runner.SessionInfo.IsOpen);
    }

    public async void JoinSession(string sessionName)
    {
        if (runner == null)
        {
            runner = gameObject.AddComponent<NetworkRunner>();
        }

        runner.ProvideInput = true;
        runner.AddCallbacks(this);

        var result = await runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            SessionName = sessionName,
            PlayerCount = 2,
        });

        if (result.Ok)
        {
            Debug.Log("result.ok");
            UIManager.Instance.SetStatusMessage("Connect Success");
        }
        else
        {
            UIManager.Instance.SetStatusMessage("Connect Failed");
        }
    }

    public float averageRate;

    public async void CreateSession()
    {
        isHost = true;
        var customProps = new Dictionary<string, SessionProperty>
        {
            ["averageScore"] = myScore,
            ["averageRate"] = 40,
        };

        if (runner == null)
        {
            runner = gameObject.AddComponent<NetworkRunner>();
        }

        runner.ProvideInput = true;
        runner.AddCallbacks(this);

        var result = await runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            PlayerCount = 2,
            
            SessionProperties = customProps
        });

        if (result.Ok)
        {
            Debug.Log("result.ok");
            UIManager.Instance.SetStatusMessage("Connect Success");
        }
        else
        {
            UIManager.Instance.SetStatusMessage("Connect Failed");
        }
    }

    public void Disconnect()
    {
        runner.Disconnect(runner.LocalPlayer);
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log("OnConnectToServer");
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

    public void SetPlayerList(int playerId, PlayerInfo playerInfo)
    {
        dic_PlayerInfos.Add(playerId, playerInfo);

        foreach (var item in dic_PlayerInfos)
        {
            userList[item.Key].gameObject.SetActive(true);
            userList[item.Key].Initialize(dic_PlayerInfos[item.Key].nickname);
        }

        if(testNetwork.IsHost && dic_PlayerInfos.Count == runner.SessionInfo.MaxPlayers)
        {
            UIManager.Instance.SetStatusMessage("Game Start at Host");
            testNetwork.RpcStartGame();
        }
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.LogWarning("OnPlayerJoined");
        playerCount++;
        if (player == runner.LocalPlayer)
        {
            NetworkObject networkObject = runner.Spawn(playerPrefab, Vector3.zero, Quaternion.identity, player);
            StartCoroutine(Initialize(networkObject, player.PlayerId, playerName, player));
            UIManager.Instance.SetServerMessage("This player is me");
        }
        else
        {
            UIManager.Instance.SetServerMessage("Another Player Join");
        }

        if (runner.SessionInfo.PlayerCount == runner.SessionInfo.MaxPlayers)
        {
            Debug.LogWarning("Room is Full");
            isLastPlayer = true;
        }
    }

    public IEnumerator Initialize(NetworkObject networkObject, int playerId, string playerName, PlayerRef playerRef)
    {
        while (!networkObject.IsValid)
        {
            yield return null;
        }

        TestNetwork network = networkObject.GetComponent<TestNetwork>();
        this.testNetwork = network;
        testNetwork.Init(playerId, playerName, playerRef);
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        //runner.Despawn();
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

    public bool CompareScore(int targetScore, int myScore, int compareRate)
    {
        float rate = compareRate * 0.001f;
        float targetScoreRate = targetScore * rate;
        int targetRate = Mathf.RoundToInt(targetScoreRate);

        bool isJoin = Mathf.Abs(targetScore - myScore) <= targetRate;
        return isJoin;
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        if (sessionList.Count == 0)
        {
            CreateSession();
            return;
        }

        bool isCreate = false;

        foreach (var session in sessionList)
        {
            if(!session.IsOpen) continue;
            Debug.Log(session.Name);
            Debug.Log(session.Properties["averageScore"]);

            if (CompareScore(session.Properties["averageScore"], myScore, session.Properties["averageRate"]))
            {
                JoinSession(session.Name);
                isCreate = true;
                break;
            }
        }

        if (!isCreate)
        {
            CreateSession();
        }
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {

    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {

    }
}
