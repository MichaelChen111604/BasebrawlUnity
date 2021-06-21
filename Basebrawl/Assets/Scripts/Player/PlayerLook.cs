using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Handles looking and aiming for the player
public class PlayerLook : MonoBehaviour
{

    public PlayerCharacterController pcc;
    
    public float mouseXSensitivity = 1000f;
    public float reticleYSensitivity = 1.2f;

    public Image swingBallReticle, swingTargetReticle;
    [Header("Swing Reticle")]
    public float swingBallReticleX;
    public float swingBallReticleRadius = 80;
    private float SwingBallReticleMaxY, SwingBallReticleMinY;

    // Start is called before the first frame update
    void Start()
    {
        swingBallReticle.rectTransform.anchoredPosition = new Vector2(50, -50);
        swingBallReticle.rectTransform.sizeDelta = new Vector2(swingBallReticleRadius, swingBallReticleRadius);
        SwingBallReticleMaxY = 100;
        SwingBallReticleMinY = -100;
        
    }

    // Update is called once per frame
    void Update()
    {

        // Horizontal mouse look
        float mouseX = Input.GetAxis("Mouse X") * mouseXSensitivity * Time.deltaTime;
        // Rotate the entire player around the y-axis
        transform.Rotate(Vector3.up * mouseX);

        // Move the reticle up and down
        float mouseY = Input.GetAxis("Mouse Y") * reticleYSensitivity * Time.deltaTime;
        

        if (SwingBallReticleMinY < swingBallReticle.rectTransform.anchoredPosition.y + mouseY && swingBallReticle.rectTransform.anchoredPosition.y + mouseY < SwingBallReticleMaxY)
        {
            swingBallReticle.rectTransform.anchoredPosition += mouseY * Vector2.up;
        }

    }
}
