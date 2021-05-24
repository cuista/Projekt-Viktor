using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactiveBox : MonoBehaviour, ReactiveObject
{
    [SerializeField] public GameObject item;
    [SerializeField] public GameObject explosionEffect;

    public void ReactToHit(){
        StartCoroutine(Open());
    }

    private IEnumerator Open() {
        this.transform.Rotate(-75, 0, 0);
        ExplosionController.MakeItBoom(explosionEffect, transform);

        yield return new WaitForSeconds(0.5f);

        Instantiate(item, transform.position, transform.rotation);
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
