using UnityEngine;

public class DisparoLanzagranadas : MonoBehaviour
{
    [Header("Referencias de Apuntado")]
    public Transform camara1ra;
    public Transform camara3ra;
    public Transform puntoCañon;
    public GameObject prefabGranada;

    [Header("Fuerza Matemática")]
    public float fuerzaDisparo = 15f;
    public float anguloElevacion = 10f;

    void Update()
    {
        if (Input.GetButtonDown("Fire2")) LanzarGranada();
    }

    Transform ObtenerCamaraActiva()
    {
        if (camara1ra.gameObject.activeInHierarchy) return camara1ra;
        return camara3ra;
    }

    void LanzarGranada()
    {
        Transform cam = ObtenerCamaraActiva();

        // 1. Instancia la granada en la NARIZ del dron
        GameObject nuevaGranada = Instantiate(prefabGranada, puntoCañon.position, puntoCañon.rotation);

        // 2. Busca el punto exacto al que está mirando la cámara
        Vector3 puntoDestino;
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, 100f))
        {
            puntoDestino = hit.point;
        }
        else
        {
            puntoDestino = cam.position + cam.forward * 100f;
        }

        // 3. Se calcula  la dirección desde el cañón hacia ese punto, y se inclina para la parábola
        Vector3 direccionHaciaDestino = (puntoDestino - puntoCañon.position).normalized;
        Vector3 direccionDisparo = Quaternion.AngleAxis(-anguloElevacion, cam.right) * direccionHaciaDestino;

        Vector3 velocidadInicial = direccionDisparo * fuerzaDisparo;

        Granada scriptGranada = nuevaGranada.GetComponent<Granada>();
        if (scriptGranada != null)
        {
            scriptGranada.IniciarTrayectoria(velocidadInicial);
        }
    }
}