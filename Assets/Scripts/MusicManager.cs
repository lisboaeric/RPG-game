using UnityEngine;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; } // Instância Singleton
    public AudioSource audioSource; // Referência ao AudioSource
    public List<AudioClip> musicClips; // Lista de músicas

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Torna o MusicManager persistente
        }
        else
        {
            Destroy(gameObject); // Se já existir uma instância, destrua essa nova
            return;
        }

        if (audioSource == null) // Se não tiver um AudioSource no componente
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Se a lista tiver pelo menos uma música, toca a primeira
        if (musicClips.Count > 0 && audioSource.clip == null)
        {
            PlayMusic(0);
        }
    }

    // Função para tocar uma música específica pelo índice na lista
    public void PlayMusic(int index)
    {
        if (index < 0 || index >= musicClips.Count)
        {
            Debug.LogWarning("Índice de música inválido!");
            return;
        }

        audioSource.clip = musicClips[index];
        audioSource.Play();
    }

    // Função para parar a música
    public void StopMusic()
    {
        audioSource.Stop();
    }

    // Função para pausar a música
    public void PauseMusic()
    {
        audioSource.Pause();
    }

    // Função para retomar a música
    public void ResumeMusic()
    {
        audioSource.UnPause();
    }

    // Função para mudar o volume da música
    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }
}
