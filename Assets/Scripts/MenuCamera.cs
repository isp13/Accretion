using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    public Vector3 Targetposition;
    
    public bool camera_move_enabled = true;

    void Update()
    {

        if (camera_move_enabled)
        {
            this.transform.position = Vector3.Lerp(transform.position, Targetposition, 0.1f * Time.deltaTime);
        }

    }


}
