using UnityEngine;
using System.Collections;

public class ISOGRID : MonoBehaviour {

	public Transform CellPrefab;
	public Vector3 Size;
	public Transform[,] Grid;
	public GameObject selectedTile;

	Color tintTile = new Color(0.7f, 0.7f, 0.7f, 1.0f);
	Color restoreTile = new Color(1.0f, 1.0f, 1.0f, 1.0f);
	//Color tintTile = new Color(178.0f, 178.0f, 178.0f, 255.0f);

	public Transform newObject;

	// Use this for initialization
	void Start () {
		CreateGrid ();
	}

	// Update is called once per frame
	void Update () {
		
	}

	void CreateGrid(){
		Grid = new Transform[(int)Size.x,(int)Size.y];
		for (int y= 0; y < Size.y; y++) {
			for (int x = 0; x < Size.x; x++) {
				Transform newCell;
				newCell = (Transform)Instantiate (CellPrefab, new Vector3 ((x - y) * 1 , (x + y)*(.5F), 0), Quaternion.identity); 
				newCell.name = string.Format("({0},{1})",x,y);
				newCell.parent = transform;
				newCell.GetComponent<TileScript>().Position = new Vector2(x,y);
				Grid[x,y] = newCell;
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

		}
		// a tile was previously selected and must be cleaned up
		else
		{
				
			selectedTile.renderer.material.color = restoreTile;
			selectedTile.GetComponent<TileScript>().selected = false;

			selectedTile = newTile;

			selectedTile.GetComponent<TileScript>().selected = true;
			
			//selectedTile.renderer.material.SetColor("_TintColor", tintTile);
			selectedTile.renderer.material.color = tintTile;
		}
	}

}
