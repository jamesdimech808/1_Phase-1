using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

	//Part 5 - Make these variables public so that we can modify it in...
	//the inspector.

	//Have the ball move at 12.0f speed
	public float moveSpeed = 12.0f;

	//Have the ball move left when initiated. 
	public Vector2 ballDirection = Vector2.left;

	//variables to use if powerups which will manipulate the gameobject's sizes for difficulty.
	private float playerPaddleHeight, playerPaddleWidth, aiPaddleHeight, aiPaddleWidth, playerPaddleMaxX, playerPaddleMaxY, playerPaddleMinX, playerPaddleMinY, aiPaddleMaxX, aiPaddleMaxY, aiPaddleMinX, aiPaddleMinY, ballWidth, ballHeight;


	private GameObject paddlePlayer, paddleAi;

	// Use this for initialization
	void Start () {

		paddlePlayer = GameObject.Find ("player_paddle");
		paddleAi = GameObject.Find ("ai_player");

		//Part 5 - Set the dimensions for the game objects.
		playerPaddleHeight = paddlePlayer.transform.GetComponent<SpriteRenderer> ().bounds.size.y;
		playerPaddleWidth = paddlePlayer.transform.GetComponent<SpriteRenderer> ().bounds.size.x;
		aiPaddleHeight = paddleAi.transform.GetComponent<SpriteRenderer> ().bounds.size.y;
		aiPaddleWidth = paddleAi.transform.GetComponent<SpriteRenderer> ().bounds.size.x;		
		ballHeight = transform.GetComponent<SpriteRenderer> ().bounds.size.y;
		ballWidth = transform.GetComponent<SpriteRenderer> ().bounds.size.x;

	}
	
	// Update is called once per frame
	void Update () {

		Move ();
	
	}

	//We want to know if the ball collided with either of the paddles
	bool CheckCollision () {

	}

	void Move () {

		transform.localPosition += (Vector3)ballDirection * moveSpeed * Time.deltaTime;

	}


}
