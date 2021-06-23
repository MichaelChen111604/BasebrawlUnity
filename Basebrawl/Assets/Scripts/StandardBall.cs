using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StandardBall : MonoBehaviour
{
    protected GameObject Player, Ground;
    protected PlayerCharacterController playerScript;
    protected GameObject[] BorderWalls;
    protected Camera playerCamera;
    protected PlayerLook playerLook;
    protected Image swingReticle;
    public Collider _collider;
    public Rigidbody _rigidbody;
    // Used to determine how much damage should be caused by this ball - given in mph
    public float thrownSpeed { get; set; } = 70f;
    // Used to determine if currently overlapping with swing reticle - z component is distance from player
    private Vector3 cameraPosition;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        Ground = GameObject.FindWithTag("Ground");
        BorderWalls = GameObject.FindGameObjectsWithTag("Map Border");
        playerCamera = Player.GetComponentInChildren<Camera>();
        playerScript = Player.GetComponent<PlayerCharacterController>();
        playerLook = Player.GetComponent<PlayerLook>();
        swingReticle = playerLook.swingBallReticle;
        gameObject.tag = "Ball";
    }

    // Update is called once per frame
    void Update()
    {
        // Ball rotates after being thrown
        transform.Rotate(Vector3.forward * 50);
        
        // Get position of this ball in camera space
        cameraPosition = playerCamera.WorldToScreenPoint(transform.position);
        // Account for anchoring offset
        cameraPosition.x -= 0.5f * Screen.width;
        cameraPosition.y -= 0.5f * Screen.height;
        // Check if it's inside the swing reticle and the player is facing the ball
        if (cameraPosition.z <= playerScript.swingRange && cameraPosition.z > 0)
        {
            Vector2 toCenter = new Vector2(cameraPosition.x - swingReticle.rectTransform.anchoredPosition.x, cameraPosition.y - swingReticle.rectTransform.anchoredPosition.y);
            if (toCenter.sqrMagnitude < 0.25 * playerLook.swingBallReticleRadius * playerLook.swingBallReticleRadius)
            {
                swingReticle.color = Color.red;
                if (!playerScript.targetedBalls.Contains(gameObject))
                    playerScript.targetedBalls.Add(gameObject);
            }
            else
            {
                playerScript.targetedBalls.Remove(gameObject);
                if (playerScript.targetedBalls.Count == 0) swingReticle.color = Color.white;
            }
        }
        else
        {
            playerScript.targetedBalls.Remove(gameObject);
            if (playerScript.targetedBalls.Count == 0) swingReticle.color = Color.white;
        }
    }

    // Called when ball collides with something
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("MapBorder") || collision.gameObject.tag.Equals("Ground")) 
        {
            IgnorePlayer();
        }
    }

    // Ignore collisions with the player
    public void IgnorePlayer()
    {
        Physics.IgnoreCollision(_collider, Player.GetComponentInChildren<CapsuleCollider>());
    }
}
