using UnityEngine;
using System.Collections;
using System.Collections.Generic;


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

	// Use this for initialization
	void Start () {
		CreateGrid ();
	}

	// Update is called once per frame
	void Update () {
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
		}
	}



	//function moves day forward and clears selection
	public void advanceDay(){
		moneyScript.addDay ();
		deselectThisTile ();
		if (moneyScript.day == 3) {
			Grid[0,enterTile].GetComponent<TileScript>().currentObject.GetComponent<spawnHobo>().startHobos();
		}
	}

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

		enterTile = Random.Range(2, (int)Size.y-1);
		int exit = Random.Range (2, (int)Size.x-1);
		// create enterTile and exitTile
		Grid[0,enterTile].GetComponent<TileScript>().buildObject(enterTilePref, "spawn");
		Grid[exit,0].GetComponent<TileScript>().buildObject(exitTilePref, "spawn");


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
		if (x<Size.x)
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
		if (y<Size.y)
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
		if (x<Size.x)
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
		if (y<Size.y)
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
