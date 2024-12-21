using UnityEngine;

public class AreaEntrance : MonoBehaviour
{
    [SerializeField] private string transitionName;

    private void Start()
    {
        string currentTransitionName = SceneManagement.Instance.SceneTransitionName;

        if (currentTransitionName != null && currentTransitionName.Equals(transitionName))
        {
            // Atribui a posição do jogador para a posição deste transform
            if (PlayerController.Instance != null)
            {
                PlayerController.Instance.transform.position = transform.position;
            }
            else
            {
                Debug.LogWarning("PlayerController.Instance não está atribuído.");
            }
        }
    }
}
