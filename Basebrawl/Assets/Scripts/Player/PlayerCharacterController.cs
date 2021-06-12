using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{
    private CharacterController _controller;
    public Transform playerBody;

    [Header("Movement")]
    public float mouseXSensitivity = 800f;
    public float walkSpeed = 9.5f;
    public float sprintSpeed = 13f;

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

        // WASD walking/sprinting
        float moveX = Input.GetAxis("Horizontal") * Time.deltaTime;
        float moveZ = Input.GetAxis("Vertical") * Time.deltaTime;
        // Walking
        if (!Input.GetButton("Sprint") || !Input.GetButton("VerticalButton"))
            _controller.Move(walkSpeed * (transform.right * moveX + transform.forward * moveZ));
        // Sprinting
        else
            _controller.Move(sprintSpeed * (transform.right * moveX + transform.forward * moveZ));
    }

}
