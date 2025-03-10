using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControler : MonoBehaviour
{
    // Start is called before the first frame update

    public float limInf = 3.5f;  
    public float limSup = 0.1666525f;  
    public float boundX = 2f; 
    private Rigidbody2D rb2d;   

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>(); 
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var pos = transform.position;
        pos.x = mousePos.x;
        pos.y = mousePos.y;
        transform.position = pos;

        if (pos.y > limSup) {                  
            pos.y = limSup;                     // Corrige a posicao da raquete caso ele ultrapasse o limite superior
        }
        else if (pos.y < -limInf) {
            pos.y = -limInf;                    // Corrige a posicao da raquete caso ele ultrapasse o limite superior
        }
        transform.position = pos;               // Atualiza a posição da raquete

        if (pos.x > boundX) {                  
            pos.x = boundX;                     // Corrige a posicao da raquete caso ele ultrapasse o limite superior
        }
        else if (pos.x < -boundX) {
            pos.x = -boundX;                    // Corrige a posicao da raquete caso ele ultrapasse o limite superior
        }
        transform.position = pos;               // Atualiza a posição da raquete



    }
}
