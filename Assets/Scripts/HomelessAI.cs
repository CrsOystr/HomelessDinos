using UnityEngine;
using System.Collections;

public class HomelessAI : MonoBehaviour 
{

	public Vector2 gridPosition;
	public ISOGRID gridScript;

	public Vector2 previousTile;

	int hungerLevel;

	float waitTime = 3.0f;
	float currentTime = 0.0f;

	/*
	float walkSpeed = 1.0f;

	float rightBound = 5.0f;
	float leftBound = -5.0f;

	float direction = 1.0f;
	Vector3 amount;
	*/

	// Use this for initialization
	void Start () 
	{
		hungerLevel = Random.Range(0,3);
		previousTile.x = gridPosition.x;
		previousTile.y = gridPosition.y;
	}
	
	// Update is called once per frame
	void Update () 
	{
		/*
		amount.x = direction * walkSpeed * Time.deltaTime;
		
		if (direction > 0.0f && transform.position.x >= rightBound)
			direction = -1.0f;
		else if (direction < 0.0f && transform.position.x <= leftBound)
			direction = 1.0f;
		
		transform.Translate(amount);
		*/
		// delay actions
		currentTime += Time.deltaTime;
		if (currentTime >= waitTime)
		{
			//check nearby tiles for free need

			//check nearby tiles for unwalked path

			//walk previously walked path
		}



	}
}
