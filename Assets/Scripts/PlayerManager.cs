using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public static bool gameOver;
    public GameObject gameOverPanel;

    public static bool isGamerStarted;
    public GameObject startingText;
    public GameObject gameTitle;

    public static int numberOfCoins;
    public Text coinsText;

    /*Inicia as variaveis*/
    void Start()
    {
        gameOver = false;
        Time.timeScale = 1;
        isGamerStarted = false;
        numberOfCoins = 0;
    }

    void Update()
    {
        /*Ativa o painel de game over quando encostar em um objeto*/
        if(gameOver)
        {
            Time.timeScale = 0;
            gameOverPanel.SetActive(true);
        }
        coinsText.text = "Donuts: " + numberOfCoins;
        /*Se clicar na tela inicia o jogo*/
        if(SwipeManager.tap)
        {
            isGamerStarted = true;
            Destroy(gameTitle);
            Destroy(startingText);
        }
    }

}
