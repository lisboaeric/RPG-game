using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    [SerializeField] GameObject battleUI; // Referência à interface de batalha
    [SerializeField] private Character enemyCharacter;

    private Character activeCharacter; // Personagem atualmente controlado pelo jogador
 
    public static BattleSystem Instance { get; private set; }

    // Verificações de pressionamento de botões para a corrotina PlayerTurn
    private bool attackCommand = false;
    private bool defendCommand = false;

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
        // Obtém o personagem principal da party
        activeCharacter = PartyManager.Instance.GetActiveCharacter();
        enemyCharacter = new Character("Enemy", 80, 15, 0, 10);
    }
    public void StartBattle()
    {
        if(enemyCharacter.CurrentHP > 0) 
        {
            attackCommand = false;
            defendCommand = false;

            enemyCharacter.ATBCharge = 0;

            Debug.Log("Começou");

            battleUI.SetActive(true);

            // Negocio que atualizar o canvas (não sei se funciona)
            Canvas.ForceUpdateCanvases();

            // Chamando o gerenciador de UI
            BattleUIManager.Instance.SetPlayerUI(activeCharacter.Name, activeCharacter.CurrentHP, activeCharacter.MaxHP);
            BattleUIManager.Instance.SetEnemyUI(enemyCharacter.Name, enemyCharacter.CurrentHP, enemyCharacter.MaxHP);
            BattleUIManager.Instance.UpdateMessage("The battle begins!");
            
            StartCoroutine(BattleLoop());
        }
        else EndBattle();
    }

    public void EndBattle()
    {
        battleUI.SetActive(false);
        GameController.Instance.EndBattle();
    }

    public void Update()
    {
        // Captura os inputs do jogador e armazena
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            attackCommand = true;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            defendCommand = true;
        }

        UpdateATB();
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
            StartCoroutine(PlayerTurn());
            StartCoroutine(EnemyTurn());

            while(!IsBattleOver())
            {
                yield return null;
            }

            break;
        }

        EndBattle();
    }

    private IEnumerator PlayerTurn()
    {
        while (true) // Mantém o loop do Player ativo
        {
            if(IsBattleOver()) yield break; // Para se a batalha acabar

            bool actionTaken = false;
            
            while (!actionTaken)
            {
                // Permite ao jogador escolher o personagem ativo antes do turno
                SelectCharacter();
                
                // Garante que `activeCharacter` não seja nulo
                if (activeCharacter == null)
                {
                    activeCharacter = PartyManager.Instance.GetActiveCharacter();
                }
                BattleUIManager.Instance.SetPlayerUI(activeCharacter.Name, activeCharacter.CurrentHP, activeCharacter.MaxHP);

                if (activeCharacter.ATBCharge >= 100) // Verifica se o jogador está pronto para agir
                {
                    Debug.Log($"{activeCharacter.Name} está pronto para agir!");

                    if (attackCommand) // Atacar
                    {
                        attackCommand = false; // Reseta a variável
                        yield return ExecuteAttack(activeCharacter);
                        actionTaken = true;
                        activeCharacter.ATBCharge = 0; // Reset da barra ATB
                    }
                    else if (defendCommand) // Defender
                    {
                        defendCommand = false; // Reseta a variável
                        Debug.Log($"{activeCharacter.Name} está se defendendo!");
                        actionTaken = true;
                        activeCharacter.ATBCharge = 0; // Reset da barra ATB
                    }


                }
                yield return null;
            }

            yield return null;
        }
    }

    private IEnumerator EnemyTurn()
    {
        while (true) // Mantém o loop do Inimigo ativo
        {
            if (IsBattleOver()) yield break;

            Debug.Log("Enemy's Turn");
            if (enemyCharacter.ATBCharge >= 100) // O inimigo só age quando a barra ATB estiver cheia
            {
                // Escolha de ação (ex.: ataque aleatório)
                Debug.Log("Enemy attacked the player!");
                
                yield return ExecuteEnemyAttack();

                enemyCharacter.ATBCharge = 0;
            }
            yield return null;
        }
    }

    private bool IsBattleOver()
    {
        if (activeCharacter.IsDefeated)
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

    private IEnumerator ExecuteAttack(Character attacker)
    {
        Debug.Log($"{attacker.Name} attacks!");

        // Reduz a vida do alvo
        enemyCharacter.CurrentHP -= attacker.AttackPower;

        if (enemyCharacter.CurrentHP <= 0)
        {
            enemyCharacter.CurrentHP = 0;
        }

        Debug.Log($"Enemy HP: {enemyCharacter.CurrentHP}/{enemyCharacter.MaxHP}");

        // Atualiza a interface do inimigo
        BattleUIManager.Instance.SetEnemyUI(enemyCharacter.Name, enemyCharacter.CurrentHP, enemyCharacter.MaxHP);
        BattleUIManager.Instance.UpdateMessage($"{attacker.Name} attacked the enemy!");

        // Verificar se o inimigo foi derrotado
        if (enemyCharacter.IsDefeated)
        {
            Debug.Log("Enemy is defeated!");
            yield break; // Sai do ataque porque o inimigo foi derrotado
        }

        // yield return new WaitForSeconds(1); // Simula animação de ataque
        yield return null;
    }

    private IEnumerator ExecuteEnemyAttack()
    {
        // Seleciona um alvo aleatório da party
        List<Character> partyMembers = PartyManager.Instance.GetPartyMembers();
        Character target = partyMembers[Random.Range(0, partyMembers.Count)];

        Debug.Log($"Enemy attacks {target.Name}!");

        target.CurrentHP -= enemyCharacter.AttackPower;

        if (target.CurrentHP <= 0)
        {
            target.CurrentHP = 0;
        }

        Debug.Log($"{target.Name} HP: {target.CurrentHP}/{target.MaxHP}");

        // Atualiza a UI do personagem atacado
        BattleUIManager.Instance.SetPlayerUI(target.Name, target.CurrentHP, target.MaxHP);
        BattleUIManager.Instance.UpdateMessage($"Enemy attacked {target.Name}!");

        // Verificar se o alvo foi derrotado
        if (target.IsDefeated)
        {
            Debug.Log($"{target.Name} is defeated!");
            yield break; // Para o ataque caso o alvo tenha sido derrotado
        }

        yield return new WaitForSeconds(1); // Tempo de espera para a animação

        // Reseta a barra ATB do inimigo após atacar
        enemyCharacter.ATBCharge = 0;
    }

    private void UpdateATB()
    {
        foreach (Character character in PartyManager.Instance.GetPartyMembers())
        {
            character.ATBCharge += character.Speed * Time.deltaTime;

            if (character.ATBCharge >= 100)
            {
                character.ATBCharge = 100; // Garante que não passe de 100%
            }
        }

        enemyCharacter.ATBCharge += enemyCharacter.Speed * Time.deltaTime;

        if (enemyCharacter.ATBCharge >= 100)
        {
            enemyCharacter.ATBCharge = 100;
        }

        BattleUIManager.Instance.UpdateATBUI(activeCharacter, activeCharacter.ATBCharge, enemyCharacter.ATBCharge);
    }

    private void SelectCharacter()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) 
        {
            activeCharacter = PartyManager.Instance.GetCharacter(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) 
        {
            activeCharacter = PartyManager.Instance.GetCharacter(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) 
        {
            activeCharacter = PartyManager.Instance.GetCharacter(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            activeCharacter = PartyManager.Instance.GetCharacter(3);
        }

        Debug.Log($"Personagem {activeCharacter.Name} selecionado");
    }

}
