using UnityEngine;
using System.Collections;

public class ISOGRID : MonoBehaviour {

	public Transform CellPrefab;
	public Vector3 Size;
	public Transform[,] Grid;
	// Use this for initialization
	void Start () {
		CreateGrid ();
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

}
