using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 center = Vector3.zero;
    public float speed = 5.0f;
    public float sensitivity = 5.0f;
    public float scrollSens = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Move the camera forward, backward, left, and right
        transform.position += transform.forward * (Input.GetAxis("Vertical") + Input.mouseScrollDelta.y * scrollSens) * speed * Time.deltaTime;
        transform.position += transform.right * Input.GetAxis("Horizontal") * speed * Time.deltaTime;

        if (Input.GetMouseButton(1))
        {
            // Rotate the camera based on the mouse movement
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            //transform.eulerAngles += new Vector3(-mouseY * sensitivity, mouseX * sensitivity, 0);
            //transform.Rotate(-mouseX, -mouseY, 0);
            //transform.Rotate(-mouseX, -mouseY, 0);
            transform.RotateAround(center, Vector3.up, mouseX * speed);
            transform.RotateAround(center, transform.right, -mouseY * speed);
        }

        if (Input.GetMouseButton(2))
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            transform.Translate(-mouseX, -mouseY, 0);
        }
        
    }


}
