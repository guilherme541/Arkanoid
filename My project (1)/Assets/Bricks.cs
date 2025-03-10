using UnityEngine;

public class Bricks : MonoBehaviour
{
    [Header("Brick Settings")]
    [SerializeField] private int hitPoints = 1;        // Quantidade de golpes para destruir
    [SerializeField] private int scoreValue = 10;      // Pontos ganhos ao destruir
    
    [Header("References")]
    [SerializeField] private AudioClip breakSound;     // Som ao quebrar
    
    private SpriteRenderer spriteRenderer;
    private GameManager gameManager;
    private LevelManager levelManager;
    
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = Object.FindFirstObjectByType<GameManager>();
        levelManager = Object.FindFirstObjectByType<LevelManager>();
        
        // Verificação de segurança
        if (gameManager == null)
        {
            Debug.LogWarning("GameManager não encontrado! Pontuação não será registrada.");
        }
        
        if (levelManager == null)
        {
            Debug.LogWarning("LevelManager não encontrado! Progressão de nível pode não funcionar.");
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            HandleHit();
        }
    }
    
    private void HandleHit()
    {
        hitPoints--;
        
        if (hitPoints <= 0)
        {
            DestroyBrick();
        }
    }
    
    private void DestroyBrick()
    {
        // Adiciona pontuação usando o GameManager
        if (gameManager != null)
        {
            gameManager.AddScore(scoreValue);
        }
        
        // Notifica o LevelManager que um bloco foi destruído
        if (levelManager != null)
        {
            levelManager.OnBrickDestroyed();
        }
        
        // Reproduz som de quebra
        if (breakSound != null)
        {
            AudioSource.PlayClipAtPoint(breakSound, transform.position);
        }
        
        // Destrói o bloco
        Destroy(gameObject);
    }
}