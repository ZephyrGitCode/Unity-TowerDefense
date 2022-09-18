using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyHealth : MonoBehaviour
{
    [Tooltip("Max health.")]
    [SerializeField] public int maxHitPoints = 5;
    
    [Tooltip("Current health.")]
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
    
    /// <summary>
    /// Instantiate new units within spawn area. Called by user when spawning new units.
    /// </summary>
    void OnParticleCollision(GameObject other)
    {
        ProcessHit(other);
        CoolDown();
    }

    void ProcessHit(GameObject other)
    {
        //TODO: Other damage
        //currentHitPoints - other.damage
        currentHitPoints--;
        
        //material.color.a = 0;
        if(currentHitPoints <= 0)
        {
            Destroy(this.gameObject);
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
