using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using cakeslice;

public class MenuPolygon : MonoBehaviour
{
	public List<Vector3> verticesList;

	public Vector2[] vertices2D;

	[SerializeField]
	public Color color;

	public float rotOffset;

	private void Start () {

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

	void Update(){
		transform.rotation = Quaternion.Euler (0f, rotOffset + ((Time.time % 360f) * 20f), 0f);
		ReDraw ();
	}

	public void Setup(){

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
		verticesList.RemoveAt (i);
		ReDraw ();
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

		var filter = gameObject.GetComponent<MeshFilter>();
		filter.mesh = mesh;
	}
}
