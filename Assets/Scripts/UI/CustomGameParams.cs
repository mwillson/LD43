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
        Debug.LogError("on enable custom params");
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
        Debug.Log("OnSceneLoaded: " + scene.name);
        GameManager gm = GameObject.FindObjectOfType<GameManager>();
        if (active) gm.gameType = GameType.CasualCustom;
        else gm.gameType = GameType.CasualStandard;
    }
}
