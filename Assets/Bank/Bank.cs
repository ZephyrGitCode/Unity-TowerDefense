using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Bank : MonoBehaviour
{

    [SerializeField] int startingBalance = 150;
    [SerializeField] int currentBalance;
    MenuLogic menuLogic;
    public int CurrentBalance { get { return currentBalance; } }

    [SerializeField] TextMeshProUGUI displayBalance;
    
    private void Awake() {
        menuLogic = FindObjectOfType<MenuLogic>();
        currentBalance = startingBalance;
        UpdateDisplay();
    }
    
    public void Deposit(int amount)
    {
        currentBalance += Mathf.Abs(amount);
        UpdateDisplay();
    }

    public void Withdraw(int amount)
    {
        currentBalance -= Mathf.Abs(amount);
        UpdateDisplay();
        if(currentBalance < 0)
        {
            // If less than 1 gold, end game
            EndGame();
        }
    }

    public void UpdateDisplay()
    {
        displayBalance.text = "Gold: " + currentBalance;
    }

    public void EndGame()
    {
        //Scene currentScene = SceneManager.GetActiveScene();
        //SceneManager.LoadScene("MainMenu");
        // Call menulogic to load end game ui, return to main menu
        menuLogic.LoseUI();
    }
}
