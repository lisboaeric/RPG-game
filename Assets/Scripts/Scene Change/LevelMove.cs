using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMove : MonoBehaviour
{
    public int sceneBuildIndex;  // Índice da cena para carregar
    public Vector3 targetPosition;  // Coordenadas para onde o jogador será movido

    [SerializeField] Animator transitionAnim;
    [SerializeField] GameController gameController;

    private void OnTriggerEnter2D(Collider2D other)
    {
        print("Trigger entered");

        if (other.tag == "Player")
        {
            print("Switching Scene to " + sceneBuildIndex);

            // Armazena a posição para uso na próxima cena
            PlayerPrefs.SetFloat("TargetX", targetPosition.x);
            PlayerPrefs.SetFloat("TargetY", targetPosition.y);
            PlayerPrefs.SetFloat("TargetZ", targetPosition.z);

            StartCoroutine(LoadLevel());

            IEnumerator LoadLevel()
            {
                GameController.Instance.SetPlayerCanMove(false);
                transitionAnim.SetTrigger("Start");
                yield return new WaitForSeconds(1);
                SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single);
                transitionAnim.SetTrigger("End");
                GameController.Instance.SetPlayerCanMove(true);
            }
        }
    }
}
