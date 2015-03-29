using UnityEngine;
using System.Collections;

public class removeHobo : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		//printf("collision with %s", collision.gameObject.name);
		System.Console.WriteLine(collision.gameObject.name);
		if (collision.gameObject.name == "hobo")
		{
			// adjust currency and reputation

			// remove hobo
			Destroy(collision.gameObject);
		}
	}
}
