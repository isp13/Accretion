using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    int speed = 10;

    int speedLimit = 20;
    public static bool isMoving = false;
    private Rigidbody2D rb;

    public FloatingJoystick variableJoystick;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
    }

    // Next update in second
    private int nextUpdate = 2;
    // Update is called once per frame
    void Update()
    {
        bool somethingWasPressed = false; // checking if movement was made this frame
        
        if (Input.GetKey(KeyCode.LeftArrow))
         {
            rb.AddForce(Vector2.left * speed);
            //rb.AddForce(Vector3.up * speed);
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, speedLimit);
            somethingWasPressed = true;
        }
        
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rb.AddForce(Vector2.up * speed);
            //rb.AddForce(Vector3.up * speed);
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, speedLimit);
            somethingWasPressed = true;
        }
        
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.AddForce(Vector2.right * speed);
            //rb.AddForce(Vector3.up * speed);
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, speedLimit);
            somethingWasPressed = true;
        }
        
        if (Input.GetKey(KeyCode.DownArrow))
        {
            rb.AddForce(Vector2.down * speed);
            //rb.AddForce(Vector3.up * speed);
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, speedLimit);
            somethingWasPressed = true;
        }
        if (variableJoystick.Vertical != 0 || variableJoystick.Horizontal != 0)
        {
            Vector3 direction = Vector3.up * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;
            rb.AddForce(direction * speed);
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, speedLimit);
            somethingWasPressed = true;
        }

        if (somethingWasPressed)
        {
            // If the next update is reached
            if (Time.time >= nextUpdate)
            {
                int tmp = Random.Range(Constants.LowerSecondsGenPlanet, Constants.UpperSecondsGenPlanet);
                // Change the next update (current second+1)
                nextUpdate = Mathf.FloorToInt(Time.time) + tmp;
                // Call your fonction
                Events.InvokeNotifyMovement();
            }
            
        }

        

    }
    }
