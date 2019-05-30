using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
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

    float skyboxOffset;

	Transform wallPlane;

	[SerializeField]
	public Material radialMat;

	public bool cameraWait = false;

	[SerializeField]
	public Image healthBar;
	public float currHealth, newHealth, t, wallSpeed;

	[SerializeField]
	Text chainText, scoreText;

	[SerializeField]
	TextMesh wallText, removeText;

	int score, level, chainAmt, levelThreshold, chainThreshold, polyRangeLow, polyRangeHigh;

	public int numToRemove = 1;

	[SerializeField]
	public GameObject SuccessSoundPrefab, FailSoundPrefab, Intense1, Intense2, Intense3, Intense4;

	AudioSource audiosource;
	[SerializeField]
	public AudioClip chainSound;

	Wall wall;

	// Use this for initialization
	void Start () {
		score = 0;
		chainAmt = 0;
		chainThreshold = 5;
		levelThreshold = 1;
		level = 0;
		polyRangeLow = 0;
		polyRangeHigh = 1;
		currHealth = 1f;
		newHealth = 1f;
		t = 0f;
		wallSpeed = .05f;
		audiosource = GameObject.Find ("BGM").GetComponent<AudioSource> ();
		wallPlane = GameObject.Find ("Wall").transform.Find ("Plane");
		animDone = false;
		smallBeat = false;
        skyboxOffset = 0f;
		//screen orientation stuff
		ScreenWatcher.AddOrientationChangeListener(OnOrientationChanged);
		//Screen.orientation = ScreenOrientation.LandscapeLeft;
		wall = GameObject.FindObjectOfType<Wall>();
	
		currentPoly = GameObject.Find ("PlayerPolygon").GetComponent<TestPolygon> ();

		//PositionForLandscape ();

		if (Screen.orientation == ScreenOrientation.Landscape) {
			PositionForLandscape ();

		} else if (Screen.orientation == ScreenOrientation.Portrait) {
			PositionForPortrait ();

		} 
		GameObject.Find ("MainCanvas").GetComponent<CanvasScaler> ().referenceResolution = new Vector2 (Screen.width, Screen.height);

		polygons = new List<List<Vector3>>();
		Vector3[] shape1 = new Vector3[] { 
			new Vector3 (0f, 0f, 0f),
			new Vector3 (0f, 1f, 0f),
			new Vector3 (1f, 1f, 0f),
			new Vector3 (1f, 0f, 0f)
		};
		polygons.Add (new List<Vector3>(shape1));
		Vector3[] shape2 = new Vector3[] { 
			new Vector3 (0f, -1f, 0f),
			new Vector3 (0f, 0f, 0f),
			new Vector3 (2f, 1f, 0f),
			new Vector3 (1f, -1f, 0f)
		};
		polygons.Add (new List<Vector3>(shape2));
		Vector3[] shape3 = new Vector3[] { 
			new Vector3 (-1f, -1f, 0f),
			new Vector3 (0f, 0f, 0f),
			new Vector3 (1f, 0f, 0f),
			new Vector3 (0f, -1f, 0f)
		};
		polygons.Add (new List<Vector3>(shape3));
		Vector3[] shape4 = new Vector3[] { 
			new Vector3 (-1f, -1f, 0f),
			new Vector3 (0f, 0f, 0f),
			new Vector3 (1f, 0f, 0f),
			new Vector3 (2f, -1f, 0f)
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
		currentPoly.Setup ();
		CreateNewPlayerPolygon ();
		CreateHole ();
	}

	void PositionForPortrait(){
		Debug.Log ("portrait mode!!!");
		wall.startPos = new Vector3 (1f, 0f, 10f);
		Camera.main.fieldOfView = 120f;
		Camera.main.transform.position = new Vector3 (1f, 2f, -3f);
		wall.endPos = new Vector3 (1f, 2f, .08f);
		//set player polygons position
		currentPoly.transform.position = new Vector3 (1f, 2f, 0f);
	}

	void PositionForLandscape (){
		wall.startPos = new Vector3 (-3f, 0f, 10f);
		Camera.main.fieldOfView = 60f;
		Camera.main.transform.position = new Vector3 (1f, 1f, -3f);
		wall.endPos = new Vector3 (1f, 1f, .08f);
		//set player polygons position
		currentPoly.transform.position = new Vector3 (1f, 1f, 0f);
	}

	// Update is called once per frame
	void Update () {
        //rotates skybox back and forth over time, not used anymore
        //RenderSettings.skybox.SetFloat ("_Rotation", (Mathf.Abs(Mathf.Cos ((Time.time % 10)) * 10)  + 5));
        //offset increase every frame update
        skyboxOffset = ((skyboxOffset + .005f) % .92f);
        RenderSettings.skybox.SetTextureOffset (  "_DownTex", new Vector2(0f, skyboxOffset)   );

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
		GameObject.FindObjectOfType<Wall> ().CreateHoleShape (currentPoly.verticesList, numToRemove);
	}

	//handler for stuff to do when wall hits current player polygon
	public void WallCycle(){
		StartCoroutine (SuccessFail ());
	}

	IEnumerator SuccessFail(){
		CheckSuccess ();

		while (!animDone)
			yield return null;
		GameObject.FindObjectOfType<Wall>().transform.position = wall.startPos;
		GameObject.FindObjectOfType<Wall>().speed = wallSpeed;
		GameObject.FindObjectOfType<Wall> ().speedingup = false;
		GameObject.FindObjectOfType<PointerController> ().numRemoved = 0;

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

	public bool VerticesAreSame (TestPolygon p1, TestPolygon p2){
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
		//play sound
		GameObject soundGO = Instantiate(SuccessSoundPrefab);
		//scoring
		score += 1;
		//score specific events
		switch (score) {
		case 5:
			wallText.gameObject.SetActive (true);
			//wallText.transform.localPosition = new Vector3 (-2.6f, -1.5f, 0);
			wallText.fontSize = 20;
			wallText.text = "5 in  a  row ,\n get some health";
			StartCoroutine(TextTimer ());
			break;
		default:
			break;
		}
		scoreText.text = "Score: " + score;
		Debug.Log ("Success! Score:" + score);

		chainAmt += 1;
		if (chainAmt == chainThreshold) {
			StartCoroutine (ChainAnimation ());
			if (currHealth < 1f)
				ChainHealth (chainAmt);
		}
		chainText.text = "Chain: x" + chainAmt;
		if (score >= levelThreshold)
			GoToNextLevel ();
		
		//outline the mesh
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
		Destroy (soundGO);
		animDone = true;
	}

	void ChainHealth(int chainSize){
		float toAdd = 0f;
		switch (chainSize) {
		case 5:
			toAdd = .05f;
			break;
		case 10:
			toAdd = .07f;
			break;
		case 15:
			toAdd = .1f;
			break;
		case 20:
			toAdd = .15f;
			break;
		case 25:
			toAdd = .25f;
			break;
		case 30:
			toAdd = .35f;
			break;
		case 35:
			toAdd = .5f;
			break;
		default:
			toAdd = 0f;
			Debug.LogError ("Unrecognized chain threshold: " + chainSize);
			break;
		}
		//add either the full amount or just enough to reach 1(full health)
		HealthDrop (Mathf.Min(toAdd, 1f-currHealth));
		//next chain threshold level
		chainThreshold += 5;
	}

	IEnumerator ChainAnimation(){
		
		float t = 0;
		while (t < .3f) {
			chainText.transform.localScale += new Vector3 (.02f, .02f, 0f);
			t += Time.deltaTime;
			yield return null;
		}
		audiosource.clip = chainSound;
		audiosource.Play ();
		t = 0;
		while (t < .3f) {
			chainText.transform.localScale -= new Vector3 (.02f, .02f, 0f);
			t += Time.deltaTime;
			yield return null;
		}
		chainText.transform.localScale = new Vector3 (1f, 1f, 1f);
	}

	void GoToNextLevel(){
		level += 1;
		//loop back to level 1 after level 9
		if (level > 10)
			level = 0;
		levelThreshold += 2;

		wallText.gameObject.SetActive (true);
		wallText.text = "Level " + level;

		switch (level) {
		case 0:
			wallText.fontSize = 20;
			//wallText.transform.localPosition = new Vector3 (-2.6f, -1.5f, 0);
			wallText.text = "F it  the  shape\nin to  the  hole !";
			polyRangeLow = 0;
			polyRangeHigh = 1;
			wallSpeed = .05f;
			numToRemove = 1;
			GameObject musicTrigger0 = (GameObject)Instantiate (Intense1, new Vector3 (0f, 0f, 0f), Quaternion.identity);
			Destroy (musicTrigger0, 1f);
			break;
		case 1:
			wallText.fontSize = 40;
			wallText.transform.localPosition = new Vector3 (-2.6f, 2.5f, 0);
			removeText.gameObject.SetActive (true);
			polyRangeLow = 0;
			polyRangeHigh = 1;
			wallSpeed = .05f;
			numToRemove = 1;
			GameObject musicTrigger1 = (GameObject)Instantiate (Intense1, new Vector3 (0f, 0f, 0f), Quaternion.identity);
			Destroy (musicTrigger1, 1f);
			break;
		case 2:
			wallText.fontSize = 40;
			wallText.transform.localPosition = new Vector3 (-2.6f, 2.5f, 0);

			polyRangeHigh = 4;
			wallSpeed = .05f;
			levelThreshold += 5;
			break;
		case 3:
			wallText.fontSize = 40;
			wallText.transform.localPosition = new Vector3 (-2.6f, 2.5f, 0);
			polyRangeHigh = 8;
			break;
		case 4:
			wallText.fontSize = 40;
			wallText.transform.localPosition = new Vector3 (-2.6f, 2.5f, 0);
			polyRangeHigh = 4;
			wallSpeed = .1f;
			levelThreshold += 5;
			GameObject musicTrigger2 = (GameObject)Instantiate (Intense2, new Vector3 (0f, 0f, 0f), Quaternion.identity);
			Destroy (musicTrigger2, 1f);
			break;
		case 5:
			wallText.fontSize = 40;
			wallText.transform.localPosition = new Vector3 (-2.6f, 2.5f, 0);
			polyRangeHigh = 12;
			wallSpeed = .05f;
			levelThreshold += 10;
			break;
		case 6:
			polyRangeLow = 4;
			polyRangeHigh = 8;
			wallSpeed = .04f;
			numToRemove = 2;
			wallText.fontSize = 20;
			wallText.text += "\nRemove Multiple\nVertices !";
			//wallText.transform.localPosition = new Vector3 (-2.6f, -1.5f, 0);

			levelThreshold += 10;
			GameObject musicTrigger3 = (GameObject)Instantiate (Intense3, new Vector3 (0f, 0f, 0f), Quaternion.identity);
			Destroy (musicTrigger3, 1f);
			break;
		case 7:
			wallText.fontSize = 40;
			wallText.transform.localPosition = new Vector3 (-2.6f, 2.5f, 0);

			polyRangeLow = 0;
			polyRangeHigh = 12;
			wallSpeed = .1f;
			numToRemove = 1;
			levelThreshold += 20;
			break;
		case 8:
			polyRangeLow = 4;
			polyRangeHigh = 8;
			wallSpeed = .08f;
			numToRemove = 2;
			GameObject musicTrigger4 = (GameObject)Instantiate (Intense4, new Vector3 (0f, 0f, 0f), Quaternion.identity);
			Destroy (musicTrigger4, 1f);
			levelThreshold += 20;
			break;
		case 9:
			GameObject musicTrigger5 = (GameObject)Instantiate (Intense2, new Vector3 (0f, 0f, 0f), Quaternion.identity);
			Destroy (musicTrigger5, 1f);
			polyRangeHigh = 12;
			wallSpeed = .05f;
			numToRemove = 2;
			levelThreshold += 20;
			break;
		case 10:
			GameObject musicTrigger6 = (GameObject)Instantiate (Intense4, new Vector3 (0f, 0f, 0f), Quaternion.identity);
			Destroy (musicTrigger6, 1f);
			wallSpeed = .1f;
			numToRemove = 2;
			levelThreshold += 50;
			break;
		default:
			polyRangeLow = 0;
			polyRangeHigh = 12;
			wallSpeed = .05f;
			numToRemove = 1;
			break;
		}
		removeText.transform.Find ("RemoveNum").GetComponent<TextMesh> ().text = "" + numToRemove;
		GameObject.FindObjectOfType<Wall> ().speed = wallSpeed;
		StartCoroutine(TextTimer ());
	}

	IEnumerator TextTimer(){
		yield return new WaitForSeconds (5f);
		wallText.gameObject.SetActive (false);
	}

	IEnumerator FailAnim(){
		Debug.Log ("Fail");
		//play sound
		GameObject failSoundGO = Instantiate(FailSoundPrefab);
		//scoring
		HealthDrop (-.1f);
		chainAmt = 0;
		chainThreshold = 5;
		chainText.text = "Chain: x0";
		//animation
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
		Destroy (failSoundGO);
		animDone = true;
	}

	void HealthDrop(float amt){
		t = 0f;
		newHealth = currHealth + amt;
		if (newHealth <= 0f) {
			StartCoroutine (RestartGame ());
		}
	}

	IEnumerator RestartGame(){
		yield return new WaitForSeconds (.5f);
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

	public void CreateNewPlayerPolygon(){
        //remove the current removal markers
        foreach(Transform marker in GameObject.Find("Markers").transform)
        {
            Destroy(marker.gameObject);
        }

		//newVerts needs to be a new List of vertices, copied from the list in memory
		//polyRangeHigh should never exceed polygons.count
		List<Vector3> newVerts = new List<Vector3>(polygons[Random.Range(polyRangeLow,polyRangeHigh)]);

		// PROCEDURAL POLYGON GENERATION //
		// MAY WORK ON IN FUTURE VERSION //
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
		currentPoly.verticesList = newVerts;
		currentPoly.ResetRemovedStack ();
		currentPoly.vertices2D = System.Array.ConvertAll<Vector3, Vector2>(newVerts.ToArray(), v => v);
		currentPoly.color = playerPolyColors[UnityEngine.Random.Range(0,playerPolyColors.Length)];
		currentPoly.ReDraw ();
		Debug.Log ("Done creating player polygon, verts:" + newVerts.Count);
	}

	public void OnOrientationChanged(){
		Debug.Log ("Orientation changed!");
		if (Screen.orientation == ScreenOrientation.Landscape) {
			GameObject.FindObjectOfType<Wall> ().startPos = new Vector3 (-3f, 0f, 10f);
			Camera.main.fieldOfView = 60f;
			Camera.main.transform.position = new Vector3 (0f, 0f, -3f);
			wall.endPos = new Vector3 (1f, 1f, .08f);
			//set player polygons position
			currentPoly.transform.position = new Vector3(1f,0f,0f);

		} else {
			Debug.Log ("portrait mode!!!");
			GameObject.FindObjectOfType<Wall> ().startPos = new Vector3 (1f, -5f, 10f);
			Camera.main.fieldOfView = 120f;
			Camera.main.transform.position = new Vector3 (1f, 2f, -3f);
			wall.endPos = new Vector3 (1f, 3f, .08f);
			//set player polygons position
			currentPoly.transform.position = new Vector3(1f,3f,0f);
		}
		GameObject.Find ("MainCanvas").GetComponent<CanvasScaler> ().referenceResolution = new Vector2 (Screen.width, Screen.height);

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
