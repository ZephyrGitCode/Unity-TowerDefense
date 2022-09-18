using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField, Tooltip("This unit's name.")]
    public string myName = "Hot Roller";

    [SerializeField, Tooltip("This unity's Type.")]
    public string myType = "Slow, low health";
    [SerializeField] public int myCost = 5;

    [SerializeField] int goldReward = 25;
    [SerializeField] int lifeDamage = 1;
    [SerializeField] int incomeAdd = 1;

    public Bank myBank;

    public GameObject myPrefab;

    public bool canBeShot = false;

    public void RewardGold()
    {
        if(myBank == null) { return; }
        myBank.Deposit(goldReward);
    }

    public void TakeLife()
    {
        // TODO Take player's life
        //if(player == null) { return; }
        // Take health
        if (true) // Player health > 0
        {
            // take life
            Debug.Log("player life: ");
        }else
        {
            Debug.Log("Died");
        }
        // Spawn unit at the start of lane again
        //player.take(lifeDamage);
    }

    public void EnableBeingShot()
    {
        canBeShot = true;
    }
}
