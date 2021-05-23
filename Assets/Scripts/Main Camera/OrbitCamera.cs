using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCamera : MonoBehaviour
{

    [SerializeField] private Transform target;

    [SerializeField] private LayerMask camOcclusion;

    public float rotSpeed = 1.5f;
    private float _rotY;

    private Vector3 _offset;

    // Start is called before the first frame update
    void Start()
    {
        _rotY = transform.eulerAngles.y;
        _offset = target.position - transform.position;
    }

    void LateUpdate() {
        if(!GameEvent.isPaused) {
            float horInput = Input.GetAxis("Horizontal");

            // 1)MOUSE-MOVEMENT CAMERA
            /*
            if(horInput != 0)
            {
                _rotY += horInput * rotSpeed;
            } else {
                _rotY += Input.GetAxis("Mouse X") * rotSpeed * 3;
            }
            */

            // 2)MOUSE CAMERA
            /*
            _rotY += Input.GetAxis("Mouse X") * rotSpeed * 3;
            */

            // 3)MOUSE-MOVEMENT CAMERA (MOUSE READ EVEN FOR DIAGONAL MOVEMENT)
            ///*
            _rotY += horInput * rotSpeed;
            _rotY += Input.GetAxis("Mouse X") * rotSpeed * 3;
            //*/

            Quaternion rotation = Quaternion.Euler(0,_rotY,0);
            transform.position = target.position - (rotation*_offset);

            
            //FIXME (BURSTDRIVE PROBLEM) OR DELETEME
            RaycastHit hitPoint = new RaycastHit();
            if(Physics.Linecast(target.position, transform.position, out hitPoint, camOcclusion)){
                Vector3 camPosition = new Vector3(hitPoint.point.x + hitPoint.normal.x * 0.3f, transform.position.y, hitPoint.point.z + hitPoint.normal.z * 0.3f);
                transform.position=Vector3.Lerp(transform.position,camPosition,10f);
            }
            

            transform.LookAt(target);
        }
    }
}
