using UnityEngine;

public class SimpleHitSound : MonoBehaviour
{
    // Arraste o arquivo de som MP3 para este campo no Inspector
    public AudioClip hitSound;
    
    // Volume do som
    [Range(0f, 1f)]
    public float volume = 1.0f;
    
    // Componente AudioSource
    private AudioSource audioSource;
    
    void Start()
    {
        // Obtém ou adiciona um componente AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        // Configuração básica
        audioSource.playOnAwake = false;
        
        // Configura o som de colisão
        if (hitSound != null)
        {
            audioSource.clip = hitSound;
            audioSource.volume = volume;
        }
    }
    
    // Detecta colisões com a bola
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            // Reproduz o som de colisão
            if (hitSound != null && audioSource != null)
            {
                audioSource.Play();
            }
        }
    }
}