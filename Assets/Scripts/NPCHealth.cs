using UnityEngine;
using UnityEngine.UI;

public class NPCHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public bool isCivilian;
    public int rewardPoints = 10;
    public int penaltyPoints = -50;
    public Slider healthBar;

    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (healthBar != null) healthBar.value = currentHealth;

        if (currentHealth <= 0f) Die();
    }

    void Die()
    {
        if (isCivilian)
        {
            GameManager.instance.AddScore(penaltyPoints);
            GameManager.instance.GameOver();
        }
        else
        {
            GameManager.instance.AddScore(rewardPoints);
            GameManager.instance.EnemyKilled();
        }

        Destroy(gameObject);
    }
}