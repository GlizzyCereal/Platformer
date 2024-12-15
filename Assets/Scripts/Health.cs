using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float maxHealth = 3f;
    public List<Image> hearts;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        UpdateHearts();
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void UpdateHearts(){
        for (int i = 0; i < hearts.Count; i++)
        {
            if (i < currentHealth / 2)
            {
            hearts[i].sprite = fullHeart;
            }
            else if (i < currentHealth / 2 + 0.5f)
            {
            hearts[i].sprite = halfHeart;
            }
            else
            {
            hearts[i].sprite = emptyHeart;
            }
        }
    }
}