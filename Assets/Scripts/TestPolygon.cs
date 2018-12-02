using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TestPolygon : MonoBehaviour
{
	public List<Vector3> verticesList;

	private void Start () {
		// Create Vector2 vertices
		var vertices2D = new Vector2[] {
			new Vector2(0,0),
			new Vector2(0,1),
			new Vector2(1,1),
			new Vector2(1,2),
			new Vector2(0,2),
			new Vector2(0,3),
			new Vector2(3,3),
			new Vector2(3,2),
			new Vector2(2,2),
			new Vector2(2,1),
			new Vector2(3,1),
			new Vector2(3,0),
		};

		var vertices3D = System.Array.ConvertAll<Vector2, Vector3>(vertices2D, v => v);

		verticesList = new List<Vector3> (vertices3D);

		// Use the triangulator to get indices for creating triangles
		var triangulator = new Triangulator(vertices2D);
		var indices =  triangulator.Triangulate();

		// Generate a color for each vertex
		var colors = Enumerable.Range(0, vertices3D.Length)
			.Select(i => Random.ColorHSV())
			.ToArray();

		// Create the mesh
		var mesh = new Mesh {
			vertices = vertices3D,
			triangles = indices,
			colors = colors
		};

		mesh.RecalculateNormals();
		mesh.RecalculateBounds();

		// Set up game object with mesh;
		var meshRenderer = gameObject.AddComponent<MeshRenderer>();
		meshRenderer.material = new Material(Shader.Find("Sprites/Default"));

		var filter = gameObject.AddComponent<MeshFilter>();
		filter.mesh = mesh;
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
		var colors = Enumerable.Range(0, vertices3D.Length)
			.Select(i => Random.ColorHSV())
			.ToArray();


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
