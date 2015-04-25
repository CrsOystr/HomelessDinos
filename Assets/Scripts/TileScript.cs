using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//This class creates objects and represents a specific tile. it checks for mouse clicks and interaction

public class TileScript : MonoBehaviour, IPointerUpHandler, IPointerDownHandler {
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
	public GameObject upgradeMenu;

	public AudioClip soundBuild;
	public AudioClip soundSell;
	private AudioSource playAudio;

	private float lastTime;

	// for beds occupying two tiles
	private TileScript otherTile;

	Color tintObject = new Color(0.9f, 0.5f, 0.5f, 0.5f);
	Color restoreObject = new Color(1.0f, 1.0f, 1.0f, 0.8f);

	// Use this for initialization
	void Start () {
		parentScript = transform.parent.GetComponent<ISOGRID>();

		playAudio = gameObject.AddComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnPointerDown (PointerEventData eventData)
	{
		lastTime = Time.time;
	}

	public void OnPointerUp (PointerEventData eventData)
	{
		if (Time.time - lastTime < 0.25f)
		{
			parentScript.selectThisTile (gameObject);
			buildMenu(1);
		}
	}
	/*
	//THIS IS THE NEW WAY to implement on click function things
	public void OnPointerClick (PointerEventData eventData)
	{
		parentScript.selectThisTile (gameObject);
		buildMenu(1);
	}
	*/

	// this is THE OLD METHOD for detecting player clicking tile
	/*void OnMouseDown() {
		parentScript.selectThisTile (gameObject);
		GameObject pathTest = Instantiate (pathMenu,new Vector3 (transform.position.x, transform.position.y, 0), Quaternion.identity) as GameObject;
	}*/

	public void buildMenu (int type)
	{	
		if (parentScript.pathMode)
		//if (parentScript.moneyScript.day == 1) 
		{
			if (this.currentObject == null) {
				GameObject test1 = Instantiate (pathMenu, new Vector3 (transform.position.x, transform.position.y, 0), Quaternion.identity) as GameObject;
				Button b1 = test1.GetComponentInChildren<Button> ();
				b1.onClick.AddListener (() => this.buildObject (parentScript.pathObject, "path"));
				b1.onClick.AddListener (() => parentScript.deselectThisTile ());
				b1.GetComponentInChildren<Text> ().text = parentScript.pathObject.GetComponent<needObjectScript>().cost.ToString();
				this.currentMenu = test1;
				currentMenu.transform.parent = this.transform;
				menuUp = true;
			} else if (this.currentObject.name == "path") {
				GameObject test1 = Instantiate (removeMenu, new Vector3 (transform.position.x, transform.position.y, 0), Quaternion.identity) as GameObject;
				Button b1 = test1.GetComponentInChildren<Button> ();
				b1.onClick.AddListener (() => this.deleteObject ());
				b1.onClick.AddListener (() => parentScript.deselectThisTile ());
				b1.GetComponentInChildren<Text> ().text = currentObject.GetComponent<needObjectScript>().sellPrice.ToString();
				this.currentMenu = test1;
				currentMenu.transform.parent = this.transform;
				menuUp = true;
			}
		} 
		else if (parentScript.towerMode)
		//else if (parentScript.moneyScript.day != 1) 
		{
			if (this.currentObject == null) {
				GameObject test1 = Instantiate (mainBuildMenu, new Vector3 (transform.position.x, transform.position.y, 0), Quaternion.identity) as GameObject;
				Button[] b2= test1.GetComponentsInChildren<Button>();
				for (int i = 0; i < 5; i++)
				{
					b2[i].onClick.AddListener (() => parentScript.deselectThisTile ());
				}
				b2[0].onClick.AddListener (() => this.buildObject (parentScript.foodObject, "food"));
				b2[0].GetComponentInChildren<Text> ().text = parentScript.foodObject.GetComponent<needObjectScript>().cost.ToString();
				b2[1].onClick.AddListener (() => this.buildObject (parentScript.hygieneObject, "hygiene"));
				b2[1].GetComponentInChildren<Text> ().text = parentScript.hygieneObject.GetComponent<needObjectScript>().cost.ToString();
				b2[2].onClick.AddListener (() => this.buildObject (parentScript.healthObject, "health"));
				b2[2].GetComponentInChildren<Text> ().text = parentScript.healthObject.GetComponent<needObjectScript>().cost.ToString();  
				b2[3].onClick.AddListener (() => this.buildObject (parentScript.sleepObject, "sleep"));
				b2[3].GetComponentInChildren<Text> ().text = parentScript.sleepObject.GetComponent<needObjectScript>().cost.ToString();

				this.currentMenu = test1;
				currentMenu.transform.parent = this.transform;
				menuUp = true;
			} else if (this.currentObject.name != "path") {
				GameObject test1 = Instantiate (upgradeMenu, new Vector3 (transform.position.x, transform.position.y, 0), Quaternion.identity) as GameObject;
				Button[] b2= test1.GetComponentsInChildren<Button>();
				for (int i = 0; i < 2; i++)
				{
					b2[i].onClick.AddListener (() => parentScript.deselectThisTile ());
				}
					// UPGRADE TOWERS NEEDS TO BECOME ADDED HERE
					//b2[0].onClick.AddListener (() => currentObject.);
					b2[0].GetComponentInChildren<Text> ().text = currentObject.GetComponent<needObjectScript>().upgradeCost.ToString();
					b2[1].onClick.AddListener (() => this.deleteObject());
					b2[1].GetComponentInChildren<Text> ().text = currentObject.GetComponent<needObjectScript>().sellPrice.ToString();  

					this.currentMenu = test1;
					currentMenu.transform.parent = this.transform;
					menuUp = true;				
			}
		}		
	}



	// create object
	public bool buildObject(GameObject addObject, string type)
	{
		if (!objectPlaced && type == "spawn" || !objectPlaced && parentScript.moneyScript.currency >= addObject.GetComponent<needObjectScript>().cost)
		{
			if (type != "path" || parentScript.numPaths >0)
			{


				if (type == "sleep")
				{
					if (parentScript.Grid[(int)Position.x,(int)Position.y+1].GetComponent<TileScript>().buildObject(addObject, "temp"))
					{
						this.otherTile = parentScript.Grid[(int)Position.x,(int)Position.y+1].GetComponent<TileScript>();
						otherTile.otherTile = this;
						this.currentObject = otherTile.currentObject;
						currentObject.name = type;
						objectPlaced = true;

						// placement
						this.currentObject.GetComponent<needObjectScript>().RotateToPath(0);
						this.currentObject.transform.position = new Vector3(this.transform.position.x-0.5f,this.transform.position.y-0.54f,this.transform.position.z);
					}
					else if (parentScript.Grid[(int)Position.x,(int)Position.y-1].GetComponent<TileScript>().buildObject(addObject, "temp"))
					{
						this.otherTile = parentScript.Grid[(int)Position.x,(int)Position.y-1].GetComponent<TileScript>();
						otherTile.otherTile = this;
						this.currentObject = otherTile.currentObject;
						currentObject.name = type;
						objectPlaced = true;
						
						// placement
						this.currentObject.GetComponent<needObjectScript>().RotateToPath(1);
						this.currentObject.transform.position = new Vector3(this.transform.position.x+0.5f,this.transform.position.y-1.04f,this.transform.position.z);
					}
					else if (parentScript.Grid[(int)Position.x+1,(int)Position.y].GetComponent<TileScript>().buildObject(addObject, "temp"))
					{
						this.otherTile = parentScript.Grid[(int)Position.x+1,(int)Position.y].GetComponent<TileScript>();
						otherTile.otherTile = this;
						this.currentObject = otherTile.currentObject;
						currentObject.name = type;
						objectPlaced = true;
						
						// placement
						this.currentObject.GetComponent<needObjectScript>().RotateToPath(2);
						this.currentObject.transform.position = new Vector3(this.transform.position.x+0.52f,this.transform.position.y-0.52f,this.transform.position.z);

					}
					else if (parentScript.Grid[(int)Position.x-1,(int)Position.y].GetComponent<TileScript>().buildObject(addObject, "temp"))
					{
						this.otherTile = parentScript.Grid[(int)Position.x-1,(int)Position.y].GetComponent<TileScript>();
						otherTile.otherTile = this;
						this.currentObject = otherTile.currentObject;
						currentObject.name = type;
						objectPlaced = true;
						
						// placement
						this.currentObject.GetComponent<needObjectScript>().RotateToPath(3);
						this.currentObject.transform.position = new Vector3(this.transform.position.x-0.5f,this.transform.position.y-1.01f,this.transform.position.z);

					}
					else
					{
						return false;
					}
				}
				else
				{
					GameObject test = Instantiate(addObject, new Vector3 (transform.position.x, transform.position.y-0.5f, 0), Quaternion.identity) as GameObject; 
					test.name = type;
					this.currentObject = test;
					currentObject.transform.parent = this.transform;
					// object now on this tile
					objectPlaced = true;
				}

				// for sound
				if (playAudio != null)
				{
					playAudio.clip = soundBuild;
					playAudio.Play();
				}


				// for properly layering objects
				if (type == "path" || type == "spawn")
				{
					currentObject.renderer.sortingOrder = 1;
				}
				else
				{
					currentObject.renderer.sortingOrder = 10000-((int)Position.x + (int)Position.y);
				}
			


				deleteMenu();
				if (selected)
				{
					currentObject.renderer.material.color = tintObject;
				}
				if(null != addObject.GetComponent<needObjectScript>() && type != "temp"){
					// remove cost of this object
					parentScript.moneyScript.currency -= addObject.GetComponent<needObjectScript>().cost;
				}
				if(type == "path")
				{
					parentScript.numPaths --;

				}

				// successfully built
				return true;
			}
			else
			{
				parentScript.messageOverride = true;
				parentScript.printMessage = "You have run out of path tiles";

				return false;
			}
		}
		return false;
	}

	public void deleteObject()
	{
		if(objectPlaced)
		{
			parentScript.moneyScript.currency += currentObject.GetComponent<needObjectScript>().sellPrice;
			if (currentObject.GetComponent<needObjectScript>().sellPrice == 45)
			{
				parentScript.numPaths ++;
				parentScript.messageOverride = false;
			}


			if (this.currentObject.name == "sleep")
			{
				this.otherTile.objectPlaced = false;
			}


			Destroy(this.currentObject);
			objectPlaced = false;
			if (playAudio != null)
			{
				playAudio.clip = soundSell;
				playAudio.Play();
			}
		}
		deleteMenu ();
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
