using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;

public class TestManager : NetworkBehaviour
{
    [Rpc(sources:RpcSources.InputAuthority, targets:RpcTargets.StateAuthority)]
    public void TestMessage(string nickName)
    {
        Debug.Log("nickname : " + nickName);
    }

    void Update()
    {
        
    }
}
