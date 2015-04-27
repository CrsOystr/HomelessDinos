using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


//The main logic unit for the game. holds lots of stfuffffff
public class ISOGRID : MonoBehaviour {

	public Transform CellPrefab;
	public Vector3 Size;
	public GameObject selectedTile;

	public Transform[,] Grid;
	public GameBuilding[,] Logic_Grid;

	public scoreScript moneyScript;
	

	Color tintTile = new Color(0.7f, 0.7f, 0.7f, 1.0f);
	Color restoreTile = new Color(1.0f, 1.0f, 1.0f, 1.0f);
	//Color tintTile = new Color(178.0f, 178.0f, 178.0f, 255.0f);

	public GameObject foodObject;
	public GameObject hygieneObject;
	public GameObject healthObject;
	public GameObject sleepObject;
	public GameObject pathObject;

	public GameObject enterTilePref;
	public GameObject exitTilePref;

	public GameObject leftBackWall;
	public GameObject rightBackWall;

	private int enterTile;
	private int exitTile;
	public Text pathText;
	public Button dayButton;
	public Button newDayPathButton;
	public GameObject dayPanel;
	public GameObject blockPanel;
	public GameObject escapePanel;
	public GameObject tilePanel;


	// variables visible to other scripts but not to the unity editor
	[HideInInspector]
	public bool pathMode = true;
	public bool towerMode = false;
	public bool readyToAdvance = false;
	public int[] dayGoals = new int[] {0,15,25,45,85}; 
	public int numPaths = 10;
	public string printMessage = "";
	public bool messageOverride = false;

	// Use this for initialization
	void Start () {
		CreateGrid ();
		numPaths = moneyScript.day*10;

	}

	// Update is called once per frame
	void Update () 
	{
		//Looking to create escape menu
		if (Input.GetKeyDown (KeyCode.Escape) && !escapePanel.activeSelf){
			escapePanel.SetActive(true);
			blockPanel.SetActive(true);
		} else if(Input.GetKeyDown (KeyCode.Escape) && escapePanel.activeSelf){
			escapePanel.SetActive(false);
			blockPanel.SetActive(false);
		}
		if(pathMode && !blockPanel.activeSelf) tilePanel.SetActive(true);
		if(!pathMode) tilePanel.SetActive(false);



		//else if(Input.GetKey (KeyCode.Escape) &&
		       // GameObject.Find("returnToMenu").SetActive(false);

		if (selectedTile != null)
		{
			// press button for stove
			if(Input.GetKeyDown(KeyCode.Space))
			{ 
					selectedTile.GetComponent<TileScript>().buildObject(foodObject, "food");
			}
			// get path
			if(Input.GetKeyDown(KeyCode.P))
			{ 
				selectedTile.GetComponent<TileScript>().buildObject(pathObject, "path");
			}

			//DELETES ANYTHING
			if(Input.GetKeyDown(KeyCode.G))
			{
				selectedTile.GetComponent<TileScript>().deleteObject();
			}

			// ensures valid path whenever we are in path placing mode
			if(pathMode)
			{
				if (CheckValidPath())
				{
					// enable button
					dayButton.interactable = true;
					newDayPathButton.interactable = true;
				}
				else
				{
					// disable button
					dayButton.interactable = false;
					newDayPathButton.interactable = false;
					//print ("button should be off");
				}
			}
		}
		if (moneyScript.reputation >= dayGoals[moneyScript.day])
		{
			//ready to move on to next day
			readyToAdvance = true;
			Grid[0,enterTile].GetComponent<TileScript>().currentObject.GetComponent<spawnHobo>().stopHobos();

			/*
			advanceDay();
			dayPanel.SetActive(true);
			blockPanel.SetActive(true);
			*/
			if (GameObject.Find("hobo") == null)
            {
				advanceDay();
				dayPanel.SetActive(true);
				blockPanel.SetActive(true);
				clearGrid();
            }
        }
        
	}

	private bool CheckValidPath()
	{
		bool validPath = false;
		//printMessage = "shit";
		int tileX = 0;
		int tileY = enterTile;
		
		int prevX = tileX;
		int prevY = tileY;

		if (!messageOverride)
		{
			for(int i = 0; i < 1000; i++)
			{
				// get nearby tiles
				List<Vector2> pathList = nearbyPaths(tileX,tileY);
				//print ("" + tileX + " " + tileY);
				
				// if more than two nearby tiles, invalid path, break
				if (0 == tileX && enterTile == tileY)
				{
					if (pathList.Count != 1)
					{
						printMessage = "The entrance tile does not have a single path from it";
						validPath = false;
						break;
					}
				}
				else if (pathList.Count > 2)
				{
					printMessage = "This path is invalid. Make sure your path only goes one way";
					validPath = false;
					break;
				}
				
				// if on last tile, valid path, break
				if (tileX == exitTile && tileY == 0)
				{
					printMessage = "This path is valid! Click next to continue";
					validPath = true;
					break;
				}
				
				// if only one nearby tile, invalid path, break
				if (0 == tileX && enterTile == tileY)
				{
				}
				else if (pathList.Count < 2)
				{
					printMessage = "This path does not end at the red exit tile";
					validPath = false;
					break;
				}
				
				// move to next tile
				foreach(Vector2 tile in pathList)
				{
					if ((int)tile.x == prevX && (int)tile.y == prevY)
					{
					}
					else
					{
						prevX = tileX;
						prevY = tileY;
						tileX = (int)tile.x;
						tileY = (int)tile.y;
						break;
					}
				}
				if (i > 100)
				{
					print("shit went bad");
					break;
				}
			}
		}

		pathText.text = printMessage;

		return validPath;
	}

