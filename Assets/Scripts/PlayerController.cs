using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerController: MonoBehaviour
{
    public float moveSpeed;
    private bool isMoving;
    private Vector2 input;
    private Animator animator;
    public LayerMask solidObjectsLayer;
    public LayerMask interactableLayer;
    private Rigidbody2D rb;

    private bool canMove = false; // Variável para controlar a movimentação

    private void Awake()
    {   
        if (FindObjectsOfType<PlayerController>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Inscreve no evento de cena carregada
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Chama o método na primeira cena
        MoveToSavedPosition();
    }

    // Método chamado quando uma nova cena é carregada
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        MoveToSavedPosition();
    }

    // Lógica para reposicionar o player
    private void MoveToSavedPosition()
    {
        if (PlayerPrefs.HasKey("TargetX") && PlayerPrefs.HasKey("TargetY") && PlayerPrefs.HasKey("TargetZ"))
        {
            Vector3 targetPosition = new Vector3(
                PlayerPrefs.GetFloat("TargetX"),
                PlayerPrefs.GetFloat("TargetY"),
                PlayerPrefs.GetFloat("TargetZ")
            );

            // Define a direção inicial do jogador
            animator.SetFloat("moveX", PlayerPrefs.GetFloat("DirectionX"));
            animator.SetFloat("moveY", PlayerPrefs.GetFloat("DirectionY"));

            transform.position = targetPosition;

            // Limpa os dados salvos
            PlayerPrefs.DeleteKey("TargetX");
            PlayerPrefs.DeleteKey("TargetY");
            PlayerPrefs.DeleteKey("TargetZ");
            PlayerPrefs.DeleteKey("DirectionX");
            PlayerPrefs.DeleteKey("DirectionY");
        }
    }

    private void OnDestroy()
    {
        // Remove o evento para evitar erros
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


public void HandleUpdate()
{
    if (!canMove) // Bloqueia a movimentação
        {
            animator.SetBool("isMoving", false);
            return;
        }

    // Detecta se a tecla "X" está sendo pressionada para correr
    if (Input.GetKey(KeyCode.X))
    {
        animator.SetBool("isRunning", true);
    }
    else
    {
        animator.SetBool("isRunning", false);
    }

    //
    if (!isMoving)
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        if (input.x != 0) input.y = 0;

        if (input != Vector2.zero)
        {
            animator.SetFloat("moveX", input.x);
            animator.SetFloat("moveY", input.y);

            var targetPos = transform.position;
            targetPos.x += input.x;
            targetPos.y += input.y;

            if (IsWalkable(targetPos))
                StartCoroutine(Move(targetPos));
        }
    }

    animator.SetBool("isMoving", isMoving);

    // Verifica a tecla de interação de acordo com a tag do objeto
    if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.UpArrow))
        Interact();
}

void Interact()
{
    // Calcula a posição de interação baseada na direção do jogador
    var facingDir = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY") - 0.5f);
    var interactPos = transform.position + facingDir;

    // Desenha uma linha para debug
    // Debug.DrawLine(transform.position, interactPos, Color.red, 1f);

    // Detecta o objeto na posição de interação
    var collider = Physics2D.OverlapCircle(interactPos, 0.2f, interactableLayer);

    if (collider != null)
    {
        // Verifica se o objeto tem a tag "Door"
        if (collider.CompareTag("Door"))
        {
            // Interage apenas se a seta para cima for pressionada
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                collider.GetComponent<Interactable>()?.Interact();
            }
        }
        else
        {
            // Interage normalmente com a tecla Z para outros objetos
            if (Input.GetKeyDown(KeyCode.Z))
            {
                collider.GetComponent<Interactable>()?.Interact();
            }
        }
    }
}
    public void SetCanMove(bool value)
    {
        canMove = value;
    }
    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon) // enquanto a distancia a ser percorrida é maior que um valor minúsculo
        {
            // Detecta se a tecla "X" está sendo pressionada e ajusta a velocidade
            float currentSpeed = Input.GetKey(KeyCode.X) ? moveSpeed * 2 : moveSpeed;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, currentSpeed * Time.deltaTime);;
            yield return null;
        }
        transform.position = targetPos;
        isMoving = false;
    }

    private bool IsWalkable(Vector3 targetPos)
    {
        var adjTargetPos = targetPos;
        adjTargetPos.y = targetPos.y - 0.5f;
        if (Physics2D.OverlapCircle(adjTargetPos, 0.2f, solidObjectsLayer | interactableLayer) != null)
        {
            return false;
        }
        return true;
    }
}
