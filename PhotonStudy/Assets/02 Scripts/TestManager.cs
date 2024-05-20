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

    public string nickname;
    private void Start()
    {
        button_TestMessage.onClick.AddListener(() =>
        {
            
            RpcTestMessage(nickname);
        });

        a = 0;
    }

    public override void FixedUpdateNetwork()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            try
            {
                a++;
                RpcTestMessage(a.ToString());
                text_Status.text = "SendComplete";
            }
            catch (Exception e)
            {
                Debug.Log(e);
                text_Status.text = e.ToString();
            }
        }
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
