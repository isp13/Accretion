using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEditor;
using cakeslice;

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

    private System.DateTime lastTimeDownGrade;

    void Start()
    {
        lastTimeDownGrade = System.DateTime.Now;
        //this.name = "Planet";
        prefabs = GameObject.Find("PrefabStorage").GetComponent<PrefabsStorage>();



        if (this.name == "Asteroid" || this.tag == "Asteroid")
        {
            this.StartingMass = Constants.AsteroidStartingMass;
            this.GetComponent<Rigidbody2D>().AddForce(Constants.GravityPower * new Vector3(rnd.Next(), rnd.Next(), rnd.Next()).normalized);

        }

        // подсветка включается только у планет и звезд. поэтому отключаем сейчас
        if (this.tag == "Player" && this.name == "Asteroid")
            this.GetComponent<Outline>().enabled = false;


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
        this.GetComponent<TrailRenderer>().enabled = false;
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
                // добавил второе условие, чтобы притягивал только больший по рангу, не было взаимного притяжения
                if (this.name == "Asteroid" && otherBody.gameObject.GetComponent<GravitationalBody>().name == "Asteroid")
                {
                    
                }
                else if ((this.name != "Asteroid") && Constants.HierarchyDict[this.name] >= Constants.HierarchyDict[otherBody.GetComponent<GravitationalBody>().name])
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


    // если объект захвачен на орбиту, предоставить возможность его "съесть по нажатии на него"
    void OnMouseDown()
    {
        Debug.Log("CLICK");
        // this object was clicked - do something
        if (onOrbit)
        {
            Debug.Log("DESROYING");
            // увеличиваем массу центра системы из космических объектов кода съели
            GameObject.Find("Player").GetComponent<GravitationalBody>().StartingMass += 1;
            Debug.Log("Destroying by click");
            Destroy(this.gameObject);
            
        }
    }


    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "destroyer")
            return;
        Debug.Log("OnCollisionEnter2D");


        Debug.Log(coll.gameObject.name);
        Debug.Log(this.gameObject.name);
        // если наш объект больше другого на две позиции - увеличение массы возможно даже врезавшись. 
        if (Constants.HierarchyDict[name] > Constants.HierarchyDict[coll.gameObject.GetComponent<GravitationalBody>().name] + 1)
        {
            Debug.Log("Destroying object because a bigger one was near");
            Destroy(coll.gameObject);
            this.StartingMass += 1;

            if (StartingMass / Constants.HierarchyMaxMass[name] >= 1f)
            {
                // transform to new object
                Debug.Log("NEED TO TRANSFORM");

                Upgrade_Retransform_Object();


                if (this.tag == "Player" && (this.name == "Planet" || this.name.Contains("Star")))
                    this.GetComponent<Outline>().enabled = true;
                else
                {
                    this.GetComponent<Outline>().enabled = false;
                }

                
            }
        }
        else if (Constants.HierarchyDict[name] > Constants.HierarchyDict[coll.gameObject.GetComponent<GravitationalBody>().name]) // если больше на одну позицию
        {
            Debug.Log("Destroying object because a bigger one was near");
            //GameObject.Find("OnlyAsteroidsGenerator").GetComponent<Spawner>().SpawnAsteroidConstantPosition(this.transform.position + new Vector3((float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble()));
            Destroy(coll.gameObject);
            this.StartingMass -= 1;

            if (StartingMass < Constants.HierarchyMinMass[name]) // если объект потерял массу и стал весить меньше обычного
            {
                // transform to new object
                Debug.Log("NEED TO TRANSFORM");

                DownGrade_Retransform_Object();


                if (this.tag == "Player" && (this.name == "Planet" || this.name.Contains("Star")))
                    this.GetComponent<Outline>().enabled = true;
                else
                {
                    this.GetComponent<Outline>().enabled = false;
                }

                // изменение мешей, массы и тп при трансформации

                
            }
        }
        else if (this.name == "Asteroid" && Constants.HierarchyDict[name] == Constants.HierarchyDict[coll.gameObject.GetComponent<GravitationalBody>().name])
        {
            if (this.tag == "Player")
            {
                this.startingMass += 1;
                Destroy(coll.gameObject);

                if (StartingMass / Constants.HierarchyMaxMass[name] >= 1f)
                {
                    // transform to new object
                    Debug.Log("NEED TO TRANSFORM");

                    // номер следующего объекта из словаря
                    int nextRank = Constants.HierarchyDict[name] + 1;

                    this.name = Constants.HierarchyDict.FirstOrDefault(x => x.Value == nextRank).Key;
                    if (this.tag != "Player")
                        this.tag = Constants.HierarchyDict.FirstOrDefault(x => x.Value == nextRank).Key;


                    if (this.tag == "Player" && (this.name == "Planet" || this.name.Contains("Star")))
                        this.GetComponent<Outline>().enabled = true;
                    else
                    {
                        this.GetComponent<Outline>().enabled = false;
                    }

                    // изменение мешей, массы и тп при трансформации

                    if (name == "Asteroid")
                        Retransform_Asteroid();

                    else if (name == "DwarfPlanet")
                        Retransform_DwarfPlanet();

                    
                }
                else {
                    if (this.name == "Asteroid") // небольшое увеличение астероида после удара в другой астероид
                        this.transform.localScale *= 1.1f;
                }

            }
        }
        else if (Constants.HierarchyDict[name] == Constants.HierarchyDict[coll.gameObject.GetComponent<GravitationalBody>().name])
        {

            //нужно переродить объекты в меньшие и заспавнить космический мусор
            if (lastTimeDownGrade.AddSeconds(1) <= System.DateTime.Now && this.GetHashCode() > coll.gameObject.GetHashCode())
            {
                lastTimeDownGrade = System.DateTime.Now;
                DownGrade_Retransform_Object();
                coll.gameObject.GetComponent<GravitationalBody>().DownGrade_Retransform_Object();
                this.GetComponent<Rigidbody2D>().AddForce((StartingMass * 0.5f) * Constants.GravityPower * DetermineGravitationalForce(coll.gameObject.GetComponent<Rigidbody2D>()));
                coll.gameObject.GetComponent<Rigidbody2D>().AddForce(-0.5f * (StartingMass * 0.4f) * Constants.GravityPower * DetermineGravitationalForce(coll.gameObject.GetComponent<Rigidbody2D>()));
            }

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


    public void Upgrade_Retransform_Object()
    {

        // transform to new object
        Debug.Log("NEED TO TRANSFORM");

        // номер следующего объекта из словаря
        int nextRank = Constants.HierarchyDict[name] + 1;

        this.name = Constants.HierarchyDict.FirstOrDefault(x => x.Value == nextRank).Key;
        if (this.tag != "Player")
            this.tag = Constants.HierarchyDict.FirstOrDefault(x => x.Value == nextRank).Key;
        // изменение мешей, массы и тп при трансформации

        if (name == "Asteroid")
            Retransform_Asteroid();

        else if (name == "DwarfPlanet")
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
        else if (name == "BlackHole")
            Retransform_BlackHole();
        else
            Debug.Log("tag error");

    }

    public void DownGrade_Retransform_Object()
    {
        float saveMass = this.StartingMass;
        int nextRank = Constants.HierarchyDict[name] - 1;
        this.name = Constants.HierarchyDict.FirstOrDefault(x => x.Value == nextRank).Key;
        if (this.tag != "Player")
            this.tag = Constants.HierarchyDict.FirstOrDefault(x => x.Value == nextRank).Key;

        if (name == "Asteroid")
            Retransform_Asteroid();

        else if (name == "DwarfPlanet")
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
        else if (name == "BlackHole")
            Retransform_BlackHole();
        else
            Debug.Log("tag error");

        this.StartingMass = (int)(saveMass * 0.8f);
    }

    public void Retransform_Asteroid()
    {
        Debug.Log("Retransform_Asteroid");
        // получаем рандомную планету из префабов
        Object planetPrefab = prefabs.asteroids[rnd.Next(0, prefabs.asteroids.Length)] as GameObject;
        GameObject randomOne = Instantiate(planetPrefab, new Vector3(1, 1, 1) * 10000, this.transform.rotation) as GameObject;
        this.gameObject.GetComponent<MeshFilter>().mesh = randomOne.GetComponent<MeshFilter>().mesh;
        this.gameObject.GetComponent<MeshRenderer>().materials = randomOne.GetComponent<MeshRenderer>().materials;

        this.StartingMass = Constants.HierarchyMinMass[name];
        this.ImaginaryMass = Constants.HierarchyMaxMass[name];

        this.transform.localScale = new Vector3(Constants.AsteroidScale, Constants.AsteroidScale, Constants.AsteroidScale);
        float scaledRadius = Mathf.Max(transform.localScale.x, transform.localScale.y);
        this.GetComponent<CircleCollider2D>().radius = scaledRadius;
        Destroy(randomOne);


        if (this.tag == "Player")
        {
            Constants.PlayersNextObject = "Dwarf Planet";
            GameObject.Find("Main Camera").GetComponent<CameraScript>().SmoothChangeOrthographicSize(Constants.AsteroidMainCameraDistance);
        }

    }
    /// <summary>
    /// Функция для превращения модели объекта в модель карликовой планеты
    /// </summary>
    public void Retransform_DwarfPlanet()
    {
        Debug.Log("Retransform_DwarfPlanet");
        // получаем рандомную планету из префабов
        Object planetPrefab = prefabs.planets[rnd.Next(0, prefabs.planets.Length)] as GameObject;
        GameObject randomOne = Instantiate(planetPrefab, new Vector3(1,1,1) * 10000, this.transform.rotation) as GameObject;
        this.gameObject.GetComponent<MeshFilter>().mesh = randomOne.GetComponent<MeshFilter>().mesh;
        this.gameObject.GetComponent<MeshRenderer>().materials = randomOne.GetComponent<MeshRenderer>().materials;

        this.StartingMass = Constants.HierarchyMinMass[name];
        this.ImaginaryMass = Constants.HierarchyMaxMass[name];

        this.transform.localScale = new Vector3(Constants.DwarfPlanetScale, Constants.DwarfPlanetScale, Constants.DwarfPlanetScale);
        

        CircleCollider2D[] colliders = this.transform.GetComponents<CircleCollider2D>();
        foreach (CircleCollider2D collider in colliders)
        {
            if (!collider.isTrigger)
            {
                // This is collider is not a trigger
                collider.radius = Constants.ColliderRadius_DwarfPlanet;
            }
        }
        
        Destroy(randomOne);
        

        if (this.tag == "Player")
        {
            Constants.PlayersNextObject = "Planet";
            GameObject.Find("Main Camera").GetComponent<CameraScript>().SmoothChangeOrthographicSize(Constants.PlanetMainCameraDistance);
        }
    }

    public void Retransform_Planet()
    {
        Debug.Log("Retransform_Planet");
        // получаем рандомную планету из префабов
        Object planetPrefab = prefabs.planets[rnd.Next(0, prefabs.planets.Length)] as GameObject;
        GameObject randomOne = Instantiate(planetPrefab, new Vector3(1, 1, 1) * 10000, this.transform.rotation) as GameObject;
        this.gameObject.GetComponent<MeshFilter>().mesh = randomOne.GetComponent<MeshFilter>().mesh;
        this.gameObject.GetComponent<MeshRenderer>().materials = randomOne.GetComponent<MeshRenderer>().materials;

        this.StartingMass = Constants.HierarchyMinMass[name];
        this.ImaginaryMass = Constants.HierarchyMaxMass[name];

        this.transform.localScale = new Vector3(Constants.PlanetScale, Constants.PlanetScale, Constants.PlanetScale);
        float scaledRadius = Mathf.Max(transform.localScale.x, transform.localScale.y);
        this.GetComponent<CircleCollider2D>().radius = scaledRadius;

        CircleCollider2D[] colliders = this.transform.GetComponents<CircleCollider2D>();
        foreach (CircleCollider2D collider in colliders)
        {
            if (!collider.isTrigger)
            {
                // This is collider is not a trigger
                collider.radius = Constants.ColliderRadius_Planet;
            }
        }

        Destroy(randomOne);
        

        if (this.tag == "Player")
        {
            Constants.PlayersNextObject = "Dwarf Star";
            GameObject.Find("Main Camera").GetComponent<CameraScript>().SmoothChangeOrthographicSize(Constants.PlanetMainCameraDistance);
        }
    }

    public void Retransform_DwarfStar()
    {
        Debug.Log("Retransform_DwarfStar");

        // тк звезда, то увеличиваем подсвевиваемость префаба
        var cam = GameObject.Find("Main Camera").GetComponent<cakeslice.OutlineEffect>().lineIntensity = 1.3f;

        // получаем рандомную карликовую звезду из префабов
        Object planetPrefab = prefabs.stars[1] as GameObject; // dark white
        GameObject randomOne = Instantiate(planetPrefab, new Vector3(1, 1, 1) * 10000, this.transform.rotation) as GameObject;

        this.gameObject.GetComponent<MeshFilter>().mesh = randomOne.GetComponent<MeshFilter>().mesh;
        this.gameObject.GetComponent<MeshRenderer>().materials = randomOne.GetComponent<MeshRenderer>().materials;

        this.StartingMass = Constants.HierarchyMinMass[name];
        this.ImaginaryMass = Constants.HierarchyMaxMass[name];

        this.transform.localScale = new Vector3(Constants.DwarfStarScale, Constants.DwarfStarScale, Constants.DwarfStarScale);
        float scaledRadius = Mathf.Max(transform.localScale.x, transform.localScale.y);
        this.GetComponent<CircleCollider2D>().radius = scaledRadius;

        CircleCollider2D[] colliders = this.transform.GetComponents<CircleCollider2D>();
        foreach (CircleCollider2D collider in colliders)
        {
            if (!collider.isTrigger)
            {
                // This is collider is not a trigger
                collider.radius = Constants.ColliderRadius_DwarfStar;
            }
        }

        Destroy(randomOne);

        if (this.tag == "Player")
        {
            Constants.PlayersNextObject = "Star";
            GameObject.Find("Main Camera").GetComponent<CameraScript>().SmoothChangeOrthographicSize(Constants.PlanetMainCameraDistance);
        }
    }

    public void Retransform_Star()
    {
        Debug.Log("Retransform_Star");
        // получаем рандомную звезду из префабов
        Object planetPrefab = prefabs.stars[2] as GameObject; // yellow
        GameObject randomOne = Instantiate(planetPrefab, new Vector3(1, 1, 1) * 10000, this.transform.rotation) as GameObject;

        this.gameObject.GetComponent<MeshFilter>().mesh = randomOne.GetComponent<MeshFilter>().mesh;
        this.gameObject.GetComponent<MeshRenderer>().materials = randomOne.GetComponent<MeshRenderer>().materials;

        this.StartingMass = Constants.HierarchyMinMass[name];
        this.ImaginaryMass = Constants.HierarchyMaxMass[name];

        this.transform.localScale = new Vector3(Constants.StarScale, Constants.StarScale, Constants.StarScale);
        float scaledRadius = Mathf.Max(transform.localScale.x, transform.localScale.y);
        this.GetComponent<CircleCollider2D>().radius = scaledRadius;

        CircleCollider2D[] colliders = this.transform.GetComponents<CircleCollider2D>();
        foreach (CircleCollider2D collider in colliders)
        {
            if (!collider.isTrigger)
            {
                // This is collider is not a trigger
                collider.radius = Constants.ColliderRadius_Star;
            }
        }

        Destroy(randomOne);

        if (this.tag == "Player")
        {
            Constants.PlayersNextObject = "Giant Star";
            GameObject.Find("Main Camera").GetComponent<CameraScript>().SmoothChangeOrthographicSize(Constants.PlanetMainCameraDistance);
        }
    }

    public void Retransform_GiantStar()
    {
        Debug.Log("Retransform_GiantStar");
        // получаем рандомную звезду из префабов
        Object planetPrefab = prefabs.stars[0] as GameObject; //red
        GameObject randomOne = Instantiate(planetPrefab, new Vector3(1, 1, 1) * 10000, this.transform.rotation) as GameObject;

        this.gameObject.GetComponent<MeshFilter>().mesh = randomOne.GetComponent<MeshFilter>().mesh;
        this.gameObject.GetComponent<MeshRenderer>().materials = randomOne.GetComponent<MeshRenderer>().materials;

        this.StartingMass = Constants.HierarchyMinMass[name];
        this.ImaginaryMass = Constants.HierarchyMaxMass[name];

        this.transform.localScale = new Vector3(Constants.GiantStarScale, Constants.GiantStarScale, Constants.GiantStarScale);
        float scaledRadius = Mathf.Max(transform.localScale.x, transform.localScale.y);
        this.GetComponent<CircleCollider2D>().radius = scaledRadius;

        CircleCollider2D[] colliders = this.transform.GetComponents<CircleCollider2D>();
        foreach (CircleCollider2D collider in colliders)
        {
            if (!collider.isTrigger)
            {
                // This is collider is not a trigger
                collider.radius = Constants.ColliderRadius_GiantStar;
            }
        }

        Destroy(randomOne);
        

        if (this.tag == "Player")
        {
            Constants.PlayersNextObject = "Neutron Star";
            GameObject.Find("Main Camera").GetComponent<CameraScript>().SmoothChangeOrthographicSize(Constants.PlanetMainCameraDistance);
        }
    }

    public void Retransform_NeutronStar()
    {
        Debug.Log("Retransform_NeutronStar");
        // получаем рандомную звезду из префабов
        Object planetPrefab = prefabs.stars[4] as GameObject; // blue star
        GameObject randomOne = Instantiate(planetPrefab, new Vector3(1, 1, 1) * 10000, this.transform.rotation) as GameObject;

        this.gameObject.GetComponent<MeshFilter>().mesh = randomOne.GetComponent<MeshFilter>().mesh;
        this.gameObject.GetComponent<MeshRenderer>().materials = randomOne.GetComponent<MeshRenderer>().materials;

        this.StartingMass = Constants.HierarchyMinMass[name];
        this.ImaginaryMass = Constants.HierarchyMaxMass[name];

        float scaledRadius = Mathf.Max(transform.localScale.x, transform.localScale.y);
        this.GetComponent<CircleCollider2D>().radius = scaledRadius;
        this.transform.localScale = new Vector3(Constants.NeutronStarScale, Constants.NeutronStarScale, Constants.NeutronStarScale);

        CircleCollider2D[] colliders = this.transform.GetComponents<CircleCollider2D>();

        foreach (CircleCollider2D collider in colliders)
        {
            if (!collider.isTrigger)
            {
                // This is collider is not a trigger
                collider.radius = Constants.ColliderRadius_NeutronStar;
            }
        }

        Destroy(randomOne);
        

        if (this.tag == "Player")
        {
            Constants.PlayersNextObject = "Black Hole";
            GameObject.Find("Main Camera").GetComponent<CameraScript>().SmoothChangeOrthographicSize(Constants.PlanetMainCameraDistance);
        }
    }

    public void Retransform_BlackHole()
    {
        // получаем рандомную звезду из префабов
        Object planetPrefab = prefabs.BlackHole[0] as GameObject;
        GameObject randomOne = Instantiate(planetPrefab, new Vector3(1, 1, 1) * 10000, this.transform.rotation) as GameObject;

        this.gameObject.GetComponent<MeshFilter>().mesh = randomOne.GetComponent<MeshFilter>().mesh;
        this.gameObject.GetComponent<MeshRenderer>().materials = randomOne.GetComponent<MeshRenderer>().materials;

        this.StartingMass = Constants.HierarchyMinMass[name];
        this.ImaginaryMass = Constants.HierarchyMaxMass[name];

        this.transform.localScale = new Vector3(Constants.BlackHoleScale, Constants.BlackHoleScale, Constants.BlackHoleScale);
        float scaledRadius = Mathf.Max(transform.localScale.x, transform.localScale.y);
        this.GetComponent<CircleCollider2D>().radius = scaledRadius;

        CircleCollider2D[] colliders = this.transform.GetComponents<CircleCollider2D>();
        foreach (CircleCollider2D collider in colliders)
        {
            if (!collider.isTrigger)
            {
                // This is collider is not a trigger
                collider.radius = Constants.ColliderRadius_BlackHole;
            }
        }

        Destroy(randomOne);
        Constants.PlayersNextObject = "???";
    }




}