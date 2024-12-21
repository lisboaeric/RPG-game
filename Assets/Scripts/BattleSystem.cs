using System.Collections;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    [SerializeField] GameObject battleUI; // Referência à interface de batalha
    [SerializeField] private Character playerCharacter;
    [SerializeField] private Character enemyCharacter;

    public static BattleSystem Instance { get; private set; }

    private void Awake()
    {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
        
        Instance = this;
        DontDestroyOnLoad(gameObject); // Torna o Battle System persistente entre cenas
        battleUI.SetActive(false);
    }
    private void Start()
    {
        // Inicializando personagens (apenas para testes, pode usar dados externos)
        playerCharacter = new Character("Player", 100, 30);
        enemyCharacter = new Character("Enemy", 80, 15);
    }
    public void StartBattle()
    {
        Debug.Log("Começou");

        battleUI.SetActive(true);

        // Negocio que atualizar o canvas (não sei se funciona)
        Canvas.ForceUpdateCanvases();

        // Chamando o gerenciador de UI
        BattleUIManager.Instance.SetPlayerUI(playerCharacter.Name, playerCharacter.CurrentHP, playerCharacter.MaxHP);
        BattleUIManager.Instance.SetEnemyUI(enemyCharacter.Name, enemyCharacter.CurrentHP, enemyCharacter.MaxHP);
        BattleUIManager.Instance.UpdateMessage("The battle begins!");
        
        StartCoroutine(BattleLoop());
    }

    public void EndBattle()
    {
        battleUI.SetActive(false);
        GameController.Instance.EndBattle();
    }

    public void HandleUpdate()
    {
        // Lógica para atualizar a batalha no frame atual
    }

    private IEnumerator BattleLoop()
    {
        // Exemplo de fluxo de batalha
        while (true)
        {
            yield return PlayerTurn();
            if (IsBattleOver()) break;

            yield return EnemyTurn();
            if (IsBattleOver()) break;
        }
        EndBattle();
    }

    private IEnumerator PlayerTurn()
    {
        Debug.Log("Player's Turn");
        bool actionTaken = false;

        while (!actionTaken)
        {
            // Aqui você implementa lógica para detectar entrada do jogador
            if (Input.GetKeyDown(KeyCode.UpArrow)) // Exemplo: seta pra cima para atacar
            {
                Debug.Log("Player chose to Attack!");
                actionTaken = true;
                yield return ExecutePlayerAttack();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow)) // Futuro: defender ou outro comando
            {
                Debug.Log("Player chose to Defend!");
                actionTaken = true;

                yield return new WaitForSeconds(1); // Placeholder para outra ação
            }
            yield return null; // Espera o próximo frame
        } 
    }

    private IEnumerator EnemyTurn()
    {
        Debug.Log("Enemy's Turn");

        // Escolha de ação (ex.: ataque aleatório)
        yield return new WaitForSeconds(1); // Placeholder para "pensar"
        Debug.Log("Enemy attacked the player!");
        
        yield return ExecuteEnemyAttack();
    }

    private bool IsBattleOver()
    {
        if (playerCharacter.IsDefeated)
        {
            Debug.Log("Player has been defeated!");
            return true;
        }
        if (enemyCharacter.IsDefeated)
        {
            Debug.Log("Enemy has been defeated!");
            return true;
        }
        return false;
    }

    private IEnumerator ExecutePlayerAttack()
    {
        Debug.Log("Player attacks!");
        enemyCharacter.CurrentHP -= playerCharacter.AttackPower;
        if (enemyCharacter.CurrentHP <= 0) {enemyCharacter.CurrentHP = 0;};
        Debug.Log($"Enemy HP: {enemyCharacter.CurrentHP}/{enemyCharacter.MaxHP}");

        BattleUIManager.Instance.SetEnemyUI(enemyCharacter.Name, enemyCharacter.CurrentHP, enemyCharacter.MaxHP);
        BattleUIManager.Instance.UpdateMessage("Player attacked the enemy!");

        // Verificar se o inimigo foi derrotado
        if (enemyCharacter.IsDefeated)
        {
            Debug.Log("Enemy is defeated!");
            yield break; // Sai do ataque porque o inimigo foi derrotado
        }

        yield return new WaitForSeconds(1); // Animação de ataque ou tempo de espera
    }

    private IEnumerator ExecuteEnemyAttack()
    {
        Debug.Log("Enemy attacks!");
        playerCharacter.CurrentHP -= enemyCharacter.AttackPower;
        Debug.Log($"Player HP: {playerCharacter.CurrentHP}/{playerCharacter.MaxHP}");

        BattleUIManager.Instance.SetPlayerUI(playerCharacter.Name, playerCharacter.CurrentHP, playerCharacter.MaxHP);
        BattleUIManager.Instance.UpdateMessage("Enemy attacked player!");

        // Verificar se o jogador foi derrotado
        if (playerCharacter.IsDefeated)
        {
            Debug.Log("Player is defeated!");
            yield break; // Sai do ataque porque o jogador foi derrotado
        }

        yield return new WaitForSeconds(1); // Animação de ataque ou tempo de espera
    }

}
