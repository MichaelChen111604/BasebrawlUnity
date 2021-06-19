using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{
    public PlayerMovement movement;

    public CharacterController _controller;
    // 2 different camera locations
    public Transform fpCameraLocation, tpCameraLocation;
    public Camera _camera;

    [Header("Health and Damage")]
    public float StartingHealth = 100;
    private float health; // Current health
    [Tooltip("DamageMultipler * Ball velocity = damage taken")]
    public float DamageMultiplier = 0.5f;

    // 1 = swing, 2 = melee
    private int batMode;

    [Header("Bat")]
    public Transform bat;
    [Tooltip("Bat idle position")]
    public Vector3 batIdlePosition;
    [Tooltip("Bat idle rotation quaternion")]
    public Quaternion batIdleQuat;
    [Tooltip("Bat position when blocking")]
    public Vector3 batBlockPosition;
    [Tooltip("Bat rotation quaternion when blocking")]
    public Quaternion batBlockQuat;
    [Tooltip("Box collider used for blocking")]
    public BoxCollider blockCollider;

    // Start is called before the first frame update
    void Start()
    {
        // Hide the mouse cursor
        Cursor.lockState = CursorLockMode.Locked;

        _controller = GetComponent<CharacterController>();

        batMode = 1;
        health = StartingHealth;
        
        batIdlePosition = new Vector3(0.512f, 0.049f, 0.946f);
        batIdleQuat = new Quaternion(-0.7f, 0, 0, 0.709f);
        batBlockQuat = new Quaternion(-0.63f, 0.32f, -0.32f, 0.63f);
        batBlockPosition = new Vector3(-0.89f, 0.266f, 0.947f);
        // Not blocking by default
        ChangeBlocking(false);

    }

    // Update is called once per frame
    void Update()
    {

        // Change bat mode
        if (Input.GetButtonDown("Change bat mode"))
        {
            SetBatMode(batMode == 1 ? 2 : 1);
        }

        // Blocking
        if (Input.GetButtonDown("Block"))
        {
            ChangeBlocking(true);
        }
        else if (Input.GetButtonUp("Block"))
        {
            ChangeBlocking(false);
        }
    }

    // Called to change bat mode between swing and melee - 1 for swing, 2 for melee
    private void SetBatMode(int mode) 
    {
        if (mode == 1)
        {
            batMode = 1;
            _camera.transform.position = fpCameraLocation.position;
        }
        else
        {
            batMode = 2;
            _camera.transform.position = tpCameraLocation.position;
        }
    }

    // Called to start/stop blocking
    private void ChangeBlocking(bool blocking)
    {
        if (blocking)
        {
            bat.localPosition = batBlockPosition;
            bat.localRotation = batBlockQuat;
            blockCollider.isTrigger = false;
        }
        else
        {
            bat.localPosition = batIdlePosition;
            bat.localRotation = batIdleQuat;
            blockCollider.isTrigger = true;
        }
    }

    // Called on collision
    void OnCollisionEnter(Collision collision)
    {
    }

    // Called when hit by ball
    public void TakeDamage(Rigidbody ball)
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
