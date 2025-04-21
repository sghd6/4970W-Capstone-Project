using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreCalculator : MonoBehaviour
{
    public ComboCounter comboCounter;
    int score = 0;
    public TextMeshProUGUI scoreUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

  void Update()
  {
   
  }
    // Update is called once per frame
    public void UpdateScore(int hit)
    {
        score += (hit*hit);
        scoreUI.text = "Score: " + score.ToString();
        Debug.Log(score);

    }
    
}
