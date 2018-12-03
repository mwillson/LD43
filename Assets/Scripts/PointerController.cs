using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerController : MonoBehaviour {

	float zDepth;
	Transform polygon;
	GameObject highlightGO;
	TestPolygon polygonScript;
	bool highlightedOne;

	// Use this for initialization
	void Start () {
		polygon = GameObject.Find ("PlayerPolygon").transform;
		//mainCam = GameObject.Find ("Main Camera").GetComponent<Camera> ();
		polygonScript = polygon.GetComponent<TestPolygon>();
		highlightGO = transform.Find ("Highlighter").gameObject;
		highlightedOne = false;
	}
	
	// Update is called once per frame
	void Update () {
		//the higher the polygon's z value, the farther it is from the camera
		zDepth = polygon.position.z + 3;
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = zDepth;
		mousePos = Camera.main.ScreenToWorldPoint (mousePos);
		//Debug.Log ("mouse: " + mousePos);
		//set this transform's position to the mouse's world position
		transform.position = mousePos;
		//check if near a polygon vertex
		highlightedOne = false;
		foreach (Vector3 vert in polygonScript.verticesList) {
			if (Vector3.Distance (mousePos, vert) < .25f) {
				highlightedOne = true;
				highlightGO.GetComponent<SpriteRenderer> ().enabled = true;
				highlightGO.GetComponent<HighlightVertex> ().SetHighlightedVertex (vert);
				highlightGO.GetComponent<HighlightVertex> ().isHighlighting = true;
			}
		}
		//if we were not near any vertex, disable highlighter this frame
		if (!highlightedOne) {
			highlightGO.GetComponent<SpriteRenderer> ().enabled = false;
		}
	}

	void OnMouseDown(){
		//if highlighter is highlighting something
		//remove the vertex its highlighting
		if (highlightedOne && polygonScript.verticesList.Count > 3) {
			Debug.Log ("clicked while highlighted");

			polygonScript.RemoveVertex(highlightGO.GetComponent<HighlightVertex>().removalIndex);
		}
	}
}
