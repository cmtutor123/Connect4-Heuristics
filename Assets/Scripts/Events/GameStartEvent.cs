using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartEvent : MonoBehaviour
{
    public static event Action<bool> OnGameStart;

    public static void GameStart(bool isRedTurn)
    {
        Debug.Log("Game Start".Color("blue"));
        OnGameStart?.Invoke(isRedTurn);
    }
}
