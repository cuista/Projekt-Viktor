using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private int _counter;

    [SerializeField] GameObject counterLabel;

    // Start is called before the first frame update
    void Start()
    {
        _counter = 1;
        counterLabel.GetComponent<TMPro.TextMeshPro>().text=_counter.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        counterLabel.transform.rotation = Quaternion.LookRotation( counterLabel.transform.position - Camera.main.transform.position );
    }

    public float GetRadius() {
        return GetComponent<Renderer>().bounds.size.x;
    }

    public void AddBombOver(){
        _counter++;
        counterLabel.GetComponent<TMPro.TextMeshPro>().text=_counter.ToString();
    }

    public int GetCounter(){
        return _counter;
    }
}
