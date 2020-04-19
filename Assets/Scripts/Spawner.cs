using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    List<string> prefabsFolderPlanets = new List<string>();
    List<string> prefabsFolderAsteroids = new List<string>();

    public string genType = "Asteroid";
    private int randomHash;
    private System.Random rnd;
    private System.DateTime lastTimeGenerated;
    
    // Start is called before the first frame update
    void Start()
    {
        randomHash = this.GetInstanceID();
        rnd = new System.Random(randomHash);
        lastTimeGenerated = System.DateTime.Now;
        LoadPrefabsFolders();

        

    }

    // Update is called once per frame
    void Update()
    { 
    }

    void LoadPrefabsFolders() 
    {
        foreach (string dirFile in Directory.GetDirectories(Constants.PlanetPrefabsFolder))
        {
            foreach (string fileName in Directory.GetFiles(dirFile))
            {
                if (!fileName.Contains(".meta"))
                    prefabsFolderPlanets.Add(fileName);
            }
        }

        foreach (string dirFile in Directory.GetFiles(Constants.AsteroidPrefabsFolder))
        {
            if (!dirFile.Contains(".meta"))
                prefabsFolderAsteroids.Add(dirFile);
        }
    }

    public void Spawn()
    {
       Vector3 vectorToSpawn = GameObject.Find("Player").GetComponent<GravitationalBody>().GetMovementVector().normalized + (rnd.Next(0,2) == 1 ? -1: 1) *  new Vector3(rnd.Next(), rnd.Next(), 0).normalized / 2;

        if (rnd.Next(0,2) == 0)
            SpawnAsteroid(vectorToSpawn);
        else SpawnPlanets(vectorToSpawn);
    }

    void SpawnPlanets(Vector3 vc) 
    {

        //Debug.Log(prefabsFolder[0]);
        // Set up instantiate
        string newplanetName = prefabsFolderPlanets[rnd.Next(0, prefabsFolderPlanets.Count)];
        Debug.Log(newplanetName);
        Object planetPrefab = AssetDatabase.LoadAssetAtPath(newplanetName, typeof(GameObject));
        var Pos = GameObject.Find("Player").GetComponent<Transform>().position + vc * Constants.DistanceToGenerateObjects;
        var newplanet = Instantiate(planetPrefab, Pos, this.transform.rotation) as GameObject;
        newplanet.tag = "Planet";
        newplanet.AddComponent<GravitationalBody>();
        newplanet.transform.localScale *= Constants.PlanetScale;

        //newplanet.AddComponent<Rotatator>();

    }

    void SpawnAsteroid(Vector3 vc)
    {
        Object asteroidPrefab = AssetDatabase.LoadAssetAtPath(prefabsFolderAsteroids[rnd.Next(0, prefabsFolderAsteroids.Count)], typeof(GameObject));
        Debug.Log(this.transform.position);
        Debug.Log(vc);
        var Pos = GameObject.Find("Player").GetComponent<Transform>().position + vc * Constants.DistanceToGenerateObjects;
        var newplanet = Instantiate(asteroidPrefab, Pos, this.transform.rotation) as GameObject;
        newplanet.tag = "Asteroid";
        newplanet.AddComponent<GravitationalBody>();
        newplanet.transform.localScale *= Constants.AsteroidScale;
    }
}

