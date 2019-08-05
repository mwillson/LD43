using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using cakeslice;

public class TestPolygon : MonoBehaviour
{
	public List<Vector3> verticesList;

	public Vector2[] vertices2D;

	[SerializeField]
	public Color color;

	[SerializeField]
	public Stack<IndexedVertex> removedStack;

    [SerializeField]
    public GameObject markerPrefab;

    [SerializeField]
    public GameObject linePrefab;

    public List<int> linesDoneDrawing;

    [SerializeField]
    public Transform markerParent;

    [SerializeField]
    public TestPolygon holeToMatch;

    [SerializeField]
    public int numToRemove;

    public void Awake()
    {
        removedStack = new Stack<IndexedVertex>();
    }

	private void Start () {

		Debug.Log ("polygon start");
		var vertices3D = System.Array.ConvertAll<Vector2, Vector3>(vertices2D, v => v);

		verticesList = new List<Vector3> (vertices3D);
        linesDoneDrawing = new List<int>();
        
		// Use the triangulator to get indices for creating triangles
		var triangulator = new Triangulator(vertices2D);
		var indices =  triangulator.Triangulate();

		// Generate a color for each vertex
		Color[] colors = new Color[vertices3D.Length];

		for (int i = 0; i < colors.Length; i++) {
			colors [i] = color;
		}

		// Create the mesh
		var mesh = new Mesh {
			vertices = vertices3D,
			triangles = indices,
			colors = colors
		};

		mesh.RecalculateNormals();
		mesh.RecalculateBounds();

		// Set up game object with mesh
		MeshRenderer meshRenderer;
		if (GetComponent<MeshRenderer> () == null) {
			meshRenderer = gameObject.AddComponent<MeshRenderer> ();
		} else {
			meshRenderer = gameObject.GetComponent<MeshRenderer> ();
		}

        //set this stuff on start for the 'hole' polygon
        if (transform.parent.name != "PlayerPolygon")
        {
            meshRenderer.material = new Material(Shader.Find("Sprites/Default"));

            //lower alpha if player (clickable) polygon
            if (gameObject.name == "PlayerPolygon")
            {
                Color myColor = meshRenderer.material.GetColor("_Color");
                myColor.a = .7f;
                meshRenderer.material.SetColor("_Color", myColor);
            }
            MeshFilter filter;
            if (GetComponent<MeshFilter>() == null)
            {
                filter = gameObject.AddComponent<MeshFilter>();
            }
            else
            {
                filter = gameObject.GetComponent<MeshFilter>();
            }
            filter.mesh = mesh;
            FindLines(vertices3D);
        }

		gameObject.AddComponent<Outline> ();
		GetComponent<Outline> ().enabled = false;
		GetComponent<Outline> ().color = 1;

		
	}
		
	public void Setup(){
		Debug.Log ("polygon start");
		var vertices3D = System.Array.ConvertAll<Vector2, Vector3>(vertices2D, v => v);

		verticesList = new List<Vector3> (vertices3D);

		// Use the triangulator to get indices for creating triangles
		var triangulator = new Triangulator(vertices2D);
		var indices =  triangulator.Triangulate();

		// Generate a color for each vertex
		Color[] colors = new Color[vertices3D.Length];

		for (int i = 0; i < colors.Length; i++) {
			colors [i] = color;
		}

		// Create the mesh
		var mesh = new Mesh {
			vertices = vertices3D,
			triangles = indices,
			colors = colors
		};

		mesh.RecalculateNormals();
		mesh.RecalculateBounds();

        // Set up game object with mesh
        MeshRenderer meshRenderer;
        if (GetComponent<MeshRenderer>() == null)
        {
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
        }
        else
        {
            meshRenderer = gameObject.GetComponent<MeshRenderer>();
        }
        meshRenderer.material = new Material(Shader.Find("Sprites/Default"));

        MeshFilter filter;
        if (GetComponent<MeshFilter>() == null)
        {
            filter = gameObject.AddComponent<MeshFilter>();
        }
        else
        {
            filter = gameObject.GetComponent<MeshFilter>();
        }
        filter.mesh = mesh;

        //gameObject.AddComponent<Outline> ();
        //GetComponent<Outline> ().enabled = false;
        //GetComponent<Outline> ().color = 1;
        markerParent = transform;
    }

