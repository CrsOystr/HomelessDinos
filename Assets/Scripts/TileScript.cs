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
	public GameObject mainBuildMenu;



	Color tintObject = new Color(0.7f, 0.7f, 0.7f, 1.0f);
	Color restoreObject = new Color(1.0f, 1.0f, 1.0f, 1.0f);

	// Use this for initialization
	void Start () {
		parentScript = transform.parent.GetComponent<ISOGRID>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//THIS IS THE NEW WAY to implement on click function things
	public void OnPointerClick (PointerEventData eventData)
	{
		parentScript.selectThisTile (gameObject);
		buildMenu(1);
	}

	// this is THE OLD METHOD for detecting player clicking tile
	/*void OnMouseDown() {
		parentScript.selectThisTile (gameObject);
		GameObject pathTest = Instantiate (pathMenu,new Vector3 (transform.position.x, transform.position.y, 0), Quaternion.identity) as GameObject;
	}*/

	public void buildMenu (int type)
	{	
		if (parentScript.moneyScript.day == 1) {
			if (this.currentObject == null) {
				GameObject test1 = Instantiate (pathMenu, new Vector3 (transform.position.x, transform.position.y, 0), Quaternion.identity) as GameObject;
				Button b1 = test1.GetComponentInChildren<Button> ();
				b1.onClick.AddListener (() => this.buildObject (parentScript.pathObject, "path"));
				b1.onClick.AddListener (() => parentScript.deselectThisTile ());
				this.currentMenu = test1;
				currentMenu.transform.parent = this.transform;
				menuUp = true;
			} else if (this.currentObject.name == "path") {
				GameObject test1 = Instantiate (removeMenu, new Vector3 (transform.position.x, transform.position.y, 0), Quaternion.identity) as GameObject;
				Button b1 = test1.GetComponentInChildren<Button> ();
				b1.onClick.AddListener (() => this.deleteMenu ());
				b1.onClick.AddListener (() => this.deleteObject ());
				b1.onClick.AddListener (() => parentScript.deselectThisTile ());
				this.currentMenu = test1;
				currentMenu.transform.parent = this.transform;
				menuUp = true;
			}
		} else if (parentScript.moneyScript.day != 1) {
			if (this.currentObject == null) {
				GameObject test1 = Instantiate (mainBuildMenu, new Vector3 (transform.position.x, transform.position.y, 0), Quaternion.identity) as GameObject;
				Button[] b2= test1.GetComponentsInChildren<Button>();
				for (int i = 0; i < 4; i++)
				{
					b2[i].onClick.AddListener (() => parentScript.deselectThisTile ());
				}
				b2[0].onClick.AddListener (() => this.buildObject (parentScript.foodObject, "food"));
				b2[1].onClick.AddListener (() => this.buildObject (parentScript.hygieneObject, "hygiene"));
				b2[2].onClick.AddListener (() => this.buildObject (parentScript.healthObject, "health"));
				b2[3].onClick.AddListener (() => this.buildObject (parentScript.sleepObject, "sleep"));
				this.currentMenu = test1;
				currentMenu.transform.parent = this.transform;
				menuUp = true;
			} else if (this.currentObject.name == "path") {
				GameObject test1 = Instantiate (removeMenu, new Vector3 (transform.position.x, transform.position.y, 0), Quaternion.identity) as GameObject;
				Button b1 = test1.GetComponentInChildren<Button> ();
				b1.onClick.AddListener (() => this.deleteMenu ());
				b1.onClick.AddListener (() => this.deleteObject ());
				b1.onClick.AddListener (() => parentScript.deselectThisTile ());
				this.currentMenu = test1;
				currentMenu.transform.parent = this.transform;
				menuUp = true;
			}
		}
		
	}



	// create object
	public void buildObject(GameObject addObject, string type)
	{
		if (!objectPlaced && type == "path" || type == "spawn" || parentScript.moneyScript.currency >= addObject.GetComponent<needObjectScript>().cost)
		{
			GameObject test = Instantiate(addObject, new Vector3 (transform.position.x, transform.position.y-0.5f, 0), Quaternion.identity) as GameObject; 
			test.name = type;
			this.currentObject = test;
			//currentObject = test;
			//newCell.name = string.Format("({0},{1})",x,y);
			currentObject.transform.parent = this.transform;
			// for properly layering objects
			if (type == "path" || type == "spawn")
			{
				currentObject.renderer.sortingOrder = 1;
			}
			else
			{
				currentObject.renderer.sortingOrder = 10000-((int)Position.x + (int)Position.y);
			}
		
			objectPlaced = true;

			deleteMenu();
			if (selected)
			{
				currentObject.renderer.material.color = tintObject;
			}
			if(null != addObject.GetComponent<needObjectScript>()){
				parentScript.moneyScript.currency -= addObject.GetComponent<needObjectScript>().cost;
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
