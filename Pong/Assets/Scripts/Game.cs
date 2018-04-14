using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

	//Part 5 - Created a method to spawn the ball in the screen.

	private GameObject ball;
	
	private int aiScore;

	private int playerScore;

	private GameObject hudCanvas;

	private Hud hud;

	private GameObject paddleAi;

	public int winningScore = 2;
	
	private Game game;

	//part 17 - adding a game state because we do not want nextround to be called if a player scores
	public enum GameState {

		playing,
		gameOver,
		paused,
		launched
	
	}

	public GameState gameState = GameState.launched;

	// Use this for initialization
	void Start () {

		paddleAi = GameObject.Find ("ai_paddle");

		hudCanvas = GameObject.Find ("HUD_Canvas");
		hud = hudCanvas.GetComponent<Hud> ();

		hud.playAgain.text = "PRESS SPACEBAR TO PLAY";

	}
	
	// Update is called once per frame
	void Update () {
		
		checkScore ();

		checkInput ();
}

	void checkInput () {

		//if game is in paused or in playing, execuse the pauseresumegame method. 
		if (gameState == GameState.paused || gameState == GameState.playing) {
			
			if(Input.GetKeyUp (KeyCode.Space)) {
				pauseResumeGame ();
			}
		}

		//if the game is launched or in a game over state, upon release of the spacebar...
		if (gameState == GameState.launched || gameState == GameState.gameOver) {

			if(Input.GetKeyUp (KeyCode.Space)) {
			//...the game will start again
				StartGame ();
			}
		}
		

	}

	//part 17
	void checkScore () {

		//check if the player and ai's score is greater than or equal too the winning score cap (2)
		if (playerScore >= winningScore || aiScore >= winningScore) {

			
			if(playerScore >= winningScore && aiScore < playerScore -1) {

				//Player wins
				playerWins ();

			} else {

				if(aiScore >= winningScore && playerScore < aiScore -1) {

					//Ai Wins
					aiWins ();

				}
			}
		}
	}

	void SpawnBall () {

		//Part 5 - Creates the ball by looking for the ball... 
		//by looking for the ball gameobject.
		 
		ball = GameObject.Instantiate ((GameObject)Resources.Load("Prefabs/ball", typeof (GameObject)));
		ball.transform.localPosition = new Vector3 (12, 0, -2);

	}
	//part 17
	private void playerWins () {

		hud.winPlayer.enabled = true;
		gameOver ();

	}
	//part 17
	private void aiWins () {

		hud.winAi.enabled = true;
		gameOver ();
	}



	public void aiPoint () {

		aiScore++;
		hud.aiScore.text = aiScore.ToString ();
		nextRound ();
	}


	public void playerPoint () {

		playerScore++;
		hud.playerScore.text = playerScore.ToString ();
		nextRound ();

	}

	private void StartGame () {

		playerScore = 0;
		aiScore = 0;

		hud.playerScore.text = "0";
		hud.aiScore.text = "0";
		hud.winPlayer.enabled = false;
		hud.winAi.enabled = false;

		hud.playAgain.enabled = false;

		gameState = GameState.playing;

		paddleAi.transform.localPosition = new Vector3 (paddleAi.transform.localPosition.x, 0, paddleAi.transform.localPosition.z);

		SpawnBall ();
	}

	//when the score is increased, respawn the ball and reset the ai paddle.
	private void nextRound () {

		if(gameState == GameState.playing) {
			//reset the paddle position
			paddleAi.transform.localPosition = new Vector3 (paddleAi.transform.localPosition.x, 0, paddleAi.transform.localPosition.z);
			//destroy the ball
			GameObject.Destroy (ball.gameObject);
			//respawn the ball by calling the SpawnBall method
			SpawnBall ();
		}

	}

	//part 17
	private void gameOver () {

		GameObject.Destroy (ball.gameObject);
		hud.playAgain.text = ("PRESS SPACEBAR TO PLAY AGAIN");
		hud.playAgain.enabled = true;
		gameState = GameState.gameOver;
	}

	//pause and restart the game
	private void pauseResumeGame () {

		if (gameState == GameState.paused) {
			
			gameState = GameState.playing;
			hud.playAgain.enabled = false;


		} else {
			
			gameState = GameState.paused;
			hud.playAgain.text = "GAME IS PAUSED - PRESS PLAY TO CONTINUE PLAYING";
			hud.playAgain.enabled = true;

		}
	}

}
