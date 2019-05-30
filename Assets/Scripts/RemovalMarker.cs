using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovalMarker : MonoBehaviour {
    
    float scale;

    public Vector3 vert;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //should oscillate between .8f and 1.2f
        scale = (.15f * Mathf.Sin(3f * Mathf.PI * Time.time)) + 1f;
        transform.localScale = new Vector3(scale, scale, 1f);
	}
}
