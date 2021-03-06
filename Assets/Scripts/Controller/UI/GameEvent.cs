using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent : MonoBehaviour
{
    public const string ENEMY_KILLED = "ENEMY_KILLED";
    public const string BOMBS_CAPACITY_CHANGED = "BOMBS_CAPACITY_CHANGED";
    public const string BOMB_PLANTED = "BOMB_PLANTED";
    public const string BOMBS_DETONATED = "BOMBS_DETONATED";
    public const string BOMBS_DETONATED_N = "BOMBS_DETONATED_N";
    public const string BOMBS_DETONATED_BECAUSE_ENEMY_DEATH = "BOMBS_DETONATED_BECAUSE_ENEMY_DEATH";
    public const string SPECIALBOMB_CHANGED = "SPECIALBOMB_CHANGED";
    public const string LIQUID_COLLECTED = "LIQUID_COLLECTED";
    public const string LIQUID_CONSUMED = "LIQUID_CONSUMED";
    public const string GAMEOVER = "GAMEOVER";
    public const string CUTSCENE_STARTED = "CUTSCENE_STARTED";
    public const string CUTSCENE_ENDED = "CUTSCENE_ENDED";
    public const string CUTSCENE_STOPPED = "CUTSCENE_STOPPED";
    public const string CREDITS_STARTED = "CREDITS_STARTED";
    public const string CREDITS_ENDED = "CREDITS_ENDED";
    public const string CREDITS_STOPPED = "CREDITS_STOPPED";
    public const string TARGET_TOTAL = "TARGET_TOTAL";
    public const string TARGET_ELIMINATED = "TARGET_ELIMINATED";
    public const string SPEED_CHANGED = "SPEED_CHANGED";

    public static bool isPaused = false;
}
