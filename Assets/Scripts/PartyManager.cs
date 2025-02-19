using System;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public static PartyManager Instance { get; private set; }
    public List<Character> partyMembers = new List<Character>(); // Lista de personagens disponíveis
    public List<GameObject> partyObjects = new List<GameObject>(); // GameObjects dos membros na cena
    [SerializeField] private GameObject partyMemberPrefab; // Prefab do membro da party
    [SerializeField] private Transform player; // Referência ao player

    private int activeCharacterIndex = 0;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Inicializa os personagens iniciais
        partyMembers.Add(new Character("Hero", 100, 30, 15, 10)); // O primeiro personagem

        partyMembers.Add(new Character("Mage", 120, 15, 10, 15));

        SpawnPartyMembers();
    }

    // Retorna a lista de personagens disponíveis
    public List<Character> GetPartyMembers()
    {
        return partyMembers;
    }

    // Retorna um personagem específico da lista, se existir
    public Character GetCharacter(int index)
    {
        if (index >= 0 && index < partyMembers.Count)
        {
            return partyMembers[index];
        }
        return null;
    }

    // E quando novos personagens são desbloqueados no jogo, eu posso adicioná-los à Party: PartyManager.Instance.partyMembers.Add(new Character("Novo Aliado", 120, 40));
    public Character GetActiveCharacter()
    {
        return partyMembers[activeCharacterIndex]; // O primeiro da lista será o controlado na exploração
    }

    // Troca o personagem ativo
    public void SetActiveCharacter(int index)
    {
        if (index >= 0 && index < partyMembers.Count)
        {
            activeCharacterIndex = index;
            Debug.Log($"Personagem ativo agora é: {partyMembers[index].Name}");
        }
    }

    private void SpawnPartyMembers()
    {
        foreach (Character member in partyMembers)
        {
            GameObject newMember = Instantiate(partyMemberPrefab, player.position, Quaternion.identity);
            partyObjects.Add(newMember);
        }
    }

    public List<GameObject> GetPartyObjects()
    {
        return partyObjects;
    }

}
