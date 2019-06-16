using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerController : MonoBehaviour{

	float zDepth;
	Transform polygon;
	GameObject highlightGO;
	TestPolygon polygonScript;
	public bool highlightedOne;
	GameManager gm;
	public int numRemoved;
	Wall wall;
    public Camera mainCam;
    float width, height;
    Vector3 touchposition, mousePos;
    public bool touching;

    public bool mouseControlEnabled;

	// Use this for initialization
	void Start () {
		polygon = GameObject.Find ("PlayerPolygon").transform;
		//mainCam = GameObject.Find ("Main Camera").GetComponent<Camera> ();
		polygonScript = polygon.GetComponent<TestPolygon>();
		highlightGO = transform.Find ("Highlighter").gameObject;
		highlightedOne = false;
		gm = GameObject.FindObjectOfType<GameManager> ();
		numRemoved = 0;
		wall = GameObject.FindObjectOfType<Wall> ();

        width = (float)Screen.width / 2.0f;
        height = (float)Screen.height / 2.0f;
        touchposition = new Vector3(0f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        //the higher the polygon's z value, the farther it is from the camera
        zDepth = polygon.position.z + 3;
        
        //check if near a polygon vertex
        highlightedOne = false;
        touching = false;

        //touch input
        if (Input.touchCount > 0)
        {
            Debug.Log("touching");
            Touch touch = Input.GetTouch(0);

            Vector2 pos = touch.position;

            Debug.Log(pos);
            // Debug.Log("touchpos:" + pos);
            //pos.x = (pos.x - width) / width;
            //pos.y = (pos.y - height) / height;
            touchposition = mainCam.ScreenToWorldPoint(new Vector3(pos.x, pos.y, zDepth));
            Debug.Log("touchpos:" + touchposition);
            touching = true;
        }

        foreach (Vector3 vert in polygonScript.verticesList)
        {
            //regular, touch control
            if (touching && Vector3.Distance(touchposition, vert + new Vector3(wall.endPos.x, wall.endPos.y, 0f)) < .35f)
            {
                touching = true;
                highlightedOne = true;
                highlightGO.GetComponent<SpriteRenderer>().enabled = true;
                highlightGO.GetComponent<HighlightVertex>().SetHighlightedVertex(vert);
                highlightGO.GetComponent<HighlightVertex>().isHighlighting = true;
                TryRemoveVertex();
                //break out, can only remove 1 per update. also prevents concurrent modification of vertices list
                break;
            }

            //mouse control for debugging in editor
            if (mouseControlEnabled)
            {
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = zDepth;
                mousePos = mainCam.ScreenToWorldPoint(mousePos);
                //set this transform's position to the mouse's world position
                //Debug.Log("MousePos:" + mousePos);
                transform.position = mousePos;
                if (Vector3.Distance(mousePos, vert + new Vector3(wall.endPos.x, wall.endPos.y, 0f)) < .35f)
                {
                    highlightedOne = true;
                    highlightGO.GetComponent<SpriteRenderer>().enabled = true;
                    highlightGO.GetComponent<HighlightVertex>().SetHighlightedVertex(vert);
                    highlightGO.GetComponent<HighlightVertex>().isHighlighting = true;
                }
            }
        }
        //if we were not near any vertex, disable highlighter this frame
        if (!highlightedOne)
        {
            highlightGO.GetComponent<SpriteRenderer>().enabled = false;
        }

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Right Mouse Button Clicked ");
            UndoLastMove();
        }

      
    }

	void OnMouseDown(){
        if (mouseControlEnabled)
        {
            //if highlighter is highlighting something
            //remove the vertex its highlighting
            Debug.Log("Mouse clicked down");
            if (highlightedOne)
            {
                Debug.Log("One definitely highlighted!");
                if (polygonScript.verticesList.Count > 3)
                {
                    Debug.Log("vertices at least 3");
                    polygonScript.RemoveVertex(highlightGO.GetComponent<HighlightVertex>().removalIndex);
                    numRemoved += 1;

                    //if we've removed all and it matches, speed up wall to finish shape
                    if (numRemoved == gm.numToRemove)
                    {
                        TestPolygon hole = GameObject.Find("Wall").GetComponentInChildren<TestPolygon>();
                        bool success = gm.VerticesAreSame(hole, polygonScript);
                        if (success)
                        {
                            GameObject.FindObjectOfType<Wall>().speedingup = true;
                        }
                    }
                }

            }
        }
	}

    void TryRemoveVertex()
    {
        if (polygonScript.verticesList.Count > 3)
        {
            Debug.Log("vertices at least 3");
            polygonScript.RemoveVertex(highlightGO.GetComponent<HighlightVertex>().removalIndex);
            numRemoved += 1;

            //if we've removed all and it matches, speed up wall to finish shape
            if (numRemoved == gm.numToRemove)
            {
                TestPolygon hole = GameObject.Find("Wall").GetComponentInChildren<TestPolygon>();
                bool success = gm.VerticesAreSame(hole, polygonScript);
                if (success)
                {
                    GameObject.FindObjectOfType<Wall>().speedingup = true;
                }
            }
        }
    }

	void UndoLastMove(){
		if (polygonScript.removedStack.Count > 0) {
			IndexedVertex toReplace = polygonScript.removedStack.Pop ();
			polygonScript.verticesList.Insert (toReplace.index, toReplace.coords);
			polygonScript.ReDraw ();
		}
	}
		
}
