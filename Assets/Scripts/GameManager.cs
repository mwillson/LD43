using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using cakeslice;

public class GameManager : Manager {

	TestPolygon currentPoly;

    //potential polygon specs to choose from
	List<List<Vector3>> polygons;

	[SerializeField]
	public Color[] playerPolyColors;

    [SerializeField]
    public LevelSpec[] levels;

    public LevelSpec currentLevel;

	int numVerts = 4;

	bool animDone;

	float bps = 2f;
	float prevBeatTime = 0f, beatTime;
	bool smallBeat;

    float skyboxOffset;

	Transform wallPlane;

	[SerializeField]
	public Material radialMat;


	[SerializeField]
	public Image healthBar;
	public float currHealth, newHealth, t, wallSpeed;

	[SerializeField]
	Text chainText, scoreText, infoText;

	[SerializeField]
	TextMesh wallText, removeText;


    int level, chainAmt, levelThreshold, polyRangeLow, polyRangeHigh, failCounter;

    public int chainThreshold, numberOfShapes;

	[SerializeField]
	public GameObject SuccessSoundPrefab, FailSoundPrefab, Intense1, Intense2, Intense3, Intense4, World3MusicTrigger, pointerControllerGO;

	AudioSource audiosource;
	[SerializeField]
	public AudioClip chainSound;

	Wall wall;

    public RectTransform mainCanvas;

    string cylinderOffsetProperty, cylinderOffsetProp2;
    bool twoOffsetProps;

    public GameType gameType;

