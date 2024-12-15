using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathPlane : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
