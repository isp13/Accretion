using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class GravitationalBody : MonoBehaviour
{
    public string name = "Planet";
    public float maxDistance;
    public float startingMass;


    private int imaginaryMass;
    public int ImaginaryMass // for transformations
    {
        get { return imaginaryMass; }
        set { imaginaryMass = value; } // here need to check if its alot mass so object can transform to the next stage
    }

    public Vector2 initialVelocity;


    private Rigidbody2D rb;
    //I use a static list of bodies so that we don't need to Find them every frame
    static List<Rigidbody2D> attractableBodies = new List<Rigidbody2D>();



    void Start()
    {
        this.name = "Planet";
        if (this.tag == "Planet")
            this.startingMass = Constants.PlanetsStartingMass;

        else if (this.tag == "Asteroid")
            this.startingMass = Constants.AsteroidStartingMass;

        this.maxDistance = Constants.MaxGravitationalDistance;

        SetupRigidbody2D();


        SetupColliders();

        SetupMeshRenderer();
        SetupTrailRenderer();


        

        //Add this gravitational body to the list, so that all other gravitational bodies can be effected by it
        attractableBodies.Add(rb);
    }

    void SetupRigidbody2D()
    {
        if (this.tag != "Player")
        {
            this.gameObject.AddComponent<Rigidbody2D>();
        }
        rb = this.gameObject.GetComponent<Rigidbody2D>();

        rb.gravityScale = 0f;
        rb.drag = 0f;
        rb.angularDrag = 0f;
        
        rb.mass = startingMass;
        rb.velocity = initialVelocity;

        
    }

    void SetupColliders()
    {
        this.gameObject.AddComponent<CircleCollider2D>();
        var rb1 = this.gameObject.GetComponent<CircleCollider2D>();

        this.gameObject.AddComponent<CircleCollider2D>();
        var rb2 = this.gameObject.GetComponent<CircleCollider2D>();
        rb2.isTrigger = true;
    }

    void SetupTrailRenderer() {
        this.gameObject.AddComponent<TrailRenderer>();
        this.GetComponent<TrailRenderer>().time = Constants.TrailDisapearTime;
        this.GetComponent<TrailRenderer>().material.color = Random.ColorHSV(0f,1f,1f,1f,1f, 1f);
    }

    void SetupMeshRenderer() {
        this.gameObject.AddComponent<MeshRenderer>();
        this.GetComponent<MeshRenderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        
    }

    void FixedUpdate()
    {

        foreach (Rigidbody2D otherBody in attractableBodies)
        {

            if (otherBody == null)
                continue;

            //We arn't going to add a gravitational pull to our own body
            else if (otherBody == rb)
                continue;
            else
            {
                
            }

            
            // first, it was without any gravity power. But movements were really slow
            otherBody.AddForce(Constants.GravityPower * DetermineGravitationalForce(otherBody));

        }

    }

    Vector2 DetermineGravitationalForce(Rigidbody2D otherBody)
    {

        Vector2 relativePosition = rb.position - otherBody.position;

        float distance = Mathf.Clamp(relativePosition.magnitude, 0, maxDistance);

        //the force of gravity will reduce by the distance squared
        float gravityFactor = 1f - (Mathf.Sqrt(distance) / Mathf.Sqrt(maxDistance));

        //creates a vector that will force the otherbody toward this body, using the gravity factor times the mass of this body as the magnitude
        Vector2 gravitationalForce = relativePosition.normalized * (gravityFactor * rb.mass);

        return gravitationalForce;

    }

    public string ShareObjectData(string option)
    {
        switch (option)
        {
            case "name":
                return name.ToString();
            case "mass":
                return rb.mass.ToString();
            case "coords":
                return rb.position.x.ToString() + "," + rb.position.y.ToString();
            case "vector":
                return rb.velocity.x.ToString() + "," + rb.velocity.y.ToString();
        }

        return "error";

    }
    public Vector3 GetMovementVector()
    {
        return rb.velocity;
        

    }


}