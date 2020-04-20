using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class GravitationalBody : MonoBehaviour
{
    public string name = "";

    public float maxDistance;

    private float startingMass;
    public float StartingMass // for transformations
    {
        get { return startingMass; }
        set { startingMass = value; } // here need to check if its alot mass so object can transform to the next stage
    }

    private float imaginaryMass;
    public float ImaginaryMass // for transformations
    {
        get { return imaginaryMass; }
        set { imaginaryMass = value; } // here need to check if its alot mass so object can transform to the next stage
    }

    public Vector2 initialVelocity;


    private Rigidbody2D rb;
    //I use a static list of bodies so that we don't need to Find them every frame
    static List<Rigidbody2D> attractableBodies = new List<Rigidbody2D>();

    public bool onOrbit = false;
    public Transform target;

    void Start()
    {
        //this.name = "Planet";
        
        if (this.name == "Planet" || this.tag == "Planet" || this.tag == "Player")
        {
            this.StartingMass = Constants.PlanetsStartingMass;
            this.ImaginaryMass = Constants.PlanetCriticalMass;
        }

        if (this.name == "Asteroid" || this.tag == "Asteroid")
        {
            this.StartingMass = Constants.AsteroidStartingMass;

        }

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
        if (this.tag != "Player" )
        {
            this.gameObject.AddComponent<Rigidbody2D>();
        }
        rb = this.gameObject.GetComponent<Rigidbody2D>();

        rb.gravityScale = 0f;
        rb.drag = 0f;
        rb.angularDrag = 0f;
        
        rb.mass = StartingMass;
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
        this.GetComponent<MeshRenderer>().material.color = Random.ColorHSV(1f, 1f, 1f, 1f, 1f, 1f);
        
    }

    void FixedUpdate()
    {
        if (!onOrbit) //если не на орбите - физика работает в нормальном режиме
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
        else
        {
            Vector3 relativePos = (target.position + new Vector3(0, 1.5f, 0)) - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos);

            Quaternion current = transform.localRotation;

            transform.localRotation = Quaternion.Slerp(current, rotation, Time.deltaTime);
            transform.Translate(0, 0, 3 * Time.deltaTime);

            //rb.mass = 0; // обнуляем массу чтобы наш объект не тянуло в сторону орбитального
            //attractableBodies.Remove(this.rb);
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

    /// <summary>
    /// get object movement vector
    /// </summary>
    /// <returns>Vector3 movement vector</returns>
    public Vector3 GetMovementVector()
    {
        return rb.velocity;
    }


}