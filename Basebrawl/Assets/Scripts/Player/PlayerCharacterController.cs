using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{
    public PlayerMovement movement;
    public PlayerLook look;

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
    private bool blocking;

    [Header("Batting")]
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
    [Tooltip("Max distance at which balls can be hit")]
    public float swingRange = 6 ;
    // Balls currently in recticle
    [System.NonSerialized]
    public ArrayList targetedBalls;

    // Start is called before the first frame update
    void Start()
    {
        // Hide the mouse cursor
        Cursor.lockState = CursorLockMode.Locked;

        _controller = GetComponent<CharacterController>();

        SetBatMode(1);
        health = StartingHealth;
        
        batIdlePosition = new Vector3(0.512f, 0.049f, 0.946f);
        batIdleQuat = new Quaternion(-0.7f, 0, 0, 0.709f);
        batBlockQuat = new Quaternion(-0.63f, 0.32f, -0.32f, 0.63f);
        batBlockPosition = new Vector3(-0.89f, 0.266f, 0.947f);
        // Not blocking by default
        ChangeBlocking(false);

        targetedBalls = new ArrayList();

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
        // Swinging
        if (Input.GetButtonDown("LeftClick") && batMode == 1)
        {
            SwingAtBall();
            Debug.Log("Swung");
        }
    }

    // Called to change bat mode between swing and melee - 1 for swing, 2 for melee
    private void SetBatMode(int mode) 
    {
        if (mode == 1)
        {
            batMode = 1;
            _camera.transform.position = fpCameraLocation.position;
            if (!blocking)
            {
                look.swingBallReticle.enabled = true;
                look.swingTargetReticle.enabled = true;
            }
        }
        else
        {
            batMode = 2;
            _camera.transform.position = tpCameraLocation.position;
            look.swingBallReticle.enabled = false;
            look.swingTargetReticle.enabled = false;
        }
    }

    // Called to start/stop blocking
    private void ChangeBlocking(bool _blocking)
    {
        if (_blocking)
        {
            bat.localPosition = batBlockPosition;
            bat.localRotation = batBlockQuat;
            blockCollider.isTrigger = false;
            look.swingBallReticle.enabled = false;
            look.swingTargetReticle.enabled = false;
            blocking = true;
        }
        else
        {
            bat.localPosition = batIdlePosition;
            bat.localRotation = batIdleQuat;
            blockCollider.isTrigger = true;
            if (batMode == 1)
            {
                look.swingBallReticle.enabled = true;
                look.swingTargetReticle.enabled = true;
            }
            blocking = false;
        }
    }

    // Called to swing at ball
    void SwingAtBall()
    {
        if (targetedBalls.Count == 0) return;
        GameObject ball = (GameObject)targetedBalls[0];
        if (ball)
        {
            Rigidbody ballrb = ball.GetComponentInChildren<Rigidbody>();
            if (ballrb)
                ballrb.velocity = new Vector3(-ballrb.velocity.x, ballrb.velocity.y, -ballrb.velocity.z);
        }
        for (int i = 1; i < targetedBalls.Count; i++)
        {
            GameObject b = (GameObject)targetedBalls[i];
            if (b)
            {
                Rigidbody brb = ball.GetComponentInChildren<Rigidbody>();
                if (brb)
                    brb.velocity = new Vector3(-brb.velocity.x, brb.velocity.y, -brb.velocity.z);
            }
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
