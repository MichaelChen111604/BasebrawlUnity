using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntController : MonoBehaviour
{
    [Header("Health")] [Tooltip("Starting health")]
    public static float BaseHealth = 100;
    // Current health
    public float Health { get; private set; }
    // Ball object to be instantiated when thrown
    public GameObject Ball;
    // Change in y-coordinate from release point to player's head
    private float throwDy;
    [Header("Throwing")]
    [Tooltip("Time in seconds between throws")]
    private float timeBetweenThrows = 3.5f;
    [Tooltip("Point to spawn thrown balls from")]
    public Transform ReleasePoint;
    [Tooltip("Horizontal speed at which balls are thrown, in mph")]
    public float ThrowSpeed = 65;
    // Speed at which balls are thrown, in m/s (Unity units)
    private float throwSpeed;
    // Time in seconds since last throw
    private float timeSinceLastThrow = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Health = BaseHealth;
        throwSpeed = ThrowSpeed * 0.44704f;
        throwDy = 1.1f - ReleasePoint.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        // Throw a ball every timeBetweenThrows seconds
        timeSinceLastThrow += Time.deltaTime;
        if (timeSinceLastThrow >= timeBetweenThrows)
        {
            ThrowBall();
            timeSinceLastThrow = 0f;
        }
        // Stay facing the player
        RotateToPlayer();
    }

    // Called to change Health
    void TakeDamage(float damage)
    {
        Health -= damage;
        if (Health <= 0) Die();
    }

    // Called when health is 0
    void Die()
    {
        Destroy(gameObject);
    }

    // Throw ball
    void ThrowBall()
    {
        GameObject ball = Instantiate(Ball, ReleasePoint.position, Quaternion.Euler(0, 0, 0));
        Vector3 p = GameObject.FindWithTag("Player").transform.position;
        Vector3 vp = GameObject.FindWithTag("Player").GetComponent<PlayerCharacterController>().velocity;

        // Calculate the starting rotation
        Vector3 pb = p - ReleasePoint.transform.position; // player-ball vector
        pb.y = 0;
        
        ball.transform.eulerAngles = transform.up * -Mathf.Atan(pb.z / pb.x) * 180 / Mathf.PI;

        float dt = 1; // Airtime of ball before hitting player
        if (vp.magnitude != 0) // Formulas are different depending on whether the player is moving
        {
            //// Calculate xz throw vector to hit moving target
            // Cosine of angle between player trajectory and diffVector
            float cosA = Mathf.Cos(Mathf.Atan(vp.x / vp.z) - Mathf.Atan(pb.x / pb.z));
            // Quadratic formula
            dt = (-2 * pb.magnitude * vp.magnitude * cosA
                + Mathf.Sqrt(4 * pb.sqrMagnitude * vp.sqrMagnitude * cosA * cosA
                + 4 * pb.sqrMagnitude * (throwSpeed * throwSpeed - vp.sqrMagnitude)))
                / (2 * (throwSpeed * throwSpeed - vp.sqrMagnitude));
        }
        else
        {
            float dx = Mathf.Sqrt(pb.x * pb.x + pb.z * pb.z);
            dt = dx / throwSpeed;
        }
        //// Calculate the vertical velocity
        float g = Physics.gravity.y;
        pb.y = throwDy;
        float v0y = pb.y / dt - 0.5f * g * dt; // diffVector.y = delta y 
        //// Direction vector x/z for enemy to throw at
        Vector2 xz = new Vector2(pb.x + vp.x * dt, pb.z + vp.z * dt);
        xz.Normalize();
        //// Throw the ball
        ball.GetComponent<Rigidbody>().velocity = throwSpeed * (Vector3.right * xz.x + Vector3.forward * xz.y) + Vector3.up * v0y;
    }

    void RotateToPlayer()
    {
        // Vector to player
        Vector3 diffVector = GameObject.FindWithTag("Player").transform.position - ReleasePoint.transform.position;
        diffVector.y = 0;
        diffVector.Normalize();
        transform.eulerAngles = Quaternion.LookRotation(diffVector).eulerAngles + transform.up * 90;
    }
}
