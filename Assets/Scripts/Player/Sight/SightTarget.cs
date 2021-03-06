using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightTarget : MonoBehaviour
{

    private GameObject _targetEnemy;

    public Material _material;
    
    // It must be longer than gameObject used for collision on the Sight
    private float maxEngageDistance = 8.5f;

    // Start is called before the first frame update
    void Start()
    {
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = _material;
        lineRenderer.widthMultiplier = 0.2f; // thickness

        /* OLD LINE */
        /*
        lineRenderer.material = new Material(Shader.Find("SuperSystems/Wireframe-Transparent-Culled"));
        // A simple 2 color gradient with a fixed alpha of 1.0f.
        float alpha = 0.3f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.yellow, 0.0f), new GradientColorKey(Color.red, 1.0f) },
            //new GradientColorKey[] { new GradientColorKey(new Color(0,219,255), 0.0f), new GradientColorKey(Color.blue, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );
        lineRenderer.colorGradient = gradient;
        */

        lineRenderer.enabled=false;
    }

    // Update is called once per frame
    void Update()
    {
        // Engaging enemy
        LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
        if(_targetEnemy!=null)
        {
            GameObject player=transform.parent.gameObject;
            Vector3 playerPosition=new Vector3(player.transform.position.x,player.transform.position.y,player.transform.position.z);
            Vector3 enemyPosition=new Vector3(_targetEnemy.transform.position.x,_targetEnemy.transform.position.y,_targetEnemy.transform.position.z);

            RaycastHit hitLinecast;
            // if there are NO obstacles between turret and player && distance is less than maxEngageDistance
            if(Physics.Linecast(enemyPosition, playerPosition, out hitLinecast) && hitLinecast.transform.gameObject.GetComponent<PlayerCharacter>() != null && hitLinecast.distance <= maxEngageDistance)
            {
                lineRenderer.enabled=true;
                lineRenderer.SetPosition(0, playerPosition);
                lineRenderer.SetPosition(1, enemyPosition);
            }
            else 
            {
            lineRenderer.enabled=false;
            _targetEnemy=null;
            }
        }
        else 
        {
            lineRenderer.enabled=false;
        }

    }

    private void OnTriggerEnter(Collider collider) {
        EnemyCharacter enemy=collider.GetComponent<EnemyCharacter>();
        if(enemy!=null){
            _targetEnemy=enemy.gameObject;
        }
    }

    public GameObject GetTargetEnemy() {
        return _targetEnemy;
    }
}
