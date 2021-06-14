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
    // Speed at which balls are thrown, in mph
    public float ThrowSpeed = 60;
    // Speed at which balls are thrown, in m/s (Unity units)
    private float throwSpeed;
    // Change in y-coordinate from release point to player's head
    private float throwDy;

    // Start is called before the first frame update
    void Start()
    {
        Health = BaseHealth;
        throwSpeed = ThrowSpeed * 0.44704f;
        // IMPORTANT! Change 0.67 to FirstPerson y coordinate if first person camera is moved for player
        throwDy = 0.67f - ReleasePoint.position.y;
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
        
        // Calculate the correct starting rotation
        Vector3 diffVector = GameObject.FindWithTag("Player").transform.position - gameObject.transform.position;
        ball.transform.eulerAngles = transform.up * -Mathf.Atan(diffVector.z / diffVector.x) * 180 / Mathf.PI;

        // Calculate the correct throwing angle, then set the ball's velocity up and lateral (towards the player)
        // float dy =  / Physics.gravity.y;

    }

    void RotateToPlayer()
    {
        Vector3 playerLocation = GameObject.FindWithTag("Player").transform.position;
        // Vector to player
        Vector3 diffVector = playerLocation - gameObject.transform.position;
        diffVector.y = 0;
        diffVector.Normalize();
        transform.eulerAngles = Quaternion.LookRotation(diffVector).eulerAngles + transform.up * 90;
    }
}
