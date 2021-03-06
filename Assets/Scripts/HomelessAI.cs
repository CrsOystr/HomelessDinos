﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HomelessAI : MonoBehaviour 
{
	public ISOGRID gridScript;
	private Animator animator;
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
		animator = gameObject.GetComponent<Animator>();
		animator.speed = 0.2f;
		repScript = GameObject.Find("ScoreKeeper").GetComponent<scoreScript>();

		// All this is for adjusting the difficulty of the game
		needsLevel = new int[4] {0, 0, 0, 0};
		needsLevel[0] = Random.Range(0,2 + repScript.day);
		if (repScript.day > 1)
		{
			needsLevel[1] = Random.Range(0,2 + repScript.day);
		}
		if (repScript.day > 2)
		{
			needsLevel[2] = Random.Range(0,2 + repScript.day);
		}
		if (repScript.day > 3)
		{
			needsLevel[3] = Random.Range(0,2 + repScript.day);
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
		waitTime = Random.Range(Mathf.Max(2.0f-((float)repScript.reputation*0.05f),0.3f),
		                        Mathf.Max(3.0f-((float)repScript.reputation*0.01f),0.9f));

		previousTile.x = gridPosition.x;
		previousTile.y = gridPosition.y;


		pressingNeed = Instantiate(iconFood, new Vector3 (transform.position.x, transform.position.y+2.3f, 0), Quaternion.identity) as GameObject; 
		pressingNeed.transform.parent = transform;
	}
	
	// Update is called once per frame
	void Update () 
	{
		// destroy icon if no more needs
		if(Mathf.Max(needsLevel) == 0 && icon == true)
		{
			Destroy(this.pressingNeed);
			icon = false;

		}
		// update need icon and what need to satisfy
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
			pressingNeed.GetComponent<SpriteRenderer>().sprite = pressingNeed.GetComponent<switchNeedIcon>().needsSprite[need];
			currentNeed = need;
		}

		// speed up hobo if all needs are met
		if (currentHoboDifficulty <= 0)
			waitTime = 0.9f;

		if(usingObject)
		{
			animator.speed = 0.0f;
			if(objectInUse == null)
			{
				usingObject = false;
			}
			else if (!objectInUse.GetComponent<needObjectScript>().inUse)
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
			animator.speed = 0.3f;
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
			// food hygiene health sleep
			// if hungry, find food
			if(currentNeed == 0 && needsLevel[0] != 0)
			{
				satisfyNeed("food",0);
			}
			// if dirty, find hygiene
			else if(currentNeed == 1 && needsLevel[1] != 0)
			{
				satisfyNeed("hygiene",1);
			}
			// if sick, find medical
			else if(currentNeed == 2 && needsLevel[2] != 0)
			{
				satisfyNeed("health",2);
			}
			// if sleepy, find sleep
			else if(currentNeed == 3 && needsLevel[3] != 0)
			{
				satisfyNeed("sleep",3);
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

	void satisfyNeed(string need, int needIndex)
	{
		if (!usingObject)
		{
			List<Vector2> objList = gridScript.nearbyNeeds((int)gridPosition.x,(int)gridPosition.y, need);
			foreach(Vector2 obj in objList)
			{
				
				//check nearby tiles for free need
				//use this item
				objectInUse = gridScript.Grid[(int)obj.x,(int)obj.y].GetComponent<TileScript>().currentObject;
				objectInUse.GetComponent<needObjectScript>().inUse = true;
				usingObject = true;
				//currentNeed = needIndex;
				break;
			}
		}
	}

	void moveToTile(int x, int y)
	{
		// move here
		previousTile.x = gridPosition.x;
		previousTile.y = gridPosition.y;
		//transform.position = new Vector3(gridScript.Grid[(int)tile.x,(int)tile.y].transform.position.x, gridScript.Grid[(int)tile.x,(int)tile.y].transform.position.y,transform.position.z);
		startLerp = transform.position;
		// creates some randomness in the ending point
		endLerp = new Vector3(gridScript.Grid[x,y].transform.position.x+Random.Range(-0.3f, 0.3f), gridScript.Grid[x,y].transform.position.y+Random.Range(-0.3f, 0.1f),transform.position.z);
		gridPosition.x = x;
		gridPosition.y = y;
		renderer.sortingOrder = 10000-((int)gridPosition.x + (int)gridPosition.y);
        lerpPos = 0.0f;
		if ((gridPosition.x - previousTile.x) > 0 && (previousTile.y == gridPosition.y)) {
			animator.SetInteger ("Direction", 1);
		} else if ((gridPosition.x - previousTile.x) < 0 && (previousTile.y == gridPosition.y)) {
			animator.SetInteger ("Direction", 3);
		} else if ((gridPosition.x == previousTile.x) && 0 < (gridPosition.y - previousTile.y)) {
			animator.SetInteger ("Direction", 2);
		} else if ((gridPosition.x == previousTile.x) && 0 > (gridPosition.y - previousTile.y)) {
			animator.SetInteger ("Direction", 4);
		}

    }
}