	//function moves day forward and clears selection
	public void advanceDay()
	{
		if (readyToAdvance)
		{
			readyToAdvance = false;
			moneyScript.addDay ();
			numPaths += 10;
			deselectThisTile ();
			Grid[0,enterTile].GetComponent<TileScript>().currentObject.GetComponent<spawnHobo>().stopHobos();
			tilePanel.SetActive(true);

			towerMode = false;
			pathMode = true;

		}

		else
		{
			if (towerMode)
			{
				deselectThisTile ();
				Grid[0,enterTile].GetComponent<TileScript>().currentObject.GetComponent<spawnHobo>().startHobos();
				//dayButton.GetComponentInChildren<Text>().text = "NEXT DAY";
			}
			
			if (pathMode)
			{
				pathMode = false;
				towerMode = true;
				//Grid[0,enterTile].GetComponent<TileScript>().currentObject.GetComponent<spawnHobo>().startHobos();


			}
		}
	}

	/*
	public void continuePrep()
	{
		deselectThisTile ();
		if(pathMode)
		{
			pathMode = false;
			towerMode = true;
			deselectThisTile ();
			Grid[0,enterTile].GetComponent<TileScript>().currentObject.GetComponent<spawnHobo>().startHobos();
		}

		else if (towerMode)
		{
			towerMode = false;
			pathMode = true;
		}
	}
	*/


	//FUNction for set up the grid
	void CreateGrid(){
		//this gives us a grid of the correct size to reference and use as logic later
		Grid = new Transform[(int)Size.x,(int)Size.y];
		Logic_Grid = new GameBuilding[(int)Size.x,(int)Size.y];

		//Visually creates all the tiles and places them into grid
		for (int y= 0; y < Size.y; y++) {
			for (int x = 0; x < Size.x; x++) {
				Transform newCell;
				newCell = (Transform)Instantiate (CellPrefab, new Vector3 ((x - y) * 1 , (x + y)*(.5F), 0), Quaternion.identity); 
				newCell.name = string.Format("({0},{1})",x,y);
				newCell.parent = transform;
				newCell.GetComponent<TileScript>().Position = new Vector2(x,y);
				Grid[x,y] = newCell;
				GameBuilding standTile = new GameBuilding(false);
				Logic_Grid[x,y] = standTile;
			}
		}

		enterTile = Random.Range(moneyScript.day * 2 + 1, (int)Size.y - (8 - moneyScript.day * 2));
		exitTile = Random.Range (moneyScript.day * 2 + 1, (int)Size.x - (8 - moneyScript.day * 2));
		// create enterTile and exitTile
		Grid[0,enterTile].GetComponent<TileScript>().buildObject(enterTilePref, "spawn");
		Grid[exitTile,0].GetComponent<TileScript>().buildObject(exitTilePref, "spawn");


		// create back left walls
		// use max y, loop on x
		for (int x = 0; x < Size.x; x++)
		{
			GameObject wall;
			wall = Instantiate (leftBackWall, new Vector3 (((x - Size.y) * 1)+0.5f , ((x + Size.y)*(.5F))-0.5f, 0), Quaternion.identity) as GameObject;
			wall.transform.parent = transform;
		}

		// create back right walls
		// use max x, loop on y
		for (int y = 0; y < Size.y; y++)
		{
			GameObject wall;
			wall = Instantiate (rightBackWall, new Vector3 (((Size.x - y) * 1)-0.5f , ((Size.x + y)*(.5F))-0.5f, 0), Quaternion.identity) as GameObject;
			wall.transform.parent = transform;
		}

	}

	public void clearGrid()
	{
		for (int y= 0; y < Size.y; y++) {
			for (int x = 0; x < Size.x; x++) {

				TileScript tile = Grid[x,y].GetComponent<TileScript>();
				if (tile.objectPlaced)
				{
					if (tile.currentObject.GetComponent<needObjectScript>() != null)
					{
						tile.deleteObject();
					}
				}
			}
		}
	}


