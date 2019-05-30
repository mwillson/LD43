using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	List<List<Vector3>> polygons;

	[SerializeField]
	public GameObject polyPrefab;

	[SerializeField] 
	public Color[] playerPolyColors;

	// Use this for initialization
	void Start () {
		polygons = new List<List<Vector3>>();
		Vector3[] shape1 = new Vector3[] { 
			new Vector3 (0f, 0f, 0f),
			new Vector3 (0f, 1f, 0f),
			new Vector3 (1f, 1f, 0f),
			new Vector3 (1f, 0f, 0f)
		};
		polygons.Add (new List<Vector3>(shape1));
		Vector3[] shape2 = new Vector3[] { 
			new Vector3 (0f, 0f, 0f),
			new Vector3 (0f, 1f, 0f),
			new Vector3 (2f, 2f, 0f),
			new Vector3 (1f, 0f, 0f)
		};
		polygons.Add (new List<Vector3>(shape2));
		Vector3[] shape3 = new Vector3[] { 
			new Vector3 (0f, 0f, 0f),
			new Vector3 (1f, 1f, 0f),
			new Vector3 (2f, 1f, 0f),
			new Vector3 (1f, 0f, 0f)
		};
		polygons.Add (new List<Vector3>(shape3));
		Vector3[] shape4 = new Vector3[] { 
			new Vector3 (0f, 0f, 0f),
			new Vector3 (1f, 1f, 0f),
			new Vector3 (2f, 1f, 0f),
			new Vector3 (3f, 0f, 0f)
		};

		//5 sided
		polygons.Add (new List<Vector3>(shape4));
		Vector3[] shape5 = new Vector3[] { 
			new Vector3 (0f, -.5f, 0f),
			new Vector3 (-.5f, 0f, 0f),
			new Vector3 (0f, 1f, 0f),
			new Vector3 (1f, 0f, 0f),
			new Vector3(.5f, -.5f, 0f)
		};
		polygons.Add (new List<Vector3>(shape5));
		Vector3[] shape6 = new Vector3[] { 
			new Vector3 (0f, -.5f, 0f),
			new Vector3 (-.5f, 0f, 0f),
			new Vector3 (-.5f, .5f, 0f),
			new Vector3 (0f, 1f, 0f),
			new Vector3(.5f, 0f, 0f)
		};
		polygons.Add (new List<Vector3>(shape6));
		Vector3[] shape7 = new Vector3[] { 
			new Vector3 (0f, 0f, 0f),
			new Vector3 (-.5f, .5f, 0f),
			new Vector3 (.5f, 1f, 0f),
			new Vector3 (1f, .5f, 0f),
			new Vector3(.5f, 0f, 0f)
		};
		polygons.Add (new List<Vector3>(shape7));
		Vector3[] shape8 = new Vector3[] { 
			new Vector3 (0f, -.5f, 0f),
			new Vector3 (-.5f, 0f, 0f),
			new Vector3 (0f, .5f, 0f),
			new Vector3 (1f, 0f, 0f),
			new Vector3(0f, 0f, 0f)
		};
		polygons.Add (new List<Vector3>(shape8));

		//6 sided
		Vector3[] shape9 = new Vector3[] { 
			new Vector3 (0f, 0f, 0f),
			new Vector3 (-.5f, 0f, 0f),
			new Vector3 (0f, 1f, 0f),
			new Vector3 (1f, .5f, 0f),
			new Vector3(.5f, .5f, 0f),
			new Vector3(1f,-.5f,0f)
		};
		polygons.Add (new List<Vector3>(shape9));
		Vector3[] shape10 = new Vector3[] { 
			new Vector3 (0f, -.5f, 0f),
			new Vector3 (-.5f, .5f, 0f),
			new Vector3 (0f, 1f, 0f),
			new Vector3 (.5f, 1f, 0f),
			new Vector3(1f, .5f, 0f),
			new Vector3(.5f,-.5f,0f)
		};
		polygons.Add (new List<Vector3>(shape10));
		Vector3[] shape11 = new Vector3[] { 
			new Vector3 (-.5f, -0f, 0f),
			new Vector3 (0f, .5f, 0f),
			new Vector3 (0f, 1f, 0f),
			new Vector3 (.5f, 1f, 0f),
			new Vector3(.5f, 0f, 0f),
			new Vector3(1f,-.5f,0f)
		};
		polygons.Add (new List<Vector3>(shape11));
		Vector3[] shape12 = new Vector3[] { 
			new Vector3 (-.5f, 0f, 0f),
			new Vector3 (-.5f, .5f, 0f),
			new Vector3 (0f, 1f, 0f),
			new Vector3 (1f, 1f, 0f),
			new Vector3(1f, -.5f, 0f),
			new Vector3(.5f,-.5f,0f)
		};
		polygons.Add (new List<Vector3>(shape12));
		float rotOff;
		for (int i = 0; i < 5; i++) {
			rotOff = UnityEngine.Random.Range (0f, 360f);
			GameObject poly = Instantiate (polyPrefab, new Vector3 (UnityEngine.Random.Range (-8f, -2f), UnityEngine.Random.Range (-5f, 5f), 0f), Quaternion.Euler(0f, rotOff, 0f));
			poly.GetComponent<MenuPolygon> ().Setup ();
			poly.GetComponent<MenuPolygon> ().rotOffset = rotOff;
			poly.GetComponent<MenuPolygon>().verticesList =  new List<Vector3>(polygons[Random.Range(0,polygons.Count)]);
			poly.GetComponent<MenuPolygon>().vertices2D = System.Array.ConvertAll<Vector3, Vector2>(poly.GetComponent<MenuPolygon>().verticesList.ToArray(), v => v);
			poly.GetComponent<MenuPolygon> ().color = playerPolyColors[UnityEngine.Random.Range(0,playerPolyColors.Length)];
			poly.GetComponent<MenuPolygon>().ReDraw ();
		}
		for (int i = 0; i < 5; i++) {
			rotOff = UnityEngine.Random.Range (0f, 360f);
			GameObject poly = Instantiate (polyPrefab, new Vector3 (UnityEngine.Random.Range (2f, 8f), UnityEngine.Random.Range (-5f, 5f), 0f), Quaternion.Euler(0f, rotOff, 0f));
			poly.GetComponent<MenuPolygon> ().Setup ();
			poly.GetComponent<MenuPolygon> ().rotOffset = rotOff;
			poly.GetComponent<MenuPolygon>().verticesList =  new List<Vector3>(polygons[Random.Range(0,polygons.Count)]);
			poly.GetComponent<MenuPolygon>().vertices2D = System.Array.ConvertAll<Vector3, Vector2>(poly.GetComponent<MenuPolygon>().verticesList.ToArray(), v => v);
			poly.GetComponent<MenuPolygon> ().color = playerPolyColors[UnityEngine.Random.Range(0,playerPolyColors.Length)];
			poly.GetComponent<MenuPolygon>().ReDraw ();
		}
	}

	public void MouseEnter(){
		GetComponent<Outline> ().enabled = true;
	}

	public void LoadMainScene(){
		SceneManager.LoadScene ("scene1");
	}

	// Update is called once per frame
	void Update () {
		
	}
}
