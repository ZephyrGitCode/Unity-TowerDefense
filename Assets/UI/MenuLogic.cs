using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLogic : MonoBehaviour
{
    [SerializeField] Canvas menuCanvas;
    [SerializeField] Canvas helpCanvas;

    // Update is called once per frame
    void Update()
    {
        
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
}
