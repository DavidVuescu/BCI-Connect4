using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameControlls;
using System;
using Unity.ItemRecever;
using Intendix.Board;

public class SceneChanger : MonoBehaviour
{

    public void Start_game()
    {
        SceneManager.LoadScene("Game");
    }

    public void Start_game_de_la_tastatura()
    {

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            SceneManager.LoadScene("Game");
        }
    }

    public void Quit_game()
    {
        Debug.Log("exitgame");
        Application.Quit();
    }

    public void Quit_game_de_la_tastatura()
    {

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Debug.Log("exitgame");
            Application.Quit();
        }
    }

    void Update()
    {
        Start_game_de_la_tastatura();
        Quit_game_de_la_tastatura();
    }
}
