using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Level Configuration")]
    [SerializeField] private int requiredBricksToDestroy = 8; // Número de blocos que DEVEM ser destruídos
    [SerializeField] private string nextLevelName = "Level_2"; // Nome da próxima cena
    
    private int bricksDestroyed = 0;
    private GameManager gameManager;
    
    void Start()
    {
        // Tenta encontrar o GameManager
        gameManager = Object.FindFirstObjectByType<GameManager>();
        
        if (gameManager == null)
        {
            Debug.LogError("GameManager não encontrado! O jogo pode não funcionar corretamente.");
        }
        
        // Registra este LevelManager no GameManager (opcional)
        if (gameManager != null)
        {
            gameManager.RegisterLevelManager(this);
        }
        
        Debug.Log($"LevelManager inicializado. Requer {requiredBricksToDestroy} blocos para avançar.");
    }
    
    // Método chamado quando um bloco é destruído
    public void OnBrickDestroyed()
    {
        bricksDestroyed++;
        Debug.Log($"Bloco destruído! {bricksDestroyed}/{requiredBricksToDestroy}");
        
        // Verifica se atingiu o número necessário
        if (bricksDestroyed >= requiredBricksToDestroy)
        {
            Debug.Log("Número necessário de blocos destruídos! Avançando para o próximo nível.");
            Invoke("LoadNextLevel", 1.0f); // Pequeno delay para efeito
        }
    }
    
    // Carrega o próximo nível
    private void LoadNextLevel()
    {
        SceneManager.LoadScene(nextLevelName);
    }
}