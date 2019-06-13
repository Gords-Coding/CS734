using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {

	private GameController gameController;

	// Use this for initialization
	void Start () {
		GameObject gameControllerObj = GameObject.FindWithTag ("GameController");
		gameController = gameControllerObj.GetComponent<GameController> ();
		GetComponent<Rigidbody>().velocity = transform.forward * gameController.rocketSpeed;
	}

}
