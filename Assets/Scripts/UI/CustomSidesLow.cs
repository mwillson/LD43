using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomSidesLow : NumberSelector
{
    
    void OnEnable()
    {
        NumberSelector.onNumberChanged += ValidateNumLessThanHigh;
    }

    private void OnDisable()
    {
        NumberSelector.onNumberChanged -= ValidateNumLessThanHigh;
    }

    public void ValidateNumLessThanHigh()
    {

        int currentSidesHigh = GameObject.Find("sideshigh").GetComponentInChildren<NumberSelector>().current;
        max = currentSidesHigh;
        if (current > currentSidesHigh)
        {
            current = currentSidesHigh;
            numText.text = "" + current;
        }
    }
    
}
