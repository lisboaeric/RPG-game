using System.Collections.Generic;
using UnityEngine;

public class PartyFollower : MonoBehaviour
{
    private List<GameObject> followers;
    private Transform player;
    private Queue<Vector3> movementQueue = new Queue<Vector3>();

    public float tileSize = 0f; // Define o tamanho de um tile
    private Vector3 lastPlayerPosition;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        followers = PartyManager.Instance.GetPartyObjects();
        lastPlayerPosition = player.position; // Armazena a posição inicial
    }

    private void Update()
    {
        if (player == null) return;

        // Armazena a posição do player somente se ele tiver se movido pelo menos um tile
        if (Vector3.Distance(player.position, lastPlayerPosition) >= tileSize)
        {
            movementQueue.Enqueue(player.position);
            lastPlayerPosition = player.position;
        }

        // Mantém um histórico suficiente para o último seguidor
        while (movementQueue.Count > followers.Count + 1)
        {
            movementQueue.Dequeue();
        }

        // Move cada seguidor para a posição correta
        for (int i = 0; i < followers.Count; i++)
        {
            if (movementQueue.Count > i + 1)
            {
                Vector3 targetPos = GetPositionAtDelay(i + 1);
                
                // Somente move se estiver longe o suficiente para evitar "encaixes errados"
                if (Vector3.Distance(followers[i].transform.position, targetPos) > 0)
                {
                    followers[i].transform.position = Vector3.MoveTowards(followers[i].transform.position, targetPos, 8f * Time.deltaTime);
                }
            }
        }
    }

    private Vector3 GetPositionAtDelay(int delay)
    {
        Vector3[] positions = movementQueue.ToArray();
        int index = Mathf.Clamp(positions.Length - 1 - delay, 0, positions.Length - 1);
        return positions[index];
    }
    /*
    private List<GameObject> followers; // Referência aos GameObjects dos membros da party
    private Transform player;
    //private List<Vector3> previousPositions = new List<Vector3>();
    private Queue<Vector3> movementQueue = new Queue<Vector3>(); // Fila para armazenar as posições do player
    private int tileDelay = 1; // Quantidade de tiles de atraso

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        followers = PartyManager.Instance.GetPartyObjects();
    }

    private void Update()
    {
        if (player == null) return;

        // Armazena a posição atual do player na fila
        movementQueue.Enqueue(player.position);

        // Mantém um histórico suficiente para garantir que o último seguidor tenha um atraso adequado
        while (movementQueue.Count > (followers.Count + 1) * tileDelay)
        {
            movementQueue.Dequeue();
        }

        // Move cada seguidor para a posição atrasada correta
        for (int i = 0; i < followers.Count; i++)
        {
            if (movementQueue.Count > (i + 1) * tileDelay)
            {
                Vector3 targetPos = GetPositionAtDelay(i + 1);

                // Corrige a interpolação para garantir que o seguidor pare exatamente no tile correto
                if (Vector3.Distance(followers[i].transform.position, targetPos) > 0.1f)
                {
                    followers[i].transform.position = Vector3.MoveTowards(followers[i].transform.position, targetPos, 10f * Time.deltaTime);
                }
            }
        }
    }

    private Vector3 GetPositionAtDelay(int delay)
    {
        Vector3[] positions = movementQueue.ToArray();

        // Calcula o índice correto para acessar a posição antiga
        int index = positions.Length - 1 - (delay * tileDelay);

        // Evita que o índice fique fora dos limites
        if (index < 0) index = 0;

        return positions[index];
    }*/
}
