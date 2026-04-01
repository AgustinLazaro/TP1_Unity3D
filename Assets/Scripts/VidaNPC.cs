using UnityEngine;

public class VidaNPC : MonoBehaviour
{
    public float vidaMaxima = 100f;
    private float vidaActual;

    [Header("Configuración del Personaje")]
    public bool esCivil; 
    public int puntosRecompensa = 10; 
    public int puntosPenalizacion = -50; 

    void Start()
    {
        vidaActual = vidaMaxima;
    }

    public void RecibirDamage(float cantidadDanio)
    {
        vidaActual -= cantidadDanio;

        if (vidaActual <= 0f)
        {
            Morir();
        }
    }

    void Morir()
    {
        
        if (esCivil)
        {
            GameManager.instancia.ModificarScore(puntosPenalizacion);
            Debug.Log("¡Penalización! Mataste a un civil.");
        }
        else
        {
            GameManager.instancia.ModificarScore(puntosRecompensa);
            Debug.Log("¡Enemigo eliminado! Ganaste puntos.");
        }

        Destroy(gameObject);
    }
}
