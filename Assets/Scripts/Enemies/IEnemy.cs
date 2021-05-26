using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    void RemoveLives(int livesToRemove);

    int GetLives();

    bool IsMoving();

    void SetMoving(bool moving);

    Vector3 GetGraviton();

    void AddGravitonAddiction(Vector3 graviton);

    void RemoveGravitonAddiction();

    void OnSpeedChanged(float value);
}
