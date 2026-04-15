using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 25f;
    public float damage = 10f;
    public float lifeTime = 4f;

    void Start() => Destroy(gameObject, lifeTime);

    void Update() => transform.Translate(Vector3.forward * speed * Time.deltaTime);

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<EnemyBrain>() != null || other.isTrigger) return;

        DroneController player = other.GetComponentInParent<DroneController>();

        if (player != null)
        {
            
            player.TakeDamage(damage);
            Destroy(gameObject);
        }
        else Destroy(gameObject);
    }
}