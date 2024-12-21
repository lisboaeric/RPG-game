using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorLevelMove : MonoBehaviour, Interactable
{
    public int sceneBuildIndex; // Índice da cena para carregar
    public Vector3 targetPosition; // Coordenadas para onde o jogador será movido
    public Vector2 targetDirection; // Direção em que o jogador estará ao carregar a nova cena
    [SerializeField] Animator doorAnim; // Animação da porta
    [SerializeField] Animator transitionAnim; // Animação de transição entre cenas
    public void Interact()
    {
        StartCoroutine(OpenDoorAndChangeScene());
    }
    private IEnumerator OpenDoorAndChangeScene()
    {
        GameController.Instance.SetPlayerCanMove(false);
        doorAnim.SetTrigger("Open"); // Ativa a animação de abertura da porta
        yield return new WaitForSeconds(0.5f); // Aguarda a animação da porta (ajuste conforme necessário)

        // Salva a posição do jogador
        PlayerPrefs.SetFloat("TargetX", targetPosition.x);
        PlayerPrefs.SetFloat("TargetY", targetPosition.y);
        PlayerPrefs.SetFloat("TargetZ", targetPosition.z);
        PlayerPrefs.SetFloat("DirectionX", targetDirection.x);
        PlayerPrefs.SetFloat("DirectionY", targetDirection.y);

        // Transição de cena
        transitionAnim.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single);
        GameController.Instance.SetPlayerCanMove(true);
        
    }
}
