using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class GameTimer : MonoBehaviour
{
    TextMeshProUGUI gameTime;
    float val;

    public float Val { get { return val; } }
    
    bool lose = false;
    void Awake()
    {
        gameTime = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        val = 0;
    }

    void Update()
    {
        if(!lose)
        {
            UpdateTimer();
        }
        
    }

    void UpdateTimer()
    {
        val += Time.deltaTime;
        double b = System.Math.Round(val, 2);
        gameTime.text = b.ToString();
    }

    public void LoseGame()
    {
        lose = true;
    }

}
