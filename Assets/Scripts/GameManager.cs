using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
   
    public static GameManager instancia;

    [Header("Sistema de Puntuación")]
    public int score = 0;
    public TextMeshProUGUI textoScore; 
    void Awake()
    {
       
        if (instancia == null)
        {
            instancia = this;
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    void Start()
    {
        ActualizarUIScore(); 
    }

    
    public void ModificarScore(int cantidad)
    {
        score += cantidad;
        ActualizarUIScore();
    }

    void ActualizarUIScore()
    {
        if (textoScore != null)
        {
            textoScore.text = "Score: " + score;
        }
    }
}