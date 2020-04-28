using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEditor;

[RequireComponent(typeof(Rigidbody2D))]
public class GravitationalBody : MonoBehaviour
{
    private PrefabsStorage prefabs;

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

    System.Random rnd = new System.Random();

    public bool onOrbit = false;
    public Transform target;

    void Start()
    {

        //this.name = "Planet";
        prefabs = GameObject.Find("PrefabStorage").GetComponent<PrefabsStorage>();



        if (this.name == "Asteroid" || this.tag == "Asteroid")
        {
            this.StartingMass = Constants.AsteroidStartingMass;
            this.GetComponent<Rigidbody2D>().AddForce(Constants.GravityPower * new Vector3(rnd.Next(), rnd.Next(), rnd.Next()).normalized);

        }
        if (this.name == "DwarfPlanet" || this.tag == "DwarfPlanet")
        {
            this.StartingMass = Constants.DwarfPlanetsStartingMass;
            this.ImaginaryMass = Constants.DwarfPlanetCriticalMass;
        }
        if (this.name == "Planet" || this.tag == "Planet")
        {
            this.StartingMass = Constants.PlanetsStartingMass;
            this.ImaginaryMass = Constants.PlanetCriticalMass;
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
                if (this.name != "Asteroid" || this.tag != "Asteroid")
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


    void OnCollisionEnter2D(Collision2D coll)
    {
        
        // если наш объект больше другого на две позиции - увеличение массы возможно даже врезавшись. ДОБАВИТТЬ СПРАВА ПЛЮС 1
        if (Constants.HierarchyDict[name] > Constants.HierarchyDict[coll.gameObject.GetComponent<GravitationalBody>().name])
        {
            Destroy(coll.gameObject);
            this.startingMass += 1;

            if (StartingMass / Constants.HierarchyMaxMass[name] >= 1f)
            {
                // transform to new object
                Debug.Log("NEED TO TRANSFORM");

                // номер следующего объекта из словаря
                int nextRank = Constants.HierarchyDict[name] + 1 ;

                this.name = Constants.HierarchyDict.FirstOrDefault(x => x.Value == nextRank).Key;
                if (this.tag != "Player")
                    this.tag = Constants.HierarchyDict.FirstOrDefault(x => x.Value == nextRank).Key;
                // изменение мешей, массы и тп при трансформации

                if (name == "DwarfPlanet")
                    Retransform_DwarfPlanet();

                else if (name == "Planet")
                    Retransform_Planet();

                else if (name == "DwarfStar")
                    Retransform_DwarfStar();
                else if (name == "Star")
                    Retransform_Star();
                else if (name == "GiantStar")
                    Retransform_GiantStar();
                else if (name == "NeutronStar")
                    Retransform_NeutronStar();
                else
                    Retransform_BlackHole();
            }
        }
        else if (Constants.HierarchyDict[name] == Constants.HierarchyDict[coll.gameObject.GetComponent<GravitationalBody>().name])
        { 

        }
    }


    public string ShareObjectData(string option)
    {
        switch (option)
        {
            case "name":
                return name.ToString();
            case "mass":
                return StartingMass.ToString();
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


    /// <summary>
    /// Функция для превращения модели объекта в модель карликовой планеты
    /// </summary>
    public void Retransform_DwarfPlanet()
    {
        // получаем рандомную планету из префабов
        Object planetPrefab = prefabs.planets[rnd.Next(0, prefabs.planets.Length)] as GameObject;
        GameObject randomOne = Instantiate(planetPrefab, new Vector3(1,1,1) * 10000, this.transform.rotation) as GameObject;
        this.gameObject.GetComponent<MeshFilter>().mesh = randomOne.GetComponent<MeshFilter>().mesh;
        this.gameObject.GetComponent<MeshRenderer>().materials = randomOne.GetComponent<MeshRenderer>().materials;

        this.StartingMass = Constants.HierarchyMinMass[name];
        this.ImaginaryMass = Constants.HierarchyMaxMass[name];
    }

    public void Retransform_Planet()
    {
        // получаем рандомную планету из префабов
        Object planetPrefab = prefabs.planets[rnd.Next(0, prefabs.planets.Length)] as GameObject;
        GameObject randomOne = Instantiate(planetPrefab, new Vector3(1, 1, 1) * 10000, this.transform.rotation) as GameObject;
        this.gameObject.GetComponent<MeshFilter>().mesh = randomOne.GetComponent<MeshFilter>().mesh;
        this.gameObject.GetComponent<MeshRenderer>().materials = randomOne.GetComponent<MeshRenderer>().materials;

        this.StartingMass = Constants.HierarchyMinMass[name];
        this.ImaginaryMass = Constants.HierarchyMaxMass[name];
    }

    public void Retransform_DwarfStar()
    {
        // получаем рандомную карликовую звезду из префабов
        Object planetPrefab = prefabs.stars[1] as GameObject; // dark white
        GameObject randomOne = Instantiate(planetPrefab, new Vector3(1, 1, 1) * 10000, this.transform.rotation) as GameObject;

        this.gameObject.GetComponent<MeshFilter>().mesh = randomOne.GetComponent<MeshFilter>().mesh;
        this.gameObject.GetComponent<MeshRenderer>().materials = randomOne.GetComponent<MeshRenderer>().materials;

        this.StartingMass = Constants.HierarchyMinMass[name];
        this.ImaginaryMass = Constants.HierarchyMaxMass[name];
    }

    public void Retransform_Star()
    {
        // получаем рандомную звезду из префабов
        Object planetPrefab = prefabs.stars[2] as GameObject; // yellow
        GameObject randomOne = Instantiate(planetPrefab, new Vector3(1, 1, 1) * 10000, this.transform.rotation) as GameObject;

        this.gameObject.GetComponent<MeshFilter>().mesh = randomOne.GetComponent<MeshFilter>().mesh;
        this.gameObject.GetComponent<MeshRenderer>().materials = randomOne.GetComponent<MeshRenderer>().materials;

        this.StartingMass = Constants.HierarchyMinMass[name];
        this.ImaginaryMass = Constants.HierarchyMaxMass[name];
    }

    public void Retransform_GiantStar()
    {
        // получаем рандомную звезду из префабов
        Object planetPrefab = prefabs.stars[0] as GameObject; //red
        GameObject randomOne = Instantiate(planetPrefab, new Vector3(1, 1, 1) * 10000, this.transform.rotation) as GameObject;

        this.gameObject.GetComponent<MeshFilter>().mesh = randomOne.GetComponent<MeshFilter>().mesh;
        this.gameObject.GetComponent<MeshRenderer>().materials = randomOne.GetComponent<MeshRenderer>().materials;

        this.StartingMass = Constants.HierarchyMinMass[name];
        this.ImaginaryMass = Constants.HierarchyMaxMass[name];
    }

    public void Retransform_NeutronStar()
    {
        // получаем рандомную звезду из префабов
        Object planetPrefab = prefabs.stars[4] as GameObject; // blue star
        GameObject randomOne = Instantiate(planetPrefab, new Vector3(1, 1, 1) * 10000, this.transform.rotation) as GameObject;

        this.gameObject.GetComponent<MeshFilter>().mesh = randomOne.GetComponent<MeshFilter>().mesh;
        this.gameObject.GetComponent<MeshRenderer>().materials = randomOne.GetComponent<MeshRenderer>().materials;

        this.StartingMass = Constants.HierarchyMinMass[name];
        this.ImaginaryMass = Constants.HierarchyMaxMass[name];
    }

    public void Retransform_BlackHole()
    {
        // получаем рандомную звезду из префабов
        Object planetPrefab = prefabs.stars[rnd.Next(0, prefabs.stars.Length)] as GameObject;
        GameObject randomOne = Instantiate(planetPrefab, new Vector3(1, 1, 1) * 10000, this.transform.rotation) as GameObject;

        this.gameObject.GetComponent<MeshFilter>().mesh = randomOne.GetComponent<MeshFilter>().mesh;
        this.gameObject.GetComponent<MeshRenderer>().materials = randomOne.GetComponent<MeshRenderer>().materials;

        this.StartingMass = Constants.HierarchyMinMass[name];
        this.ImaginaryMass = Constants.HierarchyMaxMass[name];
    }




}