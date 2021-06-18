using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardBall : MonoBehaviour
{
    protected GameObject Player, Ground;
    protected GameObject[] BorderWalls;
    public Collider _collider;
    public Rigidbody _rigidbody;
    // Used to determine how much damage should be caused by this ball - given in mph
    public float thrownSpeed { get; set; } = 70f;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        Ground = GameObject.FindWithTag("Ground");
        BorderWalls = GameObject.FindGameObjectsWithTag("Map Border");
        gameObject.tag = "Ball";
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * 50);
    }

    // Called when ball collides with something
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("MapBorder") || collision.gameObject.tag.Equals("Ground")) 
        {
            Physics.IgnoreCollision(_collider, Player.GetComponentInChildren<CapsuleCollider>());
        }
    } 
}