	//remove vertex at index i
	public void RemoveVertex(int i){
        Vector3 vertCopy = new Vector3(verticesList[i].x, verticesList[i].y, verticesList[i].z);
        Debug.Log("removing vert");
        foreach (Transform markerTF in GameObject.Find("Markers").transform)
        {
            Vector3 mvert = markerTF.GetComponent<RemovalMarker>().vert;
            if (mvert.x == vertCopy.x && mvert.y == vertCopy.y)
            {
                Debug.Log("found matching marker!");
                Destroy(markerTF.gameObject);
                break;
            }
        }

		removedStack.Push (new IndexedVertex (vertCopy, i));
		verticesList.RemoveAt (i);
		ReDraw ();
	}

	public void ResetRemovedStack(){
		removedStack.Clear ();
	}

    //start a coroutine for each vertex that draws a line from it to it's neighbor
    public void OutlineFadeInEffect()
    {
        GetComponent<MeshRenderer>().enabled = false;
        int i = 0;
        Vector3 nextNeighbor = new Vector3(0,0,0);
        foreach(Vector3 vert in verticesList)
        {
            
            if (i < verticesList.Count - 1)
                nextNeighbor = verticesList.ElementAt(i + 1);
            else if (i == (verticesList.Count - 1))
                nextNeighbor = verticesList.ElementAt(0);
            else
                Debug.LogError("How did we get outside of range of vertices list?");

            StartCoroutine(AnimatedLine(vert, new Vector3(vert.x, vert.y, vert.z), new Vector3(nextNeighbor.x, nextNeighbor.y, nextNeighbor.z)));
            i += 1;
        }
    }

    //Draws a line over a short time period between two points in space
    IEnumerator AnimatedLine(Vector3 originalVert, Vector3 first, Vector3 second)
    {
        GameObject newLineObj = Instantiate(linePrefab, transform);
        LineRenderer line = newLineObj.GetComponent<LineRenderer>();
        Vector3 newEnd = new Vector3(first.x, first.y, first.z);
        while (Mathf.Abs(Vector3.Distance(newEnd, second)) > .02f)
        {
            float lineLength = Vector3.Distance(first, second);
            newEnd = Vector3.MoveTowards(newEnd, second, lineLength/10f);
            line.SetPositions(new Vector3[2] { first, newEnd });
            line.transform.GetChild(0).localPosition = newEnd;
            yield return null;
        }
        int vertIndex = verticesList.FindIndex(v => v.x == originalVert.x && v.y == originalVert.y);
        linesDoneDrawing.Add(vertIndex);
        CheckLineDrawingIndices();
    }

    void CheckLineDrawingIndices()
    {
        for(int i =0; i < verticesList.Count; i++)
        {
            //for each index in vertices list
            //if we find one that isn't in 'done drawing' list, don't finish function
            if (!linesDoneDrawing.Contains(i)) return;
        }
        //if we found all indices, all lines are done. time to clear and draw actual polygon
        linesDoneDrawing.Clear();
        foreach (Transform lineTF in transform) Destroy(lineTF.gameObject);
        GetComponent<MeshRenderer>().enabled = true;
        ReDraw();
    }

	public void ReDraw(){
		var vertices3D = verticesList.ToArray ();
		var vertices2D = System.Array.ConvertAll<Vector3, Vector2>(vertices3D, v => v);

		// Use the triangulator to get indices for creating triangles
		var triangulator = new Triangulator(vertices2D);
		var indices =  triangulator.Triangulate();

		// Generate a color for each vertex
		//var colors = Enumerable.Range(0, vertices3D.Length)
		//	.Select(i => Random.ColorHSV())
		//	.ToArray();
		// Generate a color for each vertex
		Color[] colors = new Color[verticesList.Count];

		for (int i = 0; i < colors.Length; i++) {
			colors [i] = color;
		}

		var mesh = new Mesh {
			vertices = vertices3D,
			triangles = indices,
			colors = colors
		};

		mesh.RecalculateNormals();
		mesh.RecalculateBounds();

		// Set up game object with mesh;
		var meshRenderer = gameObject.GetComponent<MeshRenderer>();
		meshRenderer.material = new Material(Shader.Find("Sprites/Default"));
        //polygon player interacts with is slightly see-through
        if (transform.parent.name == "PlayerPolygon")
        {
            Color myColor = meshRenderer.material.GetColor("_Color");
            myColor.a = .7f;
            meshRenderer.material.SetColor("_Color", myColor);
        }

        FindLines(vertices3D);

        var filter = gameObject.GetComponent<MeshFilter>();
		filter.mesh = mesh;

        DrawMarkers();
	}

