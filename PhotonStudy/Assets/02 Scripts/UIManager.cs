using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public Button button_ConnectLobby;
    public Button button_SendMessage;
    public GameObject playerPrefab;
    public Canvas canvas;

    public TextMeshProUGUI text_Status;
    public TextMeshProUGUI text_ServerMessage;
    public TMP_InputField inputField_Nickname;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        button_ConnectLobby.onClick.AddListener(() =>
        {
            FusionConnection.Instance.ConnectToRunner(inputField_Nickname.text);
        });

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
