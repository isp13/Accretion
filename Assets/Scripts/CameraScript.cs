using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float FollowSpeed = 2f;
    public Transform Target;
    public int targetZPostition = -10;

    public float Zoom1 = 1f;
    public float Zoom2 = 2f;

    public float duration = 1.0f;
    private float elapsed = 0.0f;
    private bool transition = true;


    private void Start()
    {
        this.transform.position = new Vector3(0, 0, 0);
    }

    private void FixedUpdate()
    {
        Vector3 newPosition = Target.position;
        newPosition.z = targetZPostition;
        transform.position = Vector3.Slerp(transform.position, newPosition, FollowSpeed * Time.deltaTime);


        if (transition && this.tag == "MainCamera")
        {
            elapsed += Time.deltaTime / duration;
            this.GetComponent<Camera>().orthographicSize = Mathf.Lerp(Zoom1, Zoom2, elapsed);
            //this next line i'm not sure of, I'm not familiar with CameraMovement.ypos
           // Camera.main.GetComponent<CameraMovement>().ypos = Mathf.Lerp(ypos1, ypos2, elapsed);
            if (elapsed > 1.0f)
            {
                transition = false;
                Zoom1 = Zoom2;
            }
        }
    }


    public void SmoothChangeOrthographicSize(float max)
    {
        Zoom2 = max;
        transition = true;
        elapsed = 0.0f;
    }
}
