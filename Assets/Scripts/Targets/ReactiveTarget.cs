using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactiveTarget : MonoBehaviour, ReactiveObject
{
    [SerializeField] public GameObject explosionEffect;

    public int health;

    public void ReactToHits(int numHits){
        health-=numHits;
        if(health<=0)
            StartCoroutine(Open());
    }

    private IEnumerator Open() {
        ExplosionController.MakeItBoom(explosionEffect, transform);

        yield return new WaitForSeconds(0.5f);

        Destroy(this.gameObject);
    }

    public void AddExplosionForce(float explosionForce, Vector3 explosionPosition, float explosionRadius) {
    }
}
