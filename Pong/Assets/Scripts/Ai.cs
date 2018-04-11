﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Part 3 - For now, I am going to have the Ai Paddle mimic...
//the players movements. To do this, I copied and pasted the code...
//from Player.cs and made a few minor adjustments.  
public class Ai : MonoBehaviour {

	//Variables to determine the Movement speed, Top and Bottom boundaries...
	//and the Starting Position. 

	public float moveSpeed = 8.0f;
	public float topBounds = 8.3f;
	public float bottomBounds = -8.3f;

	//for the starting position, we changed the value to positive 13...
	//instead of negative. 
	public Vector2 startingPosition = new Vector2 (13.0f, 0.0f);


	// Use this for initialization
	void Start () {

	transform.localPosition = (Vector3)startingPosition;		
	}
	
	// Update is called once per frame
	void Update () {

		//Call the method
		CheckUserInput ();

	}

	void CheckUserInput () {
		
		// check if the user is pressing Up or Down

		if (Input.GetKey (KeyCode.UpArrow)) {
			if (transform.localPosition.y > topBounds) {
				
				//check to see if while we are moving up, 
				//if we are passed the topBounds. 
				transform.localPosition = new Vector3 (transform.localPosition.x, topBounds, transform.localPosition.z);
			
			} else {
				//otherwise, we are going to allow the paddle to move
				transform.localPosition += Vector3.up * moveSpeed * Time.deltaTime;

			}

		} else if (Input.GetKey (KeyCode.DownArrow)) {

			//opposite for the bottom boundary
			if (transform.localPosition.y < bottomBounds) {
				
				transform.localPosition = new Vector3 (transform.localPosition.x, bottomBounds, transform.localPosition.z);
			
			} else {

				transform.localPosition += Vector3.down * moveSpeed * Time.deltaTime;

			}
		}
	}
}