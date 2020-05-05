using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScollBackground : MonoBehaviour
{
    public float speed = 0.5f;

    Material m_Material;
    GameObject pl;
    // Start is called before the first frame update
    void Start()
    {
        m_Material = GetComponent<Renderer>().material;

        pl = GameObject.Find("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        var vector_velocity = pl.GetComponent<Rigidbody2D>().velocity;
        Vector2 offset = new Vector2(vector_velocity.x,  vector_velocity.y) * speed * -1;

        m_Material.mainTextureOffset += offset * Time.deltaTime;
    }
}
