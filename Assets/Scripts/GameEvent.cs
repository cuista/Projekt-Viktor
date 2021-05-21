﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent : MonoBehaviour
{
    public const string ENEMY_KILLED = "ENEMY_KILLED";
    public const string BOMBS_CAPACITY_CHANGED = "BOMBS_CAPACITY_CHANGED";
    public const string BOMB_PLANTED = "BOMB_PLANTED";
    public const string BOMBS_DETONATED = "BOMBS_DETONATED";
    public const string SPECIALBOMB_CHANGED = "SPECIALBOMB_CHANGED";
    public const string SPEED_CHANGED = "SPEED_CHANGED";

    public static bool isPaused = false;
}
