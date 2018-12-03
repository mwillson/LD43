using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using cakeslice;

public class GameManager : MonoBehaviour {

	TestPolygon currentPoly;

	List<List<Vector3>> polygons;

	[SerializeField]
	public Color[] playerPolyColors;

	int numVerts = 4;

	bool animDone;

	float bps = 2.33f;
	float prevBeatTime = 0f, beatTime;
	bool smallBeat;

	Transform wallPlane;

	[SerializeField]
	public Material radialMat;

	public bool cameraWait = false;

	[SerializeField]
	public Image healthBar;
	public float currHealth, newHealth, t;

	// Use this for initialization
	void Start () {
		currHealth = 1f;
		newHealth = 1f;
		t = 0f;
		wallPlane = GameObject.Find ("Wall").transform.Find ("Plane");
		animDone = false;
		smallBeat = false;
		currentPoly = GameObject.Find ("PlayerPolygon").GetComponent<TestPolygon> ();
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
		polygons.Add (new List<Vector3>(shape4));
		currentPoly.Setup ();
		CreateNewPlayerPolygon ();
		CreateHole ();
	}
	
	// Update is called once per frame
	void Update () {
		RenderSettings.skybox.SetFloat ("_Rotation", (Mathf.Abs(Mathf.Cos ((Time.time % 10)) * 10)  + 5));
		if (currHealth != newHealth) {
			currHealth = Mathf.Lerp (currHealth, newHealth, t);
			healthBar.fillAmount = currHealth;
			t += .5f * Time.deltaTime; 
		}
	}

	void FixedUpdate(){
		//1 divided by beats per second gives me roughly how often i should 'do a beat'
		//mod time by that and every time it loops back to 0, 'do the beat'
		beatTime = Time.time % (1f/bps);
		if (beatTime <= prevBeatTime) {
			StartCoroutine (DoBeat ());
		}
		prevBeatTime = beatTime;
	}

	IEnumerator DoBeat(){
		if (!smallBeat) {
			wallPlane.localScale = new Vector3 (.8f, .8f, .8f);
			yield return new WaitForSeconds (.1f);
		} else {
			wallPlane.localScale = new Vector3 (.68f, .68f, .68f);
			yield return new WaitForSeconds (.08f);
		}
		smallBeat = !smallBeat;
		while (wallPlane.localScale != new Vector3 (.6f, .6f, .6f)) {
			wallPlane.localScale = Vector3.MoveTowards (wallPlane.localScale, new Vector3 (.6f, .6f, .6f), .05f);
			yield return null;
		}

	}

	public void CreateHole(){
		if(GameObject.Find ("Wall").GetComponentInChildren<TestPolygon> () != null)
			Destroy (GameObject.Find ("Wall").GetComponentInChildren<TestPolygon> ().gameObject);
		GameObject.FindObjectOfType<Wall> ().CreateHoleShape (currentPoly.verticesList);
	}

	//handler for stuff to do when wall hits current player polygon
	public void WallCycle(){
		StartCoroutine (SuccessFail ());
	}

	IEnumerator SuccessFail(){
		CheckSuccess ();

		while (!animDone)
			yield return null;
		GameObject.FindObjectOfType<Wall>().transform.position = new Vector3 (-3f, 0f, 10f);

		animDone = false;
		CreateNewPlayerPolygon ();
		CreateHole ();
	}

	void CheckSuccess(){
		TestPolygon hole = GameObject.Find ("Wall").GetComponentInChildren<TestPolygon> ();
		bool success = VerticesAreSame (hole, currentPoly);
		animDone = false;
		if (success)
			StartCoroutine (SuccessAnim ());
		else
			StartCoroutine (FailAnim ());
	}

