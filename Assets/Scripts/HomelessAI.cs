using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HomelessAI : MonoBehaviour 
{

	public Vector2 gridPosition;
	public ISOGRID gridScript;

	public Vector2 previousTile;

	public int hungerLevel;

	float waitTime = 3.0f;
	float currentTime = 0.0f;

	Vector3 startLerp;
	Vector3 endLerp;
	//float lerpTime = 3.0f;
	float lerpPos = 1.0f;

	bool usingObject = false;
	GameObject objectInUse;

	GameObject pressingNeed;
	bool icon = true;

	public GameObject iconFood;

	// Use this for initialization
	void Start () 
	{
		hungerLevel = Random.Range(1,3);
		previousTile.x = gridPosition.x;
		previousTile.y = gridPosition.y;
		waitTime = Random.Range(2.0f,3.0f);

		pressingNeed = Instantiate(iconFood, new Vector3 (transform.position.x, transform.position.y+2.3f, 0), Quaternion.identity) as GameObject; 
		pressingNeed.transform.parent = transform;
	}
	
	// Update is called once per frame
	void Update () 
	{

		if(hungerLevel == 0 && icon == true)
		{
			Destroy(this.pressingNeed);
			icon = false;

		}

		if(usingObject)
		{
			if (!objectInUse.GetComponent<needObjectScript>().inUse)
			{
				hungerLevel--;
				usingObject = false;
			}
		}
		else
		{

			// transition between tiles
			if (lerpPos < 1.0f)
			{
				lerpPos += Time.deltaTime/waitTime;
				transform.position = Vector3.Lerp(startLerp,endLerp,lerpPos);
	        }

			// look for need, look for path
			mainCycle();

		}


	}

	void mainCycle()
	{
		// delay actions
		currentTime += Time.deltaTime;
		if (currentTime >= waitTime)
		{
			// if hungry, find food
			List<Vector2> objList = gridScript.nearbyNeeds((int)gridPosition.x,(int)gridPosition.y, "food");
			foreach(Vector2 obj in objList)
			{
				if(hungerLevel != 0 && !usingObject)
				{
					//check nearby tiles for free need
					//use this item
					objectInUse = gridScript.Grid[(int)obj.x,(int)obj.y].GetComponent<TileScript>().currentObject;
					objectInUse.GetComponent<needObjectScript>().inUse = true;
					usingObject = true;
					break;
				}
			}
			// if sleepy, find sleep
			
			// if dirty, find hygiene
			
			// if sick, find medical

			//check nearby tiles for unwalked path
			List<Vector2> pathList = gridScript.nearbyPaths((int)gridPosition.x,(int)gridPosition.y);
			foreach(Vector2 tile in pathList)
			{
				if (tile.x == previousTile.x && tile.y == previousTile.y)
				{
				}
				else
				{
					// move here
                    moveToTile((int)tile.x,(int)tile.y);
                    
                    currentTime = 0.0f;
                    // MIGHT NEED TO REWORK RETURN
                    return;
                }
            }
            //walk previously walked path
            moveToTile ((int)previousTile.x,(int)previousTile.y);
            
            currentTime = 0.0f;
		}
	}

	void moveToTile(int x, int y)
	{
		// move here
		previousTile.x = gridPosition.x;
		previousTile.y = gridPosition.y;
		//transform.position = new Vector3(gridScript.Grid[(int)tile.x,(int)tile.y].transform.position.x, gridScript.Grid[(int)tile.x,(int)tile.y].transform.position.y,transform.position.z);
		startLerp = transform.position;
		endLerp = new Vector3(gridScript.Grid[x,y].transform.position.x, gridScript.Grid[x,y].transform.position.y,transform.position.z);
		gridPosition.x = x;
		gridPosition.y = y;
		renderer.sortingOrder = 10000-((int)gridPosition.x + (int)gridPosition.y);
        lerpPos = 0.0f;
    }
}
