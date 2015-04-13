using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HomelessAI : MonoBehaviour 
{
	public ISOGRID gridScript;

	public Vector2 gridPosition;
	public Vector2 previousTile;

	// food hygiene health sleep
	public int[] needsLevel;
	// need currently fullfilling
	int currentNeed = 0;

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

	public int difficulty;

	scoreScript repScript;
	public int startHoboDifficulty;
	public int currentHoboDifficulty;

	// Use this for initialization
	void Start () 
	{
		repScript = GameObject.Find("ScoreKeeper").GetComponent<scoreScript>();

		// All this is for adjusting the difficulty of the game
		needsLevel = new int[4] {0, 0, 0, 0};
		needsLevel[0] = Random.Range(0,2 + repScript.reputation / 20);
		if (difficulty > 20)
		{
			needsLevel[1] = Random.Range(0,2 + repScript.reputation / 20);
		}
		if (difficulty > 30)
		{
			needsLevel[2] = Random.Range(0,2 + repScript.reputation / 20);
		}
		if (difficulty > 60)
		{
			needsLevel[3] = Random.Range(0,2 + repScript.reputation / 20);
		}

		// represents the overall difficulty of a hobo on spawn, used for scoring.
		startHoboDifficulty = needsLevel[0] + needsLevel[1] + needsLevel[2] + needsLevel[3];

		// never have hobos with no needs.
		if (startHoboDifficulty == 0)
		{
			needsLevel[0]++;
			startHoboDifficulty++;
		}
		currentHoboDifficulty = startHoboDifficulty;

		// this is how fast the hobo will move. tweak based on difficulty
		// Mathf.Max is so that if the difficulty becomes very large, we won't get negative speeds
		waitTime = Random.Range(Mathf.Max(2.0f-((float)difficulty*0.005f),0.3f),
		                        Mathf.Max(3.0f-((float)difficulty*0.001f),0.9f));

		previousTile.x = gridPosition.x;
		previousTile.y = gridPosition.y;


		pressingNeed = Instantiate(iconFood, new Vector3 (transform.position.x, transform.position.y+2.3f, 0), Quaternion.identity) as GameObject; 
		pressingNeed.transform.parent = transform;
	}
	
	// Update is called once per frame
	void Update () 
	{

		if(Mathf.Max(needsLevel) == 0 && icon == true)
		{
			Destroy(this.pressingNeed);
			icon = false;

		}
		// update need icon
		if (icon)
		{
			int max = 0;
			int need = 0;
			for(int i = 0; i < 4; i++)
			{
				if (needsLevel[i] > max)
				{
					need = i;
					max = needsLevel[i];
				}
			}
			// change sprite
			//pressingNeed.GetComponent<switchNeedIcon>().gameObject
			pressingNeed.GetComponent<SpriteRenderer>().sprite = pressingNeed.GetComponent<switchNeedIcon>().needsSprite[need];
		}

		// speed up hobo if all needs are met
		if (currentHoboDifficulty <= 0)
			waitTime = 0.9f;


		if(usingObject)
		{
			if (!objectInUse.GetComponent<needObjectScript>().inUse)
			{
				//hungerLevel--;
				needsLevel[currentNeed]--;
				repScript.currency += 150;
				currentHoboDifficulty --;
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
			if(needsLevel[0] != 0 && !usingObject)
			{
				List<Vector2> objList = gridScript.nearbyNeeds((int)gridPosition.x,(int)gridPosition.y, "food");
				foreach(Vector2 obj in objList)
				{

						//check nearby tiles for free need
						//use this item
						objectInUse = gridScript.Grid[(int)obj.x,(int)obj.y].GetComponent<TileScript>().currentObject;
						objectInUse.GetComponent<needObjectScript>().inUse = true;
						usingObject = true;
						currentNeed = 0;
						break;
				}
			}
			// if dirty, find hygiene
			if(needsLevel[1] != 0 && !usingObject)
			{
				List<Vector2> objList = gridScript.nearbyNeeds((int)gridPosition.x,(int)gridPosition.y, "hygiene");
				foreach(Vector2 obj in objList)
				{
					
					//check nearby tiles for free need
					//use this item
					objectInUse = gridScript.Grid[(int)obj.x,(int)obj.y].GetComponent<TileScript>().currentObject;
					objectInUse.GetComponent<needObjectScript>().inUse = true;
					usingObject = true;
					currentNeed = 1;
					break;
				}
			}
			// if sick, find medical
			if(needsLevel[2] != 0 && !usingObject)
			{
				List<Vector2> objList = gridScript.nearbyNeeds((int)gridPosition.x,(int)gridPosition.y, "health");
				foreach(Vector2 obj in objList)
				{
					
					//check nearby tiles for free need
					//use this item
					objectInUse = gridScript.Grid[(int)obj.x,(int)obj.y].GetComponent<TileScript>().currentObject;
					objectInUse.GetComponent<needObjectScript>().inUse = true;
					usingObject = true;
					currentNeed = 2;
					break;
				}
			}
			// if sleepy, find sleep
			if(needsLevel[3] != 0 && !usingObject)
			{
				List<Vector2> objList = gridScript.nearbyNeeds((int)gridPosition.x,(int)gridPosition.y, "sleep");
				foreach(Vector2 obj in objList)
				{
					
					//check nearby tiles for free need
					//use this item
					objectInUse = gridScript.Grid[(int)obj.x,(int)obj.y].GetComponent<TileScript>().currentObject;
					objectInUse.GetComponent<needObjectScript>().inUse = true;
					usingObject = true;
					currentNeed = 3;
					break;
				}
			}

			//check nearby tiles for unwalked path
			if(!usingObject)
			{
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
			}
            
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
