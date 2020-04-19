using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    int speed = 10;

    int speedLimit = 20;
    public static bool isMoving = false;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
    }

    // Next update in second
    private int nextUpdate = 10;
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

        if (somethingWasPressed)
        {
            // If the next update is reached
            if (Time.time >= nextUpdate)
            {
                int tmp = Random.Range(Constants.LowerSecondsGenPlanet, Constants.UpperSecondsGenPlanet);
                Debug.Log(Time.time + ">=" + nextUpdate);
                // Change the next update (current second+1)
                nextUpdate = Mathf.FloorToInt(Time.time) + tmp;
                Debug.Log("NEXT UPDATE IN " + tmp.ToString());
                // Call your fonction
                Events.InvokeNotifyMovement();
            }
            
        }

        

    }
    }
