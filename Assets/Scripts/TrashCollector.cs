using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCollector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    

    void OnCollisionEnter2D(Collision2D collision)
    {

        
        Destroy(collision.gameObject);


        Debug.Log("deleted");


    }
    
}
