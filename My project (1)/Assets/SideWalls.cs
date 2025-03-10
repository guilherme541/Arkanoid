using UnityEngine;

public class SideWalls : MonoBehaviour
{
    // Se houver referência a um método 'Score' que não existe no GameManager
    // Você precisa alterá-lo para usar o método AddScore
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica se é a bola que colidiu
        if (collision.gameObject.CompareTag("Ball"))
        {
            // Simplesmente rebate a bola nas paredes, não é necessário alterar score
            // Se você tinha algum comportamento específico aqui antes, ajuste conforme necessário
        }
    }
}