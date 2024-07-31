using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Text))]
public class Score : MonoBehaviour
{
    private int scoreAmount;
    public int ScoreAmount 
    { 
        get { return scoreAmount;  } 
        set 
        {
            scoreAmount = value;
            GetComponent<Text>().text = scoreAmount.ToString();
        } 
    }
    public void Add() => ScoreAmount++;
}
