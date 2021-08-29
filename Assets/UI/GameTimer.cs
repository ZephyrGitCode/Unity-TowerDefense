using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshPro))]
public class GameTimer : MonoBehaviour
{
    TextMeshPro gameTime;
    float val;
    void Awake()
    {
        gameTime = GetComponent<TextMeshPro>();
    }

    void Start()
    {
        val = 0;
    }

    void Update()
    {
        val += Time.deltaTime;
        double b = System.Math.Round(val, 2);
        gameTime.text = b.ToString();
    }

}
