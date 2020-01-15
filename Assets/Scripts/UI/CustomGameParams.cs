using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CustomGameParams : MonoBehaviour
{

    public int shapes, sidesLow, sidesHigh, maxRemovals;

    public Text shapesText, sidesLowText, sidesHighText, maxRemovalsText;

    public delegate void NumberChangedDelegate();

    public bool active;

    public static CustomGameParams _instance;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
        NumberSelector.onNumberChanged += ChangeParams;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        NumberSelector.onNumberChanged -= ChangeParams;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeParams()
    {
        Debug.Log("changing params!");
        shapes = int.Parse(shapesText.text);
        sidesLow = int.Parse(sidesLowText.text);
        sidesHigh = int.Parse(sidesHighText.text);
        maxRemovals = int.Parse(maxRemovalsText.text);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {


        if (scene.name == "casual" || scene.name == "scene1")
        {
            GameManager gm = GameObject.FindObjectOfType<GameManager>();
            if (active) gm.gameType = GameType.CasualCustom;
            else gm.gameType = GameType.CasualStandard;
        }
        else if(scene.name == "casualmenu")
        {
            shapesText = GameObject.Find("numshapes").transform.Find("NumSelector").Find("NumText").GetComponent<Text>();
            sidesLowText = GameObject.Find("sideslow").transform.Find("NumSelector").Find("NumText").GetComponent<Text>();
            sidesHighText = GameObject.Find("sideshigh").transform.Find("NumSelector").Find("NumText").GetComponent<Text>();
            maxRemovalsText = GameObject.Find("removalsper").transform.Find("NumSelector").Find("NumText").GetComponent<Text>();

            shapes = 1;
            sidesLow = 4;
            sidesHigh = 4;
            maxRemovals = 1;
        }
    }
}
