using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

	[SerializeField]
	public GameObject polygonPrefab;

	GameManager gm;

	bool moving;
	public bool speedingup;

	public float speed;

	public Vector3 startPos, endPos;

	// Use this for initialization
	void Start () {
		gm = GameObject.FindObjectOfType<GameManager> ();
		speed = .05f;
		speedingup = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (moving) {
			//linear increase in speed if speeding up?
			if (speedingup)
				speed += .01f;
			transform.position = Vector3.MoveTowards (transform.position, endPos, speed);
		}
		if (transform.position == endPos && moving) {
			moving = false;
			gm.WallCycle ();
		}
	}

	public void CreateHoleShape(List<Vector3> vertices, int numToRemove){
		List<Vector3> listCopy = new List<Vector3> ();
		listCopy.AddRange (vertices);
		//remove as many verts as we need
		for (int i = 0; i < numToRemove; i++){
			listCopy.RemoveAt (UnityEngine.Random.Range (0, listCopy.Count));
		}
		GameObject newPoly = (GameObject)Instantiate (polygonPrefab, transform);
		newPoly.GetComponent<TestPolygon> ().verticesList = listCopy;
		newPoly.GetComponent<TestPolygon>().vertices2D = System.Array.ConvertAll<Vector3, Vector2>(listCopy.ToArray(), v => v);
		newPoly.GetComponent<TestPolygon> ().color = Color.black;
		Debug.Log ("End hole polygon setup");

		moving = true;
	}

}
