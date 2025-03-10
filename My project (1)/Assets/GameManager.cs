using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] private int startingLives = 3;
    
    [Header("Scene Names")]
    [SerializeField] private string introSceneName = "Intro";
    [SerializeField] private string level1SceneName = "Level_1";
    [SerializeField] private string level2SceneName = "Level_2";
    [SerializeField] private string gameOverSceneName = "GameOver";
    [SerializeField] private string victorySceneName = "Victory";
    
    // UI Components criados dinamicamente
    private Canvas uiCanvas;
    private Text scoreTextComponent;
    private Text livesTextComponent;
    [SerializeField] private GameObject pauseMenu;
    
    // Estado do jogo
    private int currentLives;
    private int score = 0;
    private bool isPaused = false;
    
    // Referências
    private BallControl ball;
    private LevelManager currentLevelManager;
    
    // Singleton
    public static GameManager Instance { get; private set; }
    
    private void Awake()
    {
        // Padrão singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Cria a UI persistente
            CreatePersistentUI();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    
    private void Start()
    {
        InitializeGame();
    }
    
    private void Update()
    {
        // Controle de pausa
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
    
    private void InitializeGame()
    {
        currentLives = startingLives;
        score = 0;
        
        // Ao carregar uma nova cena (ou no início), busca referências
        SceneManager.sceneLoaded += OnSceneLoaded;
        
        UpdateUI();
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Busca a referência da bola
        ball = Object.FindFirstObjectByType<BallControl>();
        
        // Verifica se é uma cena de jogo
        if (scene.name == level1SceneName || scene.name == level2SceneName)
        {
            // Prepara o nível para jogar
            Time.timeScale = 1f;
            isPaused = false;
        }
        
        // Atualiza UI
        UpdateUI();
    }
    
    private void CreatePersistentUI()
    {
        // Cria um canvas para UI que persistirá entre cenas
        GameObject canvasObject = new GameObject("PersistentUICanvas");
        uiCanvas = canvasObject.AddComponent<Canvas>();
        canvasObject.AddComponent<CanvasScaler>();
        canvasObject.AddComponent<GraphicRaycaster>();
        
        // Configura o canvas para ser modo ScreenSpace - Overlay
        uiCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        uiCanvas.sortingOrder = 100; // Garante que fique na frente de outros canvas
        
        // Não destruir o canvas entre cenas
        DontDestroyOnLoad(canvasObject);
        
        // Cria texto de pontuação no canto superior esquerdo
        GameObject scoreTextObject = new GameObject("ScoreText");
        scoreTextObject.transform.SetParent(uiCanvas.transform, false);
        scoreTextComponent = scoreTextObject.AddComponent<Text>();
        
        // Obter uma fonte válida
        Font defaultFont = null;
        
        // Tenta obter uma fonte do sistema
        try
        {
            defaultFont = Font.CreateDynamicFontFromOSFont("Arial", 24);
        }
        catch (System.Exception)
        {
            Debug.LogWarning("Não foi possível criar uma fonte dinâmica do sistema");
        }
        
        // Se não conseguiu a fonte do sistema, tenta encontrar qualquer fonte disponível
        if (defaultFont == null)
        {
            Font[] allFonts = Resources.FindObjectsOfTypeAll<Font>();
            if (allFonts != null && allFonts.Length > 0)
            {
                defaultFont = allFonts[0];
                Debug.Log("Usando fonte encontrada: " + defaultFont.name);
            }
            else
            {
                Debug.LogError("Nenhuma fonte encontrada no projeto!");
            }
        }
        
        // Configura o texto de pontuação
        scoreTextComponent.fontSize = 24;
        scoreTextComponent.color = Color.white;
        scoreTextComponent.text = "Score: 0";
        
        if (defaultFont != null)
            scoreTextComponent.font = defaultFont;
        
        // Posiciona o texto de pontuação
        RectTransform scoreRect = scoreTextComponent.rectTransform;
        scoreRect.anchorMin = new Vector2(0, 1);
        scoreRect.anchorMax = new Vector2(0, 1);
        scoreRect.pivot = new Vector2(0, 1);
        scoreRect.anchoredPosition = new Vector2(20, -20);
        scoreRect.sizeDelta = new Vector2(200, 30);
        
        // Cria texto de vidas no canto superior direito
        GameObject livesTextObject = new GameObject("LivesText");
        livesTextObject.transform.SetParent(uiCanvas.transform, false);
        livesTextComponent = livesTextObject.AddComponent<Text>();
        
        // Configura o texto de vidas
        livesTextComponent.fontSize = 24;
        livesTextComponent.color = Color.white;
        livesTextComponent.alignment = TextAnchor.UpperRight;
        livesTextComponent.text = "Vidas: " + startingLives;
        
        if (defaultFont != null)
            livesTextComponent.font = defaultFont;
        
        // Posiciona o texto de vidas
        RectTransform livesRect = livesTextComponent.rectTransform;
        livesRect.anchorMin = new Vector2(1, 1);
        livesRect.anchorMax = new Vector2(1, 1);
        livesRect.pivot = new Vector2(1, 1);
        livesRect.anchoredPosition = new Vector2(-20, -20);
        livesRect.sizeDelta = new Vector2(200, 30);
        
        // Adiciona tags para ser compatível com scripts antigos
        scoreTextObject.tag = "ScoreText";
        livesTextObject.tag = "LivesText";
    }
    
    // Registra o LevelManager do nível atual
    public void RegisterLevelManager(LevelManager levelManager)
    {
        currentLevelManager = levelManager;
        Debug.Log("LevelManager registrado no GameManager.");
    }
    
    public void LoseLife()
    {
        currentLives--;
        UpdateUI();
        
        if (currentLives <= 0)
        {
            // Game over imediatamente, sem delay
            SceneManager.LoadScene(gameOverSceneName);
        }
    }
    
    // Adiciona pontos ao score
    public void AddScore(int points)
    {
        score += points;
        UpdateUI();
    }
    
    private void UpdateUI()
    {
        // Atualiza textos usando as referências diretas
        if (scoreTextComponent != null)
            scoreTextComponent.text = "Score: " + score;
            
        if (livesTextComponent != null)
            livesTextComponent.text = "Vidas: " + currentLives;
    }
    
    // Métodos para navegação entre cenas
    public void LoadIntroScene() => SceneManager.LoadScene(introSceneName);
    public void LoadLevel1() => SceneManager.LoadScene(level1SceneName);
    public void LoadLevel2() => SceneManager.LoadScene(level2SceneName);
    public void LoadGameOverScene() => SceneManager.LoadScene(gameOverSceneName);
    public void LoadVictoryScene() => SceneManager.LoadScene(victorySceneName);
    
    // Controle de pausa
    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
        
        // Ativa/desativa menu de pausa
        if (pauseMenu != null)
            pauseMenu.SetActive(isPaused);
    }
    
    // Reinicia o jogo
    public void RestartGame()
    {
        // Reinicia o jogo para o Level 1 com as configurações iniciais
        currentLives = startingLives;
        score = 0;
        LoadLevel1();
    }
    
    // Retorna se o jogo está pausado
    public bool IsGamePaused() => isPaused;
    
    // Retorna a pontuação atual
    public int GetScore() => score;
    
    // Retorna o número de vidas restantes
    public int GetLives() => currentLives;
    
    // Resetar a pontuação
    public void ResetScore()
    {
        score = 0;
        UpdateUI();
        Debug.Log("Score resetado para 0");
    }
    
    // Sair do jogo
    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
    
    // Desinscreve do evento ao destruir o objeto
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}