using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLook : MonoBehaviour
{

    public float mouseXSensitivity = 800f;
    public float reticleYSensitivity = 400f;

    public Image swingBallReticle;
    [Header("Swing Reticle")]
    public float defaultSwingBallReticleX = 50;
    public float defaultSwingBallReticleY = -50;
    public float defaultSwingBallReticleWidth = 80;
    public float defaultSwingBallReticleHeight = 80;

    // Start is called before the first frame update
    void Start()
    {
        swingBallReticle.rectTransform.anchoredPosition = new Vector2(defaultSwingBallReticleX, defaultSwingBallReticleY);
        swingBallReticle.rectTransform.sizeDelta = new Vector2(defaultSwingBallReticleHeight, defaultSwingBallReticleWidth); 
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
        swingBallReticle.rectTransform.anchoredPosition += new Vector2(0, mouseY);
    }
}