	public void selectThisTile(GameObject newTile)
	{
		// there is no previously selected tile
		if (selectedTile == null)
		{
			selectedTile = newTile;

			selectedTile.GetComponent<TileScript>().selected = true;

			//selectedTile.renderer.material.SetColor("_TintColor", tintTile);
			selectedTile.renderer.material.color = tintTile;
			selectedTile.GetComponent<TileScript>().highlightObject();

		}
		// a tile was previously selected and must be cleaned up
		else
		{
			selectedTile.renderer.material.color = restoreTile;
			selectedTile.GetComponent<TileScript>().unHighlightObject();
			selectedTile.GetComponent<TileScript>().selected = false;
			selectedTile.GetComponent<TileScript>().deleteMenu();

			selectedTile = newTile;

			selectedTile.GetComponent<TileScript>().selected = true;
			
			//selectedTile.renderer.material.SetColor("_TintColor", tintTile);
			selectedTile.renderer.material.color = tintTile;
			selectedTile.GetComponent<TileScript>().highlightObject();
		}
	}

	public void deselectThisTile()
	{
		if (selectedTile != null) {
			selectedTile.renderer.material.color = restoreTile;
			selectedTile.GetComponent<TileScript> ().unHighlightObject ();
			selectedTile.GetComponent<TileScript> ().selected = false;
			selectedTile.GetComponent<TileScript> ().deleteMenu ();
		}
	}



	// finds nearby paths and returns a list
	public List<Vector2> nearbyPaths(int x, int y)
	{
		List<Vector2> returnList = new List<Vector2>();

		//print (x);
		//print (y);

		if (x>0)
		{
			if(Grid[x-1,y].GetComponent<TileScript>().objectPlaced)
			{
				if (Grid[x-1,y].GetComponent<TileScript>().currentObject.name=="path" || Grid[x-1,y].GetComponent<TileScript>().currentObject.name=="spawn")
				{
					returnList.Add(new Vector2(x-1,y));
				}
			}
		}
		if (x<Size.x-1)
		{
			if(Grid[x+1,y].GetComponent<TileScript>().objectPlaced)
            {
				if (Grid[x+1,y].GetComponent<TileScript>().currentObject.name=="path" || Grid[x+1,y].GetComponent<TileScript>().currentObject.name=="spawn")
				{
					returnList.Add(new Vector2(x+1,y));
	            }
			}
        }
		if (y>0)
		{
			if(Grid[x,y-1].GetComponent<TileScript>().objectPlaced)
            {
				if (Grid[x,y-1].GetComponent<TileScript>().currentObject.name=="path" || Grid[x,y-1].GetComponent<TileScript>().currentObject.name=="spawn")
				{
					returnList.Add(new Vector2(x,y-1));
				}
			}
		}
		if (y<Size.y-1)
		{
			if(Grid[x,y+1].GetComponent<TileScript>().objectPlaced)
            {
				if (Grid[x,y+1].GetComponent<TileScript>().currentObject.name=="path" || Grid[x,y+1].GetComponent<TileScript>().currentObject.name=="spawn")
	            {
	                returnList.Add(new Vector2(x,y+1));
	            }
			}
        }
        
        return returnList;
	}
	// finds the nearby need object specified and returns a list
	public List<Vector2> nearbyNeeds(int x, int y, string need)
	{
		List<Vector2> returnList = new List<Vector2>();
		
		//print (x);
		//print (y);
		
		if (x>0)
		{
			if(Grid[x-1,y].GetComponent<TileScript>().objectPlaced)
			{
				if (Grid[x-1,y].GetComponent<TileScript>().currentObject.name==need)
				{
					if(!Grid[x-1,y].GetComponent<TileScript>().currentObject.GetComponent<needObjectScript>().inUse)
					{
						returnList.Add(new Vector2(x-1,y));
					}
				}
			}
		}
		if (x<Size.x-1)
		{
			if(Grid[x+1,y].GetComponent<TileScript>().objectPlaced)
			{
				if (Grid[x+1,y].GetComponent<TileScript>().currentObject.name==need)
				{	
					if(!Grid[x+1,y].GetComponent<TileScript>().currentObject.GetComponent<needObjectScript>().inUse)
                    {
						returnList.Add(new Vector2(x+1,y));
					}
				}
			}
		}
		if (y>0)
		{
			if(Grid[x,y-1].GetComponent<TileScript>().objectPlaced)
			{
				if (Grid[x,y-1].GetComponent<TileScript>().currentObject.name==need)
				{
					if(!Grid[x,y-1].GetComponent<TileScript>().currentObject.GetComponent<needObjectScript>().inUse)
                    {
						returnList.Add(new Vector2(x,y-1));
					}
				}
			}
		}
		if (y<Size.y-1)
        {
            if(Grid[x,y+1].GetComponent<TileScript>().objectPlaced)
            {
                if (Grid[x,y+1].GetComponent<TileScript>().currentObject.name==need)
                {
					if(!Grid[x,y+1].GetComponent<TileScript>().currentObject.GetComponent<needObjectScript>().inUse)
                    {
                    	returnList.Add(new Vector2(x,y+1));
					}
                }
            }
        }
        
        return returnList;
    }
    
}
