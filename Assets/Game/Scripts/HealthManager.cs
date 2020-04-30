using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public int health;
    public int maxHealth = 10;

    public Text lblhealth;
    public Slider sliderHealth;
    float currentHealthNormalized
    {
        get
        {
            return ((float)health / (float)maxHealth);
        }
    }

    public GameObject pnlDeath;
    public Button btnRespawn;

    // Start is called before the first frame update
    void Start()
    {
        StartBase();

        btnRespawn.onClick.AddListener(Respawn);
    }

    private void StartBase()
    {
        health = maxHealth;
        //lblhealth.text = $"Salute: {health}";
        sliderHealth.value = currentHealthNormalized;

        UpdateHealthbarColor();

        pnlDeath.SetActive(false);
    }

    public void Heal(int amount)
    {
        Damage(-amount);
    }

    public void Damage(int damageTaken)
    {
        health -= damageTaken;

        if( health < 1 )
        {
            Die();
        }
        else if( health > maxHealth )
        {
            health = maxHealth;
        }

        //lblhealth.text = $"Salute: {health}";
        sliderHealth.value = currentHealthNormalized;

        UpdateHealthbarColor();
    }

    void UpdateHealthbarColor()
    {
        Image fillImage = sliderHealth.fillRect.GetComponent<Image>();
        if ( currentHealthNormalized >= 0.75f )
        {
            fillImage.color = Color.green;
        }
        else if( currentHealthNormalized >= 0.25f && currentHealthNormalized < 0.75f)
        {
            fillImage.color = Color.yellow;
        }
        else
        {
            fillImage.color = Color.red;
        }
    }

    public void Die()
    {
        GetComponent<CharacterController>().SetMovementEnabled(false);
        pnlDeath.SetActive(true);
    }

    public void Respawn()
    {
        StartBase();
        GetComponent<CharacterController>().SetMovementEnabled(true);
    }
}
