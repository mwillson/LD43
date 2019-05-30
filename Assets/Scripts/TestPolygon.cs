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
    public Transform markerParent;

    public void Awake()
    {
        removedStack = new Stack<IndexedVertex>();
    }

	private void Start () {

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
		if (GetComponent<MeshRenderer> () == null) {
			meshRenderer = gameObject.AddComponent<MeshRenderer> ();
		} else {
			meshRenderer = gameObject.GetComponent<MeshRenderer> ();
		}
		meshRenderer.material = new Material(Shader.Find("Sprites/Default"));
        //lower alpha if player (clickable) polygon
        if (gameObject.name == "PlayerPolygon")
        {
            Color myColor = meshRenderer.material.GetColor("_Color");
            myColor.a = .7f;
            meshRenderer.material.SetColor("_Color", myColor);
        }
		MeshFilter filter;
		if (GetComponent<MeshFilter> () == null) {
			filter = gameObject.AddComponent<MeshFilter> ();
		} else {
			filter = gameObject.GetComponent<MeshFilter> ();
		}
		filter.mesh = mesh;

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
    }

	//remove vertex at index i
	public void RemoveVertex(int i){
        Vector3 vertCopy = new Vector3(verticesList[i].x, verticesList[i].y, verticesList[i].z);

        foreach (Transform markerTF in GameObject.Find("Markers").transform)
        {
            Vector3 mvert = markerTF.GetComponent<RemovalMarker>().vert;
            if (mvert.x == vertCopy.x && mvert.y == vertCopy.y)
            {
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
        if (gameObject.name == "PlayerPolygon")
        {
            Color myColor = meshRenderer.material.GetColor("_Color");
            myColor.a = .7f;
            meshRenderer.material.SetColor("_Color", myColor);
        }

        var filter = gameObject.GetComponent<MeshFilter>();
		filter.mesh = mesh;

        DrawMarkers();
	}

    void DrawMarkers()
    {
        foreach(Vector3 vert in verticesList)
        {
            GameObject markerGO = (GameObject)Instantiate(markerPrefab, markerParent);
            Debug.Log("adding marker at:"+ vert);
            Vector3 tpos = transform.position;
            markerGO.transform.position = new Vector3(tpos.x + vert.x, tpos.y + vert.y, -0.1f);
            markerGO.GetComponent<RemovalMarker>().vert = vert;
        }
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
