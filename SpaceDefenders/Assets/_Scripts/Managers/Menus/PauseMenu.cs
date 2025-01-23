using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using SpaceDefenders.GameManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private HighscoreCommunicator scoreComm;

    [SerializeField] private GameObject controls;
    [SerializeField] private GameObject pauseOverlay;
    [HideInInspector] public bool isPaused = false;
    private bool pauseWindowOpen;
    [SerializeField] private AudioSource player_MoveSource;

    private enum Mode { Def, Sur, Time};
    [SerializeField] private Mode mode;
    [SerializeField] private Timer timer;
    [SerializeField] private TimeModeGameManager manager;

    void Update()
    {
        if(isPaused)
        {
            player_MoveSource.Stop();
            controls.SetActive(false);
            Time.timeScale = 0f;
        }
        else
        {
            controls.SetActive(true);
            Time.timeScale = 1f;
        }

        if(pauseWindowOpen)
        {
            pauseOverlay.SetActive(true);
            isPaused = true;
        }
        else
        {
            pauseOverlay.SetActive(false);
            isPaused = false;
        }
    }

    public void TogglePause()
    {
        pauseWindowOpen = !pauseWindowOpen;
    }

    public void MainMenu()
    {
        switch (mode)
        {
            case Mode.Def:
                scoreComm.DefendMode = timer.minutes * 60 + timer.seconds;
                break;

            case Mode.Sur:
                scoreComm.SurvivalMode = timer.minutes * 60 + timer.seconds;
                break;

            case Mode.Time:
                scoreComm.TimeMode = manager.enemysKilled;
                break;
        }
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit()
    {
        Application.Quit();
    }
}