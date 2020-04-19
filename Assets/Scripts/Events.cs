using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Events : MonoBehaviour
{
    public delegate void EventHandler();
    public static event EventHandler NotifyMovement;
    private static System.DateTime lastTimeMovingEventFetched;
    private static Random rnd = new Random();
    // Start is called before the first frame update
    void Start()
    {
        // adding new methods that can be invoked when player is moving
        NotifyMovement += StartSpawning;
        lastTimeMovingEventFetched = System.DateTime.Now;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void InvokeNotifyMovement()
    {
        int tmp = Random.Range(Constants.LowerSecondsGenPlanet, Constants.UpperSecondsGenPlanet);
        
        Debug.Log(tmp);


        NotifyMovement?.Invoke();
        
        lastTimeMovingEventFetched = System.DateTime.Now;
        
        
        
    }
    public static void StartSpawning() {
        Debug.Log("started spawning");
        GameObject.Find("AsteroidGenerator").GetComponent<Spawner>().Spawn();
    }
}

