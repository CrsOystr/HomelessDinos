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
	}
	
	// Update is called once per frame
	void Update () {
		if (objectTypeID < 5) // simple test for now to keep current objects behaviour as they are
		{
			if (inUse)
			{
				if (!eating)
				{				                                          // increases speed by 5 percent per adjacent need object  
					needTime = baseTime - (level*(0.5f) - volunteer*2.0f) * (1.0f + 0.05f * numberAdjacentObjects);
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
}
