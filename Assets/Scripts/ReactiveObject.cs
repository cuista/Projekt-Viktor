using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ReactiveObject
{
    void ReactToHits(int numHits);

    void AddExplosionForce(float explosionForce, Vector3 explosionPosition, float explosionRadius);
}
