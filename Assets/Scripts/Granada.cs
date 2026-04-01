using UnityEngine;

public class Granada : MonoBehaviour
{
    [Header("Configuración de Explosión")]
    public float radioExplosion = 5f;
    public float DamageExplosion = 50f; // Saca el doble que el láser

    private Vector3 velocidadActual;
    private bool explotada = false;

   
    // El script del arma (DisparoLanzagranadas) llama a esta función apenas instancia la granada.
    // Me pasa un vector con la dirección y la fuerza. Lo guardo como mi velocidad de partida.
    public void IniciarTrayectoria(Vector3 velocidadInicial)
    {
        velocidadActual = velocidadInicial;
    }

    void Update()
    {
        if (explotada) return;

        // CÁLCULO DE PARÁBOLA  (Física sin Rigidbody para el TP)

        // 1. Aceleración por Gravedad

        // Fórmula cinemática: Velocidad Final = Velocidad Inicial + (Aceleración * Tiempo)
        // Agarro mi velocidad actual y la empujo hacia abajo usando la gravedad de Unity (-9.81 en Y).
        // Lo multiplico por Time.deltaTime para que sea constante sin importar los FPS.
        velocidadActual += Physics.gravity * Time.deltaTime;

        // 2. Desplazamiento del Fotograma (Frame)

        // Fórmula cinemática: Distancia = Velocidad * Tiempo
        // Sabiendo a qué velocidad voy AHORA, calculo exactamente cuántos metros me tengo 
        // que mover en este microsegundo y hacia dónde (es un Vector3, tiene dirección y magnitud).
        Vector3 desplazamiento = velocidadActual * Time.deltaTime;

        // 3. Sistema Anti-Traspaso 

        // Para que la granada no atraviese una pared por ir muy rápido (efecto tunneling), 
        // tiro un láser desde mi posición actual hacia donde me voy a mover.
        // El largo del láser es exactamente la distancia que voy a recorrer (desplazamiento.magnitude).
        RaycastHit impacto;
        if (Physics.Raycast(transform.position, desplazamiento.normalized, out impacto, desplazamiento.magnitude))
        {
            // ¡P El rayo detectó que en el medio del camino hay una pared o un enemigo.
            // Me teletransporto exactamente al punto del choque y detono.
            transform.position = impacto.point;
            Explotar();
        }
        else
        {
            // El camino está libre. Simplemente le sumo el vector de desplazamiento a mi posición.
            transform.position += desplazamiento;
        }
    }

    void Explotar()
    {
        explotada = true;

        //
        Debug.Log(" Granada explotó.");

        // MAGIA DE ÁREA (AoE): Creo una esfera invisible y guardo todo lo que toco en un array
        Collider[] objetosAlcanzados = Physics.OverlapSphere(transform.position, radioExplosion);

        foreach (Collider obj in objetosAlcanzados)
        {
            // Busco si el objeto tocado tiene el script de Vida.
            // Ojaldre: Uso GetComponentInParent porque a veces la explosión toca un collider 
            // del brazo o la pierna del modelo (hueso), y el script de vida está en el objeto Padre.
            VidaNPC vida = obj.GetComponentInParent<VidaNPC>();
            if (vida != null)
            {
                vida.RecibirDamage(DamageExplosion);
            }
        }

        // Destruyo el objeto 3D de la granada un instante después 
        Destroy(gameObject, 0.1f);
    }
}