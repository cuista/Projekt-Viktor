using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    void RemoveLives(int livesToRemove);

    int GetLives();

    bool IsAlive();

    void SetAlive(bool alive);

    void OnSpeedChanged(float value);
}
