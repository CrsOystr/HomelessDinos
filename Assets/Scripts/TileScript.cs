using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//This class creates objects and represents a specific tile. it checks for mouse clicks and interaction

public class TileScript : MonoBehaviour, IPointerClickHandler {
	public ISOGRID parentScript;

	public Vector2 Position;
	
	public bool selected = false;
	public GameObject currentObject;
	public GameObject currentMenu;
	public bool objectPlaced = false;
	private bool menuUp = false;
	public GameObject pathMenu;
	public GameObject removeMenu;


	Color tintObject = new Color(0.7f, 0.7f, 0.7f, 1.0f);
	Color restoreObject = new Color(1.0f, 1.0f, 1.0f, 1.0f);

	// Use this for initialization
	void Start () {
		parentScript = transform.parent.GetComponent<ISOGRID>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void OnPointerClick (PointerEventData eventData)
	{
		parentScript.selectThisTile (gameObject);
		buildMenu(1);
	}


	// this is for detecting player clicking tile
	/*void OnMouseDown() {
		parentScript.selectThisTile (gameObject);
		GameObject pathTest = Instantiate (pathMenu,new Vector3 (transform.position.x, transform.position.y, 0), Quaternion.identity) as GameObject;
	}*/

	public void buildMenu (int type)
	{
		if (this.currentObject == null) {
			GameObject test1 = Instantiate (pathMenu, new Vector3 (transform.position.x, transform.position.y, 0), Quaternion.identity) as GameObject;
			this.currentMenu = test1;
			currentMenu.transform.parent = this.transform;
			menuUp = true;
		}else if (this.currentObject.name == "path") {
			GameObject test1 = Instantiate (removeMenu, new Vector3 (transform.position.x, transform.position.y, 0), Quaternion.identity) as GameObject;
			this.currentMenu = test1;
			currentMenu.transform.parent = this.transform;
			menuUp = true;
		}

	}



	// create object
	public void buildObject(GameObject newObject, string type)
	{
		if (!objectPlaced)
		{
			GameObject test;

			test = Instantiate(newObject, new Vector3 (transform.position.x, transform.position.y-0.5f, 0), Quaternion.identity) as GameObject; 
			test.name = type;


			this.currentObject = test;
			//currentObject = test;
			//newCell.name = string.Format("({0},{1})",x,y);
			currentObject.transform.parent = this.transform;
			// for properly layering objects
			if (type == "path")
			{
				currentObject.renderer.sortingOrder = 1;
			}
			else
			{
				currentObject.renderer.sortingOrder = 10000-((int)Position.x + (int)Position.y);
			}
		
			objectPlaced = true;

			deleteMenu();
			//ADDED THIS EXPERIMENTING HOW TO DO LOGIC
			//this.parentScript.Logic_Grid[(int)this.Position.x,(int)this.Position.y] = new GameBuilding(true);
			//Debug.Log((int)this.Position.x);

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

	public void deleteMenu()
	{
		if(menuUp)
		{
			Destroy(this.currentMenu);
			menuUp = false;
			
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
