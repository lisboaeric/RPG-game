using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFade : MonoBehaviour
{
    public static ScreenFade Instance; // Singleton para facilitar acesso
    public CanvasGroup fadeCanvasGroup; // Referência ao CanvasGroup do Fade
    public float fadeDuration = 1f; // Tempo de fade

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Mantém entre cenas
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // Faz o fade para preto
    public IEnumerator FadeOut()
    {
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }
        fadeCanvasGroup.alpha = 1;
    }

    // Faz o fade para transparente
    public IEnumerator FadeIn()
    {
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(1, 0, t / fadeDuration);
            yield return null;
        }
        fadeCanvasGroup.alpha = 0;
    }
}
