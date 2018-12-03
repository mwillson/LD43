using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

	[SerializeField]
	public GameObject polygonPrefab;

	GameManager gm;

	bool moving;

	// Use this for initialization
	void Start () {
		gm = GameObject.FindObjectOfType<GameManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (moving) {
			transform.position = Vector3.MoveTowards (transform.position, new Vector3 (0f, 0f, .08f), .05f);
		}
		if (transform.position == new Vector3 (0f, 0f, .08f) && moving) {
			moving = false;
			gm.WallCycle ();
		}
	}

	public void CreateHoleShape(List<Vector3> vertices){
		List<Vector3> listCopy = new List<Vector3> ();
		listCopy.AddRange (vertices);
		listCopy.RemoveAt(UnityEngine.Random.Range(0, listCopy.Count));
		GameObject newPoly = (GameObject)Instantiate (polygonPrefab, transform);
		newPoly.GetComponent<TestPolygon> ().verticesList = listCopy;
		newPoly.GetComponent<TestPolygon>().vertices2D = System.Array.ConvertAll<Vector3, Vector2>(listCopy.ToArray(), v => v);
		newPoly.GetComponent<TestPolygon> ().color = Color.black;
		Debug.Log ("End hole polygon setup");

		moving = true;
	}


}
