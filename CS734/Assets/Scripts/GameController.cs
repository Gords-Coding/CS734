using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public GameObject[,] floorTiles;
	public GameObject floorTile;
	public GameObject rocketObject;
	public float spawnTime;

	private bool isGameOver;
	private bool isPause;
	private float gameTime;
	private float pauseTime;

	//TEXT
	public Text gameOverText;
	public Text pausedText;
	public Text playerHealthText;
	public Text timeText;

	//BUTTONS
	public Button retryButton;
	public Button mainmenuButton;
	public Button pauseButton;
	public Button pauseButtonPanel;
	public RectTransform pauseEndPanel;

	public AudioClip[] destroySounds;

	public float rocketSpeed;

	void Start () {
		isGameOver = false;
		isPause = false;
		gameTime = 0f;
		pauseTime = 0f;
		if (Time.timeScale == 0) {
			Time.timeScale = 1;
		}
		pauseButton.gameObject.SetActive (true);
		pauseEndPanel.gameObject.SetActive(false);
		pausedText.gameObject.SetActive (false);
		retryButton.gameObject.SetActive (false);
		mainmenuButton.gameObject.SetActive (false);



		//instantiate the floor starting from 1,1
		floorTiles = new GameObject[7, 7];
		Vector3 spawnPoint = floorTile.transform.position;
		Quaternion rotation = floorTile.transform.rotation;

		spawnPoint.x = -3; spawnPoint.z = 3;

		//All floor tiles are saved into the 7 by 7 floorTiles array

		for (int i = 0; i < floorTiles.GetLength (0); i++) {
			if (i > 0) {
				spawnPoint.z--;
				spawnPoint.x = -3;
			}
			for (int j = 0; j < floorTiles.GetLength (1); j++) {
				if (j > 0) {
					spawnPoint.x++;
				}
				//Individual floor tiles stored into a 7x7 array, same format as they are
				//displayed on the game screen (1,1 top left, 7,7 bottom right);
				floorTiles[i,j] = Instantiate(floorTile, spawnPoint, rotation) as GameObject;
			}
		}
		PlayerPrefs.SetFloat("SessionTime", Time.realtimeSinceStartup);
		UpdatePlayerHealth (3);
		StartCoroutine (spawnTimer ());
	}



	// Update is called once per frame
	void Update () {
		gameTime = Time.realtimeSinceStartup - PlayerPrefs.GetFloat ("SessionTime");
		if (isPause) {
			gameTime = pauseTime;
			PlayerPrefs.SetFloat("SessionTime", Time.realtimeSinceStartup);
		}
		//Game is still running
		if (!isGameOver && !isPause) {
			gameTime = Time.realtimeSinceStartup - PlayerPrefs.GetFloat ("SessionTime") + pauseTime;
			

			timeText.text = gameTime.ToString ("0.00") + " sec";

			int gameTimeINT = int.Parse(gameTime.ToString ("0"));

		//every 5 second, the spawn rate increases by a slight amount
			if (gameTimeINT % 5 == 0 && spawnTime > 0.8f) {
				spawnTime = spawnTime - 0.1f;
				rocketSpeed = rocketSpeed + 0.05f;
			}
		} 
		//return to the main menu on back button press
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.LoadLevel ("MainMenu");
		}

	}


	//routine to constantly spawn objects
	IEnumerator spawnTimer () {

		while (true) {
			yield return new WaitForSeconds (spawnTime);
			GameObject spawnThis = rocketObject;
			spawnIntoWorld (spawnThis);
			yield return new WaitForSeconds (spawnTime);

			if (isGameOver) {
				retryButton.gameObject.SetActive (true);
				mainmenuButton.gameObject.SetActive (true);
				break;
			}
		}

	}
	public void playSound(int clip) {
		GetComponent<AudioSource>().clip = destroySounds [clip];
		GetComponent<AudioSource>().Play ();
	}

	public void UpdatePlayerHealth(int health) {
		playerHealthText.text = "Health: " + health;
	}

	public void MainMenu() {
		Application.LoadLevel ("MainMenu");
	}

	public void Restart() {
		Application.LoadLevel ("Game");
	}
	public void Pause() {
		//unpaused
		if (isPause) {

			retryButton.gameObject.SetActive (false);
			mainmenuButton.gameObject.SetActive (false);
			pausedText.gameObject.SetActive (false);
			pauseEndPanel.gameObject.SetActive(false);
			Time.timeScale = 1;
			isPause = false;

		} else {
			retryButton.gameObject.SetActive (true);
			mainmenuButton.gameObject.SetActive (true);
			pausedText.gameObject.SetActive (true);
			pauseEndPanel.gameObject.SetActive(true);
			pauseTime = gameTime;
			Time.timeScale = 0;
			isPause = true;

		}
	}

	//Called when player reaches 0 health
	public void GameOver() {
		playSound(0);
		pauseEndPanel.gameObject.SetActive(true);
		pauseButtonPanel.gameObject.SetActive (false);
		pauseButton.gameObject.SetActive (false);
		gameOverText.text = "GAME OVER";
		isGameOver = true;
		isPause = false;

	}


//Taking 1 of the diagonal tiles from the array floorTiles, we can randomly pick where to spawn
//the next projectile / attacking object

	private void spawnIntoWorld (GameObject obj) {
		//outer coordinates depending on location, use negatives as needed
		float newX = 5f;
		float newZ = 5f;
		float newY = obj.transform.position.y;
		float n = -1f;
		float newYRot = 0f;
		// 1) pick a random tile - randTile
		// 2) pick left = 0, right = 1, top = 2, bottom = 3
		// ALL OBJECTS SHOULD INITIALLY POINT DOWNWARDS

		int randTile = Random.Range (0, 7);
		int dir = Random.Range (0, 4);

		Vector3 spawnPos = floorTiles [randTile, randTile].transform.position;    //coords of one of the diag tiles will be stored here
		Quaternion spawnRot = obj.transform.rotation;

		//set new spawn points
		//set new rotations, rotations act on the Y Axis
		switch (dir) {
			case 0:	//LEFT
				spawnPos.x = n * newX;
				newYRot = 90;
				break;

			case 1: //RIGHT
				spawnPos.x = newX;
				newYRot = -90;
				break;

			case 2: //TOP
				spawnPos.z = newZ;
				newYRot = 180;
				break;

			case 3: //BOTTOM
				spawnPos.z = n * newZ;
				newYRot = 0;
				break;
		

		}


		spawnRot = Quaternion.Euler (obj.transform.eulerAngles.x, newYRot,obj.transform.eulerAngles.z);
		spawnPos.y = newY;
		//Spawning of object

		Instantiate (obj, spawnPos, spawnRot);
	}
}
