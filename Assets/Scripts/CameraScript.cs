using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float FollowSpeed = 2f;
    public Transform Target;
    public int targetZPostition = -10;
    private void FixedUpdate()
    {
        Vector3 newPosition = Target.position;
        newPosition.z = targetZPostition;
        transform.position = Vector3.Slerp(transform.position, newPosition, FollowSpeed * Time.deltaTime);
    }
}
