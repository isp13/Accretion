using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitManager : MonoBehaviour
{
    //private int FalseEntries = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    //When the Primitive collides with the walls, it will reverse direction
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        //Debug.Log("ONTRIGGER ENTER");
        StartCoroutine("FlashesBeforeSomething");
        GameObject.Find("OrbitRenderer").GetComponent<LineRenderer>().enabled = true;
        
    }

    //When the Primitive exits the collision, it will change Color
    private void OnTriggerExit2D(Collider2D other)
    {
        //Debug.Log("ONTRIGGER EXIT");
        //StartCoroutine("FlashesBeforeSomething");

        GameObject.Find("OrbitRenderer").GetComponent<LineRenderer>().enabled = false;
        
    }

    IEnumerator FlashesBeforeSomething()
    {
        int counter = 0;
        while (counter !=8) // this just equates to "repeat forever"
        {
            
            counter += 1;
            yield return new WaitForSeconds(0.3f); // "pauses" for 2 seconds.. note, the actual game doesn't pause..
            //Debug.Log("IEnumerator");
            GameObject.Find("OrbitRenderer").GetComponent<LineRenderer>().enabled = counter % 2 != 0;
        }
    }
}