    BonusSelect bs;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "scene1") gameType = GameType.Arcade;
    }

    // Use this for initialization
    void Start () {

		score = 0;
        failCounter = 0;
		chainAmt = 0;
		chainThreshold = 5;
		levelThreshold = 1;
		level = 0;
        currentLevel = levels[0];
        numberOfShapes = 1;
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
        bs = bonusSelector.GetComponent<BonusSelect>();

        cylinderOffsetProperty = "_DownTex";
        cylinderOffsetProp2 = "_DownTex2";
        twoOffsetProps = false;
        //PositionForLandscape ();

        //set up text colors
        scoreText.color = world1Text;
        scoreText.GetComponent<UnityEngine.UI.Outline>().effectColor = world1Outline;
        chainText.color = world1Text;
        chainText.GetComponent<UnityEngine.UI.Outline>().effectColor = world1Outline;
        infoText.color = world1Text;
        infoText.GetComponent<UnityEngine.UI.Outline>().effectColor = world1Outline;


        if (Screen.orientation == ScreenOrientation.Landscape) {
			PositionForLandscape ();

		} else if (Screen.orientation == ScreenOrientation.Portrait) {
			PositionForPortrait ();

		} 
		//GameObject.Find ("MainCanvas").GetComponent<CanvasScaler> ().referenceResolution = new Vector2 (Screen.width, Screen.height);

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
			new Vector3 (1f, 2f, 0f),
			new Vector3 (1f, 0f, 0f),
			new Vector3 (0f, -1f, 0f)
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
			new Vector3 (-1f, 0f, 0f),
			new Vector3 (-.5f, 1f, 0f),
			new Vector3 (.5f, 1f, 0f),
			new Vector3 (1f, 0f, 0f)
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

        //create custom polygons list if custom game
        if (gameType == GameType.CasualCustom) CreateCustomParamsLevel(GameObject.FindObjectOfType<CustomGameParams>());

        //currentPoly.Setup ();
        currentPolys = new List<TestPolygon>();
        CreateNewPlayerPolygons();
        numToRemove = 0;
        foreach (TestPolygon p in currentPolys)
        {
            numToRemove += p.numToRemove;
        }
        currentHoles = new List<TestPolygon>();
		CreateHoles ();
	}

    void CreateCustomParamsLevel(CustomGameParams gameParams)
    {
        //re-build list of polygons to use correct-sided ones
        List<List<Vector3>> newPolys = new List<List<Vector3>>();
        foreach (List<Vector3> poly in polygons)
        {
            if(poly.Count >= gameParams.sidesLow && poly.Count <= gameParams.sidesHigh)
            {
                newPolys.Add(poly);
            }
        }
        polygons = newPolys;
        //create 10 levels, TODO: change level-looping structure so it can be arbitrary amount!!
        for (int i = 0; i < 10; i++)
        {
            LevelSpec customParamsLevel = ScriptableObject.CreateInstance<LevelSpec>();
            customParamsLevel.NumberOfShapes = gameParams.shapes;
            customParamsLevel.BaseRemoveNum = gameParams.maxRemovals;
            customParamsLevel.LevelNum = i;
            customParamsLevel.WallSpeed = .05f;
            customParamsLevel.BeatsPerSecond = 2f;
            customParamsLevel.PolyRangeLow = 0;
            customParamsLevel.PolyRangeHigh = polygons.Count - 1;
            customParamsLevel.NextLevelThreshold = 5;
            levels[i] = customParamsLevel;
        }
    }

    void PositionForPortrait(){
		Debug.Log ("portrait mode!!!");
		wall.startPos = new Vector3 (1f, 0f, 10f);
		Camera.main.fieldOfView = 120f;
		Camera.main.transform.position = new Vector3 (1f, 2f, -3f);
		wall.endPos = new Vector3 (1f, 2f, .08f);
        //set player polygons position
        GameObject.Find("PlayerPolygon").transform.position = new Vector3 (1f, 2f, 0f);
        infoText.GetComponent<RectTransform>().sizeDelta = new Vector2(mainCanvas.sizeDelta.x, 120f);
        infoText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 70f);
    }

    void PositionForLandscape (){
		wall.startPos = new Vector3 (-3f, 0f, 10f);
		Camera.main.fieldOfView = 60f;
		Camera.main.transform.position = new Vector3 (1f, 1f, -3f);
		wall.endPos = new Vector3 (1f, 1f, .08f);
        //set player polygons position
        GameObject.Find("PlayerPolygon").transform.position = new Vector3 (1f, 1f, 0f);
        infoText.GetComponent<RectTransform>().sizeDelta = new Vector2(mainCanvas.sizeDelta.x, 50f);
        infoText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 40f);
    }

    // Update is called once per frame
    void Update () {
        if (paused) return;
        //rotates skybox back and forth over time, not used anymore
        //RenderSettings.skybox.SetFloat ("_Rotation", (Mathf.Abs(Mathf.Cos ((Time.time % 10)) * 10)  + 5));
        //offset increase every frame update
        //offset loops around at .92
        skyboxOffset = ((skyboxOffset + .005f) % .95f);
        RenderSettings.skybox.SetTextureOffset ( cylinderOffsetProperty, new Vector2(0f, skyboxOffset)   );
        //if we are blending, there are two texture properties we have to offset
        if (twoOffsetProps)
        {
            RenderSettings.skybox.SetTextureOffset(cylinderOffsetProp2, new Vector2(0f, skyboxOffset));
        }

        //lerp health down if taken 'damage' in arcade mode
        if (gameType == GameType.Arcade)
        {
            if (currHealth != newHealth)
            {
                currHealth = Mathf.Lerp(currHealth, newHealth, t);
                healthBar.fillAmount = currHealth;
                t += .5f * Time.deltaTime;
            }
        }
        //shortcut to main menu
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameObject.FindObjectOfType<HighScore>().SubmitNewScore(score);
            SceneManager.LoadScene("mainmenu");
        }
	}

    public override void ChangeTexture1(Texture newTex)
    {
        RenderSettings.skybox.SetTexture("_DownTex", newTex);
    }

    public void ChangeTexture2(Texture newTex)
    {
        RenderSettings.skybox.SetTexture("_DownTex2", newTex);
    }

    private void OnApplicationQuit()
    {

        ChangeTexture1(world1Texture);
    }
    void FixedUpdate(){
        beatTime = Time.time % (1f / bps);
        if (paused)
        {
            //Debug.Log("paused!");
            if (beatTime <= prevBeatTime)
            {
                StartCoroutine(DoTextBeat());
            }
            prevBeatTime = beatTime;
            return;
        }
        else
        {
            if (beatTime <= prevBeatTime)
            {
                StartCoroutine(DoWallBeat());
            }
            prevBeatTime = beatTime;
        }	
	}

	IEnumerator DoWallBeat(){
		if (!smallBeat) {
            //wall pulse
			wallPlane.localScale = new Vector3 (.8f, .8f, .8f);
            //bonus selection text pulse
            if (bonusSelector.activeSelf)
            {
                bs.healthText.fontSize = 22;
                bs.removeText.fontSize = 22;
                bs.slowText.fontSize = 22;
            }
			yield return new WaitForSeconds (.1f);
		} else {
            //wall pulse
			wallPlane.localScale = new Vector3 (.68f, .68f, .68f);
            //bonus selection text pulse
            if (bonusSelector.activeSelf)
            {
                bs.healthText.fontSize = 18;
                bs.removeText.fontSize = 18;
                bs.slowText.fontSize = 18;
            }
            yield return new WaitForSeconds (.08f);
		}
		smallBeat = !smallBeat;
		while (wallPlane.localScale != new Vector3 (.6f, .6f, .6f)) {
			wallPlane.localScale = Vector3.MoveTowards (wallPlane.localScale, new Vector3 (.6f, .6f, .6f), .05f);
			yield return null;
		}

	}
    IEnumerator DoTextBeat()
    {
        if (!smallBeat)
        {
            //bonus selection text pulse
            if (bonusSelector.activeSelf)
            {
                bs.transform.Find("Health").GetComponent<Text>().fontSize = 22;
                bs.transform.Find("Removal").GetComponent<Text>().fontSize = 18;
                bs.transform.Find("Slow").GetComponent<Text>().fontSize = 22;
            }
            yield return new WaitForSeconds(.1f);
        }
        else
        { 
            //bonus selection text pulse
            if (bonusSelector.activeSelf)
            {
                bs.transform.Find("Health").GetComponent<Text>().fontSize = 18;
                bs.transform.Find("Removal").GetComponent<Text>().fontSize = 22;
                bs.transform.Find("Slow").GetComponent<Text>().fontSize = 18;
            }
            yield return new WaitForSeconds(.08f);
        }
        smallBeat = !smallBeat;
    }



	//handler for stuff to do when wall hits current player polygon
	public override void WallCycle(){
		StartCoroutine (SuccessFail ());
	}

	IEnumerator SuccessFail(){
		CheckSuccess ();

        //wait for success/fail animation to complete
		while (!animDone)
			yield return null;
        if (currentWallSlows > 0)
        {
            //for each wall slow applied, undo it, by multiplying wall speed by 2
            for (int i = 0; i < currentWallSlows; i++) wallSpeed *= 2f;
            currentWallSlows = 0;     
        }
		GameObject.FindObjectOfType<Wall>().transform.position = wall.startPos;
		GameObject.FindObjectOfType<Wall>().speed = wallSpeed;
		GameObject.FindObjectOfType<Wall> ().speedingup = false;
		pointerController.numRemoved = 0;

		animDone = false;
		CreateNewPlayerPolygons();
        //add the removal number values from each polygon to get teh total removal number
        //has to happen after we create the player polygons
        numToRemove = 0;
        foreach(TestPolygon p in currentPolys)
        {
            numToRemove += p.numToRemove;
        }
        //should reset num to remove whether we fail or succeed after anim is done
        UpdateRemoveText();
        CreateHoles();
	}

	void CheckSuccess(){
		//TestPolygon hole = GameObject.Find ("Wall").GetComponentInChildren<TestPolygon> ();
        bool success = VerticesAreAllSame(currentPolys, currentHoles);
		animDone = false;
		if (success)
			StartCoroutine (SuccessAnim ());
		else
			StartCoroutine (FailAnim ());
	}

    public bool VerticesAreAllSame(List<TestPolygon> polys, List<TestPolygon> holes)
    {
        bool retval = true;
        for(int i = 0; i < polys.Count; i++)
        {
           
            if (!VerticesAreSame(polys[i], holes[i])){
            retval = false;
            }
        }
        return retval;
    }

	public override bool VerticesAreSame (TestPolygon p1, TestPolygon p2){
		bool result = true;

        //if vertices count is not the same, the vertices definitely can not be the same
		if (p1.verticesList.Count != p2.verticesList.Count) {
			return false;
		}
		/*int i = 0;
		foreach (Vector3 v1 in p1.verticesList) {
			if (v1.x != p2.verticesList [i].x || v1.y != p2.verticesList [i].y)
				return false;
			i++;
		}*/

        //copy of p1 list
        List<Vector3> p1ListCopy = new List<Vector3>();
        foreach(Vector3 vec in p1.verticesList)
            p1ListCopy.Add(new Vector3(vec.x, vec.y, vec.z));
        //copy of p2 list
        List<Vector3> p2ListCopy = new List<Vector3>();
        foreach (Vector3 vec in p2.verticesList)
            p2ListCopy.Add(new Vector3(vec.x, vec.y, vec.z));
        //for each point in p1, find if there is a matching one in p2
        //if so, remove both? from their respective lists. At least remove the one from the second list
        //if at the end of this, p2 is empty, then we have succeeded. both lists are "the same".
        int i = 0;
        foreach (Vector3 vec in p1ListCopy)
        {
            //if we got rid of everything in p2 already, but we are still looping (i.e. we had more elements in p1 than
            //in p2
            if(p2ListCopy.Count == 0)
            {
                return false;
            }
            if (p2ListCopy.Exists(v => v.x == vec.x && v.y == vec.y))
            {
                Vector3 p2Vec = p2ListCopy.Find(v => v.x == vec.x && v.y == vec.y);
                p2ListCopy.Remove(p2Vec);
                
                
            }
            i++;
        }
        if (p2ListCopy.Count == 0) return true;

        return false;
        //if we made it through the loop without finding a mismatch, we good, success!
        //return true;
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

		chainAmt += 1;
		if (chainAmt == chainThreshold) {
			StartCoroutine (ChainAnimation ());
			if (currHealth < 1f)
				ChainHealth (chainAmt);
            paused = true;
            bonusSelector.SetActive(true);
            pointerControllerGO.SetActive(false);
            //next chain threshold level
            chainThreshold += 5;
        }
		chainText.text = "Chain: x" + chainAmt;
        //reset num to remove to level base value
        //numToRemove = currentLevel.BaseRemoveNum;
        if (score >= levelThreshold)
			GoToNextLevel ();
        else
        {
            //UpdateRemoveText();
        }

        //outline the mesh
        foreach (TestPolygon currentPoly in currentPolys)
        {
            bool outEnab = true;
            currentPoly.GetComponent<cakeslice.Outline>().enabled = true;
            float i = 0f;
            while (i < .6f)
            {
                i += .05f;
                outEnab = !outEnab;
                currentPoly.GetComponent<cakeslice.Outline>().enabled = outEnab;
                yield return new WaitForSeconds(.05f);
            }

            currentPoly.GetComponent<cakeslice.Outline>().enabled = false;
        }
        //done flashing,
        //destroy success sound object
		Destroy (soundGO);
        //move wall plane past camera so its like the player is "going through the wall"
        Vector3 wallEndPos = GameObject.Find("Main Camera").transform.position;
        while (wallPlane.parent.position.z > wallEndPos.z)
        {
            wallPlane.parent.position = Vector3.MoveTowards(wallPlane.parent.position, wallEndPos, .25f);
            yield return null;
        }
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
	
	}

    //BONUS EVENTS

    public void AddFromHealthBonus()
    {
        BonusSelect bonus = bonusSelector.GetComponent<BonusSelect>();

        if (bonus.healthBonus > 0)
        {
            //add either the full amount or just enough to reach 1(full health)
            HealthDrop(Mathf.Min(.1f, 1f - currHealth));
            bonus.healthBonus -= 1;
            bonus.healthText.text = "" + bonus.healthBonus;
        }
    }

    public void SlowWall()
    {
        BonusSelect bonus = bonusSelector.GetComponent<BonusSelect>();
        if (bonus.slowBonus > 0 )
        {
            Debug.LogWarning("wall slow!");
            wallSpeed *= .5f;
            wall.speed = wallSpeed;
            currentWallSlows += 1;
            bonus.slowBonus -= 1;
            bonus.slowText.text = "" + bonus.slowBonus;
        }
    }

    public void RemoveRandomVertex()
    {
        BonusSelect bonus = bonusSelector.GetComponent<BonusSelect>();

        if (bonus.removeBonus > 0)
        {
            //get the polygons in a random order
            int[] randomInts = Shuffle(currentPolys.Count);
            
            foreach(int j in randomInts) {
                //get a random polygon from player polygons
                TestPolygon p = currentPolys[j];
                //if it has more vertices than its corresponding hole, we shall remove one from it
                if (p.verticesList.Count > p.holeToMatch.verticesList.Count)
                {

                    bool foundOne = false;
                    int i = 0;
                    Vector3 toCheck = new Vector3(0, 0, 0);
                    while (!foundOne)
                    {
                        //get a random vertex from this player polygon
                        i = Random.Range(0, p.verticesList.Count);
                        toCheck = p.verticesList[i];
                        //if the target polygon doesn't have it, good to remove it from player polygon
                        if (!(p.holeToMatch.verticesList.Exists(v => v.x == toCheck.x && v.y == toCheck.y)))
                        {
                            RemovePlayerVertex(i, p);
                            foundOne = true;
                        }
                        //if target polygon DOES have that vertex, we don't care, try to find another one to remove
                    }
                    bonus.removeBonus -= 1;
                    bonus.removeText.text = "" + bonus.removeBonus;
                    //we removed one, break from our loop looking for a candidate polygon
                    break;
                }
            }
        }
    }

    private static int[] Shuffle(int n)
    {
        var random = new System.Random();
        var result = new int[n];
        for (var i = 0; i < n; i++)
        {
            var j = random.Next(0, i + 1);
            if (i != j)
            {
                result[i] = result[j];
            }
            result[j] = i;
        }
        return result;
    }

    //everything involved in removing a vertex.
    //removes it, updates the ui, and checks for successful match
    public void RemovePlayerVertex(int removalIndex, TestPolygon p)
    {
        p.RemoveVertex(removalIndex);
        
        numToRemove -= 1;
        UpdateRemoveText();

        
        //if we've removed the correct amount and it matches, speed up wall to finish shape
        if (numToRemove == 0)
        {
            bool success = false;
            Transform polygon = GameObject.Find("PlayerPolygon").transform;
            //reworked so that it accounts for all holes
            foreach (TestPolygon tp in polygon.GetComponentsInChildren<TestPolygon>())
            {
                TestPolygon hole = tp.holeToMatch;
                bool individualsuccess = VerticesAreSame(hole, tp);
                //if we failed one check, it is a total fail, so break from loop
                if (!individualsuccess)
                {
                    success = false;
                    break;
                }
                success = true;

            }

            if (success)
            {
                GameObject.FindObjectOfType<Wall>().speedingup = true;
            }
        }
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

    // NEXT LEVEL

	void GoToNextLevel(){
		level += 1;
		//loop back to level 1 after level 9
		if (level > 10)
			level = 0;
		levelThreshold += 2;

		wallText.gameObject.SetActive (true);
		wallText.text = "Level " + level;

        currentLevel = levels[level];
        polyRangeLow = currentLevel.PolyRangeLow;
        polyRangeHigh = currentLevel.PolyRangeHigh;
        wallSpeed = currentLevel.WallSpeed;
        levelThreshold += currentLevel.NextLevelThreshold;

        //after this function completes, a new polygon or polygons will be created 
        numberOfShapes = currentLevel.NumberOfShapes;
        
        //numToRemove = currentLevel.BaseRemoveNum;
        //UpdateRemoveText();


        switch (level) {
		case 0:
			wallText.fontSize = 20;
			GameObject musicTrigger0 = (GameObject)Instantiate (Intense1, new Vector3 (0f, 0f, 0f), Quaternion.identity);
			Destroy (musicTrigger0, 1f);
			break;
		case 1:
			wallText.fontSize = 40;
			wallText.transform.localPosition = new Vector3 (-2.6f, 2.5f, 0);
			//removeText.gameObject.SetActive (true);
			
			break;
		case 2:
			wallText.fontSize = 40;
			wallText.transform.localPosition = new Vector3 (-2.6f, 2.5f, 0);
            GameObject musicTrigger1 = (GameObject)Instantiate(Intense2, new Vector3(0f, 0f, 0f), Quaternion.identity);
            Destroy(musicTrigger1, 1f);
            //levelThreshold += 5;
            break;
		case 3:
			wallText.fontSize = 40;
			wallText.transform.localPosition = new Vector3 (-2.6f, 2.5f, 0);
			break;
		case 4:
			wallText.fontSize = 40;
			wallText.transform.localPosition = new Vector3 (-2.6f, 2.5f, 0);
			//levelThreshold += 5;
                GoToNextWorld(2);
                GameObject musicTrigger2 = (GameObject)Instantiate (World3MusicTrigger, new Vector3 (0f, 0f, 0f), Quaternion.identity);
			Destroy (musicTrigger2, 1f);
                bps = 2.33f;
			break;
		case 5:
			wallText.fontSize = 40;
			wallText.transform.localPosition = new Vector3 (-2.6f, 2.5f, 0);
			//levelThreshold += 10;
			break;
		case 6:
			wallText.fontSize = 20;
			infoText.text = "\nRemove Multiple\nVertices !";

                //levelThreshold += 10;
                //GoToNextWorld(3);
			GameObject musicTrigger3 = (GameObject)Instantiate (Intense2, new Vector3 (0f, 0f, 0f), Quaternion.identity);
			Destroy (musicTrigger3, 1f);
			break;
		case 7:
			wallText.fontSize = 40;
			wallText.transform.localPosition = new Vector3 (-2.6f, 2.5f, 0);
			//levelThreshold += 20;
			break;
		case 8:
			GameObject musicTrigger4 = (GameObject)Instantiate (Intense3, new Vector3 (0f, 0f, 0f), Quaternion.identity);
			Destroy (musicTrigger4, 1f);
			//levelThreshold += 20;
			break;
		case 9:
			GameObject musicTrigger5 = (GameObject)Instantiate (Intense4, new Vector3 (0f, 0f, 0f), Quaternion.identity);
			Destroy (musicTrigger5, 1f);
			//levelThreshold += 20;
			break;
		case 10:
			GameObject musicTrigger6 = (GameObject)Instantiate (Intense2, new Vector3 (0f, 0f, 0f), Quaternion.identity);
			Destroy (musicTrigger6, 1f);
			//levelThreshold += 50;
			break;
		default:
            Debug.LogError("Not on a valid level?");
			polyRangeLow = 0;
			polyRangeHigh = 12;
			wallSpeed = .05f;
			numToRemove = 1;
			break;
		}
		//removeText.transform.Find ("RemoveNum").GetComponent<TextMesh> ().text = "" + numToRemove;
        //infoText.text = "" + numToRemove;
        GameObject.FindObjectOfType<Wall> ().speed = wallSpeed;
		StartCoroutine(TextTimer ());
	}

    public override void UpdateRemoveText()
    {
        infoText.text = "Remove " + numToRemove;
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
        //only done in arcade mode
        if (gameType == GameType.Arcade)
        {
            HealthDrop(-.34f);
            failCounter += 1;
            if (failCounter == 2)
            {
                infoText.text = "Two finger tap to Undo";
            }
            chainAmt = 0;
            chainThreshold = 5;
            chainText.text = "Chain: x0";
        }
        //animation
        foreach (TestPolygon playerPoly in currentPolys)
        {
            playerPoly.color = Color.grey;
            playerPoly.ReDraw();
            GameObject.FindObjectOfType<CameraControl>().ScreenShake(.2f, 0f, .06f);
            while (cameraWait)
                yield return null;
            float i = 0f;
            Vector3 originalPos = playerPoly.transform.position;
            while (i < .1f)
            {
                playerPoly.transform.position += new Vector3(0f, .2f, 0f);
                i += .04f;
                yield return new WaitForSeconds(.02f);
            }
            i = 0f;
            while (i < .4f)
            {
                playerPoly.transform.position += new Vector3(0f, -.4f, 0f);
                i += .02f;
                yield return new WaitForSeconds(.02f);
            }
            playerPoly.transform.position = originalPos;
        }
		Destroy (failSoundGO);
		animDone = true;
	}

	void HealthDrop(float amt){
		t = 0f;
		newHealth = currHealth + amt;
		if (newHealth <= 0f) {
            //show game over stuff
            //check if highscore, overwrite if so.
            GameObject.FindObjectOfType<HighScore>().SubmitNewScore(score);
            //show game over screen
            GameOverUI.SetActive(true);
            GameOverUI.transform.Find("HS").GetComponent<Text>().text = "Your Score: " + score + "\nHigh Score: " + PlayerPrefs.GetInt("highestScore");

            paused = true;
			//StartCoroutine (RestartGame ());
		}
	}

	IEnumerator RestartGame(){
        GameObject.FindObjectOfType<HighScore>().SubmitNewScore(score);
		yield return new WaitForSeconds (.5f);
        ChangeTexture1(world1Texture);
        SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

    public void GoToNextWorld(int worldNum)
    {
        StartCoroutine(NextWorldTransition(worldNum));
    }

    //change world we are in
    IEnumerator NextWorldTransition(int worldNum)
    {
        twoOffsetProps = true;
        float blendVal = 0f;
        while (blendVal < 1f)
        {
            blendVal += .005f;
            if(blendVal < .5f)
            {
                RenderSettings.skybox.SetTextureOffset(cylinderOffsetProp2, new Vector2(blendVal, skyboxOffset));
                RenderSettings.skybox.SetTextureOffset(cylinderOffsetProperty, new Vector2(blendVal, skyboxOffset));

            }
            else if (blendVal >= .5f)
            {
                cylinderOffsetProperty = "_DownTex2";
                cylinderOffsetProp2 = "_DownTex";
                RenderSettings.skybox.SetTextureOffset(cylinderOffsetProp2, new Vector2(blendVal-1f, skyboxOffset));
                RenderSettings.skybox.SetTextureOffset(cylinderOffsetProperty, new Vector2(blendVal-1f, skyboxOffset));
            }
        
            RenderSettings.skybox.SetFloat("_Blend", blendVal);
            yield return null;
        }
        //ensure blend gets to exactly 1. maybe not necessary, but whatevs
        RenderSettings.skybox.SetFloat("_Blend", 1f);
        twoOffsetProps = false;
        //change to the next set of textures (texture1 is the new current, texture2 is for the next world transition)
        switch (worldNum)
        {
            case 2:
                ChangeTexture1(world2Texture);
                ChangeTexture2(world3Texture);
                break;
            case 3:
                ChangeTexture1(world3Texture);
                ChangeTexture2(world2Texture);
                break;
            default:
                Debug.LogError("Error: Unknown World Number");
                break;
        }
        //reset offset properties so the main one is _downtex
        cylinderOffsetProperty = "_DownTex";
        cylinderOffsetProp2 = "_DownTex2";
        //reset blend to 0
        RenderSettings.skybox.SetFloat("_Blend", 0f);
        SetWorldTextColors(worldNum);

        yield return null;
    }

    public void SetWorldTextColors(int world)
    {
        Color textColor, outlineColor;
        switch (world)
        {
            case 1:
                textColor = world1Text;
                outlineColor = world1Outline;
                break;
            case 2:
                textColor = world2Text;
                outlineColor = world2Outline;
                break;
            case 3:
                textColor = world3Text;
                outlineColor = world3Outline;
                break;
            default:
                Debug.LogError("Unknown World in SetTextColors!");
                textColor = world1Text;
                outlineColor = world1Outline;
                break;
        }
        //set up text colors
        scoreText.color = textColor;
        scoreText.GetComponent<UnityEngine.UI.Outline>().effectColor = outlineColor;
        chainText.color = textColor;
        chainText.GetComponent<UnityEngine.UI.Outline>().effectColor = outlineColor;
        infoText.color = textColor;
        infoText.GetComponent<UnityEngine.UI.Outline>().effectColor = outlineColor;
    }

	public void CreateNewPlayerPolygons(){

        Transform polygonParent = GameObject.Find("PlayerPolygon").transform;
        //remove the current removal markers
        //this needs to account for markers on multiple polygon objects
        foreach (GameObject marker in GameObject.FindGameObjectsWithTag("marker"))
        {
            Destroy(marker);
        }

        //create as many new polygons as the current level dictates
        //first, clear the currentPoly's list and destroy the objects that exist for them already
        if(currentPolys.Count > 0)
        {
            foreach(TestPolygon playerPolygon in polygonParent.GetComponentsInChildren<TestPolygon>())
            {
                Destroy(playerPolygon.gameObject);
            }
            currentPolys.Clear();
        }
        for (int i = 0; i < currentLevel.NumberOfShapes; i++)
        {
            //create a polygon
            
            TestPolygon newPolygon = ((GameObject)Instantiate(polyPrefab, polygonParent)).GetComponent<TestPolygon>();
            newPolygon.Setup();
            //different positioning for different number of shapes
            //special case for the third element in a 3 shape setup
            if(currentLevel.NumberOfShapes == 3)
            {
                if(i == 2)
                    newPolygon.transform.localPosition = new Vector3(0f, .5f, 0f);
                else
                    newPolygon.transform.localPosition = new Vector3(-1.2f + i * 2.4f, -1.8f, 0f);

            }
            else
            {
                newPolygon.transform.localPosition = new Vector3(-1.2f + i * 2.4f, 0f, 0f);
            }
            
            //newVerts needs to be a new List of vertices, copied from the list in memory
            //polyRangeHigh should never exceed polygons.count
            List<Vector3> newVerts = new List<Vector3>(polygons[Random.Range(polyRangeLow, polyRangeHigh)]);
            newPolygon.verticesList = newVerts;

            //base the number to remove on the base maximum and also how many vertices exist in the new shape
            int maxCanRemove = newPolygon.verticesList.Count - 3;
            newPolygon.numToRemove = Mathf.Clamp(Random.Range(1, maxCanRemove + 1), 1, currentLevel.BaseRemoveNum);

            
            newPolygon.vertices2D = System.Array.ConvertAll<Vector3, Vector2>(newVerts.ToArray(), v => v);
            newPolygon.color = playerPolyColors[UnityEngine.Random.Range(0, playerPolyColors.Length)];
            newPolygon.OutlineFadeInEffect();
            currentPolys.Add(newPolygon);
        }

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

        //currentPoly.ReDraw ();
        pointerController.removedVertices.Clear();
	}

    public void CreateHoles()
    {

        //if there is at least one hole object
        //destroy all hole objects and clear the current holes list
        if (currentHoles.Count != 0)
        {
            foreach (TestPolygon hole in GameObject.Find("Wall").GetComponentsInChildren<TestPolygon>())
            {
                Destroy(hole.gameObject);
            }
            currentHoles.Clear();
        }
        //create any new holes to be associated with the new player polygons
        int i = 0;
        foreach (TestPolygon playerPoly in currentPolys)
        {
            TestPolygon newHolePolygon = GameObject.FindObjectOfType<Wall>().CreateHoleShape(playerPoly.verticesList, playerPoly.numToRemove);
            //different positioning for different number of shapes
            //special case for the third element in a 3 shape setup
            if (currentLevel.NumberOfShapes == 3)
            {
                if (i == 2)
                    newHolePolygon.transform.localPosition = new Vector3(0f, .5f, 0f);
                else
                    newHolePolygon.transform.localPosition = new Vector3(-1.2f + i * 2.4f, -1.8f, 0f);

            }
            else
            {
                newHolePolygon.transform.localPosition = new Vector3(-1.2f + i * 2.4f, 0f, 0f);
            }

            //newHolePolygon.transform.localPosition = new Vector3(-1f + i * 2f, 0f, 0f);
            currentHoles.Add(newHolePolygon);
            //make sure the player polygon has a reference to it's approaching "hole"
            playerPoly.holeToMatch = newHolePolygon;
            i += 1;
        }
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

public enum GameType
{
    Arcade,
    CasualStandard,
    CasualCustom
}
