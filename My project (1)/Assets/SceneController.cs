using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] private string introSceneName = "Intro";
    [SerializeField] private string level1SceneName = "Level_1";
    [SerializeField] private string level2SceneName = "Level_2";
    [SerializeField] private string gameOverSceneName = "GameOver";
    [SerializeField] private string victorySceneName = "Victory";
    
    [Header("UI References")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button quitButton;
    
    private GameManager gameManager;
    
    private void Start()
    {
        // Busca o GameManager
        gameManager = Object.FindFirstObjectByType<GameManager>();
        
        // Configura os botões baseado na cena atual
        SetupButtons();
    }
    
    private void SetupButtons()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        
        // Configura botões de acordo com a cena
        if (currentScene == introSceneName)
        {
            if (startButton != null)
                startButton.onClick.AddListener(StartGame);
                
            if (quitButton != null)
                quitButton.onClick.AddListener(QuitGame);
        }
        else if (currentScene == gameOverSceneName)
        {
            if (restartButton != null)
                restartButton.onClick.AddListener(RestartGame);
                
            if (mainMenuButton != null)
                mainMenuButton.onClick.AddListener(LoadMainMenu);
        }
        else if (currentScene == victorySceneName)
        {
            if (restartButton != null)
                restartButton.onClick.AddListener(RestartGame);
                
            if (mainMenuButton != null)
                mainMenuButton.onClick.AddListener(LoadMainMenu);
        }
    }
    
    // Métodos para transição entre cenas
    
    public void StartGame()
    {
        SceneManager.LoadScene(level1SceneName);
    }
    
    public void RestartGame()
    {
        SceneManager.LoadScene(level1SceneName);
        
        // Se quiser reiniciar o score através do GameManager
        if (gameManager != null)
        {
            gameManager.ResetScore();
        }
    }
    


    
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(introSceneName);
    }
    
    public void LoadNextLevel()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        
        if (currentScene == level1SceneName)
        {
            SceneManager.LoadScene(level2SceneName);
        }
        else if (currentScene == level2SceneName)
        {
            SceneManager.LoadScene(victorySceneName);
        }
    }
    
    public void LoadGameOver()
    {
        SceneManager.LoadScene(gameOverSceneName);
    }
    
    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
    
    // Método para exibir a pontuação final nas cenas de Game Over e Victory
    public void DisplayFinalScore(Text scoreText)
    {
        if (scoreText != null && gameManager != null)
        {
            scoreText.text = "Pontuação Final: " + gameManager.GetScore().ToString();
        }
    }
}