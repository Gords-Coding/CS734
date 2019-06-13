using UnityEngine;
using System.Collections;

public class SwipeGesture : MonoBehaviour {

	private float MIN_SWIPE_DIST = 100f;

	private Touch firstTouch = new Touch(); 
	private float distOfSwipe = 0;
	private bool swiped = false;
	// Update is called once per frame
	void Update () {
	
		foreach (Touch t in Input.touches) {

			if (t.phase == TouchPhase.Began) {
				firstTouch = t;

			} else if (t.phase == TouchPhase.Moved) {
				float changeX = firstTouch.position.x - t.position.x;
				float changeY = firstTouch.position.y - t.position.y;
			
			//using change of x and y, we can calculate the hypotenuse (swipe vector)
				float distOfSwipe = Mathf.Sqrt(Mathf.Pow (changeX, 2) + Mathf.Pow (changeY, 2));
				bool horizontal = Mathf.Abs(changeX) > Mathf.Abs (changeY);
			
				if (distOfSwipe >= MIN_SWIPE_DIST) {
				//Horizontal Swipes
					if (horizontal) { 
					//User swiped LEFT
						if (changeX >= 0) {
							this.transform.Translate(new Vector3 (-1, 0, 0));

					//User swiped RIGHT
						} else {
							this.transform.Translate(new Vector3 (1, 0, 0));
						}

				//Vertical Swips
					} else {
					//User swiped DOWN
						if (changeY >= 0) {
							this.transform.Translate(new Vector3 (0, 0, -1));
			
					//User swiped UP
						} else {
							this.transform.Translate(new Vector3 (0, 0, 1));

						}
					}
					swiped = true;
				}
			} else if (t.phase == TouchPhase.Ended) {
				firstTouch = new Touch();
				swiped = false;
			}
		}
	}
}
