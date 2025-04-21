using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

//This should be attached to a weapon collision script. Use the regiter hit there in order for thi script to do its thing.
//It should be noted everything reverts once the timer reaches zero. May have to adjust that if implementing a super attack.
public class ComboCounter : MonoBehaviour
{
public TextMeshProUGUI comboText;
public ScoreCalculator scoreCalculator;

//Player Hits System
public int hits = 0;

//Timer Elements
float currentTime;
public float timerStart = 4f;

//Gets input from tool collision
    public void registerHit()
    {
        hits ++;
        currentTime = timerStart;
        
    }


    
    void Update()
    {
///Updating timer for combos, combos outputted to canvas
        currentTime -= 1 * Time.deltaTime;

        if (currentTime > 0)
        {
            currentTime -= 1 * Time.deltaTime;
        }

        else
        {
            currentTime = 0;
            scoreCalculator.UpdateScore(hits);
            Debug.Log(hits);
            hits = 0;
            comboText.text = "";
        }

        if (hits >= 1)
        {
        comboText.text = hits.ToString() + " Hit Combo!";
        
        }
    }
}

