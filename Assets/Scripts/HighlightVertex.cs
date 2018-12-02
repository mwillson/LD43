using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightVertex : MonoBehaviour {

	TestPolygon polygon;
	[SerializeField]
	public int removalIndex;
	public bool isHighlighting;

	// Use this for initialization
	void Start () {
		polygon = GameObject.FindObjectOfType<TestPolygon> ();	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetHighlightedVertex(Vector3 v){
		removalIndex = polygon.verticesList.FindIndex (vec => vec.x == v.x && vec.y == v.y && vec.z == v.z);
	}
}
