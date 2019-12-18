using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomMaxRemovals : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        NumberSelector.onNumberChanged += ChangeNumberIfOverMaxSides;
        GetComponent<NumberSelector>().max = 1;
    }

    void OnDisable()
    {
        NumberSelector.onNumberChanged -= ChangeNumberIfOverMaxSides;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeNumberIfOverMaxSides()
    {
        int currentSidesHigh = GameObject.Find("sideshigh").GetComponentInChildren<NumberSelector>().current;
        //maximum number we should be able to use for removals is whatever can take us from the high number of sides down to 3 sides
        int newMax = currentSidesHigh - 3;
        NumberSelector numSelect = GetComponent<NumberSelector>();
        numSelect.max = newMax;
        if (numSelect.current > newMax)
        {
            numSelect.current = newMax;
            numSelect.numText.text = "" + numSelect.current;
        }

    }
}