	bool VerticesAreSame (TestPolygon p1, TestPolygon p2){
		bool result = true;
		if (p1.verticesList.Count != p2.verticesList.Count) {
			Debug.Log ("non-matching vertex count");
			return false;
		}
		int i = 0;
		foreach (Vector3 v1 in p1.verticesList) {
			if (v1.x != p2.verticesList [i].x || v1.y != p2.verticesList [i].y)
				return false;
			i++;
		}
		//if we made it through the loop without finding a mismatch, we good, success!
		return true;
	}

	IEnumerator SuccessAnim(){
		Debug.Log ("Success!");
		//outline the mesh please
		bool outEnab = true;
		currentPoly.GetComponent<cakeslice.Outline>().enabled = true;
		float i = 0f;
		while (i < .6f) {
			i += .05f;
			outEnab = !outEnab;
			currentPoly.GetComponent<cakeslice.Outline> ().enabled = outEnab;
			yield return new WaitForSeconds (.05f);
		}

		currentPoly.GetComponent<cakeslice.Outline>().enabled = false;
		animDone = true;
	}


	IEnumerator FailAnim(){
		Debug.Log ("Fail");
		HealthDrop ();

		currentPoly.color = Color.grey;
		currentPoly.ReDraw ();
		GameObject.FindObjectOfType<CameraControl> ().ScreenShake (.2f, 0f, .06f);
		while (cameraWait)
			yield return null;
		float i = 0f;
		Vector3 originalPos = currentPoly.transform.position;
		while (i < .1f) {
			currentPoly.transform.position += new Vector3 (0f, .2f, 0f);
			i += .04f;
			yield return new WaitForSeconds (.02f);
		}
		i = 0f;
		while (i < .4f) {
			currentPoly.transform.position += new Vector3 (0f, -.4f, 0f);
			i += .02f;
			yield return new WaitForSeconds (.02f);
		}
		currentPoly.transform.position = originalPos;
		animDone = true;
	}

	void HealthDrop(){
		t = 0f;
		float failDrop = .1f;
		newHealth = currHealth - failDrop;

	}

	public void CreateNewPlayerPolygon(){
		//newVerts needs to be a new List of vertices, copied from the list in memory
		List<Vector3> newVerts = new List<Vector3>(polygons[Random.Range(0,polygons.Count)]);

		/*List<XYPair> confirmedVerts = new List<XYPair> ();
		//generate 'numVerts' number of unique vertices for polygon
		for (int i = 0; i < numVerts; i++) {
			int xVal = -1, yVal = -1;
			bool foundNew = false;
			while(!foundNew){
				xVal = Random.Range (0, 3);
				yVal = Random.Range (0, 3);
				//only proceed to add if certain conditions are met in confirmed verts list
				//can't be more than 1 of either x or y value
				//also can't be a vert in list that has both x and y already
				if (confirmedVerts.FindAll (p => p.x == xVal).Count < 2 &&
				    confirmedVerts.FindAll (p => p.y == yVal).Count < 2 &&
					!confirmedVerts.Exists (p => p.x == xVal && p.y == yVal)) {

						confirmedVerts.Add (new XYPair (xVal, yVal));
						foundNew = true;
					
				}
			}
				
			Vector3 newVert = new Vector3 (xVal, yVal, 0f);
			newVerts.Add (newVert);
			Debug.Log ("new Vert added: " + newVert);
		}*/
		currentPoly.GetComponent<TestPolygon> ().verticesList = newVerts;
		currentPoly.GetComponent<TestPolygon>().vertices2D = System.Array.ConvertAll<Vector3, Vector2>(newVerts.ToArray(), v => v);
		currentPoly.GetComponent<TestPolygon> ().color = playerPolyColors[UnityEngine.Random.Range(0,playerPolyColors.Length)];
		currentPoly.ReDraw ();
		Debug.Log ("Done creating player polygon, verts:" + newVerts.Count);
	}

	
}

public struct XYPair{
	public int x; 
	public int y;

	public XYPair(int p1, int p2)
	{
		x = p1;
		y = p2;
	}
}
