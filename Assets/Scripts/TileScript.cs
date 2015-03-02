using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileScript : MonoBehaviour {
	ISOGRID parentScript;

	public Vector2 Position;
	
	public bool selected = false;
	public GameObject currentObject;
	bool objectPlaced = false;


	Color tintObject = new Color(0.7f, 0.7f, 0.7f, 1.0f);
	Color restoreObject = new Color(1.0f, 1.0f, 1.0f, 1.0f);

	// Use this for initialization
	void Start () {
		parentScript = transform.parent.GetComponent<ISOGRID>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// this is for detecting player clicking tile
	void OnMouseDown() {
		parentScript.selectThisTile(gameObject);
	}

	// create object
	public void buildObject(GameObject newObject)
	{
		if (!objectPlaced)
		{
			GameObject test = Instantiate(newObject, new Vector3 (transform.position.x, transform.position.y-0.5f, 0), Quaternion.identity) as GameObject; 
			this.currentObject = test;
			//currentObject = test;
			//newCell.name = string.Format("({0},{1})",x,y);
			currentObject.transform.parent = this.transform;
			currentObject.renderer.sortingOrder = 10000-((int)Position.x + (int)Position.y);
			objectPlaced = true;

			//ADDED THIS EXPERIMENTING HOW TO DO LOGIC
			this.parentScript.Logic_Grid[(int)this.Position.x,(int)this.Position.y] = new GameBuilding(false);

			if (selected)
			{
				currentObject.renderer.material.color = tintObject;
			}
		}
	}

	public void deleteObject()
	{
		if(objectPlaced)
		{
			Destroy(this.currentObject);
			objectPlaced = false;

		}
	}

	public void highlightObject()
	{
		if (objectPlaced)
		{
			currentObject.renderer.material.color = tintObject;
		}
	}
	public void unHighlightObject()
	{
		if (objectPlaced)
		{
			currentObject.renderer.material.color = restoreObject;
		}
	}
}
