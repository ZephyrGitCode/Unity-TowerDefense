using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IncomeManager : MonoBehaviour
{
    [SerializeField, Tooltip("Time remaining in seconds")]
    public float setTimeRemaining = 30f;

    private float countdown;

    [Tooltip("")]
    public TextMeshProUGUI IncomeTimer;

    public List<Bank> allBanks = new List<Bank>();

    void Start()
    {
        countdown = setTimeRemaining;
    }

    /// <summary>
    /// Add a bank to all banks. Called by PlayerSelector.
    /// </summary>
    public void AddBank(Bank givenBank)
    {
        allBanks.Add(givenBank);
    }

    /// <summary>
    /// Countdown timer, trigger bank income then reset timer
    /// </summary>
    void Update()
    {
        if (countdown > 0)
        {
            countdown -= Time.deltaTime;
            IncomeTimer.text = countdown.ToString();
        }
        else
        {
            BankIncome();
            countdown = setTimeRemaining;
            IncomeTimer.text = countdown.ToString();
        }
    }

    /// <summary>
    /// Tell the bank to give income. Called by IncomeManager.
    /// </summary>
    void BankIncome()
    {
        foreach (Bank myBank in allBanks)
        {
            myBank.GiveIncome();
        }
    }
}
