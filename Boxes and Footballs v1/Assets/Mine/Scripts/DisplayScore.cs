using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI score_your;
    [SerializeField] private TextMeshProUGUI score_;
    [SerializeField] private TextMeshProUGUI score_their;
    [SerializeField] private TextMeshProUGUI result;
    [SerializeField] private TextMeshProUGUI final_score;
    public void display()
    {
        int your_score = System.Int32.Parse(score_your.text);
        int their_score = System.Int32.Parse(score_their.text);
        string output;

        Color to_display = Color.white;

        if (your_score > their_score) 
        {
            output = "YOU WIN";
            to_display = Color.blue;
        }
        else if (your_score < their_score)
        {
            output = "YOU LOSE";
            to_display = Color.red;
        }
        else
        {
            output = "DRAW";
        }
        GetComponent<TextMeshProUGUI>().text = "";
        score_.text = "";
        score_their.text = "";
        score_your.text = "";
        result.text = output;
        result.color = to_display;
        final_score.text = "" + your_score + "  -  " + their_score;
    }
}
