using UnityEngine;
using System.Collections;

public class HomelessAI : MonoBehaviour 
{

	float walkSpeed = 1.0f;

	float rightBound = 5.0f;
	float leftBound = -5.0f;

	float direction = 1.0f;
	Vector3 amount;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		amount.x = direction * walkSpeed * Time.deltaTime;
		
		if (direction > 0.0f && transform.position.x >= rightBound)
			direction = -1.0f;
		else if (direction < 0.0f && transform.position.x <= leftBound)
			direction = 1.0f;
		
		transform.Translate(amount);
	}
}
