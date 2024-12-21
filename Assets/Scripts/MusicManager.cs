using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; } // Instância Singleton
    public AudioSource audioSource; // Referência ao AudioSource
    public AudioClip musicClip; // O tema musical

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
        }

        if (audioSource == null) // Se não tiver um AudioSource no componente
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Se a música ainda não estiver tocando, tocá-la
        if (audioSource.clip != musicClip)
        {
            audioSource.clip = musicClip;
            audioSource.Play();
        }
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
