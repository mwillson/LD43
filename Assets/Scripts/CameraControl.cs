using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ScreenShake(float amount, float vrange, float hrange){
		StartCoroutine (Shake (amount, vrange, hrange));
	}

	//shakes screen for amt time, shake range is represented by vrng(vertical) and hrange(horizontal)
	public IEnumerator Shake(float amt, float vrng, float hrng){
		GameObject.FindObjectOfType<GameManager> ().cameraWait = true;
		Vector3 origpos = transform.position;
		//shake for amt time
		float endTime = Time.time + amt;
		//every .05 second, add random direction shift
		while (Time.time < endTime) {
			transform.position += new Vector3(Random.Range (-1*hrng, hrng), Random.Range (-1*vrng, vrng), 0f);
			//shake by changing position 8 times
			yield return new WaitForSeconds (.02f);
		}
		//return camera to original pre-shake position
		transform.position = origpos;
		GameObject.FindObjectOfType<GameManager> ().cameraWait = false;

	}
}
