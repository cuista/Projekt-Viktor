using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : MonoBehaviour, IEnemy
{
    [SerializeField] private int lives;

    private bool _isMoving;
    private bool _isShieldActive = false;

    private Vector3 _graviton;

    public void RemoveLives(int livesToRemove){
        if(!_isShieldActive)
            lives -= livesToRemove;
    }

    public int GetLives(){
        return lives;
    }

    public bool IsMoving(){
        return _isMoving;
    }

    public void SetMoving(bool moving){
        _isMoving=moving;
    }

    public void SetShieldActive(bool isShieldActive)
    {
        _isShieldActive=isShieldActive;
    }

    public Vector3 GetGraviton(){
        return _graviton;
    }

    public void AddGravitonAddiction(Vector3 graviton)
    {
        _graviton = graviton;
    }

    public void RemoveGravitonAddiction()
    {
        _graviton = Vector3.zero;
    }

    public void OnSpeedChanged(float value){
    }
    
}
