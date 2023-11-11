using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinEvent : MonoBehaviour
{
    public static event Action<bool> OnWin;

    public static void Win(bool didRedWin)
    {
        Debug.Log("Switch Turn");
        OnWin?.Invoke(didRedWin);
    }
}
