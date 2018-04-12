using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

	//Part 5 - Created a method to spawn the ball in the screen.

	private GameObject ball;

	// Use this for initialization
	void Start () {
		
		SpawnBall ();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void SpawnBall () {

		//Part 5 - Creates the ball by looking for the ball... 
		//by looking for the ball gameobject.
		 
		ball = GameObject.Instantiate ((GameObject)Resources.Load("Prefabs/ball", typeof (GameObject)));
		ball.transform.localPosition = new Vector3 (12, 0, -2);

	}
}
