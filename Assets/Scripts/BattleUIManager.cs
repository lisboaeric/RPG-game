﻿using UnityEngine;
using UnityEngine.UI;

public class BattleUIManager : MonoBehaviour
{
    [SerializeField] private Text playerNameText;
    [SerializeField] private Text playerHPText;
    [SerializeField] private Slider playerHPSlider;

    [SerializeField] private Text enemyNameText;
    [SerializeField] private Text enemyHPText;
    [SerializeField] private Slider enemyHPSlider;

    [SerializeField] private Text messageText;

    [SerializeField] private Slider atbSliderPlayer;
    [SerializeField] private Slider atbSliderEnemy;

    public static BattleUIManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void SetPlayerUI(string name, int currentHP, int maxHP)
    {
        playerNameText.text = name;
        playerHPText.text = $"{currentHP}/{maxHP}";
        playerHPSlider.maxValue = maxHP;
        playerHPSlider.value = currentHP;
    }

    public void SetEnemyUI(string name, int currentHP, int maxHP)
    {
        enemyNameText.text = name;
        enemyHPText.text = $"{currentHP}/{maxHP}";
        enemyHPSlider.maxValue = maxHP;
        enemyHPSlider.value = currentHP;
    }

    public void UpdateMessage(string message)
    {
        messageText.text = message;
    }

    public void UpdateATBUI(Character character, float atbValuePlayers, float atbValueEnemy)
    {
        atbSliderPlayer.value = atbValuePlayers / 100f; // Normaliza para 0-1
        atbSliderEnemy.value = atbValueEnemy / 100f;
    }
}
