using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberSelector : MonoBehaviour
{

    public int min, max, current;
    public Text numText;


    public delegate void OnNumberChanged();

    public static event OnNumberChanged onNumberChanged;

    // Start is called before the first frame update
    void Start()
    {
        int i = min;
        if(Int32.TryParse(numText.text, out i))current = i;

        if (numText == null) numText = transform.Find("NumText").GetComponent<Text>();
        numText.text = "" + current;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextNum()
    {
        if (current < max) current += 1;
        else if (current == max) current = min;
        else Debug.Log("we got a problem. bad number in selector");
        numText.text = "" + current;
        if (onNumberChanged != null) onNumberChanged();
    }

    public void PreviousNum()
    {
        if (current > min) current -= 1;
        else if (current == min) current = max;
        else Debug.Log("we got a problem. bad number in selector");
        numText.text = "" + current;
        if (onNumberChanged != null) onNumberChanged();

    }
}
