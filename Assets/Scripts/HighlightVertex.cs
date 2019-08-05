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
		//polygon = GameObject.Find("PlayerPolygon").GetComponent<TestPolygon> ();	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public TestPolygon GetPolygon()
    {
        return polygon;
    }

	public void SetHighlightedVertex(TestPolygon whichPolygon, Vector3 v){
        polygon = whichPolygon;
		removalIndex = whichPolygon.verticesList.FindIndex (vec => vec.x == v.x && vec.y == v.y && vec.z == v.z);
    }
}
