using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCamera : MonoBehaviour
{

    [SerializeField] private Transform target;

    public float rotSpeed = 1.5f;
    private float _rotY;

    private Vector3 _offset;

    // Start is called before the first frame update
    void Start()
    {
        _rotY = transform.eulerAngles.y;
        _offset = target.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate() {
        if(!GameEvent.isPaused) {
            float horInput = Input.GetAxis("Horizontal");

            // 1)MOUSE-MOVEMENT CAMERA
            ///*
            if(horInput != 0)
            {
                _rotY += horInput * rotSpeed;
            } else {
                _rotY += Input.GetAxis("Mouse X") * rotSpeed * 3;
            }
            //*/

            // 2)MOUSE CAMERA
            //_rotY += Input.GetAxis("Mouse X") * rotSpeed * 3;

            Quaternion rotation = Quaternion.Euler(0,_rotY,0);
            transform.position = target.position - (rotation*_offset);

            transform.LookAt(target);
        }
    }
}
