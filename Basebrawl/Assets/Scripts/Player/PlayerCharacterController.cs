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

    [Header("Health and Damage")]
    public float StartingHealth = 100;
    private float health; // Current health
    [Tooltip("DamageMultipler * Ball velocity = damage taken")]
    public float DamageMultiplier = 0.5f;

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
        health = StartingHealth;

        PrevPos = NewPos = transform.position;

    }

    void FixedUpdate()
    {
        NewPos = transform.position;
        velocity = (NewPos - PrevPos) / Time.fixedDeltaTime;
        PrevPos = NewPos;

        gameObject.transform.position = new Vector3(gameObject.transform.position.x, 1.0001f, gameObject.transform.position.z);
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
        Debug.Log("Had a collision");
        // Called when the ball hits the ground
        if (collision.gameObject.tag.Equals("Ball"))
        {
            // Take damage
            TakeDamage(collision.gameObject.GetComponent<Rigidbody>());
        }
    }

    // Called when hit by ball
    void TakeDamage(Rigidbody ball)
    {
        float damage = DamageMultiplier * ball.GetComponent<StandardBall>().thrownSpeed;
        health -= damage;
        Debug.Log("Oof! Health = " + health);
        if (health <= 0) // Player has died
        {
            Application.Quit();
        }
    }
}
