using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHitPoints = 5;
    [Tooltip("Adds additional max health to enemy.")]
    [SerializeField] int diffucltiyRamp = 1;
    [SerializeField] int currentHitPoints = 0;

    Enemy enemy;
    Transform cube;

    Material currentMat;
    private void Start() {
        enemy = GetComponent<Enemy>();
        cube = enemy.gameObject.transform.GetChild(1);
    }

    void OnEnable()
    {
        currentHitPoints = maxHitPoints;
    }
    
    void OnParticleCollision(GameObject other)
    {
        ProcessHit();
        CoolDown();
    }
    void ProcessHit()
    {
        currentHitPoints--;
        
        //material.color.a = 0;
        if(currentHitPoints <= 0)
        {
            gameObject.SetActive(false);
            maxHitPoints += diffucltiyRamp;
            enemy.RewardGold();
        }
    }

    void CoolDown()
    {
        Material currentMat = cube.GetComponent<Renderer>().material;
        Color currentColor = currentMat.color;
        Color newColor = currentColor;
        if(currentHitPoints>50)
        {
            newColor = new Color(currentColor.r, currentColor.g, currentColor.b, 0.8f);
        }
        if(currentHitPoints>30)
        {
            newColor = new Color(currentColor.r, currentColor.g, currentColor.b, 0.5f);
        }
        if(currentHitPoints>20)
        {
            newColor = new Color(currentColor.r, currentColor.g, currentColor.b, 0.3f);
        }
        if(currentHitPoints>10)
        {
            newColor = new Color(currentColor.r, currentColor.g, currentColor.b, 0.2f);
        }
        if(currentHitPoints>5)
        {
            newColor = new Color(currentColor.r, currentColor.g, currentColor.b, 0.1f);
        }
        
        currentMat.color = newColor;
    }
}
