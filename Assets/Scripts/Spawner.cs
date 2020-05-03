using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public string genType = "Asteroid";
    private int randomHash;
    private System.Random rnd;
    private System.DateTime lastTimeGenerated;

    private PrefabsStorage prefabs;

    private GameObject player;
    private int asteroidsCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        randomHash = this.GetInstanceID();
        rnd = new System.Random(randomHash);
        lastTimeGenerated = System.DateTime.Now;
        player = GameObject.Find("Player");
        prefabs = GameObject.Find("PrefabStorage").GetComponent<PrefabsStorage>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (genType == "AsteroidONLY" && asteroidsCount <= 30 )
        {
            asteroidsCount += 1;
            
            Vector3 vectorToSpawn = player.GetComponent<GravitationalBody>().GetMovementVector().normalized + (rnd.Next(0, 2) == 1 ? -1 : 1) * new Vector3(rnd.Next(), rnd.Next(), 0).normalized / 2;

            SpawnAsteroid(vectorToSpawn);
        }
        
    }


    /// <summary>
    /// берем вектор-направление игрока и спавним в том направлении
    /// </summary>
    public void Spawn()
    {
        
        Vector3 vectorToSpawn = player.GetComponent<GravitationalBody>().GetMovementVector().normalized + (rnd.Next(0,2) == 1 ? -1: 1) *  new Vector3(rnd.Next(), rnd.Next(), 0).normalized / 2;

        string[] objectsToSpawn = (Constants.LegalToSpawn[player.GetComponent<GravitationalBody>().name]);
        string objectToSpawn = objectsToSpawn[rnd.Next(0, objectsToSpawn.Length)];
        if (System.Math.Pow(vectorToSpawn.x - player.transform.position.x, 2) + System.Math.Pow(vectorToSpawn.y - player.transform.position.y, 2) >= System.Math.Pow(Constants.DistanceToGenerateObjects, 2))
        {
            switch (objectToSpawn)
            {
                case "Asteroid":
                    Debug.Log("Spawning Asteroid");
                    SpawnAsteroid(vectorToSpawn);
                    break;
                case "DwarfPlanet":
                    Debug.Log("Spawning DwarfPlanet");
                    SpawnDwarfPlanet(vectorToSpawn);
                    break;
                case "Planet":
                    Debug.Log("Spawning Planet");
                    SpawnPlanet(vectorToSpawn);
                    break;
                case "DwarfStar":
                    Debug.Log("Spawning DwarfStar");
                    SpawnDwarfStar(vectorToSpawn);
                    break;
                case "Star":
                    Debug.Log("Spawning Star");
                    SpawnStar(vectorToSpawn);
                    break;
                case "GiantStar":
                    Debug.Log("Spawning GiantStar");
                    SpawnGiantStar(vectorToSpawn);
                    break;
                case "NeutronStar":
                    Debug.Log("Spawning NeutronStar");
                    SpawnNeutronStar(vectorToSpawn);
                    break;
                case "BlackHole":
                    Debug.Log("Spawning BlackHole");
                    SpawnBlackHole(vectorToSpawn);
                    break;
                default:
                    Debug.Log("WRONG CASE SWITCH");
                    break;
            }
        }
        
    }

    void SpawnPlanets(Vector3 vc) 
    {

        // получаем рандомную планету из префабов
        
        Object planetPrefab = prefabs.planets[rnd.Next(0, prefabs.planets.Length)] as GameObject;

        // позиция игрока
        var Pos = player.GetComponent<Transform>().position + vc * Constants.DistanceToGenerateObjects;

        // инициализируем планету в игре
        var newplanet = Instantiate(planetPrefab, Pos, this.transform.rotation) as GameObject;
        newplanet.tag = "Planet";

        // добавляем гравитацию ей 
        var gvBody = newplanet.AddComponent<GravitationalBody>();
        gvBody.name = "Planet";

        // меняем ее размер, чтобы он соответствовал ее типу
        newplanet.transform.localScale *= Constants.PlanetScale;

        var rotator = newplanet.AddComponent<Rotatator>();
        rotator.Randomize_rotation();


    }

    void SpawnAsteroid(Vector3 vc)
    {

        Object asteroidPrefab = prefabs.asteroids[rnd.Next(0, prefabs.asteroids.Length)] as GameObject;

        var Pos = player.GetComponent<Transform>().position + vc * Constants.DistanceToGenerateObjects;
        var newplanet = Instantiate(asteroidPrefab, Pos, this.transform.rotation) as GameObject;
        newplanet.tag = "Asteroid";
        newplanet.AddComponent<GravitationalBody>();
        newplanet.GetComponent<GravitationalBody>().name = "Asteroid";
        newplanet.transform.localScale = new Vector3(Constants.AsteroidScale, Constants.AsteroidScale, Constants.AsteroidScale); // УБРАЛ УМНОЖИТЬ РАВНО


        newplanet.GetComponent<GravitationalBody>().StartingMass = Constants.AsteroidStartingMass + 3;
        newplanet.GetComponent<GravitationalBody>().ImaginaryMass = Constants.AsteroidCriticalMass;
        newplanet.GetComponent<Rigidbody2D>().mass = newplanet.GetComponent<GravitationalBody>().StartingMass;

        Material[] materials = new Material[] { prefabs.asteroidsMaterials[rnd.Next(0, prefabs.asteroidsMaterials.Length)] };
        
        newplanet.GetComponent<MeshRenderer>().materials = materials;


    }

    void SpawnDwarfPlanet(Vector3 vc)
    {
        Object planetPrefab = prefabs.planets[rnd.Next(0, prefabs.planets.Length)] as GameObject;

        var Pos = player.GetComponent<Transform>().position + vc * Constants.DistanceToGenerateObjects;
        var newplanet = Instantiate(planetPrefab, Pos, this.transform.rotation) as GameObject;
        newplanet.tag = "DwarfPlanet";
        newplanet.AddComponent<GravitationalBody>();
        newplanet.AddComponent<Rotatator>();
        newplanet.GetComponent<GravitationalBody>().name = "DwarfPlanet";

        newplanet.GetComponent<GravitationalBody>().StartingMass = Constants.DwarfPlanetsStartingMass + 3;
        newplanet.GetComponent<GravitationalBody>().ImaginaryMass = Constants.DwarfPlanetCriticalMass;
        newplanet.GetComponent<Rigidbody2D>().mass = newplanet.GetComponent<GravitationalBody>().StartingMass;

        newplanet.transform.localScale = new Vector3(Constants.PlanetScale, Constants.PlanetScale, Constants.PlanetScale); // УБРАЛ УМНОЖИТЬ РАВНО

    }

    void SpawnPlanet(Vector3 vc)
    {
        Object planetPrefab = prefabs.planets[rnd.Next(0, prefabs.planets.Length)] as GameObject;

        var Pos = player.GetComponent<Transform>().position + vc * Constants.DistanceToGenerateObjects;
        var newplanet = Instantiate(planetPrefab, Pos, this.transform.rotation) as GameObject;
        newplanet.tag = "Planet";
        newplanet.AddComponent<GravitationalBody>();
        newplanet.GetComponent<GravitationalBody>().name = "Planet";

        newplanet.GetComponent<GravitationalBody>().StartingMass = Constants.PlanetsStartingMass + 3;
        newplanet.GetComponent<GravitationalBody>().ImaginaryMass = Constants.PlanetCriticalMass;
        newplanet.GetComponent<Rigidbody2D>().mass = newplanet.GetComponent<GravitationalBody>().StartingMass;

        newplanet.transform.localScale = new Vector3(Constants.PlanetScale, Constants.PlanetScale, Constants.PlanetScale); // УБРАЛ УМНОЖИТЬ РАВНО
    }

    void SpawnDwarfStar(Vector3 vc)
    {
        Object dwarfStarsPrefab = prefabs.stars[rnd.Next(0, prefabs.stars.Length)] as GameObject;

        var Pos = player.GetComponent<Transform>().position + vc * Constants.DistanceToGenerateObjects;
        var newplanet = Instantiate(dwarfStarsPrefab, Pos, this.transform.rotation) as GameObject;
        newplanet.tag = "DwarfStar";
        newplanet.AddComponent<GravitationalBody>();
        newplanet.GetComponent<GravitationalBody>().name = "DwarfStar";

        newplanet.GetComponent<GravitationalBody>().StartingMass = Constants.DwarfStarStartingMass + 3;
        newplanet.GetComponent<GravitationalBody>().ImaginaryMass = Constants.DwarfStarCriticalMass;
        newplanet.GetComponent<Rigidbody2D>().mass = newplanet.GetComponent<GravitationalBody>().StartingMass;

        newplanet.transform.localScale = new Vector3(Constants.DwarfStarScale, Constants.DwarfStarScale, Constants.DwarfStarScale); // УБРАЛ УМНОЖИТЬ РАВНО
    }

    void SpawnStar(Vector3 vc)
    {
        Object StarsPrefab = prefabs.stars[rnd.Next(0, prefabs.stars.Length)] as GameObject;

        var Pos = player.GetComponent<Transform>().position + vc * Constants.DistanceToGenerateObjects;
        var newplanet = Instantiate(StarsPrefab, Pos, this.transform.rotation) as GameObject;
        newplanet.tag = "Star";
        newplanet.AddComponent<GravitationalBody>();
        newplanet.GetComponent<GravitationalBody>().name = "Star";

        newplanet.GetComponent<GravitationalBody>().StartingMass = Constants.StarStartingMass + 3;
        newplanet.GetComponent<GravitationalBody>().ImaginaryMass = Constants.StarCriticalMass;
        newplanet.GetComponent<Rigidbody2D>().mass = newplanet.GetComponent<GravitationalBody>().StartingMass;

        newplanet.transform.localScale = new Vector3(Constants.StarScale, Constants.StarScale, Constants.StarScale); // УБРАЛ УМНОЖИТЬ РАВНО
    }

    void SpawnGiantStar(Vector3 vc)
    {
        Object giantStarsPrefab = prefabs.stars[rnd.Next(0, prefabs.stars.Length)] as GameObject;

        var Pos = player.GetComponent<Transform>().position + vc * Constants.DistanceToGenerateObjects;
        var newplanet = Instantiate(giantStarsPrefab, Pos, this.transform.rotation) as GameObject;
        newplanet.tag = "GiantStar";
        newplanet.AddComponent<GravitationalBody>();
        newplanet.GetComponent<GravitationalBody>().name = "GiantStar";

        newplanet.GetComponent<GravitationalBody>().StartingMass = Constants.GiantStarStartingMass + 3;
        newplanet.GetComponent<GravitationalBody>().ImaginaryMass = Constants.GiantStarCriticalMass;
        newplanet.GetComponent<Rigidbody2D>().mass = newplanet.GetComponent<GravitationalBody>().StartingMass;

        newplanet.transform.localScale = new Vector3(Constants.GiantStarScale, Constants.GiantStarScale, Constants.GiantStarScale); // УБРАЛ УМНОЖИТЬ РАВНО
    }

    void SpawnNeutronStar(Vector3 vc)
    {
        Object neutronStarsPrefab = prefabs.stars[rnd.Next(0, prefabs.stars.Length)] as GameObject;

        var Pos = player.GetComponent<Transform>().position + vc * Constants.DistanceToGenerateObjects;
        var newplanet = Instantiate(neutronStarsPrefab, Pos, this.transform.rotation) as GameObject;
        newplanet.tag = "NeutronStar";
        newplanet.AddComponent<GravitationalBody>();
        newplanet.GetComponent<GravitationalBody>().name = "NeutronStar";

        newplanet.GetComponent<GravitationalBody>().StartingMass = Constants.NeutronStarStartingMass + 3;
        newplanet.GetComponent<GravitationalBody>().ImaginaryMass = Constants.NeutronStarCriticalMass;
        newplanet.GetComponent<Rigidbody2D>().mass = newplanet.GetComponent<GravitationalBody>().StartingMass;

        newplanet.transform.localScale = new Vector3(Constants.NeutronStarScale, Constants.NeutronStarScale, Constants.NeutronStarScale); // УБРАЛ УМНОЖИТЬ РАВНО
    }

    void SpawnBlackHole(Vector3 vc)
    {
        Object blackholePrefab = prefabs.BlackHole[rnd.Next(0, prefabs.BlackHole.Length)] as GameObject;

        var Pos = player.GetComponent<Transform>().position + vc * Constants.DistanceToGenerateObjects;
        var newplanet = Instantiate(blackholePrefab, Pos, this.transform.rotation) as GameObject;
        newplanet.tag = "BlackHole";
        newplanet.AddComponent<GravitationalBody>();
        newplanet.GetComponent<GravitationalBody>().name = "BlackHole";

        newplanet.GetComponent<GravitationalBody>().StartingMass = Constants.BlackHoleStartingMass + 3;
        newplanet.GetComponent<GravitationalBody>().ImaginaryMass = Constants.BlackHoleCriticalMass;
        newplanet.GetComponent<Rigidbody2D>().mass = newplanet.GetComponent<GravitationalBody>().StartingMass;

        newplanet.transform.localScale = new Vector3(Constants.BlackHoleScale, Constants.BlackHoleScale, Constants.BlackHoleScale); // УБРАЛ УМНОЖИТЬ РАВНО
    }

    public void SpawnAsteroidConstantPosition(Vector3 vc)
    {

        Object asteroidPrefab = prefabs.asteroids[rnd.Next(0, prefabs.asteroids.Length)] as GameObject;

       
        var newplanet = Instantiate(asteroidPrefab, vc, this.transform.rotation) as GameObject;
        newplanet.tag = "Asteroid";
        newplanet.AddComponent<GravitationalBody>();
        newplanet.GetComponent<GravitationalBody>().name = "Asteroid";
        newplanet.transform.localScale *= Constants.AsteroidScale;

        Material[] materials = new Material[] { prefabs.asteroidsMaterials[rnd.Next(0, prefabs.asteroidsMaterials.Length)] };

        newplanet.GetComponent<MeshRenderer>().materials = materials;
    }
}

