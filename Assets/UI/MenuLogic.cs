using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuLogic : MonoBehaviour
{
    [SerializeField] Canvas menuCanvas;
    [SerializeField] Canvas helpCanvas;
    [SerializeField] TextMeshProUGUI endText;

    GameTimer gameTimer;

    void Awake() {
        gameTimer = FindObjectOfType<GameTimer>();
    }
    public void gameHelp()
    {
        // disable menu UI, enable second UI over the game
        menuCanvas.gameObject.SetActive(false);

        helpCanvas.gameObject.SetActive(true);
    }

    public void MainMenu()
    {
        // disable menu UI, enable second UI over the game
        menuCanvas.gameObject.SetActive(true);

        helpCanvas.gameObject.SetActive(false);
    }

    public void LoadGame()
    {
        // Load Game Scene
        SceneManager.LoadScene("Game");
    }

    public void LoadMenu()
    {
        // Load Game Scene
        SceneManager.LoadScene("MainMenu");
    }

    public void LoseUI()
    {
        // Launch ending UI, btn to menu screen
        gameTimer.LoseGame();
        endText.gameObject.SetActive(true);
        endText.text = "Finish\nFinal Time: "+gameTimer.Val;
    }
}
