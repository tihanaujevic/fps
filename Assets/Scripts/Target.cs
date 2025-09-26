using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    [SerializeField] private float health = 100;
    [SerializeField] private ParticleSystem destroyEffect;
    [SerializeField] private Slider sliderHealth;

    public void Damage(float amount, Vector3 pos, Quaternion rot)
    {
        health -= amount;
        if (sliderHealth)
        {
            sliderHealth.value = this.health;
        }
        
        if (health <= 0)
            Death(pos, rot);
    }

    private void Death(Vector3 pos, Quaternion rot)
    {
        EnemyMovement enemyMovement = GetComponent<EnemyMovement>();
        
        if (destroyEffect)
        {
            GameObject effect = Instantiate(destroyEffect, pos, rot).gameObject;
            Destroy(effect, 1);
        }

        if (enemyMovement != null)
        {
            enemyMovement.Die();
        }
        else
            Destroy(gameObject);
    }
}