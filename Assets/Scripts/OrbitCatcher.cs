using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCatcher : MonoBehaviour
{
    bool isPlayerColliding = false;
    // How long the player needs to stay at location
    public float timerCountDown = 5.0f;

    GravitationalBody player;

    void FixedUpdate()
    {
        // Collision timer
        if (isPlayerColliding == true)
        {
            timerCountDown -= Time.deltaTime;
            if (timerCountDown < 0)
            {
                timerCountDown = 0;
            }
        }
    }

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<GravitationalBody>();
    }

    // Start the collision timer when player enters
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != "Player")
        {
            Debug.Log("object Entered");
            isPlayerColliding = true;
        }

    }
    // Check if the player is still at location, if they are spawn our secret item
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag != "Player" && isPlayerColliding == true)
        {
            if (timerCountDown <= 0)
            {
                other.GetComponent<GravitationalBody>().target = this.transform;
                other.GetComponent<GravitationalBody>().onOrbit = true;

                player.OrbitBodies.Add(other.gameObject);

                other.GetComponent<TrailRenderer>().enabled = false;
                timerCountDown = 5;
            }

        }
    }
    // If the player is not colliding reset our timer
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag != "Player")
        {
            Debug.Log("object Exited");
            isPlayerColliding = false;
            player.OrbitBodies.Remove(other.gameObject);
            other.GetComponent<GravitationalBody>().onOrbit = false;
        }
    }
}
