using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    [SerializeField] public GameObject flamesEffect;

    private static ExplosionController _explosionController;

    void Awake()
    {
        _explosionController = this;
    }

    public static void MakeItBoom(GameObject explosionEffect, Transform bombTransform){
        GameObject explosion = Instantiate(explosionEffect, bombTransform.position, bombTransform.rotation);
        Destroy(explosion,explosion.GetComponent<ParticleSystem>().main.duration);
    }

    public static void MakeFloorOnFire(Transform objTransform){
        RaycastHit raycastHit;
        if(Physics.Raycast(objTransform.position, -objTransform.up, out raycastHit))
        {
            GameObject explosion = Instantiate(_explosionController.flamesEffect, raycastHit.point, new Quaternion());
            Destroy(explosion,explosion.GetComponent<ParticleSystem>().main.duration);
        }
    }
}
