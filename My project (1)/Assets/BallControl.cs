using UnityEngine;

public class BallControl : MonoBehaviour
{
    [Header("Ball Settings")]
    [SerializeField] private float initialSpeed = 5f;
    [SerializeField] private float maxSpeed = 15f;
    [SerializeField] private float speedIncrement = 0.1f;
    
    [Header("References")]
    [SerializeField] private Transform paddle;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip loseLifeSound;
    
    private Rigidbody2D rb;
    private bool isInPlay = false;
    private Vector2 paddleToBallVector;
    
    // Referência ao GameManager
    private GameManager gameManager;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
            
        // Usando o método atualizado em vez do obsoleto
        gameManager = Object.FindFirstObjectByType<GameManager>();
        
        // Tenta encontrar o paddle automaticamente se não for definido no Inspector
        if (paddle == null)
        {
            GameObject paddleObj = GameObject.FindGameObjectWithTag("Paddle");
            if (paddleObj != null)
                paddle = paddleObj.transform;
        }
            
        // Calcula a distância inicial entre a bola e o paddle
        if (paddle != null)
            paddleToBallVector = transform.position - paddle.position;
    }
    
    void Update()
    {
        if (!isInPlay)
        {
            // Mantém a bola anexada ao paddle até o jogador iniciar
            if (paddle != null)
                transform.position = paddle.position + (Vector3)paddleToBallVector;
            
            // Lança a bola quando o jogador pressiona o espaço
            if (Input.GetButtonDown("Jump") || Input.GetMouseButtonDown(0))
            {
                LaunchBall();
            }
        }
        
        // Limita a velocidade máxima da bola
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
        
        // Corrige raros casos em que a bola poderia se mover quase horizontalmente
        FixHorizontalMovement();
    }
    
    void LaunchBall()
    {
        isInPlay = true;
        // Lança a bola em uma direção aleatória para cima
        rb.velocity = new Vector2(Random.Range(-1f, 1f), 1f).normalized * initialSpeed;
    }
    
    void FixHorizontalMovement()
    {
        // Se a bola estiver se movendo quase horizontalmente, adiciona um pouco de movimento vertical
        if (Mathf.Abs(rb.velocity.y) < 0.2f)
        {
            Vector2 velocity = rb.velocity;
            velocity.y = velocity.y < 0 ? -0.5f : 0.5f;
            rb.velocity = velocity;
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isInPlay)
        {
            // Incrementa ligeiramente a velocidade a cada colisão
            rb.velocity += rb.velocity.normalized * speedIncrement;
            
            // Toca som de colisão
            if (hitSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(hitSound);
            }
            
            // Verifica se colidiu com bloco
            if (collision.gameObject.CompareTag("Brick"))
            {
                // A destruição do bloco é tratada pelo script do bloco,
                // mas podemos adicionar comportamentos específicos da bola aqui se necessário
            }
            
            // Adiciona um pouco de aleatoriedade na colisão com o paddle
            if (collision.gameObject.CompareTag("Paddle"))
            {
                // Calcula a posição relativa ao centro do paddle (entre -1 e 1)
                float hitPosition = (transform.position.x - collision.transform.position.x) / 
                                   (collision.collider.bounds.size.x / 2);
                
                // Ajusta o ângulo da bola com base no ponto de impacto
                Vector2 newDirection = new Vector2(hitPosition, 1).normalized;
                rb.velocity = newDirection * rb.velocity.magnitude;
            }
        }
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se a bola caiu (colidiu com o trigger na parte inferior)
        if (collision.CompareTag("DeathZone"))
        {
            // Toca som de perder vida
            if (loseLifeSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(loseLifeSound);
            }
            
            // Informa ao GameManager que perdeu uma vida
            if (gameManager != null)
                gameManager.LoseLife();
            
            // Reseta a bola
            ResetBall();
        }
    }
    
    // Método público para resetar a bola (chamado pelo GameManager)
    public void ResetBall()
    {
        isInPlay = false;
        rb.velocity = Vector2.zero;
        
        if (paddle != null)
            transform.position = paddle.position + (Vector3)paddleToBallVector;
    }
    
    // Aplicar efeitos de power-up na bola
    public void ApplySpeedMultiplier(float multiplier)
    {
        if (rb.velocity != Vector2.zero)
        {
            rb.velocity *= multiplier;
        }
    }
    
    // Método para verificar se a bola está em jogo
    public bool IsBallInPlay()
    {
        return isInPlay;
    }
}