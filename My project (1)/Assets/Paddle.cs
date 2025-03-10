using UnityEngine;

public class Paddle : MonoBehaviour
{
    [Header("Paddle Settings")]
    [SerializeField] private float moveSpeed = 10f;
    
    [Header("Screen Boundaries")]
    // Ajuste estes valores para corresponder às suas paredes!
    [SerializeField] private float leftWallX = -3.8f; // Posição X da parede esquerda
    [SerializeField] private float rightWallX = 3.8f;  // Posição X da parede direita
    
    private float paddleHalfWidth;
    private GameManager gameManager;
    
    void Start()
    {
        // Obtém o GameManager
        gameManager = Object.FindFirstObjectByType<GameManager>();
        
        // Calcula a metade da largura do paddle
        paddleHalfWidth = GetComponent<SpriteRenderer>().bounds.size.x / 2;
        
        Debug.Log($"Paddle half width: {paddleHalfWidth}");
    }
    
    void Update()
    {
        // Não permite movimento se o jogo estiver pausado
        if (gameManager != null && gameManager.IsGamePaused())
            return;
        
        // Movimento horizontal com as setas ou teclas A/D
        float moveInput = Input.GetAxis("Horizontal");
        
        // Calcula a nova posição
        Vector3 pos = transform.position;
        pos.x += moveInput * moveSpeed * Time.deltaTime;
        
        // IMPORTANTE: Limita a posição considerando a largura do paddle
        // Isso garante que o paddle fique dentro das paredes
        float minPaddleX = leftWallX + paddleHalfWidth + 0.1f; // Adiciona uma pequena margem
        float maxPaddleX = rightWallX - paddleHalfWidth - 0.1f; // Adiciona uma pequena margem
        
        // Aplica os limites
        pos.x = Mathf.Clamp(pos.x, minPaddleX, maxPaddleX);
        
        // Atualiza a posição
        transform.position = pos;
    }
    
    // Método para depuração - você pode chamar isso para verificar se os limites estão corretos
    public void DebugLimits()
    {
        float minPaddleX = leftWallX + paddleHalfWidth + 0.1f;
        float maxPaddleX = rightWallX - paddleHalfWidth - 0.1f;
        Debug.Log($"Paddle movement limits: min={minPaddleX}, max={maxPaddleX}");
        Debug.Log($"Wall positions: left={leftWallX}, right={rightWallX}");
    }
}