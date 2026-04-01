using UnityEngine;

public class DisparoLaser : MonoBehaviour
{
    [Header("Referencias de Apuntado")]
    public Transform camara1ra;   
    public Transform camara3ra;  
    public Transform puntoCañon;  

    [Header("Configuración del Disparo")]
    public float distanciaDisparo = 100f;
    public LineRenderer lineaLaser;
    public bool laserEncendido = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) laserEncendido = !laserEncendido;

        if (laserEncendido) MostrarLaser();
        else lineaLaser.enabled = false;

        if (Input.GetButtonDown("Fire1")) Disparar();
    }

   
    Transform ObtenerCamaraActiva()
    {
        if (camara1ra.gameObject.activeInHierarchy) return camara1ra;
        return camara3ra;
    }

    void MostrarLaser()
    {
        Transform cam = ObtenerCamaraActiva();
        lineaLaser.enabled = true;

        
        lineaLaser.SetPosition(0, puntoCañon.position);

        RaycastHit impacto;
        
        if (Physics.Raycast(cam.position, cam.forward, out impacto, distanciaDisparo))
        {
            lineaLaser.SetPosition(1, impacto.point);
        }
        else
        {
            lineaLaser.SetPosition(1, cam.position + cam.forward * distanciaDisparo);
        }
    }

    void Disparar()
    {
        Transform cam = ObtenerCamaraActiva();
        RaycastHit impacto;

        
        if (Physics.Raycast(cam.position, cam.forward, out impacto, distanciaDisparo))
        {
            VidaNPC objetivo = impacto.transform.GetComponentInParent<VidaNPC>();
            if (objetivo != null)
            {
                objetivo.RecibirDamage(25f);
                Debug.Log("¡Láser impactó en " + objetivo.gameObject.name + "!");
            }
        }
    }
}