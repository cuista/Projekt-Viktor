using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : MonoBehaviour
{
    [SerializeField] private int lives;

    private bool _alive;

    public void RemoveLives(int livesToRemove){
        lives -= livesToRemove;
    }

    public int GetLives(){
        return lives;
    }

    public bool IsAlive(){
        return _alive;
    }

    public void SetAlive(bool alive){
        _alive=alive;
    }
    
}
