using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColliderScript : MonoBehaviour
{
    public PlayerCharacterController PlayerScript;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Ball"))
        {
            Debug.Log("Collided with a ball");
            PlayerScript.TakeDamage(collision.gameObject.GetComponent<Rigidbody>());
        }
    }
}
