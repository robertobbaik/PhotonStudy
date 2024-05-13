using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;

public class TestManager : NetworkBehaviour
{
    public override void FixedUpdateNetwork()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RpcTestMessage();
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RpcInitialSpaceshipSpawn11()
    {
        Debug.Log("asd");
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RpcTestMessage()
    {
        Debug.Log("asd");
    }

}
