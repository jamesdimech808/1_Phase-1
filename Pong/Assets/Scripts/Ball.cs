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

	//top wall boundary - Y coordinate
	public float topBounds = 9.4f;
	
	//bottom wall boundary - Y coordinate(negative)
	public float bottomBounds = -9.4f;

	//the amount of time that will pass before the speed of the ball increases
	public int speedIncreaseInterval = 20;
	//after 20 seconds the ball will increase by 1, 
	//therefore if default speed is 12... then after 20 seconds,
	//the speed will increase to 13...
	public float speedIncreaseBy = 1.0f;
	//keep track of the current time since the last time the speed has increased
	private float speedIncreaseTimer = 0;

	//variables to help set up the prefabs for collisions etc...
	private float playerPaddleHeight, playerPaddleWidth, aiPaddleHeight, aiPaddleWidth, playerPaddleMaxX, playerPaddleMaxY, playerPaddleMinX, playerPaddleMinY, aiPaddleMaxX, aiPaddleMaxY, aiPaddleMinX, aiPaddleMinY, ballWidth, ballHeight;

	private GameObject paddlePlayer, paddleAi;

	//specifies what angle the ball should bounce from
	private float bounceAngle;

	//grid coordinates which our ball will travel too
	private float vx, vy;

	//maximum angle of deflection our ball is allowed to travel in
	private float maxAngle = 45.0f;

	private bool collidedWithPlayer, collidedWithAi, collidedWithWall;

	private Game game;

	private bool assignPoint;



	// Use this for initialization
	void Start () {
		//Part 17 - searches for the Game script
		game = GameObject.Find ("Game").GetComponent<Game> ();

		if (moveSpeed < 0)
			moveSpeed = -1 * moveSpeed;

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
		playerPaddleMaxX = paddlePlayer.transform.localPosition.x + playerPaddleWidth / 1.8f;
		playerPaddleMinX = paddlePlayer.transform.localPosition.x - playerPaddleWidth / 1.8f;

		//this will check the X position of the paddle and create a boundary...
		//for the collision. 
		aiPaddleMaxX = paddleAi.transform.localPosition.x - aiPaddleWidth / 1.8f;
		aiPaddleMinX = paddleAi.transform.localPosition.x + aiPaddleWidth / 1.8f;
		
		//we will set our maxY and minY in the update method because the paddles...
		//are constantly moving and the value is always changing. 

		//can be left blank to use the default angles...
		//unless you want to specify.
		bounceAngle = GetRandomBounceAngle ();

		//will return x and y coordinates from angles. 
		vx = moveSpeed * Mathf.Cos (bounceAngle);
		vy = moveSpeed * -Mathf.Sin (bounceAngle);
	}
	
	// Update is called once per frame
	void Update () {


		if(game.gameState != Game.GameState.paused) {

		Move ();

		UpdateSpeedIncrease ();
		}
	}

	void UpdateSpeedIncrease () {
	
		if (speedIncreaseTimer >= speedIncreaseInterval) {

			speedIncreaseTimer = 0;
			
			if (moveSpeed > 0)
				moveSpeed += speedIncreaseBy;
			else 
				moveSpeed -= speedIncreaseBy;
		} else {

			speedIncreaseTimer += Time.deltaTime;
		}
	
	}

	//We want to know if the ball collided with either of the paddles
	bool CheckCollision () {

		//Part 6 - set our maxY and minY variables
		//this will check the current Y position of our paddle and add half the height of the paddle on both the top and bottom...
		//to create the boundary for the paddle. 
		playerPaddleMaxY = paddlePlayer.transform.localPosition.y + playerPaddleHeight / 1.8f;
		playerPaddleMinY = paddlePlayer.transform.localPosition.y - playerPaddleHeight / 1.8f;

		aiPaddleMaxY = paddleAi.transform.localPosition.y + aiPaddleHeight / 1.8f;
		aiPaddleMinY = paddleAi.transform.localPosition.y - aiPaddleHeight / 1.8f;
	
		//This will check to see if the ball will collide with the paddles' edge. (an x collision (sides))
		if (transform.localPosition.x - ballWidth / 1.8f < playerPaddleMaxX && transform.localPosition.x + ballWidth / 1.8f > playerPaddleMinX) {
			//this will check if the top/bottom of the paddles and ball will collide (a y collision)
			if (transform.localPosition.y - ballHeight  / 1.8f < playerPaddleMaxY && transform.localPosition.y + ballHeight / 1.8f > playerPaddleMinY) {

				//tell the ball to go right when it collides with the playerpaddle. 
				ballDirection = Vector2.right;
				collidedWithPlayer = true;
				//make sure the ball does not change direction from the inside of the paddle
				transform.localPosition = new Vector3 (playerPaddleMaxX + ballWidth / 1.8f, transform.localPosition.y, transform.localPosition.z);
				return true;

			} else {
				//part 17 - calls the game script point methods, if there is no collision, 
				//add a point. 
				if (!assignPoint) {
					assignPoint = true;
					game.aiPoint ();
				}

			}
		}
		//this will check if the ball collides with the ai_paddle. 
		if (transform.localPosition.x + ballWidth / 1.8f > aiPaddleMaxX && transform.localPosition.x - ballWidth / 1.8f < aiPaddleMinX){

			if (transform.localPosition.y - ballHeight / 1.8f < aiPaddleMaxY && transform.localPosition.y + ballHeight / 1.8f > aiPaddleMinY){

				ballDirection = Vector2.left;
				collidedWithAi = true;
				transform.localPosition = new Vector3 (aiPaddleMaxX - ballWidth / 1.8f, transform.localPosition.y, transform.localPosition.z);
				return true;
			} else {
				//part 17 - calls the game script point methods, if there is no collision, 
				//add a point. 
				if (!assignPoint) {
					assignPoint = true;
					game.playerPoint ();
				}
			}
		}
		//top bounds collision detection
		if (transform.localPosition.y > topBounds) {

			transform.localPosition = new Vector3 (transform.localPosition.x, topBounds, transform.localPosition.z);
			collidedWithWall = true;
			return true;
		
		}
		//bottom bounds collision detection
		if (transform.localPosition.y < bottomBounds) {
			transform.localPosition = new Vector3 (transform.localPosition.x, bottomBounds, transform.localPosition.z);
			collidedWithWall = true;
			return true;

		}

		return false;
	}
	//collision detection
	void Move () {

		if(!CheckCollision ()) {

			//for x - caluclate the bounce angle x movespeed 
			vx = moveSpeed * Mathf.Cos (bounceAngle);
			
			//for y - caluclate the angle based on whether the movement speed is positive or negative. 
			if (moveSpeed > 0)
				vy = moveSpeed * -Mathf.Sin (bounceAngle);
			else 
				vy = moveSpeed * Mathf.Sin (bounceAngle);

		//altered the synthax because the ball is going to be going either in a pos or neg X and will only go left or right...
		//for the Y axis because it depends only on which paddle it hits. 
		transform.localPosition += new Vector3 (ballDirection.x * vx * Time.deltaTime, vy * Time.deltaTime, 0);
		
		} else {

			 if (moveSpeed < 0)
				moveSpeed = -1 * moveSpeed; 

			//check to see if collide with player is true
			if(collidedWithPlayer) {

				//make it false because it wont be colliding in the next frame	
				collidedWithPlayer = false;
				float relativeIntersectY =  paddlePlayer.transform.localPosition.y - transform.localPosition.y;	
				float normalizeRelativeIntersectionY = (relativeIntersectY / (playerPaddleHeight /1.8f));

				bounceAngle = normalizeRelativeIntersectionY * (maxAngle * Mathf.Deg2Rad);
			
			} else if (collidedWithAi) {

				//make it false because it wont be colliding in the next frame	
				collidedWithAi = false;
				float relativeIntersectY =  paddleAi.transform.localPosition.y - transform.localPosition.y;	
				float normalizeRelativeIntersectionY = (relativeIntersectY / (playerPaddleHeight /1.8f));

				bounceAngle = normalizeRelativeIntersectionY * (maxAngle * Mathf.Deg2Rad);
				

			} else if (collidedWithWall) {
				collidedWithWall = false;

				bounceAngle = -bounceAngle;

			}
		}
	}
	//calculations to make the ball bounce at angles
	float GetRandomBounceAngle (float minDegrees = 160f, float maxDegrees = 260f) {
		
		//we need to convert the values into radians so our sine and cosine formulaes can...
		//output the x and y coordinates. 
		float minRad = minDegrees * Mathf.PI /180;
		float maxRad = maxDegrees * Mathf.PI /180;

		//will return the actual bounce angle. 
		return Random.Range (minRad, maxRad);

	}


}
