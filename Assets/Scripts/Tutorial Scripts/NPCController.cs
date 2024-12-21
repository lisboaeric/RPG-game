using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour, Interactable
{
    [SerializeField] Dialog dialog;
    public void Interact()
    {

        // Carregar o primeiro DialogBox
        GameController.Instance.LoadDialogBox(0);
        StartCoroutine(DialogManager.Instance.ShowDialog(dialog));
        Debug.Log("dialog");
    }
}
