using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotatator : MonoBehaviour {
	Vector3 rotation;
	Transform meshObject = null;
	float rotationSpeed;
	bool randomize = true;
	
	public bool Randomize 
	
	{
		get {
			return randomize;
		}
	}
	
	float maxSpeed;
	float minSpeed;

	// Use this for initialization
	void Start () 
	
		{

		if(meshObject == null) 
		
		{
			meshObject = this.gameObject.transform;
			Debug.Log(meshObject);
			if (meshObject == null)
				meshObject = transform.Find(this.name);
		}
		
		

		
		
	}
	public void Randomize_rotation()
	{

		rotation = new Vector3(RandFloat(), RandFloat(), RandFloat());
		minSpeed = Constants.minSpeed;
		maxSpeed = Constants.maxSpeed;
		rotationSpeed = Random.Range(minSpeed, maxSpeed);
	}

	float RandFloat() 
	
	{
		return Random.Range(0f,1.01f);
	}
	
	// Update is called once per frame
	void FixedUpdate() 
	
	{
		if (meshObject != null)
			meshObject.Rotate(rotation, rotationSpeed * Time.deltaTime);
		else 
		{
			Debug.Log("Mesh is nil");
		}
	}
}
