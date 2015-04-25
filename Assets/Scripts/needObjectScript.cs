using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class needObjectScript : MonoBehaviour {

	public float baseTime;
	int level = 1;
	int volunteer = 0;
	int numberAdjacentObjects = 0;
	public bool inUse = false;
	bool eating = false;
	float needTime = 10.0f;
	float currentTime = 0.0f;

	public int cost;
	public int upgradeCost;
	public int sellPrice;

	public Vector2 gridPosition;
	public ISOGRID gridScript;

	public Sprite[] rotationSprites;

	// used to distinguish between need object types

	public int objectTypeID;

	/*
	 * 0 = Table
	 * 1 = Stove
	 * 2 = Medicine Cabinet
	 * 3 = Washer/Dryer Combo
	 * 4 = Bed
	 * 
	 */

	// Use this for initialization
	void Start () {
		/*
		// When an object is created, check its adjacent tiles for same type objects and update their 
		// numAdjacentObjects appropriately
		List<Vector2> objList = gridScript.nearbyNeeds((int)gridPosition.x,(int)gridPosition.y, this.name);
		foreach(Vector2 obj in objList)
		{
			// increment the number of adjacent objects for both this object and the object in the list
			//obj.numberAdjacentObjects ++;
			this.numberAdjacentObjects ++;
		}
		*/	

		// get variables from parent to initialize
		gridScript = transform.parent.transform.parent.GetComponent<ISOGRID>();
		gridPosition.x = transform.parent.GetComponent<TileScript>().Position.x;
		gridPosition.y = transform.parent.GetComponent<TileScript>().Position.y;


		// update rotation based on nearby path
		if (this.name != "temp")
		{
			RotateToPath();
		}
	}

	public void RotateToPath(int rotation = -1)
	{
		if (objectTypeID != 0)
		{
			if (rotation == -1)
			{
				List<Vector2> pathList = gridScript.nearbyPaths((int)gridPosition.x,(int)gridPosition.y);
				foreach(Vector2 tile in pathList)
				{
					if (tile.x < gridPosition.x)
					{
						GetComponent<SpriteRenderer>().sprite = rotationSprites[1];
						break;
					}
					if (tile.x > gridPosition.x)
					{
						GetComponent<SpriteRenderer>().sprite = rotationSprites[3];
						break;
					}
					if (tile.y < gridPosition.y)
					{
						GetComponent<SpriteRenderer>().sprite = rotationSprites[0];
						break;
					}
					if (tile.y > gridPosition.y)
					{
						GetComponent<SpriteRenderer>().sprite = rotationSprites[2];
						break;
					}
				}
			}
			else if (rotation == 0)
			{
				GetComponent<SpriteRenderer>().sprite = rotationSprites[1];
			}
			else if (rotation == 1)
			{
				GetComponent<SpriteRenderer>().sprite = rotationSprites[3];
			}
			else if (rotation == 2)
			{
				GetComponent<SpriteRenderer>().sprite = rotationSprites[0];
			}
			else if (rotation == 3)
			{
				GetComponent<SpriteRenderer>().sprite = rotationSprites[2];
			}

		}
	}
	
	// Update is called once per frame
	void Update () {
		if (objectTypeID < 5) // simple test for now to keep current objects behaviour as they are
		{
			if (inUse)
			{
				if (!eating)
				{				                                          // increases speed by 5 percent per adjacent need object  
					needTime = baseTime - (level*(0.3f) - volunteer*2.0f) * (1.0f + 0.05f * numberAdjacentObjects);
					eating = true;
				}
				currentTime += Time.deltaTime;
				if (currentTime>=needTime)
				{
					// need has been fulfilled
					inUse = false;
					eating = false;
					currentTime = 0.0f;
				}
			}
		}
	}

	public bool UpgradeObject()
	{
		if (this.transform.parent.GetComponent<TileScript>().parentScript.moneyScript.currency >= upgradeCost)
		{
			if (level < 6)
			{
				level++;
				this.transform.parent.GetComponent<TileScript>().parentScript.moneyScript.currency -= upgradeCost;
				return true;
			}
			return false;
		}
		else
		{
			return false;
		}
	}
}
