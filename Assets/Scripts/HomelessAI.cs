using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
			List<Vector2> pathList = gridScript.nearbyPaths((int)gridPosition.x,(int)gridPosition.y);
			foreach(Vector2 tile in pathList)
			{
				//print (tile.x);
				//print (tile.y);
				//print (gridPosition.x);
				//print (gridPosition.y);
				//print ("got here");
				if (tile.x == previousTile.x && tile.y == previousTile.y)
				{
				}
				else
				{
					// move here
					previousTile.x = gridPosition.x;
					previousTile.y = gridPosition.y;
					transform.position = new Vector3(gridScript.Grid[(int)tile.x,(int)tile.y].transform.position.x, gridScript.Grid[(int)tile.x,(int)tile.y].transform.position.y,transform.position.z);

					gridPosition.x = tile.x;
					gridPosition.y = tile.y;

					currentTime = 0.0f;
					// MIGHT NEED TO REWORK RETURN
					return;
				}
			}
			//walk previously walked path


			currentTime = 0.0f;
		}



	}
}
