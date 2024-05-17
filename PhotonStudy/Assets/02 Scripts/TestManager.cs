using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;

public class TestManager : NetworkBehaviour
{
    public Button button_TestMessage;

    private void Start()
    {
        button_TestMessage.onClick.AddListener(() =>
        {
            RpcTestMessage();
        });
    }

    public override void FixedUpdateNetwork()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Input 0");
            RpcTestMessage();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            RpcTestMessage();
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RpcTestMessage()
    {
        Debug.Log("asd");
    }
}
