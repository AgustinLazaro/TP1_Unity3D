using UnityEngine;


[RequireComponent(typeof(Animator))]
public class CerebroEnemigo : MonoBehaviour
{
    public enum EstadoNPC { Patrullando, Persiguiendo, Atacando }

    [Header("Estado Actual")]
    public EstadoNPC estado = EstadoNPC.Patrullando;

    [Header("Referencias")]
    public Transform objetivo; 
    private DroneController droneScript;
    private Animator anim; 

    [Header("Configuración de IA")]
    public float velocidadCaminar = 2f;
    public float velocidadCorrer = 5f;
    public float distanciaVision = 15f;
    public float distanciaAtaque = 4f;

    [Header("Configuración de Ataque")]
    public float danioPorDisparo = 10f;
    public float tiempoEntreDisparos = 1f;
    private float temporizadorDisparo = 0f;

    [Header("Ruta de Patrullaje")]
    public Transform[] puntosDeRuta;
    private int puntoActual = 0;

    void Start()
    {
        if (objetivo != null)
        {
            droneScript = objetivo.GetComponent<DroneController>();
        }

      
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (objetivo == null) return;

        // 1. Calcular la distancia
        float distanciaAlJugador = Vector3.Distance(transform.position, objetivo.position);

        // 2. Transiciones de Estado
        if (distanciaAlJugador <= distanciaAtaque)
        {
            estado = EstadoNPC.Atacando;
        }
        else if (distanciaAlJugador <= distanciaVision)
        {
            estado = EstadoNPC.Persiguiendo;
        }
        else
        {
            estado = EstadoNPC.Patrullando;
        }

        // 3. Comportamiento Y ANIMACIONES
        switch (estado)
        {
            case EstadoNPC.Patrullando:
                anim.SetInteger("EstadoAnimacion", 0); // NUEVO: Manda la orden de Caminar (0)
                Patrullar();
                break;
            case EstadoNPC.Persiguiendo:
                anim.SetInteger("EstadoAnimacion", 1); // NUEVO: Manda la orden de Correr (1)
                Perseguir();
                break;
            case EstadoNPC.Atacando:
                anim.SetInteger("EstadoAnimacion", 2); // NUEVO: Manda la orden de Disparar (2)
                Atacar();
                break;
        }
    }

    void Patrullar()
    {
        if (puntosDeRuta == null || puntosDeRuta.Length == 0) return;

        Transform destino = puntosDeRuta[puntoActual];
        if (destino == null) return;

        MirarHacia(destino.position);

        Vector3 destinoEnPiso = new Vector3(destino.position.x, transform.position.y, destino.position.z);
        transform.position = Vector3.MoveTowards(transform.position, destinoEnPiso, velocidadCaminar * Time.deltaTime);

        if (Vector3.Distance(transform.position, destinoEnPiso) < 0.2f)
        {
            puntoActual = (puntoActual + 1) % puntosDeRuta.Length;
        }
    }

    void Perseguir()
    {
        MirarHacia(objetivo.position);
        Vector3 destinoEnPiso = new Vector3(objetivo.position.x, transform.position.y, objetivo.position.z);
        transform.position = Vector3.MoveTowards(transform.position, destinoEnPiso, velocidadCorrer * Time.deltaTime);
    }

    void Atacar()
    {
        MirarHacia(objetivo.position);

        temporizadorDisparo += Time.deltaTime;

        if (temporizadorDisparo >= tiempoEntreDisparos && droneScript != null)
        {
            droneScript.RecibirDaño(danioPorDisparo);
            Debug.Log("¡Impacto en el Dron! Daño realizado: " + danioPorDisparo);
            temporizadorDisparo = 0f;
        }
    }

    void MirarHacia(Vector3 puntoDestino)
    {
        Vector3 direccion = (puntoDestino - transform.position).normalized;
        direccion.y = 0;

        if (direccion != Vector3.zero)
        {
            Quaternion rotacionDeseada = Quaternion.LookRotation(direccion);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionDeseada, 10f * Time.deltaTime);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distanciaVision);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanciaAtaque);

        if (puntosDeRuta != null && puntosDeRuta.Length > 0)
        {
            Gizmos.color = Color.cyan;
            for (int i = 0; i < puntosDeRuta.Length; i++)
            {
                if (puntosDeRuta[i] != null)
                {
                    Gizmos.DrawSphere(puntosDeRuta[i].position, 0.3f);
                    int siguientePunto = (i + 1) % puntosDeRuta.Length;
                    if (puntosDeRuta[siguientePunto] != null)
                    {
                        Gizmos.DrawLine(puntosDeRuta[i].position, puntosDeRuta[siguientePunto].position);
                    }
                }
            }
        }
    }
}