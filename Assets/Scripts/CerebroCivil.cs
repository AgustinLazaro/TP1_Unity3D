using UnityEngine;

// Obligamos a que tenga un Animator, igual que hicimos con el guardia
[RequireComponent(typeof(Animator))]
public class CerebroCivil : MonoBehaviour
{
    [Header("Referencias")]
    public Transform objetivo; 
    private Animator anim;    

    [Header("Configuración de Susto")]
    public float distanciaSusto = 10f; 

    void Start()
    {
        
        anim = GetComponent<Animator>();
    }

    void Update()
    {
       
        if (objetivo == null) return;

       
        float distanciaAlJugador = Vector3.Distance(transform.position, objetivo.position);

      
        if (distanciaAlJugador <= distanciaSusto)
        {
            // (Estado 1)
            anim.SetInteger("EstadoAnimacionAdam", 1);

            
            MirarHacia(objetivo.position);
        }
        else
        {
            // (Estado 0)
            anim.SetInteger("EstadoAnimacionAdam", 0);
        }
    }

    
    void MirarHacia(Vector3 puntoDestino)
    {
        Vector3 direccion = (puntoDestino - transform.position).normalized;
        direccion.y = 0; 

        if (direccion != Vector3.zero)
        {
            Quaternion rotacionDeseada = Quaternion.LookRotation(direccion);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionDeseada, 5f * Time.deltaTime);
        }
    }

    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, distanciaSusto);
    }
}
