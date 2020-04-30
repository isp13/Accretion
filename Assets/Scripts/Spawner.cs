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
        
        if (genType == "AsteroidONLY" && asteroidsCount <= 50 )
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
                    SpawnAsteroid(vectorToSpawn);
                    break;
                case "Planet":
                    Debug.Log("Spawning Planet");
                    SpawnAsteroid(vectorToSpawn);
                    break;
                case "DwarfStar":
                    Debug.Log("Spawning DwarfStar");
                    SpawnAsteroid(vectorToSpawn);
                    break;
                case "Star":
                    Debug.Log("Spawning Star");
                    SpawnAsteroid(vectorToSpawn);
                    break;
                case "GiantStar":
                    Debug.Log("Spawning GiantStar");
                    SpawnAsteroid(vectorToSpawn);
                    break;
                case "NeutronStar":
                    Debug.Log("Spawning NeutronStar");
                    SpawnAsteroid(vectorToSpawn);
                    break;
                case "BlackHole":
                    Debug.Log("Spawning BlackHole");
                    SpawnAsteroid(vectorToSpawn);
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

        Material[] materials = new Material[] { prefabs.asteroidsMaterials[rnd.Next(0, prefabs.asteroidsMaterials.Length)] };
        
        newplanet.GetComponent<MeshRenderer>().materials = materials;
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

