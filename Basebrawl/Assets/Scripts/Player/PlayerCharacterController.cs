using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{
    private CharacterController _controller;
    private GameObject playerBody;
    // 2 different camera locations
    public Transform fpCameraLocation, tpCameraLocation;
    public Camera _camera;

    // Used for calculating velocity
    Vector3 PrevPos, NewPos;
    public Vector3 velocity;

    // 1 = swinging at balls, 2 = melee
    public int SwingMode { get; private set; }

    [Header("Movement")]
    public float mouseXSensitivity = 800f;
    public float walkSpeed = 6.5f;
    public float sprintSpeed = 9f;

    // Start is called before the first frame update
    void Start()
    {
        // Hide the mouse cursor
        Cursor.lockState = CursorLockMode.Locked;

        _controller = GetComponent<CharacterController>();

        SwingMode = 1;

        PrevPos = NewPos = transform.position;

    }

    void FixedUpdate()
    {
        NewPos = transform.position;
        velocity = (NewPos - PrevPos) / Time.fixedDeltaTime;
        PrevPos = NewPos;
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
        
        // Change bat mode
        if (Input.GetButtonDown("Change bat mode"))
        {
            if (SwingMode == 1)
            {
                SwingMode = 2;
                _camera.transform.position = tpCameraLocation.position;
            }
            else if (SwingMode == 2)
            {
                SwingMode = 1;
                _camera.transform.position = fpCameraLocation.position;
            }
        }
    }

    // Called on collision
    void OnCollisionEnter(Collision collision)
    {
    }
}
