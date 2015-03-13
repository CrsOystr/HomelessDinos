using UnityEngine;
using System.Collections;

public class MovingPerson : MonoBehaviour {
	public Vector2 personLocation;
	ISOGRID parentScript;

	public void moveLeft(){
		if (parentScript.Logic_Grid[(int)(personLocation.x - 1),(int)(personLocation.y)].passable)
		{
			Vector3 leftVect = new Vector3(-1f,-.5f,0f);
			transform.Translate(leftVect);
			personLocation.x = personLocation.x - 1;
		}

	}
	public void moveUp(){
		if (parentScript.Logic_Grid[(int)(personLocation.x),(int)(personLocation.y + 1)].passable)
		{
			Vector3 upVect = new Vector3(-1f,.5f,0f);
			transform.Translate(upVect);
			personLocation.y = personLocation.y + 1;
		}
	}

	public void moveRight(){
		if (parentScript.Logic_Grid[(int)(personLocation.x + 1),(int)(personLocation.y)].passable)
		{
			Vector3 rightVect = new Vector3(1f,.5f,0f);
			transform.Translate(rightVect);
			personLocation.x = personLocation.x + 1;
		}
	}
	public void moveDown(){
		if (parentScript.Logic_Grid[(int)(personLocation.x),(int)(personLocation.y-1)].passable)
		{
			Vector3 downVect = new Vector3(1f,-.5f,0f);
			transform.Translate(downVect);
			personLocation.y = personLocation.y - 1;
		}
	}

	void Start () 
	{
		//parentScript = GameObject.Find("MainGrid").GetComponent<ISOGRID>();
		parentScript = transform.parent.GetComponent<ISOGRID>();
		//Vector3 StartPosition = transform.position;
		//StartPosition.x = Mathf.Round(StartPosition.x);
		//StartPosition.y = Mathf.Round(StartPosition.y * 2) / 2;
		//transform.position = StartPosition;

		//SETS CORRECT LOCATION BASED ON OBJJECT SSETTINGS
		Vector3 StartPosition = new Vector3((personLocation.x - personLocation.y) * 1 ,
		                                   (personLocation.x + personLocation.y)*(.5F) + 1f ,0);

		transform.position = StartPosition;
	}

	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.H))
		{
			moveLeft();
		}
		if(Input.GetKeyDown(KeyCode.U))
		{
			moveUp();
		}
		if(Input.GetKeyDown(KeyCode.K))
		{
			moveRight();
		}
		if(Input.GetKeyDown(KeyCode.J))
		{
			moveDown();
		}
	}
}
