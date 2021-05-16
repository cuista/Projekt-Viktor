using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed=10.0f;
    public int damage=1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0,0,speed*Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Sight" || other.gameObject.tag == "Bullet") {
            Physics.IgnoreCollision(GetComponent<Collider>(),other);
        } else {
            PlayerCharacter player=other.GetComponent<PlayerCharacter>();
            if(player!=null){
                //Debug.Log("player hit");
                player.Hurt(damage);
            }
            Destroy(this.gameObject);
        }
    }
}
