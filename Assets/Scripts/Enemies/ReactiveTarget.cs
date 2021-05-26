using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactiveTarget : MonoBehaviour, ReactiveObject
{

    [SerializeField] public GameObject explosionEffect;

    public void ReactToHits(int numHits){
        IEnemy enemy=GetComponent<IEnemy>();
        if(enemy!=null){
            enemy.RemoveLives(numHits);
            ExplosionController.MakeItBoom(explosionEffect, transform);
            if(enemy.GetLives()<1){
                enemy.SetMoving(false);
                StartCoroutine(Die());
            }
        }
    }

    private IEnumerator Die() {
        this.transform.Rotate(-75, 0, 0);
        ExplosionController.MakeItBoom(explosionEffect, transform);

        yield return new WaitForSeconds(0.5f);

        Destroy(this.gameObject);
    }
    public void AddExplosionForce(float explosionForce, Vector3 explosionPosition, float explosionRadius) {
        Rigidbody rigidBody=GetComponent<Rigidbody>();
        if(rigidBody != null)
        {
            rigidBody.AddExplosionForce(1000f, explosionPosition, explosionRadius);
        }
    }
}
