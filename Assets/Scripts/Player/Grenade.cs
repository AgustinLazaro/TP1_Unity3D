using UnityEngine;

public class Grenade : MonoBehaviour
{
    public AudioClip launchSound, explosionSound;
    public float explosionRadius = 5f;
    public float travelSpeed = 15f;

    private Vector3[] path;
    private int currentIndex = 0;

    public void SetPath(Vector3[] newPath)
    {
        path = newPath;
        transform.position = path[0];
        if (launchSound) AudioSource.PlayClipAtPoint(launchSound, transform.position);
    }

    void Update()
    {
        if (path == null || currentIndex >= path.Length) return;

        transform.position = Vector3.MoveTowards(transform.position, path[currentIndex], travelSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, path[currentIndex]) < 0.1f) currentIndex++;
    }

    void OnTriggerEnter(Collider other) => Explode();

    void Explode()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider col in hits)
        {
            // CAMBIO: Usamos NPCHealth y TakeDamage
            NPCHealth health = col.GetComponentInParent<NPCHealth>();
            if (health != null) health.TakeDamage(50f);
        }

        if (explosionSound) Play2DSound(explosionSound);
        Destroy(gameObject);
    }

    void Play2DSound(AudioClip clip)
    {
        GameObject tempAudio = new GameObject("TempAudio");
        AudioSource source = tempAudio.AddComponent<AudioSource>();
        source.clip = clip;
        source.spatialBlend = 0f;
        source.Play();
        Destroy(tempAudio, clip.length);
    }
}