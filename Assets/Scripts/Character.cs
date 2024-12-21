using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    public string Name { get; private set; }
    public int MaxHP { get; private set; }
    public int CurrentHP { get; set; }
    public int AttackPower { get; private set; }

    public Character(string name, int maxHP, int attackPower)
    {
        Name = name;
        MaxHP = maxHP;
        CurrentHP = maxHP;
        AttackPower = attackPower;
    }

    public bool IsDefeated => CurrentHP <= 0;
}
