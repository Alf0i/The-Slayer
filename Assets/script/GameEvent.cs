using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEvent :MonoBehaviour
{
    public static GameEvent instance;
    private void Awake()
    {
        instance = this;
    }

    public UnityEvent<int> Onshooting;

    public void Shooting(int id)
    {
        Onshooting?.Invoke(id);
    }
}
