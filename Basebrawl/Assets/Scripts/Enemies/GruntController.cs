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
    // Point to spawn thrown balls from
    public Transform ReleasePoint;
    // Horizontal speed at which balls are thrown, in mph
    public float ThrowSpeed = 65;
    // Speed at which balls are thrown, in m/s (Unity units)
    private float throwSpeed;
    // Change in y-coordinate from release point to player's head
    private float throwDy;

    // Start is called before the first frame update
    void Start()
    {
        Health = BaseHealth;
        throwSpeed = ThrowSpeed * 0.44704f;
        throwDy = 1.35f - ReleasePoint.position.y;
        Debug.Log(throwDy);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Roll"))
        {
            ThrowBall();
        }
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
        Vector3 playerPosition = GameObject.FindWithTag("Player").transform.position;

        // Calculate the starting rotation
        Vector3 diffVector = playerPosition - ReleasePoint.transform.position;
        diffVector.y = throwDy;
        ball.transform.eulerAngles = transform.up * -Mathf.Atan(diffVector.z / diffVector.x) * 180 / Mathf.PI;

        // Calculate the vertical velocity
        float g = Physics.gravity.y;
        float deltaX = Mathf.Sqrt(diffVector.x * diffVector.x + diffVector.z * diffVector.z);
        float deltaT = deltaX / throwSpeed;
        float v0y = diffVector.y / deltaT - 0.5f * g * deltaT; // diffVector.y = delta y 
        // Create a 2d vector of magnitude 1 to store horizontal direction information
        Vector2 xz = new Vector2(diffVector.x, diffVector.z);
        xz.Normalize();
        // Throw the ball
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
