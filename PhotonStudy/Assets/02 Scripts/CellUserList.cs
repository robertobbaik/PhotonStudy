using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CellUserList : MonoBehaviour
{
    public TextMeshProUGUI text_UserId;
    public void Initialize(string userId)
    {
        text_UserId.text = userId;
    }
}
