using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float health = 100;
    [SerializeField] private Slider sliderHealth;

    private void Start()
    {
        //LoadHealth();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
            SaveHealth();
    }

    public void AddPlayerHealth(int health)
    {
        if(this.health <= 90)
        {
            this.health += health;
            sliderHealth.value = this.health;
        }
        
    }

    public void Damage(int health)
    {
        this.health -= health;
        sliderHealth.value = this.health;

        if (this.health <= 0)
        {
            PlayerDeath();
        }
    }

    private void PlayerDeath()
    {
        PickupWeapon.slotFull = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void SaveHealth()
    {
        PlayerPrefs.SetFloat(SaveNames.PlayerHealth.ToString(), this.health);
    }

    private void LoadHealth()
    {
        this.health = PlayerPrefs.GetFloat(SaveNames.PlayerHealth.ToString());
        sliderHealth.value = this.health;
    }
}

enum SaveNames
{
    PlayerHealth,
}