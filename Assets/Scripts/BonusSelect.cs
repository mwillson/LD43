using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusSelect : MonoBehaviour
{

    public int healthBonus, removeBonus, slowBonus;

    public Text healthText, removeText, slowText;

    [SerializeField]
    private Manager gameManager;

    [SerializeField]
    private GameObject pointerControllerGO;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddHealthBonus()
    {
        healthBonus += 1;
        healthText.text = "" + healthBonus;
        gameManager.paused = false;
        pointerControllerGO.SetActive(true);
        gameObject.SetActive(false);
    }

    public void AddRemoveBonus()
    {
        removeBonus += 1;
        removeText.text = "" + removeBonus;
        gameManager.paused = false;
        pointerControllerGO.SetActive(true);
        gameObject.SetActive(false);

    }

    public void AddSlowBonus()
    {
        slowBonus += 1;
        slowText.text = "" + slowBonus;
        gameManager.paused = false;
        pointerControllerGO.SetActive(true);
        gameObject.SetActive(false);

    }
}
