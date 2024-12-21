using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class DialogManager : MonoBehaviour
{
    [SerializeField] GameObject dialogBox;
    [SerializeField] Text dialogText;
    [SerializeField] int lettersPerSecond;

    public event System.Action OnShowDialog;
    public event System.Action OnHideDialog;
    public static DialogManager Instance { get; private set; }
    private GameObject dialogCanvas;

    Dialog dialog;
    int currentLine = 0;
    bool isTyping;

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            // Cria um Canvas persistente
            dialogCanvas = new GameObject("DialogCanvas");
            var canvas = dialogCanvas.AddComponent<UnityEngine.Canvas>();
            canvas.renderMode = UnityEngine.RenderMode.ScreenSpaceOverlay;

            dialogCanvas.AddComponent<GraphicRaycaster>();

            SetCanvasScale();

            DontDestroyOnLoad(dialogCanvas); // Torna o Canvas persistente
            DontDestroyOnLoad(gameObject); // Torna o DialogManager persistente
        }
        else
        {
            Destroy(gameObject);
        }

    }
    
    public GameObject CreateDialogBox(GameObject dialogBoxPrefab)
    {
         // Instancia o DialogBox como filho do Canvas persistente
        GameObject dialogBox = Instantiate(dialogBoxPrefab, dialogCanvas.transform);

        // Pega o RectTransform do DialogBox
        RectTransform rectTransform = dialogBox.GetComponent<RectTransform>();

        // Muda a posição do DialogBox (X e Y)
        rectTransform.anchoredPosition = new Vector2(-20, 75);

        return dialogBox;
    }
    public IEnumerator ShowDialog(Dialog dialog)
    {
        yield return new WaitForEndOfFrame();
        OnShowDialog?.Invoke();

        this.dialog = dialog;

        dialogBox.SetActive(true);
        StartCoroutine(TypeDialog(dialog.Lines[0]));
    }
    public void HandleUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !isTyping)
        {
            ++currentLine;
            if (currentLine < dialog.Lines.Count)
            {
                StartCoroutine(TypeDialog(dialog.Lines[currentLine]));
            }
            else
            {
                dialogBox.SetActive(false);
                currentLine = 0;
                OnHideDialog?.Invoke();
            }
        }
    }
    public IEnumerator TypeDialog(string line)
    {
        isTyping = true;
        dialogText.text = "";
        foreach (var letter in line.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
        isTyping = false;
    }

    public void SetDialogBox(GameObject newDialogBox)
{
    dialogBox = newDialogBox;
    dialogText = newDialogBox.GetComponentInChildren<Text>(); // Encontra o texto do DialogBox
}

    public void SetCanvasScale()
    {
        // Configura o CanvasScaler para Scale With Screen Size
        var canvasScaler = dialogCanvas.AddComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(800, 600); // Resolução de referência
        canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        canvasScaler.matchWidthOrHeight = 0.5f; // Ajusta para uma escala equilibrada
        

        dialogCanvas.AddComponent<GraphicRaycaster>();
    }
}
