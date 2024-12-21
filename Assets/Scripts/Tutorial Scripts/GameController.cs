using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameState { FreeRoam, Dialog, Battle };
public class GameController : MonoBehaviour
{
    [SerializeField] List<GameObject> dialogBoxPrefabs; // Lista de Prefabs de DialogBoxes
    public static GameController Instance { get; private set; }
    [SerializeField] PlayerController playerController;
    private GameObject currentDialogBox;


    GameState state;

    private void Awake()
    {
        // Configura o singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // O objeto persiste entre cenas
    }
    private void Start()
    {
        DialogManager.Instance.OnShowDialog += () =>
        {
            state = GameState.Dialog;
        };

        DialogManager.Instance.OnHideDialog += () =>
        {
            if (state == GameState.Dialog)
                state = GameState.FreeRoam;
        };
    }

    private void Update()
    {
        if (state == GameState.FreeRoam)
        {
            playerController.HandleUpdate();
        }
        else if (state == GameState.Dialog)
        {
            DialogManager.Instance.HandleUpdate();
        }
        else if (state == GameState.Battle)
        {
            BattleSystem.Instance.HandleUpdate();
        }
    }
    public void StartBattle()
    {
        state = GameState.Battle;
        BattleSystem.Instance.StartBattle();
    }

    public void EndBattle()
    {
        state = GameState.FreeRoam;
    }

    public void LoadDialogBox(int dialogIndex)
    {
        if (currentDialogBox != null)
        {
            Destroy(currentDialogBox); // Remove o DialogBox anterior
        }

        // Instancia o DialogBox com base no índice
        currentDialogBox = DialogManager.Instance.CreateDialogBox(dialogBoxPrefabs[dialogIndex]);
        
        // Passa a referência para o DialogManager
        DialogManager.Instance.SetDialogBox(currentDialogBox);
    }

    public void ChangeState(GameState newState)
    {
        state = newState;
    }

    public void SetPlayerCanMove(bool canMove)
    {
        playerController.SetCanMove(canMove);
    }

    public void StartCutscene()
    {
        SetPlayerCanMove(false); // Desativa a movimentação do jogador
        ChangeState(GameState.Dialog); // Altera o estado para cutscene/dialog
    }

    public void EndCutscene()
    {
        SetPlayerCanMove(true); // Reativa a movimentação do jogador
        ChangeState(GameState.FreeRoam); // Altera o estado para exploração livre
    }
}
