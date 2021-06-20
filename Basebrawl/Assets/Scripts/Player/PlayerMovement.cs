using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Tooltip("Player Character Controller script attached to this player")]
    public PlayerCharacterController pcc;

    // Used for calculating velocity
    Vector3 PrevPos, NewPos;
    [System.NonSerialized]
    public Vector3 velocity;

    public float walkSpeed = 6.5f;
    public float sprintSpeed = 9f;

    private void Start()
    {
        PrevPos = NewPos = transform.position;
    }

    private void FixedUpdate()
    {
        // Calculate player velocity
        NewPos = transform.position;
        velocity = (NewPos - PrevPos) / Time.fixedDeltaTime;
        PrevPos = NewPos;

        // WASD walking/sprinting
        float moveX = Input.GetAxis("Horizontal") * Time.deltaTime;
        float moveZ = Input.GetAxis("Vertical") * Time.deltaTime;
        // Walking
        if (!Input.GetButton("Sprint") || !Input.GetButton("VerticalButton"))
            pcc._controller.Move(walkSpeed * (transform.right * moveX + transform.forward * moveZ));
        // Sprinting
        else
            pcc._controller.Move(sprintSpeed * (transform.right * moveX + transform.forward * moveZ));

        gameObject.transform.position = new Vector3(gameObject.transform.position.x, 1.0001f, gameObject.transform.position.z);
    }
}
