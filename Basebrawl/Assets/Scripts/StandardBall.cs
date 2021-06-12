using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardBall : MonoBehaviour
{
    protected GameObject Player, Ground;
    protected GameObject[] BorderWalls;
    public Collider _collider;
    public Rigidbody _rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        Ground = GameObject.FindWithTag("Ground");
        BorderWalls = GameObject.FindGameObjectsWithTag("Map Border");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Called when ball collides with something
    private void OnCollisionEnter(Collision collision)
    {
        // Called when the ball hits the ground
        if (collision.gameObject.tag.Equals("Ground"))
        {
            // Prevent player from moving up after stepping on ball
            Physics.IgnoreLayerCollision(3, 6);
        }
    } 
}
