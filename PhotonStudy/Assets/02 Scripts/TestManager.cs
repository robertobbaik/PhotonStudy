using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;
using System;
using TMPro;

public class TestManager : NetworkBehaviour
{
    public Button button_TestMessage;
    public TextMeshProUGUI text_Status;
    public TextMeshProUGUI text_GetNumber;
    public int a;
    public TMP_InputField inputField_nickname;

    public string nickname;

    private void Awake()
    {

    }
    private void Start()
    {
        button_TestMessage.onClick.AddListener(() =>
        {
            RpcJoinMessage(inputField_nickname.text);
        });
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RpcJoinMessage(string nickname)
    {
        Debug.Log(nickname);

        text_Status.text = $"{nickname} has Joined Game";
    }

    public override void FixedUpdateNetwork()
    {
       
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    private void RpcTestMessage(string message)
    {
        try
        {
            text_Status.text = "SendComplete";
        }
        catch (Exception e)
        {
            Debug.Log(e);
            text_Status.text = e.ToString();
        }

        text_GetNumber.text = message;
        Debug.Log(message);
    }
}
