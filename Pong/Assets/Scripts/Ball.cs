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
		paddleAi = GameObject.Find ("ai_paddle");

		//Part 5 - Set the dimensions for the game objects.
		playerPaddleHeight = paddlePlayer.transform.GetComponent<SpriteRenderer> ().bounds.size.y;
		playerPaddleWidth = paddlePlayer.transform.GetComponent<SpriteRenderer> ().bounds.size.x;
		aiPaddleHeight = paddleAi.transform.GetComponent<SpriteRenderer> ().bounds.size.y;
		aiPaddleWidth = paddleAi.transform.GetComponent<SpriteRenderer> ().bounds.size.x;		
		ballHeight = transform.GetComponent<SpriteRenderer> ().bounds.size.y;
		ballWidth = transform.GetComponent<SpriteRenderer> ().bounds.size.x;

		//Part 6 - we are going to set the variables so that there is a max limit...
		//to where the ball can move before it counts as colliding with the paddle. 
		playerPaddleMaxX = paddlePlayer.transform.localPosition.x + playerPaddleWidth / 2;
		playerPaddleMinX = paddlePlayer.transform.localPosition.x - playerPaddleWidth / 2;

		//this will check the X position of the paddle and create a boundary...
		//for the collision. 
		aiPaddleMaxX = paddleAi.transform.localPosition.x - aiPaddleWidth / 2;
		aiPaddleMinX = paddleAi.transform.localPosition.x + aiPaddleWidth / 2;
		
		//we will set our maxY and minY in the update method because the paddles...
		//are constantly moving and the value is always changing. 

	}
	
	// Update is called once per frame
	void Update () {

		Move ();
	
	}

	//We want to know if the ball collided with either of the paddles
	bool CheckCollision () {

		//Part 6 - set our maxY and minY variables
		//this will check the current Y position of our paddle...
		//and add half the height of the paddle on both the top and bottom...
		//to create the boundary for the paddle. 
		playerPaddleMaxY = paddlePlayer.transform.localPosition.y + playerPaddleHeight / 2;
		playerPaddleMinY = paddlePlayer.transform.localPosition.y - playerPaddleHeight / 2;

		aiPaddleMaxY = paddleAi.transform.localPosition.y + aiPaddleHeight / 2;
		aiPaddleMinY = paddleAi.transform.localPosition.y - aiPaddleHeight / 2;
	
		//This will check to see if the ball will collide with the paddles' edge. (an x collision (sides))
		if (transform.localPosition.x - ballWidth / 2 < playerPaddleMaxX && transform.localPosition.x + ballWidth / 2 > playerPaddleMinX) {
			//this will check if the top/bottom of the paddles and ball will collide (a y collision)
			if (transform.localPosition.y - ballHeight  / 2 < playerPaddleMaxY && transform.localPosition.y + ballHeight / 2 > playerPaddleMinY) {

				//tell the ball to go right when it collides with the playerpaddle. 
				ballDirection = Vector2.right;
				//make sure the ball does not change direction from the inside of the paddle
				transform.localPosition = new Vector3 (playerPaddleMaxX + ballWidth / 2, transform.localPosition.y, transform.localPosition.z);
				return true;

			}
		}

		if (transform.localPosition.x + ballWidth /2 > aiPaddleMaxX && transform.localPosition.x - ballWidth /2 < aiPaddleMinX){

			if (transform.localPosition.x - ballHeight / 2 < aiPaddleMaxY && transform.localPosition.y + ballHeight / 2 > aiPaddleMinY){

				ballDirection = Vector2.left;
				transform.localPosition = new Vector3 (aiPaddleMaxX - ballWidth /2, transform.localPosition.y, transform.localPosition.z);
				return true;
			}
		}

		return false;
	}

	void Move () {

		if(!CheckCollision ()) {
			
		transform.localPosition += (Vector3)ballDirection * moveSpeed * Time.deltaTime;

		}
	}


}
