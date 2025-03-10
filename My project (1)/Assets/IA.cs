using UnityEngine;

public class BasicAIPaddle : MonoBehaviour
{
    private GameObject ball;
    private Vector3 startPosition;
    public float speed = 30f;
    public float maxMovement = 2.5f;
    
    void Start()
    {
   
        startPosition = transform.position;  
        ball = GameObject.FindGameObjectWithTag("Ball");
    }
    
    void Update()
    {
        // Se não encontrou a bola, tenta novamente
        if (ball == null)
        {
            ball = GameObject.FindGameObjectWithTag("Ball");
            return;
        }
        
        float targetX = Mathf.Clamp(ball.transform.position.x, -maxMovement, maxMovement);
        Vector3 newPosition = transform.position;
        newPosition.x = Mathf.MoveTowards(newPosition.x, targetX, speed * Time.deltaTime);

        transform.position = newPosition;
    }
    
    void RestartGame()
    {
        transform.position = startPosition;
    }
    
    void ResetBall()
    {
        transform.position = startPosition;
    }
}