    //check all sets of 3 vertices to see if they form a line
    void FindLines(Vector3[] vertices)
    {
        //clear out all existing lines
        foreach(Transform t in transform)
        {
            if (t.GetComponent<LineRenderer>() != null) Destroy(t.gameObject);
        }
        bool onXAxis = false, onYAxis = false;
        foreach (Vector3[] threecombo in Combinations(vertices, 3))
        {
            //Debug.Log("Combo:");
            //Debug.Log(threecombo[0]);
            //Debug.Log(threecombo[1]);
            //Debug.Log(threecombo[2]);
            onYAxis = (threecombo[0].x == threecombo[1].x) && (threecombo[1].x == threecombo[2].x);
            onXAxis = (threecombo[0].y == threecombo[1].y) && (threecombo[1].y == threecombo[2].y);

            if (onXAxis || onYAxis)
            {
                Debug.Log("three in a row! line draw!");
                GameObject newLineObj = Instantiate(linePrefab, transform);
                LineRenderer line = newLineObj.GetComponent<LineRenderer>();
                Color lineColor = color;
                if (gameObject.name == "PlayerPolygon") lineColor.a = .7f;

                line.startColor = lineColor;
                line.endColor = lineColor;
                line.SetPositions(threecombo);
            }
        }
    }

    // Enumerate all possible m-size combinations of [0, 1, ..., n-1] array
    // in lexicographic order (first [0, 1, 2, ..., m-1]).
    private IEnumerable<int[]> Combinations(int m, int n)
    {
        int[] result = new int[m];
        Stack<int> stack = new Stack<int>(m);
        stack.Push(0);
        while (stack.Count > 0)
        {
            int index = stack.Count - 1;
            int value = stack.Pop();
            while (value < n)
            {
                result[index++] = value++;
                stack.Push(value);
                if (index != m) continue;
                yield return (int[])result.Clone(); // thanks to @xanatos
                //yield return result;
                break;
            }
        }
    }

    IEnumerable<T[]> Combinations<T>(T[] array, int m)
    {
        T[] result = new T[m];
        foreach (int[] j in Combinations(m, array.Length))
        {
            for (int i = 0; i < m; i++)
            {
                result[i] = array[j[i]];
            }
            yield return result;
        }
    }

    void DrawMarkers()
    {
        //clear all markers on this polygon before drawing new set of markers
        foreach (Transform markerTF in markerParent)
        {
            //make sure it is a marker, it will have a tag
            //don't know what other children objects i might add in the future ugh
            if(markerTF.gameObject.tag == "marker")
                Destroy(markerTF.gameObject);
        }
        //draw a marker for each vertex
        foreach (Vector3 vert in verticesList)
        {
            GameObject markerGO = (GameObject)Instantiate(markerPrefab, markerParent);
            //Debug.Log("adding marker at:"+ vert);
            Vector3 tpos = transform.position;
            markerGO.transform.position = new Vector3(tpos.x + vert.x, tpos.y + vert.y, -0.1f);
            markerGO.GetComponent<RemovalMarker>().vert = vert;
        }
    }

    //takes a vertex defined in terms of local coordinates and 
    //returns a vector3 which represents the vertex position in global coords
    public Vector3 GetPositionAdjustedVert(Vector3 localVert)
    {
        return localVert + transform.localPosition;
    }
}



public struct IndexedVertex{
	public Vector3 coords;
	public int index;

	public IndexedVertex(Vector3 c, int i){
		coords = c;
		index = i;
	}
}
