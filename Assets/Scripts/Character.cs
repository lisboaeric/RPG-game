using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    public string Name { get; private set; }
    public int MaxHP { get; private set; }
    public int AttackPower { get; private set; }
    public int Defense { get; private set; }
    public int Speed { get; private set; }
    public float ATBCharge { get; set; } // Progresso da barra ATB

    public int CurrentHP { get; set; }

    public bool IsDefeated => CurrentHP <= 0;

    public Character(string name, int maxHP, int attackPower, int defense, int speed)
    {
        Name = name;
        MaxHP = maxHP;
        CurrentHP = maxHP;
        AttackPower = attackPower;
        Defense = defense;
        Speed = speed;
        ATBCharge = 100f;
    }

}
