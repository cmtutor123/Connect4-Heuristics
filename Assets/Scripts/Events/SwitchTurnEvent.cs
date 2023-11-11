using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTurnEvent : MonoBehaviour
{
    public static event Action<bool> OnSwitchTurn;

    public static void SwitchTurn(bool isRedTurn)
    {
        Debug.Log("Switch Turn");
        OnSwitchTurn?.Invoke(isRedTurn);
    }
}
