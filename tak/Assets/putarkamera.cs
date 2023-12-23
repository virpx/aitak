using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class putarkamera : MonoBehaviour
{
    private float rotationSpeed = 500.0f;
    [SerializeField] Camera cam;
    float zoom = 10f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (cam.orthographic)
        {
           cam.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * zoom;
        }
        else
        {
           cam.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * zoom;
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            CamOrbit();
        }
    }

    private void CamOrbit()
    {
        if (Input.GetAxis("Mouse Y") != 0 || Input.GetAxis("Mouse X") != 0)
        {
            float verticalInput = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
            float horizontalInput = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            transform.Rotate(Vector3.right, -verticalInput);
            transform.Rotate(Vector3.up, horizontalInput, Space.World);
        }
    }
}
