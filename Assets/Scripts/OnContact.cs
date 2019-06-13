using UnityEngine;
using System.Collections;

public class OnContact : MonoBehaviour {

	private GameController gameController;
	private GameObject explosion;
	public GameObject explosionContact;
	public GameObject playerExplosion;

	public AudioClip explosionSound;

	// Use this for initialization
	void Start () {
		GameObject gameControllerObj = GameObject.FindWithTag ("GameController");
		explosion = explosionContact;
		GetComponent<AudioSource>().clip = explosionSound;
		if (gameControllerObj != null) {
			gameController = gameControllerObj.GetComponent<GameController>();
		}

	}

	void OnTriggerEnter (Collider other) {
		if (other.tag == "Boundary") {
			return;
		}
		//Stop the enemies from crashing into each other
		if (other.tag == "Enemy") { 
			Physics.IgnoreCollision (other.GetComponent<Collider>(), this.GetComponent<Collider>());
			return;
		}
		if (other.tag == "Player") {
			this.gameObject.GetComponent<AudioSource>().Play ();
			int health = other.GetComponent<PlayerInfo>().health;
			if (health > 0) {
				other.GetComponent<PlayerInfo>().health--;
				explosion = explosionContact;

			} else {
				Instantiate (playerExplosion, other.transform.position, playerExplosion.transform.rotation);
				Destroy (other.gameObject);
				gameController.GameOver ();

			}
			gameController.UpdatePlayerHealth(health);
		}
		other.GetComponent<AudioSource>().Play ();
		Instantiate (explosion, gameObject.transform.position, explosion.transform.rotation);
		Destroy (this.gameObject);
	}

	void playSound() {
		this.gameObject.GetComponent<AudioSource>().Play ();
	}
}
