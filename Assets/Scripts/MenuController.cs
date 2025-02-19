using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject mainMenu; // Referência ao GameObject do menu
    public void PlayGame()
    {
        StartCoroutine(StartGameSequence());
    }

    private IEnumerator StartGameSequence()
    {
        // Faz o Fade Out
        yield return ScreenFade.Instance.FadeOut();

        // Desativa o menu depois do fade
        mainMenu.SetActive(false);

        MusicManager.Instance.PlayMusic(1);
        // Faz o Fade In
        yield return ScreenFade.Instance.FadeIn();

        GameController.Instance.SetPlayerCanMove(true);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // Sai do modo de jogo no editor
        #else
            Application.Quit(); // Fecha o jogo na versão build
        #endif
    }
}
