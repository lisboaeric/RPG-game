using UnityEngine;

public class PartyMember : MonoBehaviour
{
    private void Start()
    {
        Debug.Log($"{gameObject.name} foi spawnado na posição {transform.position}");
    }
}