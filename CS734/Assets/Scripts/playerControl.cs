using UnityEngine;
using System.Collections;

public class playerControl : MonoBehaviour {
	private Touch initialTouch = new Touch();
	private float distance = 0;
	private bool hasSwiped = false;
	private bool isPause;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.timeScale == 0) {
			isPause = true;
		} else {
			isPause = false;
		}
		if (Input.anyKeyDown && !isPause) {
			if (Input.GetKey (KeyCode.LeftArrow) && !hasSwiped) {
				Vector3 position = this.transform.position;
				if(position.x > -3) {
					position.x--;
					this.transform.position = position;
				}
				hasSwiped = true;
				Debug.Log ("position.x: " + position.x + " position.y: " + position.y);
			}
			if (Input.GetKey (KeyCode.RightArrow) && !hasSwiped && !isPause) {
				Vector3 position = this.transform.position;
				if(position.x < 3) {
					position.x++;
					this.transform.position = position;
				}
				hasSwiped = true;
				Debug.Log ("position.x: " + position.x + " position.z: " + position.z);
			}
			if (Input.GetKey (KeyCode.DownArrow) && !hasSwiped && !isPause) {
				Vector3 position = this.transform.position;
				if(position.z > -3) {
					position.z--;
					this.transform.position = position;
				}
				hasSwiped = true;
				Debug.Log ("position.x: " + position.x + " position.z: " + position.z);
			}
			if (Input.GetKey (KeyCode.UpArrow) && !hasSwiped && !isPause) {
				Vector3 position = this.transform.position;
				if(position.z < 3) {
					position.z++;
					this.transform.position = position;
				}
				hasSwiped = true;
				Debug.Log ("position.x: " + position.x + " position.z: " + position.z);
			}
			hasSwiped = false;
		}

		foreach (Touch t in Input.touches) { //When it touches the screen
			if(t.phase == TouchPhase.Began) { //When the user first presses their finger on the screen
				initialTouch = t;
			}else if(t.phase == TouchPhase.Moved && !hasSwiped) { //Swiping their finger
				float deltaX = initialTouch.position.x - t.position.x;
				float deltaY = initialTouch.position.y - t.position.y;
				
				//Calculate the distance
				distance = Mathf.Sqrt(Mathf.Pow (deltaX, 2) + Mathf.Pow (deltaY, 2));
				
				//Calculate direction
				bool swipedSideways = Mathf.Abs(deltaX) > Mathf.Abs (deltaY);
				
				if(distance > 100f) {//if distance > 100pixels we have a swipe
					if(swipedSideways && deltaX > 0) { //Swiped left
						Vector3 position = this.transform.position;
						if(position.x > -3 && !isPause) {
							position.x--;
							this.transform.position = position;
						}
					}else if(swipedSideways && deltaX < 0) { //Swiped right
						Vector3 position = this.transform.position;
						if(position.x < 4 && !isPause) {
							position.x++;
							this.transform.position = position;
						}
					}else if(!swipedSideways && deltaY > 0) { //Swiped down
						Vector3 position = this.transform.position;
						if(position.z > -4 && !isPause) {
							position.z--;
							this.transform.position = position;
						}
					}else if (!swipedSideways && deltaY < 0){ //Swiped up
						Vector3 position = this.transform.position;
						if(position.z < 3 && !isPause) {
							position.z++;
							this.transform.position = position;
						}
					}
					hasSwiped = true;
				}

				
			}else if(t.phase == TouchPhase.Ended) { //Lifted finger off the screen
				initialTouch = new Touch();
				hasSwiped = false;
			}
		}
	}
}
