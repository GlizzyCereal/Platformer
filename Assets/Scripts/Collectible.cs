using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public TMPro.TextMeshProUGUI scoreText;
    public float bobAmplitude = 0.5f;
    public float bobSpeed = 2f;
    private Vector3 startPos;
    private int score = 0;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        Vector3 temp = startPos;
        temp.y += Mathf.Sin(Time.time * bobSpeed) * bobAmplitude;
        transform.position = temp;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            scoreText.text = ++score + "/7";
            Destroy(gameObject);
        }
    }
}