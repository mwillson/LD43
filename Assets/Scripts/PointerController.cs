using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PointerController : MonoBehaviour{

	float zDepth;
	Transform polygon;
	GameObject highlightGO;
	TestPolygon polygonScript;
	public bool highlightedOne;
	Manager gm;
	public int numRemoved;
	Wall wall;
    public Camera mainCam;
    float width, height;
    Vector3 touchposition, mousePos;
    public bool touching;

    public bool mouseControlEnabled;

    public Stack<PolygonVertexPair> removedVertices;

	// Use this for initialization
	void Start () {
		polygon = GameObject.Find ("PlayerPolygon").transform;
		//mainCam = GameObject.Find ("Main Camera").GetComponent<Camera> ();
		polygonScript = polygon.GetComponent<TestPolygon>();
		highlightGO = transform.Find ("Highlighter").gameObject;
		highlightedOne = false;
        removedVertices = new Stack<PolygonVertexPair>();

        if (SceneManager.GetActiveScene().name == "casual")
            gm = GameObject.FindObjectOfType<CasualGameManager>();
        else
            gm = GameObject.FindObjectOfType<GameManager>();
        
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

        foreach (TestPolygon tp in polygon.GetComponentsInChildren<TestPolygon>())
        {

            foreach (Vector3 vert in tp.verticesList)
            {
                Vector3 vertPositionAdjusted = tp.GetPositionAdjustedVert(vert);
                //regular, touch control
                if (touching && Vector3.Distance(touchposition, vertPositionAdjusted + new Vector3(wall.endPos.x, wall.endPos.y, 0f)) < .35f)
                {
                    touching = true;
                    highlightedOne = true;
                    highlightGO.GetComponent<SpriteRenderer>().enabled = true;
                    highlightGO.GetComponent<HighlightVertex>().SetHighlightedVertex(tp, vert);
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
                    if (Vector3.Distance(mousePos, vertPositionAdjusted + new Vector3(wall.endPos.x, wall.endPos.y, 0f)) < .35f)
                    {
                        
                        highlightedOne = true;
                        //highlightGO.GetComponent<SpriteRenderer>().enabled = true;
                        highlightGO.GetComponent<HighlightVertex>().SetHighlightedVertex(tp, vert);
                        highlightGO.GetComponent<HighlightVertex>().isHighlighting = true;
                    }
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
                TestPolygon highlightedPoly = highlightGO.GetComponent<HighlightVertex>().GetPolygon();
                if (highlightedPoly.verticesList.Count > 3)
                {
                    Debug.Log("vertices at least 3");
                    int removeIndex = highlightGO.GetComponent<HighlightVertex>().removalIndex;
                    IndexedVertex indVert = highlightedPoly.GetIndexedVertex(removeIndex);
                    highlightedPoly.RemoveVertex(removeIndex);
                    //add removal info to the stack
                    removedVertices.Push(new PolygonVertexPair(highlightedPoly, indVert));

                    numRemoved += 1;
                    gm.numToRemove -= 1;
                    gm.UpdateRemoveText();
                    //if we've removed the correct amount and it matches, speed up wall to finish shape
                    if (gm.numToRemove == 0)
                    {
                        bool success = false;
                        //reworked so that it accounts for all holes
                        foreach (TestPolygon tp in polygon.GetComponentsInChildren<TestPolygon>())
                        {
                            TestPolygon hole = tp.holeToMatch;
                            bool individualsuccess = gm.VerticesAreSame(hole, tp);
                            if (!individualsuccess)
                            {
                                success = false;
                                break;
                            }
                            success = true;

                        }
                        //all checked were success, so speed up wall
                        if(success)GameObject.FindObjectOfType<Wall>().speedingup = true;
                        
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
            int removeIndex = highlightGO.GetComponent<HighlightVertex>().removalIndex;
            IndexedVertex indVert = polygonScript.GetIndexedVertex(removeIndex);
            polygonScript.RemoveVertex(removeIndex);
            //add removal info to the stack
            removedVertices.Push(new PolygonVertexPair(polygonScript, indVert));

            numRemoved += 1;
            gm.numToRemove -= 1;
            gm.UpdateRemoveText();
            //if we've removed all and it matches, speed up wall to finish shape
            if (gm.numToRemove == 0)
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
		if (removedVertices.Count > 0) {
            PolygonVertexPair toReplace = removedVertices.Pop();
            TestPolygon toAddTo = toReplace.poly;
			IndexedVertex vertToAdd = toReplace.vertex;
            Debug.Log("toaddto " + toAddTo);
            Debug.Log("vertoAdd " + vertToAdd);
            Debug.Log("vertices list? " + toAddTo.verticesList);

            toAddTo.verticesList.Insert (vertToAdd.index, vertToAdd.coords);
			toAddTo.ReDraw ();
            gm.numToRemove += 1;
            gm.UpdateRemoveText();
        }
	}
		
}

public struct PolygonVertexPair
{
    public TestPolygon poly;
    public IndexedVertex vertex;

    public PolygonVertexPair(TestPolygon p, IndexedVertex v)
    {
        poly = p;
        vertex = v;
    }
}
