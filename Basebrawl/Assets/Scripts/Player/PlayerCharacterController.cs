using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{
    private CharacterController _controller;
    public Transform playerBody;

    public float mouseXSensitivity = 800f;
    public float walkSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        // Hide the mouse cursor
        Cursor.lockState = CursorLockMode.Locked;

        _controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Horizontal mouse look
        float mouseX = Input.GetAxis("Mouse X") * mouseXSensitivity * Time.deltaTime;
        // Rotate the entire player around the y-axis
        transform.Rotate(Vector3.up * mouseX);

        // WASD walking
        float walkX = Input.GetAxis("Horizontal") * walkSpeed * Time.deltaTime;
        float walkZ = Input.GetAxis("Vertical") * walkSpeed * Time.deltaTime;
        _controller.Move(transform.right * walkX + transform.forward * walkZ);
    }


}
