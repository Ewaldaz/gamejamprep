﻿using Assets.Scripts.Core;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    [Header("Game objects")]
    public GameObject Player;
    public GameObject WinPanel;
    public GameObject LoosePanel;
    public GameObject PausePanel;
    public AudioSource CoinCollectedSound;
    public GameObject WinEffect;
    public NavMeshSurface Surface;
    public GameObject[] Tiles;
    public GameObject[] Coins;

    [Space]
    [Header("Map settings")]
    public int MapWidth = 10;
    public int MapHeight = 20;
    public int RectangleCount = 5;
    public float TileSize = 0.1f;
    [Space]
    [Header("UI Related")]
    public int CoinsRemaining;
    public Text RemainingText;
    public Text HighScoreText;

    bool ended = false;
    float restartDelay = 1f;
    const string PlayerPrefsKey = "HighScore";
    
    void Start()
    {
        Time.timeScale = 1;
        CoinsRemaining = MapGenerator.GenerateMap(Tiles, MapWidth, MapHeight, RectangleCount, TileSize, Coins, CoinCollectedSound);
        SetHighScoreText();
        InitNavMesh();
    }

    private void InitNavMesh()
    {
        Surface.BuildNavMesh();
    }

    private void SetHighScoreText(int score = 0)
    {
        if (score == 0)
        {
            score = PlayerPrefs.GetInt(PlayerPrefsKey, 0);
        }
        HighScoreText.text = string.Format("Highscore: {0}", score);
    }

    void Update()
    {
        RemainingText.text = string.Format("Remaining: {0}", CoinsRemaining);
        if (!ended && CoinsRemaining == 0)
        {
            ended = true;
            WinGame();
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            PauseResumeGame();
        }
    }

    private void PauseResumeGame()
    {
        if (PausePanel.activeSelf)
        {
            PausePanel.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            PausePanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    private void WinGame()
    {
        CheckHighScore();
        StartCoroutine(PlayWinEffect());
    }

    private IEnumerator PlayWinEffect()
    {
        if (WinEffect != null)
        {

            Instantiate(WinEffect, Player.transform);
            yield return new WaitForSeconds(2);
        }

        Invoke("ShowWinPanel", restartDelay / 2);
    }

    private void CheckHighScore()
    {
        var highScore = PlayerPrefs.GetInt(PlayerPrefsKey, 0);
        var score = Player.GetComponent<PlayerController>().Score;
        if (score > highScore)
        {
            PlayerPrefs.SetInt(PlayerPrefsKey, score);
            SetHighScoreText(score);
        }
    }

    private void ShowWinPanel()
    {
        WinPanel.SetActive(true);
    }

    public void EndGame()
    {
        if (!ended)
        {
            ended = true;
            Player.GetComponent<PlayerController>().loseLife();
            Invoke("ShowLoosePanel", restartDelay);
        }
    }

    private void ShowLoosePanel()
    {
        LoosePanel.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

