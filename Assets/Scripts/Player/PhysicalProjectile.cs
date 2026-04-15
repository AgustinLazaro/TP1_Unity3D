using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class PhysicalProjectile : MonoBehaviour
{
    public float speed = 50f;
    public float damage = 20f;
    public float lifeTime = 3f;

    void Start() => Destroy(gameObject, lifeTime);

    void Update() => transform.Translate(Vector3.forward * speed * Time.deltaTime);

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<DroneController>() != null) return;

       NPCHealth healthSystem = other.GetComponentInParent<NPCHealth>();

        if (healthSystem != null)
        {
            healthSystem.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        if (!other.isTrigger) Destroy(gameObject);
    }
}